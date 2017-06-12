using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DotNetKit.Wpf.Printing.Demo.Printing;
using DotNetKit.Wpf.Printing.Demo.Samples.MultipageReportSample;
using Reactive.Bindings;

namespace DotNetKit.Wpf.Printing.Demo.Samples.AsynchronousSample
{
    public sealed class Sample
    {
        struct CancellableTask
        {
            public Task Task { get; }
            public CancellationTokenSource CancellationTokenSource { get; }
            public bool IsDefault => Task == null;

            public void Cancel()
            {
                CancellationTokenSource?.Cancel();
            }

            public CancellableTask(Task task, CancellationTokenSource cancellationTokenSource)
            {
                Task = task;
                CancellationTokenSource = cancellationTokenSource;
            }
        }

        public PrintQueueSelector PrintQueueSelector { get; }

        public ReactiveCommand PrintCommand { get; }

        public ReactiveCommand CancelCommand { get; }

        public ReadOnlyReactiveProperty<bool> IsPrinting { get; }

        readonly ReactiveProperty<CancellationTokenSource> currentCts =
            new ReactiveProperty<CancellationTokenSource>();

        public
            Sample(
                PrintQueueSelector printQueueSelector,
                Func<PrintQueue, CancellationToken, Task> printAsync,
                SynchronizationContext context
            )
        {
            PrintQueueSelector = printQueueSelector;

            IsPrinting = currentCts.Select(cts => cts != null).ToReadOnlyReactiveProperty();

            PrintCommand = IsPrinting.Select(isPrinting => !isPrinting).ToReactiveCommand();

            CancelCommand = currentCts.Select(cts => cts != null).ToReactiveCommand();

            CancelCommand.ObserveOn(context).Subscribe(_ =>
            {
                var cts = currentCts.Value;
                if (cts != null)
                {
                    currentCts.Value = null;
                    cts.Cancel();
                }
            });

            PrintCommand.ObserveOn(context).Select(async parameter =>
            {
                var printQueue = PrintQueueSelector.SelectedPrintQueue.Value;
                var cts = new CancellationTokenSource();
                currentCts.Value = cts;
                try
                {
                    await printAsync(printQueue, cts.Token).ContinueWith(_ =>
                    {
                        Console.WriteLine("continue");
                    }).ConfigureAwait(false);
                }
                finally
                {
                    currentCts.Value = null;
                }
            }).Subscribe();
        }
    }
}

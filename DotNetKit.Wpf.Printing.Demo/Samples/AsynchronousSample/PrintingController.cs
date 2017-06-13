using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Reactive.Concurrency;
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
    public sealed class PrintingController
    {
        public PrintQueueSelector PrintQueueSelector { get; }

        public ReactiveCommand PrintCommand { get; }

        public ReactiveCommand CancelCommand { get; }

        public ReadOnlyReactiveProperty<bool> IsPrinting { get; }

        readonly ReactiveProperty<CancellationTokenSource> currentCts =
            new ReactiveProperty<CancellationTokenSource>();

        public
            PrintingController(
                PrintQueueSelector printQueueSelector,
                Func<PrintQueue, CancellationToken, Task> printAsync,
                SynchronizationContext context
            )
        {
            var scheduler = new SynchronizationContextScheduler(context);

            PrintQueueSelector = printQueueSelector;

            IsPrinting =
                currentCts
                .Select(cts => cts != null)
                .ToReadOnlyReactiveProperty(eventScheduler: scheduler);

            PrintCommand =
                IsPrinting
                .Select(isPrinting => !isPrinting)
                .ToReactiveCommand(scheduler);

            CancelCommand =
                currentCts
                .Select(cts => cts != null)
                .ToReactiveCommand(scheduler);

            CancelCommand.Subscribe(_ =>
            {
                var cts = currentCts.Value;
                if (cts != null)
                {
                    currentCts.Value = null;
                    cts.Cancel();
                }
            });

            PrintCommand.Select(async parameter =>
            {
                var printQueue = PrintQueueSelector.SelectedPrintQueue.Value;
                var cts = new CancellationTokenSource();
                currentCts.Value = cts;
                try
                {
                    await printAsync(printQueue, cts.Token);
                }
                finally
                {
                    currentCts.Value = null;
                }
            }).Subscribe();
        }
    }
}

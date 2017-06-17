using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DotNetKit.Wpf.Printing.Demo.Printing;
using Prism.Commands;
using Prism.Mvvm;

namespace DotNetKit.Wpf.Printing.Demo.Samples.AsynchronousSample
{
    public sealed class PrintingController
        : BindableBase
    {
        public PrintQueueSelector PrintQueueSelector { get; }

        public DelegateCommand PrintCommand { get; }

        public DelegateCommand CancelCommand { get; }

        bool isPrinting;
        public bool IsPrinting
        {
            get { return isPrinting; }
            set { SetProperty(ref isPrinting, value); }
        }

        CancellationTokenSource currentCtsCore;
        CancellationTokenSource CurrentCts
        {
            get { return currentCtsCore; }
            set
            {
                SetProperty(ref currentCtsCore, value);
                IsPrinting = value != null;
                PrintCommand.RaiseCanExecuteChanged();
                CancelCommand.RaiseCanExecuteChanged();
            }
        }

        public
            PrintingController(
                PrintQueueSelector printQueueSelector,
                Func<PrintQueue, CancellationToken, Task> printAsync
            )
        {
            PrintQueueSelector = printQueueSelector;

            CancelCommand =
                new DelegateCommand(
                    () =>
                    {
                        var cts = CurrentCts;
                        if (cts != null)
                        {
                            CurrentCts = null;
                            cts.Cancel();
                        }
                    },
                    () => CurrentCts != null
                );

            PrintCommand =
                new DelegateCommand(
                    async () =>
                    {
                        var printQueue = PrintQueueSelector.SelectedPrintQueue;
                        var cts = new CancellationTokenSource();
                        CurrentCts = cts;
                        try
                        {
                            await printAsync(printQueue, cts.Token);
                        }
                        finally
                        {
                            CurrentCts = null;
                        }
                    },
                    () => CurrentCts == null
                );
        }
    }
}

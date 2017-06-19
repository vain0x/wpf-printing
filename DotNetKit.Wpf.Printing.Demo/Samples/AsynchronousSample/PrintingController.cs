using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using DotNetKit.Windows.Documents;
using DotNetKit.Wpf.Printing.Demo.Printing;
using DotNetKit.Wpf.Printing.Demo.Samples.MultipageReportSample;
using Prism.Commands;
using Prism.Mvvm;

namespace DotNetKit.Wpf.Printing.Demo.Samples.AsynchronousSample
{
    public sealed class PrintingController
        : BindableBase
    {
        public PrinterSelector PrinterSelector { get; }

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

        public PrintingController(PrinterSelector printerSelector)
        {
            PrinterSelector = printerSelector;

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
                        var printable = new OrderFormPage();
                        var pageSize = new Size(793.7, 1122.52);
                        var paginator = DataGridPrintablePaginator<Order>.Instance;

                        var printer = PrinterSelector.SelectedPrinterOrNull;
                        if (printer == null) return;

                        var cts = new CancellationTokenSource();
                        CurrentCts = cts;
                        try
                        {
                            var pages = paginator.Paginate(printable, pageSize);
                            await printer.PrintAsync(pages, pageSize, cts.Token);
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

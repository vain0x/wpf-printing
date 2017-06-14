using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using DotNetKit.Windows.Documents;
using DotNetKit.Wpf.Printing.Demo.Printing;
using DotNetKit.Wpf.Printing.Demo.Samples.MultipageReportSample;

namespace DotNetKit.Wpf.Printing.Demo.Samples.AsynchronousSample
{
    public static class PrintProgram
    {
        sealed class MyPrinter
        {
            readonly OrderFormPage printable = new OrderFormPage();

            static readonly IPaginator<IDataGridPrintable<Order>> paginator =
                DataGridPrintablePaginator<Order>.Instance;

            static readonly Size pageSize = new Size(793.7, 1122.52);

            public Task PrintAsync(PrintQueue printQueue, CancellationToken cancellationToken)
            {
                return new Printer().PrintAsync(printable, paginator, pageSize, printQueue, cancellationToken);
            }
        }

        static void PrintMain()
        {
            var dispatcher = Dispatcher.CurrentDispatcher;
            var context = new DispatcherSynchronizationContext(dispatcher);
            SynchronizationContext.SetSynchronizationContext(context);

            var printQueueSelector = PrintQueueSelectorModule.FromLocalServer();
            var printingController =
                new PrintingController(
                    printQueueSelector,
                    new MyPrinter().PrintAsync,
                    context
                );
            var mainWindow =
                new PrintingControllerWindow() { DataContext = printingController };

            mainWindow.Closed += (sender, e) =>
            {
                printQueueSelector.Dispose();
                dispatcher.InvokeShutdown();
            };

            mainWindow.Show();
            Dispatcher.Run();
        }

        public static void StartNew()
        {
            var printThread = new Thread(PrintMain);
            printThread.SetApartmentState(ApartmentState.STA);
            printThread.Name = "printer-thread";
            printThread.Start();
        }
    }
}

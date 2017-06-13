using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Reactive.Disposables;
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
            readonly OrderFormPage printable = new OrderFormPage(10);

            static readonly IPaginator<IDataGridPrintable<Order>> paginator =
                DataGridPrintablePaginator<Order>.Instance;

            static readonly Size pageSize = new Size(793.7, 1122.52);

            public Task PrintAsync(PrintQueue printQueue, CancellationToken cancellationToken)
            {
                return new Printer().PrintAsync(printable, paginator, pageSize, printQueue, cancellationToken);
            }
        }

        static void PrintMain(IDisposable disposable)
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
                disposable.Dispose();
                printQueueSelector.Dispose();
                dispatcher.InvokeShutdown();
            };

            mainWindow.Show();
            Dispatcher.Run();
        }

        public static void StartNew()
        {
            var disposable = new SingleAssignmentDisposable();

            var printThread = new Thread(() => PrintMain(disposable));
            printThread.SetApartmentState(ApartmentState.STA);
            printThread.Name = "printer-thread";
            printThread.Start();

            var onExit = new ExitEventHandler((sender, e) => printThread.Abort());
            Application.Current.Exit += onExit;
            disposable.Disposable = Disposable.Create(() => Application.Current.Exit -= onExit);
        }
    }
}

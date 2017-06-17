using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
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
        static void PrintMain()
        {
            var dispatcher = Dispatcher.CurrentDispatcher;
            var context = new DispatcherSynchronizationContext(dispatcher);
            SynchronizationContext.SetSynchronizationContext(context);

            var printerSelector = PrinterSelector.FromLocalServer();
            var printingController = new PrintingController(printerSelector);
            var mainWindow =
                new PrintingControllerWindow() { DataContext = printingController };

            mainWindow.Closed += (sender, e) =>
            {
                printerSelector.Dispose();
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

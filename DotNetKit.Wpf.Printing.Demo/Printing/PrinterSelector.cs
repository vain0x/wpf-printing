using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using DotNetKit.Misc.Disposables;
using Prism.Mvvm;

namespace DotNetKit.Wpf.Printing.Demo.Printing
{
    /// <summary>
    /// Represents a selector to choose a printer.
    /// </summary>
    public sealed class PrinterSelector<TPrinter>
        : BindableBase
        , IDisposable
    {
        public IReadOnlyList<TPrinter> Printers { get; }

        TPrinter selectedPrinterOrNull;
        public TPrinter SelectedPrinterOrNull
        {
            get { return selectedPrinterOrNull; }
            set { SetProperty(ref selectedPrinterOrNull, value); }
        }

        readonly IDisposable disposable;

        public void Dispose()
        {
            disposable.Dispose();
        }

        PrinterSelector(
            IReadOnlyList<TPrinter> printers,
            TPrinter defaultPrinterOrNull,
            IDisposable disposable
        )
        {
            this.disposable = disposable;

            Printers = printers;
            SelectedPrinterOrNull = defaultPrinterOrNull;
        }

        static int DefaultIndex(PrintQueue[] queues, PrintQueue defaultPrintQueue)
        {
            if (defaultPrintQueue != null)
            {
                for (var i = 0; i < queues.Length; i++)
                {
                    if (queues[i].Name == defaultPrintQueue.Name) return i;
                }
            }

            return queues.Length == 0 ? -1 : 0;
        }

        public static PrinterSelector<P>
            FromLocalServer<P>(Func<PrintQueue, P> printerFromPrintQueue)
            where P : class
        {
            var server = new LocalPrintServer();
            var printQueueCollection = server.GetPrintQueues();

            var disposable =
                new AnonymousDisposable(() =>
                {
                    server.Dispose();
                    printQueueCollection.Dispose();
                });

            var queues = printQueueCollection.Cast<PrintQueue>().ToArray();
            var defaultIndex = DefaultIndex(queues, server.DefaultPrintQueue);

            var printers = queues.Select(printerFromPrintQueue).ToArray();
            var defaultPrinter = defaultIndex >= 0 ? printers[defaultIndex] : null;
            return new PrinterSelector<P>(printers, defaultPrinter, disposable);
        }
    }
}

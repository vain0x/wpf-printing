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
    public sealed class PrinterSelector
        : BindableBase
        , IDisposable
    {
        public IReadOnlyList<IPrinter> Items { get; }

        IPrinter selectedPrinter;
        public IPrinter SelectedPrinter
        {
            get { return selectedPrinter; }
            set { SetProperty(ref selectedPrinter, value); }
        }

        readonly LocalPrintServer localPrintServer;
        readonly PrintQueueCollection printQueueCollection;

        public void Dispose()
        {
            localPrintServer.Dispose();
            printQueueCollection.Dispose();
        }

        PrinterSelector(
            IReadOnlyList<IPrinter> items,
            IPrinter defaultPrinter,
            LocalPrintServer localPrintServer,
            PrintQueueCollection printQueueCollection
        )
        {
            this.localPrintServer = localPrintServer;
            this.printQueueCollection = printQueueCollection;

            Items = items;
            SelectedPrinter = defaultPrinter;
        }

        public static PrinterSelector FromLocalServer()
        {
            var server = new LocalPrintServer();
            var queues = server.GetPrintQueues();

            try
            {
                var items = queues.Select(q => new Printer(q)).ToArray();

                if (items.Length == 0)
                {
                    throw new InvalidOperationException("No print queue available.");
                }

                var defaultPrinter = new Printer(server.DefaultPrintQueue);
                defaultPrinter =
                    items.FirstOrDefault(p => p.Name == defaultPrinter.Name)
                    ?? items[0];

                return new PrinterSelector(items, defaultPrinter, server, queues);
            }
            catch (Exception)
            {
                server.Dispose();
                queues.Dispose();
                throw;
            }
        }
    }
}

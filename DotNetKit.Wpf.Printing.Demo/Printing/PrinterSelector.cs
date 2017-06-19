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

        IPrinter selectedPrinterOrNull;
        public IPrinter SelectedPrinterOrNull
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
            IReadOnlyList<IPrinter> items,
            IPrinter defaultPrinterOrNull,
            IDisposable disposable
        )
        {
            this.disposable = disposable;

            Items = items;
            SelectedPrinterOrNull = defaultPrinterOrNull;
        }

        public static PrinterSelector FromLocalServer()
        {
            var server = new LocalPrintServer();
            var queues = server.GetPrintQueues();

            var disposable =
                new AnonymousDisposable(() =>
                {
                    server.Dispose();
                    queues.Dispose();
                });

            var items = queues.Select(q => new Printer(q)).ToArray();

            var defaultPrintQueue = server.DefaultPrintQueue;

            var defaultPrinter = default(IPrinter);
            if (defaultPrintQueue == null)
            {
                defaultPrinter = items.FirstOrDefault();
            }
            else
            { 
                defaultPrinter =
                    items.FirstOrDefault(p => p.Name == defaultPrintQueue.Name)
                    ?? items.FirstOrDefault();
            }

            return new PrinterSelector(items, defaultPrinter, disposable);
        }
    }
}

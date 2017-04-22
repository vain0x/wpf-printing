using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;
using Reactive.Bindings;

namespace DotNetKit.Wpf.Printing.Demo.Printing
{
    public sealed class PrintQueueSelector
        : IDisposable
    {
        public IReadOnlyList<KeyValuePair<string, PrintQueue>> Items { get; }
        public ReactiveProperty<PrintQueue> SelectedPrintQueue { get; }

        readonly IDisposable disposable;
        public void Dispose()
        {
            disposable.Dispose();
        }

        public
            PrintQueueSelector(
                IReadOnlyList<KeyValuePair<string, PrintQueue>> items,
                PrintQueue defaultPrintQueue,
                IDisposable disposable
            )
        {
            Items = items;
            SelectedPrintQueue = new ReactiveProperty<PrintQueue>(defaultPrintQueue);
            this.disposable = disposable;
        }
    }

    public static class PrintQueueSelectorModule
    {
        public static PrintQueueSelector FromLocalServer()
        {
            var server = new LocalPrintServer();
            var queues = server.GetPrintQueues();
            var disposable = StableCompositeDisposable.Create(server, queues);

            try
            {
                var items =
                    queues
                    .Select(q => new KeyValuePair<string, PrintQueue>(q.Name, q))
                    .ToArray();

                if (items.Length == 0)
                {
                    throw new InvalidOperationException("No print queue available.");
                }

                var defaultPrintQueue = server.DefaultPrintQueue ?? items[0].Value;
                return new PrintQueueSelector(items, defaultPrintQueue, disposable);
            }
            catch (Exception)
            {
                disposable.Dispose();
                throw;
            }
        }
    }
}

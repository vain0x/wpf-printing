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
    public sealed class PrintQueueSelector
        : BindableBase
        , IDisposable
    {
        public IReadOnlyList<KeyValuePair<string, PrintQueue>> Items { get; }

        PrintQueue selectedPrintQueue;
        public PrintQueue SelectedPrintQueue
        {
            get { return selectedPrintQueue; }
            set { SetProperty(ref selectedPrintQueue, value); }
        }

        readonly LocalPrintServer localPrintServer;
        readonly PrintQueueCollection printQueueCollection;

        public void Dispose()
        {
            localPrintServer.Dispose();
            printQueueCollection.Dispose();
        }

        public
            PrintQueueSelector(
                IReadOnlyList<KeyValuePair<string, PrintQueue>> items,
                PrintQueue defaultPrintQueue,
                LocalPrintServer localPrintServer,
                PrintQueueCollection printQueueCollection
            )
        {
            this.localPrintServer = localPrintServer;
            this.printQueueCollection = printQueueCollection;

            Items = items;
            SelectedPrintQueue = defaultPrintQueue;
        }
    }

    public static class PrintQueueSelectorModule
    {
        public static PrintQueueSelector FromLocalServer()
        {
            var server = new LocalPrintServer();
            var queues = server.GetPrintQueues();

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

                var defaultPrintQueue = server.DefaultPrintQueue;
                defaultPrintQueue =
                    items
                    .Select(kv => kv.Value)
                    .FirstOrDefault(kv => kv.FullName == defaultPrintQueue.FullName)
                    ?? items[0].Value;

                return new PrintQueueSelector(items, defaultPrintQueue, server, queues);
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

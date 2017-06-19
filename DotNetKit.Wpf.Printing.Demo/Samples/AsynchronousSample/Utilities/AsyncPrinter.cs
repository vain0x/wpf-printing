using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using DotNetKit.Windows.Documents;
using DotNetKit.Wpf.Printing.Demo.Printing.Xps;

namespace DotNetKit.Wpf.Printing.Demo.Samples.AsynchronousSample.Utilities
{
    public sealed class AsyncPrinter
        : IAsyncPrinter
    {
        readonly PrintQueue printQueue;

        public string Name
        {
            get { return printQueue.Name; }
        }

        public Task
            PrintAsync(
                IEnumerable pages,
                Size pageSize,
                CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            var isLandscape = pageSize.Width > pageSize.Height;
            var mediaSize = isLandscape ? new Size(pageSize.Height, pageSize.Width) : pageSize;

            // Set up print ticket.
            var ticket = printQueue.DefaultPrintTicket;
            ticket.PageMediaSize = new PageMediaSize(mediaSize.Width, mediaSize.Height);
            ticket.PageOrientation = PageOrientation.Portrait;

            // Generate FixedDocument to be printed.
            var document = new FixedDocumentCreator().FromDataContexts(pages, pageSize);

            // Print asynchronously.
            var writer = PrintQueue.CreateXpsDocumentWriter(printQueue);
            return writer.WriteAsyncAsTask(document, cancellationToken);
        }

        public AsyncPrinter(PrintQueue printQueue)
        {
            this.printQueue = printQueue;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Xps;
using DotNetKit.Windows.Documents;
using DotNetKit.Wpf.Printing.Demo.Printing.Xps;

namespace DotNetKit.Wpf.Printing.Demo.Printing
{
    public sealed class Printer
        : IPrinter
    {
        readonly PrintQueue printQueue;

        public string Name
        {
            get { return printQueue.Name; }
        }

        sealed class PrintFunction<TPrintable>
        {
            readonly TPrintable printable;
            readonly IPaginator<TPrintable> paginator;
            readonly Size pageSize;
            readonly PrintQueue printQueue;
            readonly CancellationToken cancellationToken;

            readonly bool isLandscape;
            readonly Size mediaSize;

            void Setup()
            { 
                var ticket = printQueue.DefaultPrintTicket;
                ticket.PageMediaSize = new PageMediaSize(mediaSize.Width, mediaSize.Height);
                ticket.PageOrientation = PageOrientation.Portrait;
            }

            FixedDocument Document()
            {
                return paginator.ToFixedDocument(printable, pageSize);
            }

            XpsDocumentWriter Writer()
            {
                return PrintQueue.CreateXpsDocumentWriter(printQueue);
            }

            public void Print()
            {
                Setup();
                var document = Document();
                var writer = Writer();
                writer.Write(document);
            }

            public Task PrintAsync()
            {
                Setup();
                var document = Document();
                var writer = Writer();
                return writer.WriteAsyncAsTask(document, cancellationToken);
            }

            public
                PrintFunction(
                    TPrintable printable,
                    IPaginator<TPrintable> paginator,
                    Size pageSize,
                    PrintQueue printQueue,
                    CancellationToken cancellationToken
                )
            {
                this.printable = printable;
                this.paginator = paginator;
                this.pageSize = pageSize;
                this.printQueue = printQueue;
                this.cancellationToken = cancellationToken;

                isLandscape = pageSize.Width > pageSize.Height;
                mediaSize = isLandscape ? new Size(pageSize.Height, pageSize.Width) : pageSize;
            }
        }

        public void
            Print<P>(
                P printable,
                IPaginator<P> paginator,
                Size pageSize
            )
        {
            new PrintFunction<P>(
                printable,
                paginator,
                pageSize,
                printQueue,
                CancellationToken.None
            ).Print();
        }

        public Task
            PrintAsync<P>(
                P printable,
                IPaginator<P> paginator,
                Size pageSize,
                CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            return
                new PrintFunction<P>(
                    printable,
                    paginator,
                    pageSize,
                    printQueue,
                    cancellationToken
                ).PrintAsync();
        }

        public Printer(PrintQueue printQueue)
        {
            this.printQueue = printQueue;
        }
    }
}

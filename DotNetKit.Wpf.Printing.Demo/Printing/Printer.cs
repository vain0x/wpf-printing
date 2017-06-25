using System;
using System.Collections;
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

        public void Print(IEnumerable pages, Size pageSize)
        {
            var isLandscape = pageSize.Width > pageSize.Height;
            var mediaSize = isLandscape ? new Size(pageSize.Height, pageSize.Width) : pageSize;

            // Set up print ticket.
            var ticket = printQueue.DefaultPrintTicket;
            ticket.PageMediaSize = new PageMediaSize(mediaSize.Width, mediaSize.Height);
            ticket.PageOrientation = PageOrientation.Portrait;

            // Generate FixedDocument to be printed from data contexts.
            var document = new FixedDocumentCreator().FromDataContexts(pages, pageSize);

            // Print.
            var writer = PrintQueue.CreateXpsDocumentWriter(printQueue);
            writer.Write(document);
        }

        public Printer(PrintQueue printQueue)
        {
            this.printQueue = printQueue;
        }
    }
}

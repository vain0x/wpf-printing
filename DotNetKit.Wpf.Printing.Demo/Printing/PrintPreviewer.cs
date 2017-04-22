using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using DotNetKit.Windows.Documents;
using Reactive.Bindings;

namespace DotNetKit.Wpf.Printing.Demo.Printing
{
    public class PrintPreviewer
        : IDisposable
    {
        readonly object printable;
        readonly IPaginator paginator;
        readonly Printer printer;

        static IReadOnlyList<PrintPreviewPage> emptyPages = new PrintPreviewPage[] { };

        public ReactiveProperty<IReadOnlyList<PrintPreviewPage>> Pages { get; } =
            new ReactiveProperty<IReadOnlyList<PrintPreviewPage>>(emptyPages);

        public MediaSizeSelector MediaSizeSelector { get; } =
            new MediaSizeSelector();

        public ReactiveProperty<bool> IsLandscape { get; } =
            new ReactiveProperty<bool>(false);

        public PrintQueueSelector PrintQueueSelector { get; }

        public ReactiveCommand PreviewCommand { get; } =
            new ReactiveCommand();

        public ReactiveCommand PrintCommand { get; } =
            new ReactiveCommand();

        Size PageSize
        {
            get
            {
                var mediaSize = MediaSizeSelector.SelectedSize.Value;
                return
                    IsLandscape.Value
                        ? new Size(mediaSize.Height, mediaSize.Width)
                        : mediaSize;

            }
        }

        public void UpdatePreview()
        {
            var pageSize = PageSize;
            Pages.Value =
                paginator.Paginate(printable, PageSize)
                .Cast<object>()
                .Select(content => new PrintPreviewPage(content, pageSize))
                .ToArray();
        }

        public void Print()
        {
            var printQueue = PrintQueueSelector.SelectedPrintQueue.Value;
            printer.Print(printable, paginator, PageSize, printQueue);
        }

        public void Dispose()
        {
            PrintQueueSelector.Dispose();
        }

        public PrintPreviewer(object printable, IPaginator paginator, Printer printer, PrintQueueSelector printQueueSelector)
        {
            this.printable = printable;
            this.paginator = paginator;
            this.printer = printer;
            PrintQueueSelector = printQueueSelector;

            PreviewCommand.Subscribe(_ => UpdatePreview());
            PrintCommand.Subscribe(_ => Print());
        }
    }
}

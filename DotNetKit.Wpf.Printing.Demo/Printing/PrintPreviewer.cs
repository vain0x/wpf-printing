using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using DotNetKit.Windows.Documents;
using Prism.Commands;
using Prism.Mvvm;

namespace DotNetKit.Wpf.Printing.Demo.Printing
{
    public interface IPrintPreviewer
        : IDisposable
    {
        ScaleSelector ScaleSelector { get; }
    }

    public class PrintPreviewer<TPrintable>
        : BindableBase
        , IPrintPreviewer
    {
        readonly TPrintable printable;
        readonly IPaginator<TPrintable> paginator;
        readonly Printer printer;

        static readonly IReadOnlyList<PrintPreviewPage> emptyPages = new PrintPreviewPage[] { };

        IReadOnlyList<PrintPreviewPage> pages = emptyPages;
        public IReadOnlyList<PrintPreviewPage> Pages
        {
            get { return pages; }
            set { SetProperty(ref pages, value); }
        }

        public MediaSizeSelector MediaSizeSelector { get; } =
            new MediaSizeSelector();

        bool isLandscape;
        public bool IsLandscape
        {
            get { return isLandscape; }
            set { SetProperty(ref isLandscape, value); }
        }

        public ScaleSelector ScaleSelector { get; } =
            new ScaleSelector();

        public PrintQueueSelector PrintQueueSelector { get; }

        public DelegateCommand PreviewCommand { get; }

        public DelegateCommand PrintCommand { get; }

        Size PageSize
        {
            get
            {
                var mediaSize = MediaSizeSelector.SelectedSize;
                return
                    IsLandscape
                        ? new Size(mediaSize.Height, mediaSize.Width)
                        : mediaSize;
            }
        }

        public void UpdatePreview()
        {
            var pageSize = PageSize;
            Pages =
                paginator.Paginate(printable, PageSize)
                .Cast<object>()
                .Select(content => new PrintPreviewPage(content, pageSize))
                .ToArray();
        }

        public void Print()
        {
            var printQueue = PrintQueueSelector.SelectedPrintQueue;
            printer.Print(printable, paginator, PageSize, printQueue);
        }

        public void Dispose()
        {
            PrintQueueSelector.Dispose();
        }

        public
            PrintPreviewer(
                TPrintable printable,
                IPaginator<TPrintable> paginator,
                Printer printer,
                PrintQueueSelector printQueueSelector
            )
        {
            this.printable = printable;
            this.paginator = paginator;
            this.printer = printer;
            PrintQueueSelector = printQueueSelector;

            PreviewCommand = new DelegateCommand(UpdatePreview);
            PrintCommand = new DelegateCommand(Print);
        }
    }
}

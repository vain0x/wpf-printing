using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace DotNetKit.Windows.Documents
{
    public static class PaginatableExtension
    {
        /// <summary>
        /// Paginates the specified printable into pages
        /// and converts to a <see cref="FixedDocument"/>.
        /// </summary>
        public static FixedDocument
            ToFixedDocument<P>(this IPaginator<P> paginator, P printable, Size pageSize)
        {
            var isLandscape = pageSize.Width > pageSize.Height;
            var mediaSize =
                isLandscape
                    ? new Size(pageSize.Height, pageSize.Width)
                    : pageSize;

            var document = new FixedDocument();

            foreach (var content in paginator.Paginate(printable, pageSize))
            {
                var presenter =
                    new ContentPresenter()
                    {
                        Content = content,
                        Width = pageSize.Width,
                        Height = pageSize.Height,
                    };

                if (isLandscape)
                {
                    presenter.LayoutTransform = new RotateTransform(90.0);
                }

                var page =
                    new FixedPage()
                    {
                        Width = mediaSize.Width,
                        Height = mediaSize.Height,
                    };
                page.Children.Add(presenter);

                page.Measure(mediaSize);
                page.Arrange(new Rect(new Point(0, 0), mediaSize));
                page.UpdateLayout();

                var pageContent = new PageContent() { Child = page };
                document.Pages.Add(pageContent);
            }

            return document;
        }
    }
}

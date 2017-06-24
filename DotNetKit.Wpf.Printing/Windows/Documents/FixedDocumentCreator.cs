using System;
using System.Collections;
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
    /// <summary>
    /// Provides a function.
    /// </summary>
    public struct FixedDocumentCreator
    {
        /// <summary>
        /// Converts data contexts to a <see cref="FixedDocument"/>.
        /// </summary>
        public FixedDocument FromDataContexts(IEnumerable contents, Size pageSize)
        {
            var isLandscape = pageSize.Width > pageSize.Height;
            var mediaSize = isLandscape ? new Size(pageSize.Height, pageSize.Width) : pageSize;

            var document = new FixedDocument();

            foreach (var content in contents)
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

                var pageContent = new PageContent() { Child = page };
                document.Pages.Add(pageContent);
            }

            return document;
        }
    }
}

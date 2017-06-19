using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using DotNetKit.Windows.Controls;
using DotNetKit.Windows.Documents;

namespace DotNetKit.Windows.Documents
{
    public struct DataGridPrintablePaginator<TItem>
    {
        sealed class PaginateAsyncFunction
        {
            readonly IDataGridPrintable<TItem> printable;
            readonly TItem[] allItems;
            readonly Size pageSize;
            readonly CancellationToken cancellationToken;

            int index;
            int pageIndex;

            ContentPresenter PagePresenterFromRestItems()
            {
                var restItems = new ArraySegment<TItem>(allItems, index, allItems.Length - index);
                var presenter =
                    new ContentPresenter()
                    {
                        Content = printable.CreatePage(restItems, pageIndex, pageCount: 1),
                        Width = pageSize.Width,
                        Height = pageSize.Height,
                    };
                presenter.Measure(pageSize);
                presenter.Arrange(new Rect(new Point(0, 0), pageSize));
                presenter.UpdateLayout();
                return presenter;
            }

            IPrintableDataGrid DataGridFromPagePresenter(ContentPresenter presenter)
            {
                var control =
                    VisualTreeHelper.GetChild(presenter, 0)
                    as IPrintableDataGridContainer;
                if (control == null)
                {
                    throw new InvalidOperationException($"{nameof(DataTemplate)} of printable page must directly generate a control implementing {nameof(IPrintableDataGridContainer)}.");
                }
                return control.DataGrid;
            }

            int CountVisibleRows(IPrintableDataGrid dataGrid)
            {
                var actualMeasure = dataGrid.ActualMeasure;

                var totalMeasure = 0.0;
                var count = 0;
                while (index + count < allItems.Length)
                {
                    totalMeasure += dataGrid.ItemMeasure(count);
                    if (totalMeasure > actualMeasure) break;

                    count++;
                }
                return count;
            }

            object[] PagesFromChunks(List<ArraySegment<TItem>> chunks)
            {
                var pages = new object[chunks.Count];
                for (var i = 0; i < chunks.Count; i++)
                {
                    pages[i] = printable.CreatePage(chunks[i], i, chunks.Count);
                }
                return pages;
            }

            public async Task<IEnumerable> PaginateAsync()
            {
                var chunks = new List<ArraySegment<TItem>>();

                while (index < allItems.Length)
                {
                    await awaitable;
                    cancellationToken.ThrowIfCancellationRequested();

                    var presenter = PagePresenterFromRestItems();
                    var dataGrid = DataGridFromPagePresenter(presenter);
                    var count = CountVisibleRows(dataGrid);
                    if (count == 0) throw new InfinitePaginationException();

                    chunks.Add(new ArraySegment<TItem>(allItems, index, count));
                    index += count;
                    pageIndex++;
                }

                return PagesFromChunks(chunks);
            }

            public
                PaginateAsyncFunction(
                    IDataGridPrintable<TItem> printable,
                    TItem[] allItems,
                    Size pageSize,
                    CancellationToken cancellationToken
                )
            {
                this.printable = printable;
                this.pageSize = pageSize;
                this.allItems = allItems;
            }
        }

        public Task<IEnumerable>
            PaginateAsync(
                IDataGridPrintable<TItem> printable,
                Size pageSize,
                CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            return
                new PaginateAsyncFunction(
                    printable,
                    printable.Items.ToArray(),
                    pageSize,
                    cancellationToken
                ).PaginateAsync();
        }
    }
}

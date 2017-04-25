using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using DotNetKit.Windows.Controls;
using DotNetKit.Windows.Documents;
using DotNetKit.Windows.Media;

namespace DotNetKit.Windows.Documents
{
    public sealed class DataGridPrintablePaginator
        : IPaginator
    {
        #region Paginate
        sealed class PaginateFunction
        {
            readonly IDataGridPrintable printable;
            readonly object[] allItems;
            readonly Size pageSize;

            int index;
            int pageIndex;

            ContentPresenter PagePresenterFromRestItems()
            {
                var restItems = new ArraySegment<object>(allItems, index, allItems.Length - index);
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
                    as IHavePrintableDataGrid;
                if (control == null)
                {
                    throw new InvalidOperationException($"{nameof(DataTemplate)} of printable page must directly generate a control implementing {nameof(IHavePrintableDataGrid)}.");
                }
                return control.DataGrid;
            }

            int CountVisibleRows(IPrintableDataGrid dataGrid)
            {
                var grid = dataGrid.Grid;
                var scrollViewer = dataGrid.ScrollViewer;
                var frozenRowCount = dataGrid.FrozenRowCount;

                var totalRowHeight = 0.0;

                totalRowHeight +=
                    grid.RowDefinitions.Take(frozenRowCount).Sum(r => r.ActualHeight);

                var count = 0;
                while (index + count < allItems.Length)
                {
                    var rowDefinition = grid.RowDefinitions[frozenRowCount + count];
                    totalRowHeight += rowDefinition.ActualHeight;
                    if (totalRowHeight > scrollViewer.ViewportHeight) break;

                    count++;
                }

                if (count == 0)
                {
                    throw new InvalidOperationException("Page size is too small to show contents of the data grid.");
                }

                return count;
            }

            object[] PagesFromChunks(List<ArraySegment<object>> chunks)
            {
                var pages = new object[chunks.Count];
                for (var i = 0; i < chunks.Count; i++)
                {
                    pages[i] = printable.CreatePage(chunks[i], i, chunks.Count);
                }
                return pages;
            }

            public IEnumerable Paginate()
            {
                var chunks = new List<ArraySegment<object>>();

                while (index < allItems.Length)
                {
                    var presenter = PagePresenterFromRestItems();
                    var dataGrid = DataGridFromPagePresenter(presenter);
                    var count = CountVisibleRows(dataGrid);

                    chunks.Add(new ArraySegment<object>(allItems, index, count));
                    index += count;
                    pageIndex++;
                }

                return PagesFromChunks(chunks);
            }

            public PaginateFunction(IDataGridPrintable printable, object[] allItems, Size pageSize)
            {
                this.printable = printable;
                this.pageSize = pageSize;
                this.allItems = allItems;
            }
        }

        public IEnumerable
            Paginate(
                IDataGridPrintable printable,
                Size pageSize
            )
        {
            var allItems = printable.Items.Cast<object>().ToArray();
            return new PaginateFunction(printable, allItems, pageSize).Paginate();
        }

        IEnumerable IPaginator.Paginate(object printable, Size pageSize)
        {
            return Paginate((IDataGridPrintable)printable, pageSize);
        }
        #endregion

        DataGridPrintablePaginator()
        {
        }

        public static IPaginator Instance { get; } = new DataGridPrintablePaginator();
    }
}

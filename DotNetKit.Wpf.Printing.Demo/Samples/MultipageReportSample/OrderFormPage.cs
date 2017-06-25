using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetKit.Windows.Controls;
using DotNetKit.Windows.Documents;

namespace DotNetKit.Wpf.Printing.Demo.Samples.MultipageReportSample
{
    public sealed class OrderFormPage
        : IDataGridPrintable<Order>
    {
        public OrderFormHeader Header { get; }
        public IReadOnlyList<Order> Items { get; }

        #region IDataGridPrintable
        IEnumerable<Order> IDataGridPrintable<Order>.Items => Items;

        object IDataGridPrintable<Order>.CreatePage(IReadOnlyList<Order> items, int pageIndex, int pageCount)
        {
            var header = Header.UpdatePageIndexCount(pageIndex, pageCount);
            return new OrderFormPage(header, items);
        }
        #endregion

        public OrderFormPage(OrderFormHeader header, IReadOnlyList<Order> items)
        {
            Header = header;
            Items = items;
        }

        /// <summary>
        /// Constructs with random data.
        /// </summary>
        public OrderFormPage()
        {
            Items =
                Enumerable.Range(1, 50)
                .Select(i => new Order($"Item {i}", i * 100))
                .ToArray();
            Header =
                new OrderFormHeader(
                    "Foo Bar Inc.",
                    new DateTime(2017, 01, 15),
                    Items.Sum(item => item.TotalPrice),
                    pageIndex: 0,
                    pageCount: 1
                );
        }

        public static OrderFormPage DesignerInstance { get; } =
            new OrderFormPage();
    }
}

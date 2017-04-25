using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetKit.Wpf.Printing.Demo.Samples.MultipageReportSample
{
    /// <summary>
    /// Represents a header part of an order form.
    /// </summary>
    public sealed class OrderFormHeader
    {
        public string TargetName { get; }
        public DateTime OrderDate { get; }
        public decimal TotalPrice { get; }
        public int PageIndex { get; }
        public int PageCount { get; }

        public int PageIndexPlus1 => PageIndex + 1;

        public
            OrderFormHeader(
                string targetName,
                DateTime orderDate,
                decimal totalPrice,
                int pageIndex,
                int pageCount
            )
        {
            TargetName = targetName;
            OrderDate = orderDate;
            TotalPrice = totalPrice;
            PageIndex = pageIndex;
            PageCount = pageCount;
        }

        public OrderFormHeader UpdatePageIndexCount(int pageIndex, int pageCount)
        {
            return
                new OrderFormHeader(
                    TargetName,
                    OrderDate,
                    TotalPrice,
                    pageIndex,
                    pageCount
                );
        }
    }
}

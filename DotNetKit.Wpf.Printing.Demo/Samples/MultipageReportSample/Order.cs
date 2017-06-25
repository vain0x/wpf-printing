using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetKit.Wpf.Printing.Demo.Samples.MultipageReportSample
{
    /// <summary>
    /// Represents an order.
    /// </summary>
    public sealed class Order
    {
        static Random Random { get; } =
            new Random();

        public string Name { get; }
        public int Count { get; }
        public decimal UnitPrice { get; }
        public decimal TotalPrice { get; }
        public string Note { get; }

        public Order(string name, int unitPrice)
        {
            Name = name;
            Count = Random.Next(1, 2000);
            TotalPrice = Count * unitPrice;

            // Make a multiline note to make pagination non-trivial.
            Note =
                string.Join(
                    Environment.NewLine,
                    Enumerable.Range(0, Random.Next(0, 3)).Select(i => $"Note {i + 1}")
                );
        }
    }
}

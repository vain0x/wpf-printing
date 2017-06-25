using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DotNetKit.Windows.Controls;

namespace DotNetKit.Windows.Documents
{
    /// <summary>
    /// Represents a printable object which contains items.
    /// </summary>
    public interface IDataGridPrintable<TItem>
    {
        /// <summary>
        /// Gets the sequence of items.
        /// </summary>
        IEnumerable<TItem> Items { get; }

        /// <summary>
        /// Creates an object which represents a page with the specified items.
        /// </summary>
        object CreatePage(IReadOnlyList<TItem> items, int pageIndex, int pageCount);
    }
}

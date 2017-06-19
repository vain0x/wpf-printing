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
    /// Represents a printable object which contains a collection.
    /// </summary>
    public interface IDataGridPrintable<TItem>
    {
        IEnumerable<TItem> Items { get; }

        object CreatePage(IReadOnlyList<TItem> items, int pageIndex, int pageCount);
    }
}

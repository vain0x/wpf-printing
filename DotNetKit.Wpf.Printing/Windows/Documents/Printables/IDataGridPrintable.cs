using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DotNetKit.Windows.Documents
{
    /// <summary>
    /// Represents a printable object
    /// whose <see cref="DataTemplate"/> produces a control
    /// which implements <see cref="IHavePrintableDataGrid"/>.
    /// </summary>
    public interface IDataGridPrintable
    {
        IEnumerable Items { get; }

        object CreatePage(IEnumerable items, int pageIndex, int pageCount);
    }
}

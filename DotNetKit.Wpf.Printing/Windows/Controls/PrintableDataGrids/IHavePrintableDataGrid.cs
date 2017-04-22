using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DotNetKit.Windows.Controls
{
    /// <summary>
    /// Represents a printable control which contains a <see cref="PrintableDataGrid"/>.
    /// </summary>
    public interface IHavePrintableDataGrid
    {
        IPrintableDataGrid DataGrid { get; }
    }
}

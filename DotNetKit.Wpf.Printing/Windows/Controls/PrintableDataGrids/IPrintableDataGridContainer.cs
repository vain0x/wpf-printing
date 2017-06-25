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
    /// Represents a UI element which contains a <see cref="IPrintableDataGrid"/>.
    /// </summary>
    public interface IPrintableDataGridContainer
    {
        /// <summary>
        /// Gets the data grid.
        /// </summary>
        IPrintableDataGrid DataGrid { get; }
    }
}

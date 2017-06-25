using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DotNetKit.Windows.Controls
{
    /// <summary>
    /// Represents a UI element to display a list of items linearly.
    /// </summary>
    public interface IPrintableDataGrid
    {
        /// <summary>
        /// Gets the measure (width or height) of the item at the specified index.
        /// </summary>
        double ItemMeasure(int index);

        /// <summary>
        /// Gets the measure (width or height) of the area to display items.
        /// If the value is less than sum of <see cref="ItemMeasure(int)"/>,
        /// some of items are clipped.
        /// </summary>
        double ActualMeasure { get; }
    }
}

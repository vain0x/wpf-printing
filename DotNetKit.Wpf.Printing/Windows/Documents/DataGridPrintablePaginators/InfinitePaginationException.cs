using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetKit.Windows.Documents
{
    /// <summary>
    /// Represents an exception thrown when the pagination doesn't stop.
    /// </summary>
    public class InfinitePaginationException
        : InvalidOperationException
    {
        /// <summary>
        /// Gets the error message.
        /// </summary>
        public override string Message =>
            "Page size is too small to display any item in the data grid.";
    }
}

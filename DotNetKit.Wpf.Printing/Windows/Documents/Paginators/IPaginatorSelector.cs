using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetKit.Windows.Documents
{
    /// <summary>
    /// Represents an object to select an appropriate paginator.
    /// </summary>
    public interface IPaginatorSelector
    {
        /// <summary>
        /// Tries to select an appropriate paginator for the specified printable.
        /// </summary>
        /// <param name="printable"></param>
        /// <param name="paginator"></param>
        /// <returns></returns>
        bool TrySelect(object printable, out IPaginator paginator);
    }
}

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
    /// Provides <see cref="Paginate(Size)"/>.
    /// </summary>
    public interface IPaginator<in TPrintable>
    {
        /// <summary>
        /// Paginates the printable into pages.
        /// </summary>
        IEnumerable Paginate(TPrintable printable, Size pageSize);
    }
}

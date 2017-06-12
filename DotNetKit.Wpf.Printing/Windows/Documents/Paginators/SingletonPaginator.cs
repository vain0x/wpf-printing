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
    /// Represents a paginator to paginate any printable to a single page.
    /// </summary>
    public sealed class SingletonPaginator<TPrintable>
        : IPaginator<TPrintable>
    {
        /// <summary>
        /// Returns a sequence which consists of the specified printable.
        /// </summary>
        /// <param name="printable"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IEnumerable Paginate(TPrintable printable, Size pageSize)
        {
            return new[] { printable };
        }

        SingletonPaginator()
        {
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        public static IPaginator<TPrintable> Instance { get; } =
            new SingletonPaginator<TPrintable>();
    }
}

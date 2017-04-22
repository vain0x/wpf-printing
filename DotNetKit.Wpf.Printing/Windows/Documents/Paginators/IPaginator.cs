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
    public interface IPaginator
    {
        /// <summary>
        /// Paginates the printable into pages.
        /// </summary>
        IEnumerable Paginate(object printable, Size pageSize);
    }

    sealed class AnonymousPaginator<TReport>
        : IPaginator
    {
        readonly Func<TReport, Size, IEnumerable> paginate;

        public IEnumerable Paginate(TReport page, Size pageSize)
        {
            return paginate(page, pageSize);
        }

        IEnumerable IPaginator.Paginate(object printable, Size pageSize)
        {
            return Paginate((TReport)printable, pageSize);
        }

        public AnonymousPaginator(Func<TReport, Size, IEnumerable> paginate)
        {
            this.paginate = paginate;
        }
    }

    public static class PaginatorModule
    {
        public static IPaginator FromFunc<R>(Func<R, Size, IEnumerable> paginate)
        {
            return new AnonymousPaginator<R>(paginate);
        }
    }
}

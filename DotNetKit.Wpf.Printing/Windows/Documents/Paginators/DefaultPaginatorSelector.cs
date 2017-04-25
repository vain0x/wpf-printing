using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetKit.Windows.Controls;

namespace DotNetKit.Windows.Documents
{
    public sealed class DefaultPaginatorSelector
        : IPaginatorSelector
    {
        readonly Dictionary<Type, IPaginator> paginatorFromType =
            new Dictionary<Type, IPaginator>();

        public void Register(Type type, IPaginator paginator)
        {
            if (paginatorFromType.ContainsKey(type))
            {
                paginatorFromType[type] = paginator;
            }
            else
            {
                paginatorFromType.Add(type, paginator);
            }
        }

        public bool TrySelect(object printable, out IPaginator paginator)
        {
            if (printable == null) throw new ArgumentNullException(nameof(printable));

            if (paginatorFromType.TryGetValue(printable.GetType(), out paginator))
            {
                return true;
            }

            if (printable is ISinglePagePrintable)
            {
                paginator = SingletonPaginator.Instance;
                return true;
            }

            if (printable is IDataGridPrintable)
            {
                paginator = DataGridPrintablePaginator.Instance;
                return true;
            }

            return false;
        }
    }
}

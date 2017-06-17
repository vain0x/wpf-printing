using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using DotNetKit.Windows.Documents;

namespace DotNetKit.Wpf.Printing.Demo.Printing
{
    public interface IPrinter
    {
        void
            Print<P>(
                P printable,
                IPaginator<P> paginator,
                Size pageSize,
                PrintQueue printQueue
            );
    }

    public interface IAsyncPrinter
    {
        Task
            PrintAsync<P>(
                P printable,
                IPaginator<P> paginator,
                Size pageSize,
                PrintQueue printQueue,
                CancellationToken cancellationToken = default(CancellationToken)
            );
    }
}

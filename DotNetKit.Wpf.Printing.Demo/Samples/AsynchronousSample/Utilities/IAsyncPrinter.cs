using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace DotNetKit.Wpf.Printing.Demo.Samples.AsynchronousSample.Utilities
{
    public interface IAsyncPrinter
    {
        string Name { get; }

        Task
            PrintAsync(
                IEnumerable pages,
                Size pageSize,
                CancellationToken cancellationToken = default(CancellationToken)
            );
    }
}

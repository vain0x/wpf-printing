using System;
using System.Collections;
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
        string Name { get; }

        void Print(IEnumerable pages, Size pageSize);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetKit.Windows.Documents
{
    public abstract class AsyncProgressReporter
    {
        public abstract void Report(int index, int totalCount, int pageIndex);
    }
}

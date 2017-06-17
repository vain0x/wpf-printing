using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetKit.Windows.Documents
{
    public class InfinitePagePaginationException
        : InvalidOperationException
    {
        public override string Message =>
            "Page size is too small to display any item in the data grid.";
    }
}

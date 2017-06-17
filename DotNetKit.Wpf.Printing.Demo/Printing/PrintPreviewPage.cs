using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DotNetKit.Wpf.Printing.Demo.Printing
{
    public sealed class PrintPreviewPage
    {
        public object Content { get; }
        public Size PageSize { get; }

        public PrintPreviewPage(object content, Size pageSize)
        {
            Content = content;
            PageSize = pageSize;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;

namespace DotNetKit.Wpf.Printing.Demo.PrintPreviewers
{
    public sealed class ScaleSelector
        : BindableBase
    {
        double scale = 1;
        public double Scale
        {
            get { return scale; }
            set { SetProperty(ref scale, value); }
        }

        public void Zoom(int delta)
        {
            Scale *= 1 + Math.Sign(delta) * 0.1;
        }
    }
}

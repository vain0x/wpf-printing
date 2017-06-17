using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;

namespace DotNetKit.Wpf.Printing.Demo.Printing
{
    public class ScaleSelector
        : BindableBase
    {
        double scale = 1;
        public double Scale
        {
            get { return scale; }
            set { SetProperty(ref scale, value); }
        }

        #region Zoom
        double ScaleFactor(int delta)
        {
            var d = Math.Sign(delta);
            var tan = Math.Tan(Math.PI / 2);
            var x = 0.1 / (2.0 * 1.1 * tan);
            return 1 / (1 - (x * d) * tan * 2);
        }

        public void Zoom(int delta)
        {
            Scale *= ScaleFactor(delta);
        }
        #endregion
    }
}

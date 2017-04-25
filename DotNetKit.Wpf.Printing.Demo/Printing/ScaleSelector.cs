using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetKit.Wpf.Printing.Demo.Printing
{
    public class ScaleSelector
    {
        public ReactiveProperty<double> Scale { get; } =
            new ReactiveProperty<double>(1.0);

        public IReadOnlyReactiveProperty<string> ScalePercentage { get; }

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
            Scale.Value *= ScaleFactor(delta);
        }
        #endregion

        public ScaleSelector()
        {
            ScalePercentage = Scale.Select(scale => $"{Math.Truncate(scale * 100)}%").ToReadOnlyReactiveProperty();
        }
    }
}

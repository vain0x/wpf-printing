using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DotNetKit.Windows.Controls
{
    /// <summary>
    /// Represents a control which contains a UIElement and downscales it if flooded.
    /// It doesn't upscale.
    /// Basically it contains a TextBlock with text-wrapping or a WrapPanel.
    /// </summary>
    public partial class ScaleControl
        : UserControl
    {
        const double downscaleFactor = 0.95;
        const double minimumScale = 0.0001;

        double scale;

        double Downscale(double scale, double rate)
        {
            return
                Math.Max(
                    minimumScale,
                    Math.Min(
                        scale * downscaleFactor,
                        (scale + rate) / 2
                    ));
        }

        /// <summary>
        /// Measures.
        /// </summary>
        protected override Size MeasureOverride(Size availableSize)
        {
            if (availableSize.Height <= double.Epsilon)
            {
                return new Size(0, 0);
            }

            scale = 1.0;

            while (true)
            {
                var width = 0.0;
                var height = 0.0;

                var childSize = new Size(availableSize.Width / scale, double.PositiveInfinity);
                for (var i = 0; i < VisualChildrenCount; i++)
                {
                    var child = VisualTreeHelper.GetChild(this, i) as UIElement;
                    if (child == null) continue;

                    child.Measure(childSize);
                    height += child.DesiredSize.Height;
                    width = Math.Max(width, child.DesiredSize.Width);
                }

                var availableHeight = availableSize.Height / scale;
                var nextScale = Downscale(scale, height / availableHeight);

                if (height <= availableHeight || nextScale < minimumScale)
                {
                    return new Size(width * scale, height * scale);
                }

                scale = nextScale;
            }
        }

        /// <summary>
        /// Arranges.
        /// </summary>
        protected override Size ArrangeOverride(Size finalSize)
        {
            var scaleTransform = new ScaleTransform(scale, scale);
            var height = 0.0;
            for (var i = 0; i < VisualChildrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(this, i) as UIElement;
                if (child == null) continue;

                child.RenderTransform = scaleTransform;
                child.Arrange(new Rect(new Point(0, scale * height), new Size(finalSize.Width / scale, child.DesiredSize.Height)));
                height += child.DesiredSize.Height;
            }

            return finalSize;
        }
    }
}

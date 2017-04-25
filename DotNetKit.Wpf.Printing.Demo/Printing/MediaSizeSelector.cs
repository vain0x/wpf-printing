using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Reactive.Bindings;

namespace DotNetKit.Wpf.Printing.Demo.Printing
{
    public sealed class MediaSizeSelector
    {
        #region Items
        sealed class MediaSizeListBuilder
        {
            List<KeyValuePair<string, Size>> List { get; } =
                new List<KeyValuePair<string, Size>>();

            // NOTE: FixedPage は 96 dpi 固定。
            const double dpi = 96.0;

            // Inch per millimeter.
            const double ipmm = 0.0393701;

            const double dpmm = ipmm * dpi;

            public MediaSizeListBuilder
                Add(
                    string name,
                    int widthMillimeter,
                    int heightMillimeter
                )
            {
                var width = widthMillimeter * dpmm;
                var height = heightMillimeter * dpmm;
                var size = new Size(width, height);
                List.Add(new KeyValuePair<string, Size>(name, size));
                return this;
            }

            public KeyValuePair<string, Size>[] ToArray()
            {
                return List.ToArray();
            }
        }

        static readonly IReadOnlyList<KeyValuePair<string, Size>> items =
            new MediaSizeListBuilder()
            .Add("A0", 841, 1189)
            .Add("A1", 594, 841)
            .Add("A2", 420, 594)
            .Add("A3", 297, 420)
            .Add("A4", 210, 297)
            .Add("A5", 148, 210)
            .Add("A6", 105, 148)
            .Add("A7", 74, 105)
            .Add("B3 (JIS)", 364, 515)
            .Add("B4 (JIS)", 257, 364)
            .Add("B5 (JIS)", 182, 257)
            .Add("B6 (JIS)", 128, 182)
            .ToArray();

        public IReadOnlyList<KeyValuePair<string, Size>> Items => items;
        #endregion

        public ReactiveProperty<Size> SelectedSize { get; } =
            new ReactiveProperty<Size>(items.First(item => item.Key == "A5").Value);
    }
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace DotNetKit.Wpf.Printing.Demo.PrintPreviewers
{
    public sealed class DoublePercentConverter
        : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((double)value) * 100;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return double.Parse((string)value) / 100;
        }

        public static DoublePercentConverter Instance { get; } =
            new DoublePercentConverter();
    }
}

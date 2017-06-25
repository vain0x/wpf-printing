using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DotNetKit.Windows.Documents;
using DotNetKit.Wpf.Printing.Demo.Printing;
using DotNetKit.Wpf.Printing.Demo.Samples.MultipageReportSample;

namespace DotNetKit.Wpf.Printing.Demo.Samples.AsynchronousSample
{
    /// <summary>
    /// SampleControl.xaml の相互作用ロジック
    /// </summary>
    public partial class SampleControl : UserControl
    {
        void openButton_Click(object sender, RoutedEventArgs e)
        {
            PrintProgram.StartNew();
        }

        public SampleControl()
        {
            InitializeComponent();
        }
    }
}

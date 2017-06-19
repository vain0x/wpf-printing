using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace DotNetKit.Wpf.Printing.Demo.Samples.HelloWorldSample
{
    /// <summary>
    /// SampleControl.xaml の相互作用ロジック
    /// </summary>
    public partial class SampleControl : UserControl
    {
        public SampleControl()
        {
            InitializeComponent();

            var previewer =
                new PrintPreviewer<HelloWorldPage>(
                    new HelloWorldPage(),
                    (page, _) => new[] { page },
                    PrinterSelector<IPrinter>.FromLocalServer<IPrinter>(q => new Printer(q))
                );
            DataContext = previewer;

            Loaded += (sender, e) =>
            {
                previewer.UpdatePreview();
            };
        }
    }
}

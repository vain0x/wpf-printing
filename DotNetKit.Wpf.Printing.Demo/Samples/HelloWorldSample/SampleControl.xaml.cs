using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
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
using DotNetKit.Wpf.Printing.Demo.PrintPreviewers;

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

            DataContext = this;
        }

        /// <summary>
        /// Gets the data for the page to be printed.
        /// </summary>
        public HelloWorldPage Page { get; } =
            new HelloWorldPage();

        /// <summary>
        /// Gets the page size (A4).
        /// </summary>
        public Size PageSize
        {
            get { return new Size(793.7, 1122.52); }
        }

        /// <summary>
        /// Prints the page.
        /// </summary>
        void printMenuItem_Click(object sender, RoutedEventArgs e)
        {
            using (var server = new LocalPrintServer())
            using (var printQueue = server.DefaultPrintQueue)
            {
                // Create a printer which provides Print function.
                var printer = new Printer(printQueue);

                // Print single A4-size page.
                printer.Print(new[] { Page }, PageSize);
            }
        }
    }
}

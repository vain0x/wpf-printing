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
using DotNetKit.Windows.Controls;

namespace DotNetKit.Wpf.Printing.Demo.Samples.MultipageReportSample
{
    /// <summary>
    /// OrderFormPageControl.xaml の相互作用ロジック
    /// </summary>
    public partial class OrderFormPageControl
        : UserControl
        , IHavePrintableDataGrid
    {
        public OrderFormPageControl()
        {
            InitializeComponent();
        }

        IPrintableDataGrid IHavePrintableDataGrid.DataGrid => dataGrid;
    }
}

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

namespace DotNetKit.Wpf.Printing.Demo.PrintPreviewers
{
    /// <summary>
    /// PrintPreviewerControl.xaml の相互作用ロジック
    /// </summary>
    public partial class PrintPreviewerControl : UserControl
    {
        IPrintPreviewer Previewer => DataContext as IPrintPreviewer;

        public PrintPreviewerControl()
        {
            InitializeComponent();

            scrollViewer.PreviewMouseWheel += (sender, e) =>
            {
                var previewer = Previewer;
                if (previewer != null && Keyboard.Modifiers.HasFlag(ModifierKeys.Control))
                {
                    previewer.ScaleSelector.Zoom(e.Delta);
                    e.Handled = true;
                }
            };
        }
    }
}

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
        sealed class View
            : IDisposable
        {
            SingleThreadSynchronizationContext Context { get; }

            public Sample Model { get; }

            public PrintQueueSelector PrintQueueSelector { get; }

            #region PrintAsync
            readonly OrderFormPage printable = new OrderFormPage(10);

            static readonly IPaginator<IDataGridPrintable<Order>> paginator =
                DataGridPrintablePaginator<Order>.Instance;

            static readonly Size pageSize = new Size(793.7, 1122.52);

            public Task PrintAsync(PrintQueue printQueue, CancellationToken cancellationToken)
            {
                return new Printer().PrintAsync(printable, paginator, pageSize, printQueue, cancellationToken);
            }
            #endregion

            public void Dispose()
            {
                Context.Post(() =>
                {
                    PrintQueueSelector.Dispose();
                });

                Context.Dispose();
            }

            public View(SingleThreadSynchronizationContext context)
            {
                Context = context;
                PrintQueueSelector = PrintQueueSelectorModule.FromLocalServer();
                Model = new Sample(PrintQueueSelector, PrintAsync, Context);
            }
        }

        View view;

        public SampleControl()
        {
            InitializeComponent();

            Loaded += (sender, e) =>
            {
                if (view == null)
                {
                    var mainContext = SynchronizationContext.Current;

                    var printContext = new SingleThreadSynchronizationContext();
                    printContext.Thread.SetApartmentState(ApartmentState.STA);
                    printContext.Thread.Name = "printer-thread";
                    printContext.Thread.IsBackground = true;
                    printContext.Start();

                    printContext.Post(() =>
                    {
                        view = new View(printContext);

                        mainContext.Post(dataContext =>
                        {
                            DataContext = dataContext;
                        }, view.Model);
                    });
                }
            };

            Unloaded += (sender, e) =>
            {
                view.Dispose();
                view = null;
            };
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetKit.Windows.Documents;

namespace DotNetKit.Wpf.Printing.Demo.Samples.HelloWorldSample
{
    public class HelloWorldPage
    {
        public string Title => "HELLO WORLD REPORT";
        public DateTime IssueDate => new DateTime(2007, 8, 31);
        public string Message => "Hello, world!";

        public static HelloWorldPage DesignerInstance { get; } =
            new HelloWorldPage();
    }
}

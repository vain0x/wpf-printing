# WPF Printing
[![NuGet version](https://badge.fury.io/nu/DotNetKit.Wpf.Printing.svg)](https://badge.fury.io/nu/DotNetKit.Wpf.Printing)

This repository hosts a library to provide utilities for printing with WPF and demonstrates how to print UI elements.

## Install
Install the [DotNetKit.Wpf.Printing](https://www.nuget.org/packages/DotNetKit.Wpf.Printing) package via NuGet.

## Usage
See the Demo project in the repository for usage.

NOTE: Don't overlook ``App.xaml``.

### MainWindow
`MainWindow` contains a tab control. Each tab page represents a sample independently.

### Samples/HelloWorldSample
The `HelloWorldSample` is the simplest sample. It contains a simple previewer which displays page content and print button. When the button is clicked, a page is printed.

`HelloWorldPage` provides data for the printed page and `HelloWorldPageControl` defines the visual of the page. However, the page's content is actually defined by the first `DataTemplate` in ``App.xaml``.

- To print, the default printer of the computer is used.
- This sample isn't designed in the MVVM architecture and doesn't dispose resources for simplicity.

### Samples/MultipageReportSample
The `MultipageReportSample` demonstrates how to print a grid with many rows by paginating it into multiple pages.

It contains more complex previewer to choose a printer and paper size and enable zooming.

An `OrderFormPage` is the ViewModel object to be printed and `OrderFormPageControl` defines its visual. To display orders, a DataGrid-like control `PrintableDataGird` is used because there's a problem to print WPF's `DataGrid`/`ListView`. Note that `OrderFormPageControl` implements `IPrintableDataGridContainer`, which is required by `DataGridPrintablePaginator`.

Usually the grid can't display 50 orders in an A4 size paper. To paginate it, you can use ``DataGridPrintablePaginator<_>``'s `Paginate` method.

- `IPrinter` is a wrapper of `PrintQueue` to avoid direct dependency to printers in the ViewModel layer.

### Samples/AsynchronousSample
This is an experiment.

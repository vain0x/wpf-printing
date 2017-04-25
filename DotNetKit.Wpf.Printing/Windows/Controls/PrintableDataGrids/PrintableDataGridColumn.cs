using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace DotNetKit.Windows.Controls
{
    public class PrintableDataGridColumn
        : DependencyObject
    {
        #region Width
        public static DependencyProperty WidthProperty { get; } =
            DependencyProperty.Register(
                nameof(Width),
                typeof(GridLength),
                typeof(PrintableDataGridColumn)
            );

        public GridLength Width
        {
            get { return (GridLength)GetValue(WidthProperty); }
            set { SetValue(WidthProperty, value); }
        }
        #endregion

        #region Header
        public static DependencyProperty HeaderProperty { get; } =
            DependencyProperty.Register(
                nameof(Header),
                typeof(object),
                typeof(PrintableDataGridColumn)
            );

        public object Header
        {
            get { return GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }
        #endregion

        #region HeaderTemplate
        public static DependencyProperty HeaderTemplateProperty { get; } =
            DependencyProperty.Register(
                nameof(HeaderTemplate),
                typeof(DataTemplate),
                typeof(PrintableDataGridColumn)
            );

        public DataTemplate HeaderTemplate
        {
            get { return (DataTemplate)GetValue(HeaderTemplateProperty); }
            set { SetValue(HeaderTemplateProperty, value); }
        }
        #endregion

        #region HeaderTemplateSelector
        public static DependencyProperty HeaderTemplateSelectorProperty { get; } =
            DependencyProperty.Register(
                nameof(HeaderTemplateSelector),
                typeof(DataTemplateSelector),
                typeof(PrintableDataGridColumn)
            );

        public DataTemplateSelector HeaderTemplateSelector
        {
            get { return (DataTemplateSelector)GetValue(HeaderTemplateSelectorProperty); }
            set { SetValue(HeaderTemplateSelectorProperty, value); }
        }
        #endregion

        #region CellTemplate
        public static DependencyProperty CellTemplateProperty { get; } =
            DependencyProperty.Register(
                nameof(CellTemplate),
                typeof(DataTemplate),
                typeof(PrintableDataGridColumn)
            );

        public DataTemplate CellTemplate
        {
            get { return (DataTemplate)GetValue(CellTemplateProperty); }
            set { SetValue(CellTemplateProperty, value); }
        }
        #endregion

        #region CellTemplateSelector
        public static DependencyProperty CellTemplateSelectorProperty { get; } =
            DependencyProperty.Register(
                nameof(CellTemplateSelector),
                typeof(DataTemplateSelector),
                typeof(PrintableDataGridColumn)
            );

        public DataTemplateSelector CellTemplateSelector
        {
            get { return (DataTemplateSelector)GetValue(CellTemplateSelectorProperty); }
            set { SetValue(CellTemplateSelectorProperty, value); }
        }
        #endregion

        #region CellBinding
        static Binding cellBindingDefault = new Binding();

        Binding cellBinding;

        /// <summary>
        /// Gets or sets the binding expression for the cell content.
        /// <c>{Binding}</c> by default. Never returns <c>null</c>.
        /// </summary>
        public Binding CellBinding
        {
            get { return cellBinding ?? cellBindingDefault; }
            set { cellBinding = value; }
        }
        #endregion

        public Style CellStyle { get; set; }
    }
}

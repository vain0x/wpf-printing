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
    /// <summary>
    /// Represents a column of <see cref="PrintableDataGrid"/>.
    /// </summary>
    public class PrintableDataGridColumn
        : DependencyObject
    {
        #region Width
        /// <summary>
        /// Gets the dependency property of <see cref="Width"/>.
        /// </summary>
        public static readonly DependencyProperty WidthProperty =
            DependencyProperty.Register(
                nameof(Width),
                typeof(GridLength),
                typeof(PrintableDataGridColumn)
            );

        /// <summary>
        /// Gets or sets the width of this column.
        /// </summary>
        public GridLength Width
        {
            get { return (GridLength)GetValue(WidthProperty); }
            set { SetValue(WidthProperty, value); }
        }
        #endregion

        #region Header
        /// <summary>
        /// Gets the dependency property of <see cref="Header"/>.
        /// </summary>
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register(
                nameof(Header),
                typeof(object),
                typeof(PrintableDataGridColumn)
            );

        /// <summary>
        /// Gets or sets the header content for this column.
        /// </summary>
        public object Header
        {
            get { return GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }
        #endregion

        #region HeaderTemplate
        /// <summary>
        /// Gets the dependency property of <see cref="HeaderTemplate"/>.
        /// </summary>
        public static readonly DependencyProperty HeaderTemplateProperty =
            DependencyProperty.Register(
                nameof(HeaderTemplate),
                typeof(DataTemplate),
                typeof(PrintableDataGridColumn)
            );

        /// <summary>
        /// Gets or sets the data template for this column's header.
        /// </summary>
        public DataTemplate HeaderTemplate
        {
            get { return (DataTemplate)GetValue(HeaderTemplateProperty); }
            set { SetValue(HeaderTemplateProperty, value); }
        }
        #endregion

        #region HeaderTemplateSelector
        /// <summary>
        /// Gets the dependency property of <see cref="HeaderTemplateSelector"/>.
        /// </summary>
        public static readonly DependencyProperty HeaderTemplateSelectorProperty =
            DependencyProperty.Register(
                nameof(HeaderTemplateSelector),
                typeof(DataTemplateSelector),
                typeof(PrintableDataGridColumn)
            );

        /// <summary>
        /// Gets or sets the data template selector for this column's header.
        /// </summary>
        public DataTemplateSelector HeaderTemplateSelector
        {
            get { return (DataTemplateSelector)GetValue(HeaderTemplateSelectorProperty); }
            set { SetValue(HeaderTemplateSelectorProperty, value); }
        }
        #endregion

        #region CellTemplate
        /// <summary>
        /// Gets the dependency property of <see cref="CellTemplate"/>.
        /// </summary>
        public static readonly DependencyProperty CellTemplateProperty =
            DependencyProperty.Register(
                nameof(CellTemplate),
                typeof(DataTemplate),
                typeof(PrintableDataGridColumn)
            );

        /// <summary>
        /// Gets or sets the data template for this column's cells.
        /// </summary>
        public DataTemplate CellTemplate
        {
            get { return (DataTemplate)GetValue(CellTemplateProperty); }
            set { SetValue(CellTemplateProperty, value); }
        }
        #endregion

        #region CellTemplateSelector
        /// <summary>
        /// Gets the dependency property of <see cref="CellTemplateSelector"/>.
        /// </summary>
        public static readonly DependencyProperty CellTemplateSelectorProperty =
            DependencyProperty.Register(
                nameof(CellTemplateSelector),
                typeof(DataTemplateSelector),
                typeof(PrintableDataGridColumn)
            );

        /// <summary>
        /// Gets or sets the data template selector for this column's cells.
        /// </summary>
        public DataTemplateSelector CellTemplateSelector
        {
            get { return (DataTemplateSelector)GetValue(CellTemplateSelectorProperty); }
            set { SetValue(CellTemplateSelectorProperty, value); }
        }
        #endregion

        #region CellBinding
        static readonly Binding cellBindingDefault = new Binding();

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

        /// <summary>
        /// Gets or sets the style for cells.
        /// </summary>
        public Style CellStyle { get; set; }
    }
}

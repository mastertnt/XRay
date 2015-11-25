using XTreeListView.DataTemplate;
using XTreeListView.Behaviors.Column;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace XTreeListView.Gui
{
    /// <summary>
    /// Class extending the <see cref="GridViewColumn"/> control.
    /// </summary>
    public class ExtendedGridViewColumn : GridViewColumn
    {
        #region Fields

        /// <summary>
        /// Stores the column width.
        /// </summary>
        private GridLength mWidth;

        #endregion // Fields.

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtendedGridViewColumn"/> class.
        /// </summary>
        private ExtendedGridViewColumn()
        {
        }

        #endregion // Constructors.

        #region Properties

        /// <summary>
        /// Gets or sets the column width.
        /// </summary>
        public new GridLength Width
        {
            get
            {
                return this.mWidth;
            }

            set
            {
                this.mWidth = value;
                if (this.mWidth.GridUnitType == GridUnitType.Pixel)
                {
                    RangeColumn.ApplyWidth(this, 0, this.mWidth.Value, double.MaxValue);
                }
                else if (this.mWidth.GridUnitType == GridUnitType.Star)
                {
                    ProportionalColumn.ApplyWidth(this, this.mWidth.Value);
                }
            }
        }

        #endregion // Properties.

        #region Methods

        /// <summary>
        /// Creates a GUI column from the given properties.
        /// </summary>
        /// <param name="pColumn">The column properties.</param>
        /// <param name="pIndex">The column index.</param>
        /// <returns>The created column.</returns>
        public static ExtendedGridViewColumn CreateFrom(TreeListViewColumn pColumn, int pIndex)
        {
            ExtendedGridViewColumn lColumn = new ExtendedGridViewColumn();

            // Header.
            lColumn.Header = pColumn.Header;

            // Width.
            GridLength lWidth = new GridLength();
            if (pColumn.Stretch)
            {
                lWidth = new GridLength(pColumn.Width, GridUnitType.Star);
            }
            else
            {
                lWidth = new GridLength(pColumn.Width, GridUnitType.Pixel);
            }
            lColumn.Width = lWidth;

            // Template selector.
            if (pColumn.TemplateSelector != null)
            {
                lColumn.CellTemplateSelector = pColumn.TemplateSelector;
            }
            else
            {
                lColumn.CellTemplateSelector = new DefaultPresenterHierarchicalTemplateSelector(pIndex, pColumn.DataMemberBindingPath);
            }

            return lColumn;
        }

        /// <summary>
        /// Clone this column.
        /// </summary>
        /// <returns>The resulting column.</returns>
        public ExtendedGridViewColumn Clone()
        {
            ExtendedGridViewColumn lColumn = new ExtendedGridViewColumn();
            lColumn.Header = this.Header;
            lColumn.Width = this.Width;
            lColumn.CellTemplate = this.CellTemplate;
            lColumn.CellTemplateSelector = this.CellTemplateSelector;
            lColumn.DisplayMemberBinding = this.DisplayMemberBinding;

            return lColumn;
        }

        #endregion // Methods.
    }
}

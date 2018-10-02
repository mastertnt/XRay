using XTreeListView.Behaviors.Column;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Diagnostics;
using System.ComponentModel;
using System.Reflection;
using XTreeListView.Extensions;

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
        public new GridLength GridLength
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
            lColumn.GridLength = lWidth;

            // Template selector.
            if (pColumn.TemplateSelector != null)
            {
                lColumn.CellTemplateSelector = pColumn.TemplateSelector;
            }
            else
            {
                // Template.
                lColumn.CellTemplate = XTreeListView.Resources.All.Instance.GetCellTemplate(pColumn.DisplayMemberPath);
            }

            return lColumn;
        }

        /// <summary>
        /// Ensures final column desired width is no less than the given value.
        /// </summary>
        internal double EnsureDesiredWidth(double pWidth)
        {
            return this.CallMethod<GridViewColumn, double>("EnsureWidth", pWidth);
        }

        #endregion // Methods.

        #region Properties

        /// <summary>
        /// Gets or sets the rendering state of the column.
        /// </summary>
        internal ExtendedGridViewColumnMeasureState State
        {
            get
            {
                return this.GetPropertyValue<GridViewColumn, ExtendedGridViewColumnMeasureState>("State");
            }
            set
            {
                this.SetPropertyValue<GridViewColumn, int>("State", (int)value);
            }
        }

        /// <summary>
        /// Gets or sets the index of the column in the grid.
        /// </summary>
        /// <remarks>
        /// Perf optimization. To avoid re-search index again and again by every GridViewRowPresenter, add an index here.
        /// </remarks>
        internal int ActualIndex
        {
            get
            {
                return this.GetPropertyValue<GridViewColumn, int>("ActualIndex");
            }
        }

        /// <summary>
        /// Gets or sets the width of the column if the Width property is NaN.
        /// </summary>
        internal double DesiredWidth
        {
            get
            {
                return this.GetPropertyValue<GridViewColumn, double>("DesiredWidth");
            }
            set
            {
                this.SetPropertyValue<GridViewColumn, double>("DesiredWidth", value);
            }
        }

        #endregion // Properties.
    }
}

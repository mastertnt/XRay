using System;
using System.Windows;
using System.Windows.Controls;

namespace XTreeListView.Behaviors.Column
{
    /// <summary>
    /// Class describing a column with a size limited by a width range.
    /// </summary>
	public sealed class RangeColumn : ALayoutColumn
    {
        #region Dependencies

        /// <summary>
        /// Identifies the MinWidth attached property.
        /// </summary>
        public static readonly DependencyProperty MinWidthProperty = DependencyProperty.RegisterAttached("MinWidth", typeof(double), typeof(RangeColumn));

		/// <summary>
        /// Identifies the MaxWidth attached property.
		/// </summary>
		public static readonly DependencyProperty MaxWidthProperty = DependencyProperty.RegisterAttached("MaxWidth", typeof(double), typeof(RangeColumn));

        /// <summary>
        /// Identifies the IsFillColumn attached property.
        /// </summary>
		public static readonly DependencyProperty IsFillColumnProperty = DependencyProperty.RegisterAttached("IsFillColumn", typeof(bool), typeof(RangeColumn));

        #endregion // Dependencies.
        
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RangeColumn"/> class.
        /// </summary>
        private RangeColumn()
		{
		}

        #endregion // Constructors.

        #region Methods

        /// <summary>
        /// Gets the minimum width defined in the attached property.
        /// </summary>
        /// <param name="pObject">The object from witch to get the value.</param>
        /// <returns>The attached value.</returns>
        public static double GetMinWidth(DependencyObject pObject)
		{
            return (double)pObject.GetValue(MinWidthProperty);
		}

        /// <summary>
        /// Sets the minimum width in the attached property.
        /// </summary>
        /// <param name="pObject">The object in witch the value is set.</param>
        /// <param name="pMinWidth">The width to set.</param>
        public static void SetMinWidth(DependencyObject pObject, double pMinWidth)
		{
            pObject.SetValue(MinWidthProperty, pMinWidth);
		}

        /// <summary>
        /// Gets the maximum width defined in the attached property.
        /// </summary>
        /// <param name="pObject">The object from witch to get the value.</param>
        /// <returns>The attached value.</returns>
        public static double GetMaxWidth(DependencyObject pObject)
		{
            return (double)pObject.GetValue(MaxWidthProperty);
		}

        /// <summary>
        /// Sets the maximum width in the attached property.
        /// </summary>
        /// <param name="pObject">The object in witch the value is set.</param>
        /// <param name="pMaxWidth">The width to set.</param>
        public static void SetMaxWidth(DependencyObject pObject, double pMaxWidth)
		{
            pObject.SetValue(MaxWidthProperty, pMaxWidth);
		}

        /// <summary>
        /// Gets the fill flag defined in the attached property.
        /// </summary>
        /// <param name="pObject">The object from witch to get the value.</param>
        /// <returns>The attached value.</returns>
        public static bool GetIsFillColumn(DependencyObject pObject)
		{
            return (bool)pObject.GetValue(IsFillColumnProperty);
		}

        /// <summary>
        /// Sets the fill flag in the attached property.
        /// </summary>
        /// <param name="pObject">The object in witch the value is set.</param>
        /// <param name="pIsFillColumn">The width to set.</param>
        public static void SetIsFillColumn(DependencyObject pObject, bool pIsFillColumn)
		{
            pObject.SetValue(IsFillColumnProperty, pIsFillColumn);
		}

        /// <summary>
        /// Returns a flag to know if the given column is a range column.
        /// </summary>
        /// <param name="pColumn">The tested column.</param>
        /// <returns>True if the size has been defined in this column, false otherwise.</returns>
		public static bool IsRangeColumn(GridViewColumn pColumn)
		{
			if ( pColumn == null )
			{
				return false;
			}

			return (HasPropertyValue(pColumn, MinWidthProperty) || HasPropertyValue(pColumn, MaxWidthProperty) || HasPropertyValue(pColumn, IsFillColumnProperty));
		}

        /// <summary>
        /// Returns the minimum width for a given column if any.
        /// </summary>
        /// <param name="pColumn">The column to test.</param>
        /// <returns>The minimum width if defined, false otherwise.</returns>
		public static double? GetRangeMinWidth(GridViewColumn pColumn)
		{
			return GetColumnWidth(pColumn, MinWidthProperty);
		}

        /// <summary>
        /// Returns the maximum width for a given column if any.
        /// </summary>
        /// <param name="pColumn">The column to test.</param>
        /// <returns>The maximum width if defined, false otherwise.</returns>
		public static double? GetRangeMaxWidth(GridViewColumn pColumn)
		{
			return GetColumnWidth(pColumn, MaxWidthProperty);
		}

        /// <summary>
        /// Returns the fill flag for a given column if any.
        /// </summary>
        /// <param name="pColumn">The column to test.</param>
        /// <returns>The fill flag if defined, false otherwise.</returns>
		public static bool? GetRangeIsFillColumn(GridViewColumn pColumn)
		{
			if (pColumn == null)
			{
                throw new ArgumentNullException("pColumn");
			}

			object lValue = pColumn.ReadLocalValue(IsFillColumnProperty);
			if (lValue != null && lValue.GetType() == IsFillColumnProperty.PropertyType)
			{
				return (bool)lValue;
			}

			return null;
		}

        /// <summary>
        /// Applies the range width property on the given column.
        /// </summary>
        /// <param name="pColumn">The column in witch to apply the properties.</param>
        /// <param name="pMinWidth">The minimum width.</param>
        /// <param name="pWidth">The default width.</param>
        /// <param name="pMaxWidth">The maximum width.</param>
        /// <returns>The configured column.</returns>
        public static GridViewColumn ApplyWidth(GridViewColumn pColumn, double pMinWidth, double pWidth, double pMaxWidth)
		{
            return ApplyWidth(pColumn, pMinWidth, pWidth, pMaxWidth, false);
		}

        /// <summary>
        /// Applies the range width property on the given column.
        /// </summary>
        /// <param name="pColumn">The column in witch to apply the properties.</param>
        /// <param name="pMinWidth">The minimum width.</param>
        /// <param name="pWidth">The default width.</param>
        /// <param name="pMaxWidth">The maximum width.</param>
        /// <param name="pIsFillColumn">Flag defining if the column fill the remaining space.</param>
        /// <returns>The configured column.</returns>
        public static GridViewColumn ApplyWidth(GridViewColumn pColumn, double pMinWidth, double pWidth, double pMaxWidth, bool pIsFillColumn)
		{
            SetMinWidth(pColumn, pMinWidth);
            pColumn.Width = pWidth;
            SetMaxWidth(pColumn, pMaxWidth);
            SetIsFillColumn(pColumn, pIsFillColumn);
            return pColumn;
        }

        #endregion // Methods.
    }
}

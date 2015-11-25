using System.Windows;
using System.Windows.Controls;

namespace XTreeListView.Behaviors.Column
{
    /// <summary>
    /// Class describing a column with a proportional size.
    /// </summary>
	public sealed class ProportionalColumn : ALayoutColumn
	{
        #region Dependencies

        /// <summary>
        /// Identifies the Width attached property.
        /// </summary>
        public static readonly DependencyProperty WidthProperty = DependencyProperty.RegisterAttached("Width", typeof(double), typeof(ProportionalColumn));

        #endregion // Dependencies.

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ProportionalColumn"/> class.
        /// </summary>
        private ProportionalColumn()
		{
		}

        #endregion // Constructors.

        #region Methods

        /// <summary>
        /// Gets the width defined in the attached property.
        /// </summary>
        /// <param name="pObject">The object from witch to get the value.</param>
        /// <returns>The attached width value.</returns>
        public static double GetWidth(DependencyObject pObject)
        {
            return (double)pObject.GetValue(WidthProperty);
        }

        /// <summary>
        /// Sets the width in the attached property.
        /// </summary>
        /// <param name="pObject">The object in witch the value is set.</param>
        /// <param name="pWidth">The width to set.</param>
        public static void SetWidth(DependencyObject pObject, double pWidth)
        {
            pObject.SetValue(WidthProperty, pWidth);
        }

        /// <summary>
        /// Returns a flag to know if the given column is a proportional column.
        /// </summary>
        /// <param name="pColumn">The tested column.</param>
        /// <returns>True if the proportional size has been defined in this column, false otherwise.</returns>
        public static bool IsProportionalColumn(GridViewColumn pColumn)
        {
            if (pColumn == null)
            {
                return false;
            }

            return HasPropertyValue(pColumn, WidthProperty);
        }

        /// <summary>
        /// Returns the propertional width for a given column if any.
        /// </summary>
        /// <param name="pColumn">The column to test.</param>
        /// <returns>The proportional width if defined, false otherwise.</returns>
        public static double? GetProportionalWidth(GridViewColumn pColumn)
        {
            return GetColumnWidth(pColumn, WidthProperty);
        }

        /// <summary>
        /// Applies the proportional width property on the given column.
        /// </summary>
        /// <param name="pColumn">The column in witch to apply the properties.</param>
        /// <param name="pWidth">The proportional width.</param>
        /// <returns>The configured column.</returns>
        public static GridViewColumn ApplyWidth(GridViewColumn pColumn, double pWidth)
        {
            SetWidth(pColumn, pWidth);
            return pColumn;
        }

        #endregion // Methods.
	}
} 

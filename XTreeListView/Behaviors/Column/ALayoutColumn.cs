using System;
using System.Windows;
using System.Windows.Controls;

namespace XTreeListView.Behaviors.Column
{
    /// <summary>
    /// Class defining the base class for the column descriptors used by the layout manager.
    /// </summary>
    public abstract class ALayoutColumn
    {
        #region Methods

        /// <summary>
        /// Verifies if the given dependency property is attached to the given grid view column and if the property value is defined.
        /// </summary>
        /// <param name="pColumn">The tested column.</param>
        /// <param name="pProperty">The dependency property.</param>
        /// <returns>True if the dependency property is attached to the given grid view column and if the property value is defined, false otherwise.</returns>
        protected static bool HasPropertyValue(GridViewColumn pColumn, DependencyProperty pProperty)
        {
            if (pColumn == null)
            {
                throw new ArgumentNullException("pColumn");
            }

            object lValue = pColumn.ReadLocalValue(pProperty);
            if (lValue != null && lValue.GetType() == pProperty.PropertyType)
            {
                return true;
            }

            return false;
        } 

        /// <summary>
        /// Gets the column width if the dependency property is attached to the column.
        /// </summary>
        /// <param name="pColumn">The tested column.</param>
        /// <param name="pProperty">The width dependency property.</param>
        /// <returns>The width value if the width has been defined using the given dependency property.</returns>
        protected static double? GetColumnWidth(GridViewColumn pColumn, DependencyProperty pProperty)
        {
            if (ALayoutColumn.HasPropertyValue(pColumn, pProperty))
            {
                return System.Convert.ToDouble(pColumn.ReadLocalValue(pProperty));
            }

            return null;
        }

        #endregion // Methods.
    }
}

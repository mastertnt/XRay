using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using XTreeListView.Gui;

namespace XTreeListView.Behaviors
{
    /// <summary>
    /// Class defined to know if the parent tree of an item is multi column or not.
    /// </summary>
    internal static class IsParentTreeMultiColumnsBehavior
    {
        #region Dependencies

        /// <summary>
        /// Identifies the ColumnsCollection attached dependency property.
        /// </summary>
        public static readonly DependencyProperty IsParentTreeMultiColumnsProperty = DependencyProperty.RegisterAttached("IsParentTreeMultiColumns", typeof(bool), typeof(IsParentTreeMultiColumnsBehavior), new FrameworkPropertyMetadata(false));

        #endregion // Dependencies.

        #region Methods

        /// <summary>
        /// Sets the flag indicating if the parent tree of an item is multi column or not.
        /// </summary>
        /// <param name="pItem">The item of interest.</param>
        /// <param name="pValue">The new flag.</param>
        public static void SetIsParentTreeMultiColumns(TreeListViewItem pItem, bool pValue)
        {
            pItem.SetValue(IsParentTreeMultiColumnsProperty, pValue);
        }

        /// <summary>
        /// Returns the flag indicating if the parent tree of an item is multi column or not.
        /// </summary>
        /// <param name="pItem">The item of interest.</param>
        /// <returns>The flag.</returns>
        public static bool GetIsParentTreeMultiColumns(TreeListViewItem pItem)
        {
            return (bool)pItem.GetValue(IsParentTreeMultiColumnsProperty);
        }

        #endregion // Methods.
    }
}

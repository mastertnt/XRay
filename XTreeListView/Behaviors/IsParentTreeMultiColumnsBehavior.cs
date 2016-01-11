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
    public static class IsParentTreeMultiColumnsBehavior
    {
        /// <summary>
        /// Identifies the ColumnsCollection attached dependency property.
        /// </summary>
        public static readonly DependencyProperty IsParentTreeMultiColumnsProperty = DependencyProperty.RegisterAttached("IsParentTreeMultiColumns", typeof(bool), typeof(IsParentTreeMultiColumnsBehavior), new FrameworkPropertyMetadata(false));

        /// <summary>
        /// Sets the flag indicating if the parent tree of an item is multi column or not.
        /// </summary>
        /// <param name="pElement">The modified element.</param>
        /// <param name="pValue">The new flag.</param>
        public static void SetIsParentTreeMultiColumns(UIElement pElement, bool pValue)
        {
            pElement.SetValue(IsParentTreeMultiColumnsProperty, pValue);
        }

        /// <summary>
        /// Returns the flag indicating if the parent tree of an item is multi column or not.
        /// </summary>
        /// <param name="pElement">The framework element.</param>
        /// <returns>The flag.</returns>
        public static TreeListViewColumnCollection GetIsParentTreeMultiColumns(UIElement pElement)
        {
            return (TreeListViewColumnCollection)pElement.GetValue(IsParentTreeMultiColumnsProperty);
        }
    }
}

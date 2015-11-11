using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace XGraph.Extensions
{
    static class DependencyObjectExtensions
    {
        /// <summary>
        /// Finds a parent of a given item on the visual tree.
        /// </summary>
        /// <typeparam name="T">The type of the queried item.</typeparam>
        /// <param name="pThis">A direct or indirect child of the queried item.</param>
        /// <returns>The first parent item that matches the submitted type parameter. 
        /// If not matching item can be found, a null reference is being returned.</returns>
        public static T FindVisualParent<T>(this DependencyObject pThis) where T : DependencyObject
        {
            // get parent item
            DependencyObject lParentObject = VisualTreeHelper.GetParent(pThis);

            // we’ve reached the end of the tree
            if (lParentObject == null)
            {
                return null;
            }

            // check if the parent matches the type we’re looking for
            T lParent = lParentObject as T;
            if (lParent != null)
            {
                return lParent;
            }
            else
            {
                // use recursion to proceed with next level
                return lParentObject.FindVisualParent<T>();
            }
        }
    }
}

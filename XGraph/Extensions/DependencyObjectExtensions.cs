using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace XGraph.Extensions
{
    /// <summary>
    /// Class extanding the <see cref="DependencyObject"/> class.
    /// </summary>
    public static class DependencyObjectExtensions
    {
        #region Methods

        /// <summary>
        /// Finds a parent of a given item on the visual tree and on the adorner visual tree if possible.
        /// </summary>
        /// <typeparam name="T">The type of the queried item.</typeparam>
        /// <param name="pThis">A direct or indirect child of the queried item.</param>
        /// <returns>The first parent item that matches the submitted type parameter. 
        /// If not matching item can be found, a null reference is being returned.</returns>
        public static T FindVisualParent<T>(this DependencyObject pThis) where T : DependencyObject
        {
            T lParentObject = pThis.InnerFindVisualParent<T>();
            if (lParentObject == null)
            {
                Adorner lAdornerParent = pThis.InnerFindVisualParent<Adorner>();
                if (lAdornerParent != null)
                {
                    return lAdornerParent.AdornedElement.FindVisualParent<T>();
                }
            }

            return lParentObject;
        }

        /// <summary>
        /// Finds a parent of a given item on the visual tree.
        /// </summary>
        /// <typeparam name="T">The type of the queried item.</typeparam>
        /// <param name="pThis">A direct or indirect child of the queried item.</param>
        /// <returns>The first parent item that matches the submitted type parameter. 
        /// If not matching item can be found, a null reference is being returned.</returns>
        private static T InnerFindVisualParent<T>(this DependencyObject pThis) where T : DependencyObject
        {
            // Get parent item.
            DependencyObject lParentObject = VisualTreeHelper.GetParent(pThis);

            // We’ve reached the end of the tree.
            if (lParentObject == null)
            {
                return null;
            }

            // Checks if the parent matches the type we’re looking for.
            T lParent = lParentObject as T;
            if (lParent != null)
            {
                return lParent;
            }
            else
            {
                // Use recursion to proceed with next level.
                return lParentObject.InnerFindVisualParent<T>();
            }
        }

        #endregion // Methods.
    }
}

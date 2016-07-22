using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls.Primitives;

namespace XControls.Core.Utilities
{
    /// <summary>
    /// Class defining a visual tree helper.
    /// </summary>
    internal static class TreeHelper
    {
        #region Methods

        /// <summary>
        /// Tries its best to return the specified element's parent. It will 
        /// try to find, in this order, the VisualParent, LogicalParent, LogicalTemplatedParent.
        /// It only works for Visual, FrameworkElement or FrameworkContentElement.
        /// </summary>
        /// <param name="pElement">The element to which to return the parent. It will only 
        /// work if element is a Visual, a FrameworkElement or a FrameworkContentElement.</param>
        /// <remarks>If the logical parent is not found (Parent), we check the TemplatedParent
        /// (see FrameworkElement.Parent documentation). But, we never actually witnessed
        /// this situation.</remarks>
        public static DependencyObject GetParent(DependencyObject pElement)
        {
            return TreeHelper.GetParent(pElement, true);
        }

        /// <summary>
        /// Tries its best to return the specified element's parent. It will 
        /// try to find, in this order, the VisualParent, LogicalParent, LogicalTemplatedParent.
        /// It only works for Visual, FrameworkElement or FrameworkContentElement.
        /// </summary>
        /// <param name="pElement">The element to which to return the parent. It will only work if element is a Visual, a FrameworkElement or a FrameworkContentElement.</param>
        /// <param name="pRecurseIntoPopup">Flag to know if the research mus be done in popup to.</param>
        /// <remarks>If the logical parent is not found (Parent), we check the TemplatedParent
        /// (see FrameworkElement.Parent documentation). But, we never actually witnessed
        /// this situation.</remarks>
        private static DependencyObject GetParent(DependencyObject pElement, bool pRecurseIntoPopup)
        {
            if (pRecurseIntoPopup)
            {
                // To correctly detect parent of a popup we must do that exception case.
                Popup lPopup = pElement as Popup;

                if ((lPopup != null) && (lPopup.PlacementTarget != null))
                {
                    return lPopup.PlacementTarget;
                }
            }

            Visual lVisual = pElement as Visual;
            DependencyObject lParent = (lVisual == null) ? null : VisualTreeHelper.GetParent(lVisual);

            if (lParent == null)
            {
                // No Visual parent. Check in the logical tree.
                FrameworkElement lFrameworkElt = pElement as FrameworkElement;

                if( lFrameworkElt != null )
                {
                    lParent = lFrameworkElt.Parent;
                    if( lParent == null )
                    {
                        lParent = lFrameworkElt.TemplatedParent;
                    }
                }
                else
                {
                    FrameworkContentElement lFrameworkContentElt = pElement as FrameworkContentElement;

                    if( lFrameworkContentElt != null )
                    {
                        lParent = lFrameworkContentElt.Parent;
                        if( lParent == null )
                        {
                            lParent = lFrameworkContentElt.TemplatedParent;
                        }
                    }
                }
            }

            return lParent;
        }

        /// <summary>
        /// This will search for a parent of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of the element to find</typeparam>
        /// <param name="pStartingObject">The node where the search begins. This element is not checked.</param>
        /// <returns>Returns the found element. Null if nothing is found.</returns>
        public static T FindParent<T>(DependencyObject pStartingObject) where T : DependencyObject
        {
            return TreeHelper.FindParent<T>(pStartingObject, false, null);
        }

        /// <summary>
        /// This will search for a parent of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of the element to find</typeparam>
        /// <param name="pStartingObject">The node where the search begins.</param>
        /// <param name="pCheckStartingObject">Should the specified startingObject be checked first.</param>
        /// <returns>Returns the found element. Null if nothing is found.</returns>
        public static T FindParent<T>(DependencyObject pStartingObject, bool pCheckStartingObject) where T : DependencyObject
        {
            return TreeHelper.FindParent<T>(pStartingObject, pCheckStartingObject, null);
        }

        /// <summary>
        /// This will search for a parent of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of the element to find</typeparam>
        /// <param name="pStartingObject">The node where the search begins.</param>
        /// <param name="pCheckStartingObject">Should the specified startingObject be checked first.</param>
        /// <param name="pAdditionalCheck">Provide a callback to check additional properties 
        /// of the found elements. Can be left Null if no additional criteria are needed.</param>
        /// <returns>Returns the found element. Null if nothing is found.</returns>
        /// <example>Button button = TreeHelper.FindParent&lt;Button&gt;( this, foundChild => foundChild.Focusable );</example>
        public static T FindParent<T>(DependencyObject pStartingObject, bool pCheckStartingObject, Func<T, bool> pAdditionalCheck) where T : DependencyObject
        {
            T lFoundElement;
            DependencyObject lParent = (pCheckStartingObject ? pStartingObject : TreeHelper.GetParent(pStartingObject, true));

            while (lParent != null)
            {
                lFoundElement = lParent as T;
                if (lFoundElement != null)
                {
                    if (pAdditionalCheck == null)
                    {
                        return lFoundElement;
                    }
                    else
                    {
                        if (pAdditionalCheck(lFoundElement))
                        {
                            return lFoundElement;
                        }
                    }
                }

                lParent = TreeHelper.GetParent(lParent, true);
            }

            return null;
        }

        /// <summary>
        /// This will search for a child of the specified type. The search is performed 
        /// hierarchically, breadth first (as opposed to depth first).
        /// </summary>
        /// <typeparam name="T">The type of the element to find</typeparam>
        /// <param name="pParent">The root of the tree to search for. This element itself is not checked.</param>
        /// <returns>Returns the found element. Null if nothing is found.</returns>
        public static T FindChild<T>(DependencyObject pParent) where T : DependencyObject
        {
            return TreeHelper.FindChild<T>( pParent, null );
        }

        /// <summary>
        /// This will search for a child of the specified type. The search is performed 
        /// hierarchically, breadth first (as opposed to depth first).
        /// </summary>
        /// <typeparam name="T">The type of the element to find</typeparam>
        /// <param name="pParent">The root of the tree to search for. This element itself is not checked.</param>
        /// <param name="pAdditionalCheck">Provide a callback to check additional properties 
        /// of the found elements. Can be left Null if no additional criteria are needed.</param>
        /// <returns>Returns the found element. Null if nothing is found.</returns>
        /// <example>Button button = TreeHelper.FindChild&lt;Button&gt;( this, foundChild => foundChild.Focusable );</example>
        public static T FindChild<T>(DependencyObject pParent, Func<T, bool> pAdditionalCheck) where T : DependencyObject
        {
            int lChildrenCount = VisualTreeHelper.GetChildrenCount(pParent);
            T lChild;

            for (int lIndex = 0; lIndex < lChildrenCount; lIndex++)
            {
                lChild = VisualTreeHelper.GetChild(pParent, lIndex) as T;
                if (lChild != null)
                {
                    if (pAdditionalCheck == null)
                    {
                        return lChild;
                    }
                    else
                    {
                        if (pAdditionalCheck(lChild))
                        {
                            return lChild;
                        }
                    }
                }
            }

            for (int lIndex = 0; lIndex < lChildrenCount; lIndex++)
            {
                lChild = TreeHelper.FindChild<T>(VisualTreeHelper.GetChild(pParent, lIndex), pAdditionalCheck);
                if (lChild != null)
                {
                    return lChild;
                }
            }

            return null;
        }

        /// <summary>
        /// Returns true if the specified element is a child of parent somewhere in the visual 
        /// tree. This method will work for Visual, FrameworkElement and FrameworkContentElement.
        /// </summary>
        /// <param name="pElement">The element that is potentially a child of the specified parent.</param>
        /// <param name="pParent">The element that is potentially a parent of the specified element.</param>
        public static bool IsDescendantOf(DependencyObject pElement, DependencyObject pParent)
        {
            return TreeHelper.IsDescendantOf(pElement, pParent, true);
        }

        /// <summary>
        /// Returns true if the specified element is a child of parent somewhere in the visual 
        /// tree. This method will work for Visual, FrameworkElement and FrameworkContentElement.
        /// </summary>
        /// <param name="pElement">The element that is potentially a child of the specified parent.</param>
        /// <param name="pParent">The element that is potentially a parent of the specified element.</param>
        public static bool IsDescendantOf(DependencyObject pElement, DependencyObject pParent, bool recurseIntoPopup)
        {
            while (pElement != null)
            {
                if (pElement == pParent)
                {
                    return true;
                }

                pElement = TreeHelper.GetParent(pElement, recurseIntoPopup);
            }

            return false;
        }

        #endregion // Methods.
    }
}

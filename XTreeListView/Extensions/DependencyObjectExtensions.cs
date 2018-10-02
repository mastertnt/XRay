using System.Windows.Media;
using System.Windows;

namespace XTreeListView.Extensions
{
    /// <summary>
    /// Class extending the <see cref="System.Windows.DependencyObject"/> class.
    /// </summary>
    public static class DependencyObjectExtensions
    {
        #region Methods

        /// <summary>
        /// This function is used to look for an ancestor of a given type defined in a DataTemplate in a XAML.
        /// </summary>
        /// <typeparam name="TAncestor">The type of the object requested.</typeparam>
        /// <param name="pDependencyObject">The dependency object.</param>
        /// <returns>The object retrieved if any, NULL otherwise.</returns>
        public static TAncestor FindVisualParent<TAncestor>(this DependencyObject pDependencyObject) 
            where TAncestor : class
        {
            DependencyObject lTarget = pDependencyObject;
            do
            {
                lTarget = VisualTreeHelper.GetParent(lTarget);
            }
            while (lTarget != null && (lTarget is TAncestor) == false);

            return lTarget as TAncestor;
        }

        #endregion // Methods.
    }
}

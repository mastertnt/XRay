using System.Windows;
using System.Windows.Controls;
using XGraph.ViewModels;

namespace XGraph.Controls
{
    /// <summary>
    /// Graph view is a list box : it then already handle the selection ... etc ...
    /// </summary>
    public class GraphView : ListBox
    {
        #region Constructors

        /// <summary>
        /// Initializes the <see cref="GraphView"/> class.
        /// </summary>
        static GraphView()
        {
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(GraphView), new FrameworkPropertyMetadata(typeof(GraphView)));
        }

        #endregion // Constructors.

        #region Methods

        /// <summary>
        /// Creates or identifies the element used to display a specified item.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Windows.Controls.ListBoxItem" />.
        /// </returns>
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new GraphItem();
        }

        /// <summary>
        /// Returns the node view containing the given view model.
        /// </summary>
        /// <param name="pItem">The item contained by the view.</param>
        /// <returns>The found view if any, null otherwise.</returns>
        public TContainer GetContainerForViewModel<TViewModel, TContainer>(TViewModel pItem) where TViewModel : IGraphItemViewModel where TContainer : FrameworkElement
        {
            GraphItem lItemView = this.ItemContainerGenerator.ContainerFromItem(pItem) as GraphItem;
            if (lItemView != null)
            {
                return lItemView.TemplateControl as TContainer;
            }

            return null;
        }

        #endregion // Methods
    }
}

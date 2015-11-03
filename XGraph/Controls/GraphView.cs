using System.Windows;
using System.Windows.Controls;

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
            GraphView.DefaultStyleKeyProperty.OverrideMetadata(typeof(GraphView), new FrameworkPropertyMetadata(typeof(GraphView)));
        }

        #endregion // Constructors.

        #region Methods

        /// <summary>
        /// Creates or identifies the element used to display a specified item.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Windows.Controls.ListBoxItem" />.
        /// </returns>
        protected override System.Windows.DependencyObject GetContainerForItemOverride()
        {
            return new NodeView();
        }

        #endregion // Methods.
    }
}

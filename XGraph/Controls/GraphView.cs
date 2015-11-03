using System.Windows.Controls;

namespace XGraph.Controls
{
    /// <summary>
    /// Graph view is a list box : it then already handle the selection ... etc ...
    /// </summary>
    public class GraphView : ListBox
    {
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
    }
}

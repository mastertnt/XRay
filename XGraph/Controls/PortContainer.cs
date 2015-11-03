using System.Windows.Controls;

namespace XGraph.Controls
{
    /// <summary>
    /// This class stores all the container.
    /// </summary>
    public class PortContainer : ItemsControl
    {
        /// <summary>
        /// Creates or identifies the element that is used to display the given item.
        /// </summary>
        /// <returns>
        /// The element that is used to display the given item.
        /// </returns>
        protected override System.Windows.DependencyObject GetContainerForItemOverride()
        {
            return new PortView();
        }
    }
}

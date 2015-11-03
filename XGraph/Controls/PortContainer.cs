using System.Windows;
using System.Windows.Controls;

namespace XGraph.Controls
{
    /// <summary>
    /// This class stores all the ports.
    /// </summary>
    public class PortContainer : ItemsControl
    {
        #region Constructors

        /// <summary>
        /// Initializes the <see cref="PortContainer"/> class.
        /// </summary>
        static PortContainer()
        {
            PortContainer.DefaultStyleKeyProperty.OverrideMetadata(typeof(PortContainer), new FrameworkPropertyMetadata(typeof(PortContainer)));
        }

        #endregion // Constructors.

        #region Methods

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

        #endregion // Methods.
    }
}

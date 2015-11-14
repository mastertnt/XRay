using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

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
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(PortContainer), new FrameworkPropertyMetadata(typeof(PortContainer)));
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

        /// <summary>
        /// Prespares the container for the given item.
        /// </summary>
        /// <param name="pElement">The item container.</param>
        /// <param name="pItem">The contained item.</param>
        protected override void PrepareContainerForItemOverride(DependencyObject pElement, object pItem)
        {
            base.PrepareContainerForItemOverride(pElement, pItem);

            PortView lContainer = pElement as PortView;
            if (lContainer != null)
            {
                // Binding the background.
                lContainer.Background = this.Background;
            }
        }

        #endregion // Methods.
    }
}

using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using XGraph.ViewModels;

namespace XGraph.Controls
{
    /// <summary>
    /// Class representing a port.
    /// </summary>
    public class PortView : ContentControl
    {
        #region Dependencies

        /// <summary>
        /// Identifies the Direction dependency property.
        /// </summary>
        public static readonly DependencyProperty DirectionProperty = DependencyProperty.Register("Direction", typeof(PortDirection), typeof(PortView), new FrameworkPropertyMetadata(PortDirection.Output));

        #endregion // Dependencies.

        #region Constructors

        /// <summary>
        /// Initializes the <see cref="PortContainer"/> class.
        /// </summary>
        static PortView()
        {
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(PortView), new FrameworkPropertyMetadata(typeof(PortView)));
        }

        #endregion // Constructors.

        #region Properties

        /// <summary>
        /// Gets or sets direction.
        /// </summary>
        public PortDirection Direction
        {
            get
            {
                return (PortDirection)this.GetValue(DirectionProperty);
            }
            set
            {
                this.SetValue(DirectionProperty, value);
            }
        }

        #endregion // Properties.

        #region Methods

        /// <summary>
        /// Method called when the control content changed.
        /// </summary>
        /// <param name="pOldContent">The previous content.</param>
        /// <param name="pNewContent">The new content.</param>
        protected override void OnContentChanged(object pOldContent, object pNewContent)
        {
            base.OnContentChanged(pOldContent, pNewContent);

            BindingOperations.ClearAllBindings(this);

            // The content is the view model.
            PortViewModel lNewContent = pNewContent as PortViewModel;
            if (lNewContent != null)
            {
                // Stting the content data template.
                this.ContentTemplate = lNewContent.DataTemplate;

                // Binding the direction.
                Binding lDirectionBinding = new Binding("Direction") { Source = lNewContent, Mode = BindingMode.TwoWay };
                this.SetBinding(PortView.DirectionProperty, lDirectionBinding);
            }
        }

        #endregion // Methods.
    }
}

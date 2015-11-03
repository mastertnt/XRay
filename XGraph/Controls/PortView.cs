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
        #region Constructors

        /// <summary>
        /// Initializes the <see cref="PortContainer"/> class.
        /// </summary>
        static PortView()
        {
            PortView.DefaultStyleKeyProperty.OverrideMetadata(typeof(PortView), new FrameworkPropertyMetadata(typeof(PortView)));
        }

        #endregion // Constructors.

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
            }
        }

        #endregion // Methods.
    }
}

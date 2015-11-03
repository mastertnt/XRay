using System.Windows.Controls;
using System.Windows.Data;
using XGraph.ViewModels;

namespace XGraph.Controls
{
    public class PortView : ContentControl
    {
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
    }
}

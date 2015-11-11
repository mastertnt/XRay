using System.Windows;
using System.Windows.Controls;
using XGraph.ViewModels;

namespace XGraph.Templates
{   
    public class GraphDataTemplateSelector : DataTemplateSelector
    {
        private readonly DataTemplates mDataTemplates;

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphDataTemplateSelector"/> class.
        /// </summary>
        public GraphDataTemplateSelector()
        {
            this.mDataTemplates = new DataTemplates();
        }

        /// <summary>
        /// Selects the template.
        /// </summary>
        /// <param name="pItem">The item.</param>
        /// <param name="pContainer">The container.</param>
        /// <returns></returns>
        public override DataTemplate SelectTemplate(object pItem, DependencyObject pContainer)
        {
            if (pItem is ConnectionViewModel)
            {
                return this.mDataTemplates["ConnectionDataTemplate"] as DataTemplate;
            }

            return this.mDataTemplates["NodeViewTemplate2"] as DataTemplate;
        }
    }
}
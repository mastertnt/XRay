using System.Windows;
using System.Windows.Controls;

namespace XGraph.Templates
{   
    public class GraphDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate DefaultnDataTemplate { get; set; }
        public DataTemplate BooleanDataTemplate { get; set; }
        public DataTemplate EnumDataTemplate { get; set; }

        public override DataTemplate SelectTemplate(object pItem, DependencyObject pContainer)
        {
            DependencyPropertyInfo dpi = item as DependencyPropertyInfo;
            if (dpi.PropertyType == typeof(bool))
            {
                return BooleanDataTemplate;
            }
            if (dpi.PropertyType.IsEnum)
            {
                return EnumDataTemplate;
            }

            return DefaultnDataTemplate;
        }
    }
}
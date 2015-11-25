using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows;
using XTreeListView.ViewModel;
using XTreeListView.DataTemplate;
using System.Windows.Data;
using XSystem;
using XSystem.Collections;
using XTreeListView.Core;

namespace XTreeListView.DataTemplate
{
    /// <summary>
    /// This class is used to select a data template according to a view model.
    /// </summary>
    /// <typeparam name="TSelector">The type of the template selector to load.</typeparam>
    public class AHierarchicalTemplateSelector<TSelector> : DataTemplateSelector where TSelector : class, IHierarchicalTemplateSelector
    {
        #region Fields

        /// <summary>
        /// This field stores a cache to avoid lookup in all sub-data template selectors.
        /// </summary>
        private CacheDictionary<string, TSelector> mCachedSelectorTemplates = new CacheDictionary<string, TSelector>(100);

        /// <summary>
        /// This field stores a cache to avoid lookup in all sub-data template.
        /// </summary>
        private CacheDictionary<string, System.Windows.DataTemplate> mCachedDataTemplates = new CacheDictionary<string, System.Windows.DataTemplate>(100); 

        /// <summary>
        /// The list of Variable template setlector plugins
        /// </summary>
        private static List<TSelector> sTemplateSelectorPlugins;

        /// <summary>
        /// The index of the column owner.
        /// </summary>
        private readonly int mColumnIndex;

        /// <summary>
        /// Stores the default data member binding path.
        /// </summary>
        private readonly string mDefaultDataMemberBindingPath;

        #endregion // Fields.

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AHierarchicalTemplateSelector{TSelector}"/> class.
        /// </summary>
        /// <param name="pColumnIndex">The column index.</param>
        /// <param name="pDefaultDataMemberBindingPath">Defines the data member binding path used when no data template is found.</param>
        public AHierarchicalTemplateSelector(int pColumnIndex, string pDefaultDataMemberBindingPath)
        {
            this.mColumnIndex = pColumnIndex;
            this.mDefaultDataMemberBindingPath = pDefaultDataMemberBindingPath;
            if (sTemplateSelectorPlugins == null)
            {
                sTemplateSelectorPlugins = typeof(TSelector).CreateAll<TSelector>().ToList();
            }
        }

        #endregion // Constructors.

        #region Methods

        /// <summary>
        /// This function is used to select a data template according to an item.
        /// </summary>
        /// <param name="pItem">The item to select.</param>
        /// <param name="pContainer">The dependecy container.</param>
        /// <returns>The selected tempate</returns>
        public override System.Windows.DataTemplate SelectTemplate(object pItem, DependencyObject pContainer)
        {
            IHierarchicalItemViewModel lViewModel = pItem as IHierarchicalItemViewModel;
            if
                (   (lViewModel == null)
                ||  (lViewModel.IsDisposed)
                )
            {
                return null;
            }

            string lKey = pItem.GetType().Name + Constants.KEY_SEPARATOR + this.mColumnIndex;
            if (this.mCachedDataTemplates.ContainsKey(lKey))
            {
                if (this.mCachedDataTemplates[lKey] != null)
                {
                    return this.mCachedDataTemplates[lKey];
                }
            }
            if (this.mCachedSelectorTemplates.ContainsKey(lKey))
            {
                if (this.mCachedSelectorTemplates[lKey] != null)
                {
                    return this.mCachedSelectorTemplates[lKey].FindDataTemplate(lViewModel, this.mColumnIndex);
                }
            }

            // Look for a datatemplate in a plug-in
            System.Windows.DataTemplate lTemplate = null;
            foreach
                (TSelector lPlugin in sTemplateSelectorPlugins)
            {
                lTemplate = lPlugin.FindDataTemplate(lViewModel, this.mColumnIndex);
                if
                    (lTemplate != null)
                {
                    this.mCachedSelectorTemplates.Add(lKey, lPlugin);
                    return lTemplate;
                }
            }

            if (string.IsNullOrEmpty(this.mDefaultDataMemberBindingPath) == false)
            {
                FrameworkElementFactory lTextBlockFactory = new FrameworkElementFactory(typeof(System.Windows.Controls.TextBlock));
                Binding lDataMemberBinding = new Binding(this.mDefaultDataMemberBindingPath);
                lTextBlockFactory.SetBinding(System.Windows.Controls.TextBlock.TextProperty, lDataMemberBinding);
                System.Windows.DataTemplate lDataTemplate = new System.Windows.DataTemplate() { VisualTree = lTextBlockFactory };
                lDataTemplate.Seal();
                this.mCachedDataTemplates.Add(lKey, lDataTemplate);
                return lDataTemplate;
            }
            return lTemplate;
        }

        #endregion // Methods.
    }
}

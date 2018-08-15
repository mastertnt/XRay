using System;
using System.Windows;
using XTreeListView.ViewModel;

namespace XTreeListView.DataTemplate
{
    /// <summary>
    /// This interface defines a template selector for a tree view.
    /// </summary>
    public interface IHierarchicalTemplateSelector
    {
        #region Methods

        /// <summary>
        /// This method finds the data template according to a view model.
        /// </summary>
        /// <param name="pViewModel">The view model.</param>
        /// <param name="pColumnIndex">The column index</param>
        /// <returns>The retrieved data template, null otherwise.</returns>
        System.Windows.DataTemplate FindDataTemplate(IHierarchicalItemViewModel pViewModel, int pColumnIndex);

        #endregion // Methods.
    }
}

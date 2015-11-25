using System;

namespace XTreeListView.DataTemplate
{
    /// <summary>
    /// This class is used to select a data template according to a view model.
    /// </summary>
    /// <!-- NBY -->
    public class DefaultPresenterHierarchicalTemplateSelector : AHierarchicalTemplateSelector<IPresenterHierarchicalTemplateSelector>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultPresenterHierarchicalTemplateSelector"/> class.
        /// </summary>
        /// <param name="pColumnIndex">The column index.</param>
        /// <param name="pDefaultDataMemberBindingPath">Defines the data member binding path used when no data template is found.</param>
        public DefaultPresenterHierarchicalTemplateSelector(Int32 pColumnIndex, string pDefaultDataMemberBindingPath)
            : base(pColumnIndex, pDefaultDataMemberBindingPath)
        {
        }

        #endregion // Constructors.
    }
}

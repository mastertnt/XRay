
namespace XTreeListView.ViewModel.Generic
{
    /// <summary>
    /// This interface defines the generic root view model interface.
    /// </summary>
    /// <typeparam name="T">The type of the owned object.</typeparam>
    /// <!-- DPE -->
    public interface IRootHierarchicalItemViewModel<T> : IRootHierarchicalItemViewModel
    {
        #region Properties.

        /// <summary>
        /// Gets or sets the model associated to this view model.
        /// </summary>
        T Model { get; set; }

        #endregion // Properties.
    }
}


namespace XTreeListView.ViewModel.Generic
{
    /// <summary>
    /// This interface defines the generic root view model interface.
    /// </summary>
    /// <typeparam name="TModel">The type of the owned object.</typeparam>
    public interface IRootHierarchicalItemViewModel<TModel> : IRootHierarchicalItemViewModel
    {
        #region Properties

        /// <summary>
        /// Gets or sets the model associated to this view model.
        /// </summary>
        TModel Model 
        { 
            get; 
            set; 
        }

        #endregion // Properties.
    }
}

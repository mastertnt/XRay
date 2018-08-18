
namespace XTreeListView.ViewModel.Generic
{
    /// <summary>
    /// This interface defines the generic view model interface.
    /// </summary>
    /// <typeparam name="TModel">The type of the owned object.</typeparam>
    public interface IViewModel<TModel> : IViewModel
    {
        #region Properties

        /// <summary>
        /// Gets the owned object if any.
        /// </summary>
        TModel OwnedObject 
        { 
            get; 
        }

        #endregion // Properties.
    }
}

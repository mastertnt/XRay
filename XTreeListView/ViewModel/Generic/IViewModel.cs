
namespace XTreeListView.ViewModel.Generic
{
    /// <summary>
    /// This interface defines the generic view model interface.
    /// </summary>
    /// <typeparam name="T">The type of the owned object.</typeparam>
    /// <!-- DPE -->
    public interface IViewModel<T> : IViewModel
    {
        #region Properties

        /// <summary>
        /// Gets the owned object if any.
        /// </summary>
        T OwnedObject { get; }

        #endregion // Properties.
    }
}

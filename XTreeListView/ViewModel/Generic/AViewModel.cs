
namespace XTreeListView.ViewModel.Generic
{
    /// <summary>
    /// This class implements a generic view model.
    /// </summary>
    /// <!-- DPE -->
    public abstract class AViewModel<T> : AViewModel, IViewModel<T>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the AViewModel class.
        /// </summary>
        /// <param name="pOwnedObject">The owned object.</param>
        protected AViewModel(T pOwnedObject)
            : base(pOwnedObject)
        {
        }

        #endregion // Constructors.

        #region Properties

        /// <summary>
        /// Gets the owned object if any.
        /// </summary>
        public T OwnedObject
        {
            get
            {
                return (T)this.UntypedOwnedObject;
            }
        }

        #endregion // Properties.
    }
}

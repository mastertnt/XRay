
namespace XTreeListView.ViewModel.Generic
{
    /// <summary>
    /// This class implements a generic view model.
    /// </summary>
    /// <typeparam name="TModel">The type of the owned object.</typeparam>
    public abstract class AViewModel<TModel> : AViewModel, IViewModel<TModel>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the AViewModel class.
        /// </summary>
        /// <param name="pOwnedObject">The owned object.</param>
        protected AViewModel(TModel pOwnedObject)
            : base(pOwnedObject)
        {
        }

        #endregion // Constructors.

        #region Properties

        /// <summary>
        /// Gets the owned object if any.
        /// </summary>
        public TModel OwnedObject
        {
            get
            {
                return (TModel)this.UntypedOwnedObject;
            }
        }

        #endregion // Properties.
    }
}

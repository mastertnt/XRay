
namespace XTreeListView.ViewModel.Generic
{
    /// <summary>
    /// This class defines a tree list view item view model using genericity to explicitly define the type of the owned object.
    /// </summary>
    /// <typeparam name="TModel">The type of the owned object.</typeparam>
    public abstract class AHierarchicalItemViewModel<TModel> : AHierarchicalItemViewModel, IHierarchicalItemViewModel<TModel>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AHierarchicalItemViewModel"/> class.
        /// </summary>
        /// <param name="pOwnedObject">The owned object.</param>
        protected AHierarchicalItemViewModel(TModel pOwnedObject)
            : base(pOwnedObject)
        {
        }

        #endregion // Constructors.

        #region Properties

        /// <summary>
        /// Gets or sets the owned object.
        /// </summary>
        public TModel OwnedObject 
        {
            get
            {
                return (TModel)base.UntypedOwnedObject;
            }
        }

        #endregion // Properties.
    }
}

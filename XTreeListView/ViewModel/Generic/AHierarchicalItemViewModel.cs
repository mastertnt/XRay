
namespace XTreeListView.ViewModel.Generic
{
    /// <summary>
    /// This class defines a tree list view item view model using genericity to explicitly define the type of the owned object.
    /// </summary>
    /// <typeparam name="T">The type of the owned object.</typeparam>
    /// <!-- DPE -->
    public abstract class AHierarchicalItemViewModel<T> : AHierarchicalItemViewModel, IHierarchicalItemViewModel<T>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AHierarchicalItemViewModel"/> class.
        /// </summary>
        /// <param name="pOwnedObject">The owned object.</param>
        protected AHierarchicalItemViewModel(T pOwnedObject)
            : base(pOwnedObject)
        {
        }

        #endregion // Constructors.

        #region Properties

        /// <summary>
        /// Gets or sets the owned object.
        /// </summary>
        public T OwnedObject 
        {
            get
            {
                return (T)base.UntypedOwnedObject;
            }
        }

        #endregion // Properties.
    }
}

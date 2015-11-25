using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Media;

namespace XTreeListView.ViewModel.Generic
{
    /// <summary>
    /// This class defines the root of the view model to give to the tree list view.
    /// </summary>
    /// <!-- DPE -->
    public abstract class ARootHierarchicalItemViewModel<T> : AHierarchicalItemViewModel, IRootHierarchicalItemViewModel<T>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ARootHierarchicalItemViewModel{T}"/> class.
        /// </summary>
        /// TODO Edit XML Comment Template for #ctor
        protected ARootHierarchicalItemViewModel()
            : base(null)
        {
            this.ChildrenRegistered = true;
            this.IsExpanded = true;
        }

        #endregion // Constructors.

        #region Properties

        /// <summary>
        /// Gets or sets the model associated to this view model.
        /// </summary>
        public virtual T Model
        {
            get
            {
                return (T)this.UntypedOwnedObject;
            }

            set
            {
                this.UntypedOwnedObject = value;
                this.NotifyPropertyChanged("Model");
            }
        }

        /// <summary>
        /// Gets the view model.
        /// </summary>
        public IEnumerable<IHierarchicalItemViewModel> ViewModel
        {
            get
            {
                return this.Children;
            }
        }

        /// <summary>
        /// Gets the flag indicating if the items are loaded on demand.
        /// </summary>
        protected override bool LoadItemsOnDemand
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the icon to display in the item.
        /// </summary>
        public override ImageSource IconSource
        {
            get
            {
                return null;
            }
        }

        #endregion // Properties.

        #region Events

        /// <summary>
        /// Event called when some items are added.
        /// </summary>
        public event TreeViewEventHandler ItemViewModelsAdded;

        /// <summary>
        /// Event called when some items are removed.
        /// </summary>
        public event TreeViewEventHandler ItemViewModelsRemoved;

        /// <summary>
        /// Event called when some items are modified.
        /// </summary>
        public event TreeViewItemEventHander ItemViewModelModified;

        #endregion // Events.

        #region Methods

        /// <summary>
        /// Convert the item to the generic version.
        /// </summary>
        /// <typeparam name="U">The type of the owned object.</typeparam>
        /// <returns>The generic version of the item.</returns>
        IRootHierarchicalItemViewModel<U> IRootHierarchicalItemViewModel.ToGeneric<U>()
        {
            return (this as IRootHierarchicalItemViewModel<U>);
        }

        /// <summary>
        /// Method to call when new children are added to this view model.
        /// </summary>
        /// <param name="pChild">The child removed from the children list.</param>
        protected sealed override void NotifyChildAdded(IHierarchicalItemViewModel pChild)
        {
            if (this.ItemViewModelsAdded != null)
            {
                this.ItemViewModelsAdded(this, new IHierarchicalItemViewModel[] { pChild });
            }
        }

        /// <summary>
        /// Method to call when children are removed from this view model.
        /// </summary>
        /// <param name="pChild">The child added to the children list.</param>
        protected sealed override void NotifyChildRemoved(IHierarchicalItemViewModel pChild)
        {
            if (this.ItemViewModelsRemoved != null)
            {
                this.ItemViewModelsRemoved(this, new IHierarchicalItemViewModel[] { pChild });
            }
        }

        /// <summary>
        /// Delegate called when the properties of this view model is modified.
        /// </summary>
        /// <param name="pSender">The item view model event sender.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        protected sealed override void NotifyItemViewModelModified(object pSender, PropertyChangedEventArgs pEventArgs)
        {
            if (this.ItemViewModelModified != null)
            {
                this.ItemViewModelModified(pSender, pEventArgs);
            }
        }

        #endregion // Methods.
    }
}

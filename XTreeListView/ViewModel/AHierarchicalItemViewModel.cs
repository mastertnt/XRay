using XTreeListView.ViewModel.Generic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using XTreeListView.Core;

namespace XTreeListView.ViewModel
{
    /// <summary>
    /// Defines a delegate used to define a specific view model creation.
    /// </summary>
    /// <param name="pOwnedObject">The view model owned object.</param>
    public delegate AHierarchicalItemViewModel CreateChildViewModelDelegate(object pOwnedObject);

    /// <summary>
    /// Sets the order.
    /// </summary>
    public enum SorterOrder
    {
        /// <summary>
        /// Enumeration value indicating the items are sorted in increasing order.
        /// </summary>
        Ascending,

        /// <summary>
        /// Enumeration value indicating the items are sorted in decreasing order.
        /// </summary>
        Descending,

        /// <summary>
        /// Enumeration value indicating the items are unordered.
        /// </summary>
        Unsorted,
    }

    /// <summary>
    /// 
    /// </summary>
    public enum SortKey
    {
        /// <summary>
        /// The display string is considered as the sort key.
        /// </summary>
        DisplayString,

        /// <summary>
        /// Enumeration value indicating the sort key 
        /// </summary>
        ChildCount,
    }

    /// <summary>
    /// This class defines an abstract tree list view item view model.
    /// </summary>
    public abstract class AHierarchicalItemViewModel : AViewModel, IHierarchicalItemViewModel
    {
        #region Fields

        /// <summary>
        /// Stores the children of this item.
        /// </summary>
        private readonly ObservableCollection<AHierarchicalItemViewModel> mChildren;

        /// <summary>
        /// The parent view model.
        /// </summary>
        private AHierarchicalItemViewModel mParent;

        /// <summary>
        /// Stores the flag indicating if the item is expanded or not.
        /// </summary>
        private Boolean mIsExpanded;

        /// <summary>
        /// This field stores the flag to know if the item is selected.
        /// </summary>
        private Boolean mIsSelected;

        /// <summary>
        /// Gets the index of the node in its parent collection.
        /// </summary>
        private Int32 mIndex;

        /// <summary>
        /// Stores the decorator width (part where are displayed the expander, icon...).
        /// </summary>
        private Double mDecoratorWidth;

        /// <summary>
        /// This field stores the key selector.
        /// </summary>
        private Func<IHierarchicalItemViewModel, IHierarchicalItemViewModel, object> mComparerKeySelector;

        /// <summary>
        /// This field stores the key comparer.
        /// </summary>
        private IComparer<object> mComparer;

        /// <summary>
        /// Stores the bindings of property changed names between the mViewModel and the viewModel.
        /// e.g. with an entry ["Name", "DisplayString"] when the ViewModel (this) gets notified of the 
        /// mViewModel's "Name" property change, it fires a propertyChangedEvent whose name is "DisplayString".
        /// NOTE : It will only be used if mOwnedObject implements INotifyPropertyChanged.
        /// </summary> 
        private readonly Dictionary<INotifyCollectionChanged, CreateChildViewModelDelegate> mChildrenBinding;

        #endregion // Fields.

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AHierarchicalItemViewModel"/> class.
        /// </summary>
        /// <param name="pOwnedObject">The owned object.</param>
        internal AHierarchicalItemViewModel(object pOwnedObject)
            : base(pOwnedObject)
        {
            this.mIsExpanded = false;
            this.mIsSelected = false;
            this.ChildrenRegistered = false;
            this.IsInRegistrationMode = false;
            this.mIndex = -1;
            (this as IHierarchicalItemViewModel).ChildrenAreLoaded = false;
            this.PropertyChanged += this.NotifyItemViewModelModified;

            // The parent of the children (this) is set here. 
            // An item does not have any parent while it has not been added to a children list.
            this.mChildren = new ViewModelCollection(this);
            this.mChildren.CollectionChanged += this.OnChildrenCollectionChanged;

            // Initializing the dictionaries storing the binding between model and view model.
            this.mChildrenBinding = new Dictionary<INotifyCollectionChanged, CreateChildViewModelDelegate>();
        }

        #endregion // Constructors.

        #region Events

        /// <summary>
        /// Event raised when the children list is modified.
        /// </summary>
        public event NotifyCollectionChangedEventHandler ChildrenCollectionChanged;

        #endregion // Events.

        #region Properties

        /// <summary>
        /// Gets the index of the item in the parent list.
        /// </summary>
        public int Index
        {
            get
            {
                return this.mIndex;
            }
        }

        /// <summary>
        /// Gets or sets the parent of this object.
        /// Property defined to implement the IHierarchicalItemViewModel interface.
        /// </summary>
        IHierarchicalItemViewModel IHierarchicalItemViewModel.Parent
        {
            get
            {
                return this.Parent;
            }
        }

        /// <summary>
        /// Gets the root hierarchical view models.
        /// </summary>
        public AHierarchicalItemViewModel Root
        {
            get
            {
                AHierarchicalItemViewModel lRoot = this.Parent;
                if (lRoot == null)
                {
                    return null;
                }
                
                while (lRoot.Parent != null)
                {
                    lRoot = lRoot.Parent;
                }

                return lRoot;
            }
        }

        /// <summary>
        /// Gets or sets the parent of the current view model.
        /// </summary>
        public AHierarchicalItemViewModel Parent
        {
            get
            {
                return this.mParent;
            }

            private set
            {
                if (this.mParent != value)
                {
                    this.mParent = value;
                    this.NotifyPropertyChanged("Parent");
                }
            }
        }

        /// <summary>
        /// Gets or sets the parent of this object.
        /// Property defined to implement the IHierarchicalItemViewModel interface.
        /// </summary>
        IEnumerable<IHierarchicalItemViewModel> IHierarchicalItemViewModel.Children
        {
            get
            {
                return this.Children;
            }
        }

        /// <summary>
        /// Gets the children list.
        /// </summary>
        public IEnumerable<AHierarchicalItemViewModel> Children
        {
            get
            {
                return this.mChildren;
            }
        }

        /// <summary>
        /// Gets the flag indicating if the item has children.
        /// </summary>
        public bool HasChildren
        {
            get
            {
                if (this.ChildrenRegistered)
                {
                    return (this.mChildren.Count > 0) && (this.Children.Any(pChild => pChild.Visibility == Visibility.Visible));
                }

                return this.HasChildrenLoadedOnDemand;
            }
        }

        /// <summary>
        /// Flag defining if some children will be loaded on demand.
        /// </summary>
        public virtual bool HasChildrenLoadedOnDemand
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets or sets the flag indicating if the item is expanded or not.
        /// </summary>
        public bool IsExpanded
        {
            get
            {
                return this.mIsExpanded;
            }
            set
            {
                if (this.mIsExpanded != value)
                {
                    // Registering the items if it is not done.
                    if (value)
                    {
                        this.RegisterChildren();
                    }

                    // Expanding or collapse this node.
                    this.mIsExpanded = value;
                    this.OnIsExpandedChanged(value);
                    this.NotifyPropertyChanged("IsExpanded");
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance can be selected.
        /// </summary>
        public virtual bool CanBeSelected
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Gets or sets the flag to know if the item is selected.
        /// </summary>
        public bool IsSelected
        {
            get
            {
                return this.mIsSelected;
            }
            set
            {
                if (this.mIsSelected != value && this.CanBeSelected)
                {
                    this.mIsSelected = value;
                    this.NotifyPropertyChanged("IsSelected");
                }
            }
        }


        /// <summary>
        /// Gets or sets the flag indicating if the children are loaded into the tree.
        /// </summary>
        bool IHierarchicalItemViewModel.ChildrenAreLoaded
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the decarator width.
        /// </summary>
        public double DecoratorWidth
        {
            get
            {
                return this.mDecoratorWidth;
            }

            set
            {
                this.mDecoratorWidth = value;
                this.NotifyPropertyChanged("DecoratorWidth");
            }
        }

        /// <summary>
        /// Gets the list of visible children.
        /// </summary>
        IEnumerable<IHierarchicalItemViewModel> IHierarchicalItemViewModel.AllVisibleChildren 
        { 
            get
            {
                return this.AllVisibleChildren;
            }
        }

        /// <summary>
        /// Gets the list of visible children.
        /// </summary>
        private IEnumerable<AHierarchicalItemViewModel> AllVisibleChildren
        {
            get
            {
                int lLevel = this.Level;
                AHierarchicalItemViewModel lViewModel = this;
                while
                    (true)
                {
                    lViewModel = lViewModel.NextVisibleViewModel;
                    if
                        (   (lViewModel != null)
                        &&  (lViewModel.Level > lLevel)
                        )
                    {
                        yield return lViewModel;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Gets the flag indicating if the children view model have been registered.
        /// </summary>
        /// <remarks>
        /// Children are automatically tagged as registered if the load on demand is not activated.
        /// </remarks>
        protected bool ChildrenRegistered
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the flag indicating if calling the children modification methods is made during a registration process.
        /// </summary>
        private bool IsInRegistrationMode
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the flag indicating if the items are loaded on demand.
        /// </summary>
        protected virtual bool LoadItemsOnDemand
        {
            get
            {
                if
                    (this.Parent != null)
                {
                    return this.Parent.LoadItemsOnDemand;
                }

                return true;
            }
        }

        /// <summary>
        /// Gets the level of the node (depth).
        /// </summary>
        private int Level
        {
            get
            {
                if
                    (this.Parent == null)
                {
                    return -1;
                }

                return this.Parent.Level + 1;
            }
        }

        /// <summary>
        /// Gets the next visible view model.
        /// </summary>
        private AHierarchicalItemViewModel NextVisibleViewModel
        {
            get
            {
                if
                    (   (this.IsExpanded)
                    &&  (this.mChildren.Count > 0)
                    )
                {
                    return this.mChildren[0];
                }
                else
                {
                    AHierarchicalItemViewModel lNextViewModel = this.NextViewModel;
                    if
                        (lNextViewModel != null)
                    {
                        return lNextViewModel;
                    }
                }
                return this.BottomViewModel;
            }
        }

        /// <summary>
        /// Gets the bottom view model.
        /// </summary>
        private AHierarchicalItemViewModel BottomViewModel
        {
            get
            {
                if
                    (this.Parent != null)
                {
                    if
                        (this.Parent.NextViewModel != null)
                    {
                        return this.Parent.NextViewModel;
                    }

                    return this.Parent.BottomViewModel;
                }

                return null;
            }
        }

        /// <summary>
        /// Gets the next view model (sibling).
        /// </summary>
        private AHierarchicalItemViewModel NextViewModel
        {
            get
            {
                if
                    (this.Parent != null)
                {
                    if
                        (this.mIndex < this.Parent.mChildren.Count - 1)
                    {
                        return this.Parent.mChildren[this.mIndex + 1];
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// This indexer is used to correctly bind the property of inherited view model.
        /// </summary>
        /// <param name="pIndex">The property indexer.</param>
        /// <returns>The value of the indexed property.</returns>
        public virtual object this[int pIndex]
        {
            get
            {
                return null;
            }
            // ReSharper disable once ValueParameterNotUsed
            set
            {
                // Nothing to do.
            }
        }

        #endregion // Properties.

        #region Methods

        /// <summary>
        /// Add a sorter using the provided comparer.
        /// </summary>
        /// <param name="pKeySelector">The key selector.</param>
        /// <param name="pComparer">The key comparer.</param>
        public void SetSorter(Func<IHierarchicalItemViewModel, IHierarchicalItemViewModel, object> pKeySelector, IComparer<object> pComparer)
        {
            this.mComparerKeySelector = pKeySelector;
            this.mComparer = pComparer;
        }

        /// <summary>
        /// Removes the sorter from the view model.
        /// </summary>
        public void RemoveSorter()
        {
            this.mComparerKeySelector = null;
            this.mComparer = null;
        }

        /// <summary>
        /// Adds a child in the children list.
        /// </summary>
        /// <param name="pChild">The child to add.</param>
        public virtual void AddChild(IHierarchicalItemViewModel pChild)
        {
            AHierarchicalItemViewModel lChild = pChild as AHierarchicalItemViewModel;
            if (lChild != null)
            {
                this.AddChild(lChild);
            }
        }

        /// <summary>
        /// Insert a child at the given position.
        /// </summary>
        /// <param name="pIndex">The index where the hild has to be inserted.</param>
        /// <param name="pChild">The child to insert.</param>
        public void InsertChild(Int32 pIndex, IHierarchicalItemViewModel pChild)
        {
            AHierarchicalItemViewModel lChild = pChild as AHierarchicalItemViewModel;
            if (lChild != null)
            {
                this.InsertChild(pIndex, lChild);
            }
        }

        /// <summary>
        /// Removes a child from the children list.
        /// </summary>
        /// <param name="pChild">The child to remove.</param>
        public void RemoveChild(IHierarchicalItemViewModel pChild)
        {
            AHierarchicalItemViewModel lChild = pChild as AHierarchicalItemViewModel;
            if (lChild != null)
            {
                this.RemoveChild(lChild);
            }
        }

        /// <summary>
        /// Removes the child at the given position.
        /// </summary>
        /// <param name="pIndex">The position of the child to remove.</param>
        public void RemoveChildAt(Int32 pIndex)
        {
            this.RemoveChildAtInternal(pIndex);
        }

        /// <summary>
        /// Adds a child to this item.
        /// </summary>
        /// <param name="pChild">The item to add.</param>
        private void AddChild(AHierarchicalItemViewModel pChild)
        {
            if (this.IsInRegistrationMode)
            {
                // Adding the children.
                this.AddOrganizedChild(pChild);
            }
            else
            {
                // If never expanded, registers the already defined children.
                this.RegisterChildren();

                // Adding the children.
                this.AddOrganizedChild(pChild);
            }
        }

        /// <summary>
        /// Insert a child to this item.
        /// </summary>
        /// <param name="pIndex">The index where the item has to be inserted.</param>
        /// <param name="pChild">The item to insert.</param>
        /// <returns>True if the child has been added, false if the index is out of range.</returns>
        private void InsertChild(int pIndex, AHierarchicalItemViewModel pChild)
        {
            // Verifying if the item is in the range.
            if
                (   (pIndex < 0)
                ||  (pIndex >= this.mChildren.Count)
                )
            {
                return;
            }

            if (this.IsInRegistrationMode)
            {
                // Adding the children.
                this.InsertOrganizedChild(pIndex, pChild);
            }
            else
            {
                // If never expanded, registers the already defined children.
                this.RegisterChildren();

                // Adding the children.
                this.InsertOrganizedChild(pIndex, pChild);
            }
        }

        /// <summary>
        /// Moves a child from the old index to the new index.
        /// </summary>
        /// <param name="pOldIndex">The old index of the child.</param>
        /// <param name="pNewIndex">The new index of the child.</param>
        /// <returns>True if the child has been moved, false if one of the indexes is out of range.</returns>
        public void MoveChild(int pOldIndex, int pNewIndex)
        {
            // Verifying if the item is in the range.
            if
                (   (pOldIndex < 0)
                ||  (pOldIndex >= this.mChildren.Count)
                ||  (this.mComparerKeySelector != null)
                )
            {
                return;
            }

            AHierarchicalItemViewModel lChild = this.mChildren.ElementAt(pOldIndex);
            if (lChild != null)
            {
                this.mChildren.Move(pOldIndex, pNewIndex);
                this.NotifyChildMoved(lChild, pOldIndex, pNewIndex);
            }
        }

        /// <summary>
        /// Removes a child from this item.
        /// </summary>
        /// <param name="pChild">The child to remove.</param>
        /// <returns>True if the child has been removed, false otherwise.</returns>
        private bool RemoveChild(AHierarchicalItemViewModel pChild)
        {
            // Removing the items from the children list.
            if (this.mChildren.Remove(pChild))
            {
                this.NotifyChildRemoved(pChild);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Removes the child at the given position.
        /// </summary>
        /// <param name="pIndex">The position of the child to remove.</param>
        /// <returns>The removed item if any, null otherwise.</returns>
        private AHierarchicalItemViewModel RemoveChildAtInternal(int pIndex)
        {
            AHierarchicalItemViewModel lChild = this.mChildren.ElementAt(pIndex);
            if (lChild != null && this.RemoveChild(lChild))
            {
                return lChild;
            }

            return null;
        }

        /// <summary>
        /// Clears the children list.
        /// </summary>
        /// <param name="pDispose">Flag indicating if the children must be disposed or not.</param>
        public void ClearChildren(bool pDispose = true)
        {
            // Clearing the children list.
            while (this.mChildren.Count != 0)
            {
                // Processing Dispose will remove the item from the parent children list as well.
                AHierarchicalItemViewModel lItem = this.mChildren[0];
                lItem.DisableNotifyPropertyChangedEvent();
                lItem.ClearChildren(pDispose);
                this.mChildren.RemoveAt(0);
                lItem.EnableNotifyPropertyChangedEvent();
                this.NotifyChildRemoved(lItem);

                if (pDispose)
                {
                    lItem.Dispose();
                }
            }
        }

        /// <summary>
        /// Add a child to the children list taking in account the comparer.
        /// </summary>
        /// <param name="pChild">The child to add.</param>
        private void AddOrganizedChild(AHierarchicalItemViewModel pChild)
        {
            if (this.mComparerKeySelector != null)
            {
                var lPair = this.mChildren.Select((pValue, pIndex) => new { value = pValue, index = pIndex }).FirstOrDefault(pElt => this.mComparer.Compare(this.mComparerKeySelector(this, pElt.value), this.mComparerKeySelector(this, pChild)) > 0);
                if (lPair == null)
                {
                    //Propagate visibility to children
                    pChild.Visibility = this.Visibility;
                    this.mChildren.Add(pChild);
                    this.NotifyChildAdded(pChild);
                }
                else
                {
                    //Propagate visibility to children
                    pChild.Visibility = this.Visibility;
                    this.mChildren.Insert(lPair.index, pChild);
                    this.NotifyChildAdded(pChild);
                }
            }
            else
            {
                //Propagate visibility to children
                pChild.Visibility = this.Visibility;
                this.mChildren.Add(pChild);
                this.NotifyChildAdded(pChild);
            }
        }

        /// <summary>
        /// Insert a child to the children list taking in account the comparer.
        /// </summary>
        /// <param name="pIndex">The index where the hild has to be inserted.</param>
        /// <param name="pChild">The child to insert.</param>
        private void InsertOrganizedChild(int pIndex, AHierarchicalItemViewModel pChild)
        {
            if (this.mComparerKeySelector != null)
            {
                this.AddOrganizedChild(pChild);
            }
            else
            {
                this.mChildren.Insert(pIndex, pChild);
                this.NotifyChildAdded(pChild);
            }
        }

        /// <summary>
        /// Registers the children of this item on demand.
        /// </summary>
        private void RegisterChildren()
        {
            if (this.ChildrenRegistered == false)
            {
                this.IsInRegistrationMode = true;

                // If a binding has been registered, the synchronization must be done. 
                // The children loading process is no more handled by the user.
                if (this.mChildrenBinding.Count != 0)
                {
                    this.SynchronizeChildren();
                }
                else
                {
                    this.InternalRegisterChildren();
                }

                this.IsInRegistrationMode = false;
                this.ChildrenRegistered = true;
            }
        }

        /// <summary>
        /// Registers the children into the given list on demand.
        /// </summary>
        protected virtual void InternalRegisterChildren()
        {
            // Nothing to do.
        }

        /// <summary>
        /// Synchronize the view model children with the owned object binded collection.
        /// </summary>
        private void SynchronizeChildren()
        {
            foreach (var lChildrenBinding in this.mChildrenBinding)
            {
                IList lModelCollection = lChildrenBinding.Key as IList;
                this.SynchronizeChildren(lModelCollection);
            }
        }

        /// <summary>
        /// Synchronize the view model children with the owned object binded collection.
        /// </summary>
        /// <param name="pCollection">The collection to synchronize</param>
        private void SynchronizeChildren(IList pCollection)
        {
            INotifyCollectionChanged lCollection = pCollection as INotifyCollectionChanged;
            CreateChildViewModelDelegate lCreator = null;
            if (lCollection != null)
            {
                if (this.mChildrenBinding.ContainsKey(lCollection))
                {
                    lCreator = this.mChildrenBinding[lCollection];
                }
            }

            if (pCollection != null && lCreator != null)
            {
                // Synchronizing the children.
                foreach (object lModel in pCollection)
                {
                    // Creating the view model using the apropriate delegate.
                    AHierarchicalItemViewModel lItemViewModel = lCreator(lModel);
                    if (lItemViewModel != null)
                    {
                        this.AddChild(lItemViewModel);
                    }
                }
            }
        }

        /// <summary>
        /// Bind an expression of a property from the owned object to the Children property in the current view model.
        /// </summary>
        /// <param name="pModelProperty">The property name of the owned model.</param>
        /// <param name="pChildViewModelType">The type of the view model to create.</param>
        protected void BindChildren(string pModelProperty, Type pChildViewModelType)
        {
            // Using the native view model creator delegate.
            this.BindChildren(pModelProperty, pOwnedObject => this.CreateChildViewModel(pChildViewModelType, pOwnedObject));
        }

        /// <summary>
        /// Bind an expression of a property from the owned object to the Children property in the current view model.
        /// </summary>
        /// <param name="pModelProperty">The property name of the owned model.</param>
        /// <param name="pCreationDelegate">Specific child view model creation delegate.</param>
        protected void BindChildren(string pModelProperty, CreateChildViewModelDelegate pCreationDelegate)
        {            
            // Registering the binding.
            INotifyCollectionChanged lCollectionNotifier = this.GetCollectionFromPropertyName(this.UntypedOwnedObject, pModelProperty);
            if (lCollectionNotifier != null)
            {
                // Binding the collections.
                this.mChildrenBinding.Add(lCollectionNotifier, pCreationDelegate);
                lCollectionNotifier.CollectionChanged += this.OnOwnedObjectCollectionChanged;

                // Synchronize it if the item is already expanded.
                if (this.IsExpanded)
                {
                    this.SynchronizeChildren(lCollectionNotifier as IList);
                }
            }
        }

        /// <summary>
        /// Unbind the property binded to the Children view model property.
        /// </summary>
        protected void UnbindChildren()
        {
            foreach (var lChildrenBinding in this.mChildrenBinding)
            {
                // Unregister the notification handler.
                lChildrenBinding.Key.CollectionChanged -= this.OnOwnedObjectCollectionChanged;

                // Clearing the children.
                this.ClearChildren();
            }
            this.mChildrenBinding.Clear();
        }

        /// <summary>
        /// Called each time a binded collection of the owned object gets changed to update the children.
        /// </summary>
        /// <param name="pSender">The modified collection.</param>
        /// <param name="pEvent">The event's arguments.</param>
        private void OnOwnedObjectCollectionChanged(Object pSender, NotifyCollectionChangedEventArgs pEvent)
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                INotifyCollectionChanged lCollection = pSender as INotifyCollectionChanged;
                CreateChildViewModelDelegate lCreator = null;
                if (lCollection != null)
                {
                    if (this.mChildrenBinding.ContainsKey(lCollection))
                    {
                        lCreator = this.mChildrenBinding[lCollection];
                    }
                }

                IList lSender = pSender as IList;
                if (lSender != null)
                {
                    // Update the binded children.
                    switch (pEvent.Action)
                    {
                        case NotifyCollectionChangedAction.Add:
                            {
                                bool lDoExplicitAdd = true;
                                int lIndex = pEvent.NewStartingIndex;
                                if (this.ChildrenRegistered == false)
                                {
                                    this.RegisterChildren();

                                    // Loading children will maybe apply the modifications.
                                    if (this.mChildren.Count == lSender.Count)
                                    {
                                        lDoExplicitAdd = false;
                                    }
                                }

                                if (lDoExplicitAdd && lCreator != null)
                                {
                                    foreach (object lModel in pEvent.NewItems)
                                    {
                                        // Creating the view model using the apropriate delegate.
                                        AHierarchicalItemViewModel lItemViewModel = lCreator(lModel);
                                        if (lItemViewModel != null)
                                        {
                                            if (lIndex == this.mChildren.Count)
                                            {
                                                this.AddChild(lItemViewModel);
                                            }
                                            else
                                            {
                                                this.InsertChild(lIndex, lItemViewModel);
                                            }
                                        }

                                        // Increment indices.
                                        ++lIndex;
                                    }
                                }
                            }

                            break;

                        case NotifyCollectionChangedAction.Remove:
                            {
                                List<AHierarchicalItemViewModel> lModelToRemove = new List<AHierarchicalItemViewModel>();

                                foreach (AHierarchicalItemViewModel lViewModel in this.Children)
                                {
                                    if (pEvent.OldItems.Contains(lViewModel.UntypedOwnedObject))
                                    {
                                        lModelToRemove.Add(lViewModel);
                                    }
                                }

                                // ReSharper disable once UnusedVariable
                                foreach (AHierarchicalItemViewModel lModel in lModelToRemove)
                                {
                                    Int32 lRemoveIndex = this.mChildren.IndexOf(lModel);
                                    this.RemoveChildAt(lRemoveIndex);
                                }
                            }

                            break;

                        case NotifyCollectionChangedAction.Replace:
                            {
                                if (lCreator != null)
                                {
                                    int lCurrentItemIndex = 0;
                                    foreach (object lNewModel in pEvent.NewItems)
                                    {
                                        // Get old model.
                                        object lOldModel = pEvent.OldItems[lCurrentItemIndex];

                                        // Create the new Item
                                        AHierarchicalItemViewModel lItemViewModel = lCreator(lNewModel);
                                        if (lItemViewModel != null)
                                        {
                                            // Search for the removed item in the collection to update.
                                            AHierarchicalItemViewModel lViewModelToReplace = null;
                                            foreach (AHierarchicalItemViewModel lViewModel in this.Children)
                                            {
                                                if (lViewModel.UntypedOwnedObject == lOldModel)
                                                {
                                                    lViewModelToReplace = lViewModel;
                                                    break;
                                                }
                                            }

                                            if (lViewModelToReplace != null)
                                            {
                                                // Get the index of the removed item
                                                int lReplaceIndex = this.mChildren.IndexOf(lViewModelToReplace);

                                                // Replace the old item by the new one
                                                this.RemoveChild(lViewModelToReplace);
                                                this.InsertChild(lReplaceIndex, lItemViewModel);
                                            }
                                        }

                                        // Increment indices.
                                        ++lCurrentItemIndex;
                                    }
                                }
                            }

                            break;

                        case NotifyCollectionChangedAction.Move:
                            {
                                int lOldIndex = pEvent.OldStartingIndex + pEvent.OldItems.Count - 1;
                                while (lOldIndex > 0)
                                {
                                    this.MoveChild(lOldIndex, pEvent.NewStartingIndex);
                                    lOldIndex--;
                                }
                            }

                            break;

                        case NotifyCollectionChangedAction.Reset:
                            {
                                this.ClearChildren();
                            }

                            break;
                    }
                }
            }));
        }

        /// <summary>
        /// Creates a new children as ATreeListViewItemViewModel of this item.
        /// </summary>
        /// <param name="pModelViewType">The model view type.</param>
        /// <param name="pOwnedObject">The owned object of the new item.</param>
        /// <returns>The created item.</returns>
        private AHierarchicalItemViewModel CreateChildViewModel(Type pModelViewType, Object pOwnedObject)
        {
            if
                (   (pModelViewType.IsInterface == false)
                &&  (pModelViewType.IsAbstract == false)
                )
            {
                Object[] lParams = new Object[1];
                lParams[0] = pOwnedObject;
                try
                {
                    return Activator.CreateInstance(pModelViewType, lParams) as AHierarchicalItemViewModel;
                }
                catch
                    (Exception /*lEx*/)
                {
                    return null;
                }
            }

            return null;
        }

        /// <summary>
        /// Returns the collection from the given object (model or view model) property using reflexion.
        /// </summary>
        /// <remarks>The property can be a property path.</remarks>
        /// <param name="pObject">The object containing the property.</param>
        /// <param name="pPropertyName">The property name.</param>
        /// <returns>The collection if any, null otherwise.</returns>
        private INotifyCollectionChanged GetCollectionFromPropertyName(Object pObject, String pPropertyName)
        {
            List<PropertyInfo> lPropertiesInfo = new List<PropertyInfo>();

            string[] lProperties = pPropertyName.Split(new char[] { '.' });
            lPropertiesInfo.Add(pObject.GetType().GetProperty(lProperties[0]));
            for (int lP = 1; lP < lProperties.Count(); ++lP)
            {
                if (lPropertiesInfo.Last() != null)
                {
                    lPropertiesInfo.Add(lPropertiesInfo.Last().PropertyType.GetProperty(lProperties[lP]));
                }
                else
                {
                    lPropertiesInfo.Add(null);
                }
            }

            if
                (   (lPropertiesInfo.Count > 0)
                &&  (lPropertiesInfo.Last() != null)
                )
            {
                // Get collection object from propery list
                object lObject = pObject;
                foreach (PropertyInfo lPInfo in lPropertiesInfo)
                {
                    lObject = lPInfo.GetValue(lObject, null);
                }

                return lObject as INotifyCollectionChanged;
            }

            return null;
        }

        /// <summary>
        /// Delegate called if the children collection is modified.
        /// </summary>
        /// <param name="pSender">The modified collection.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        protected virtual void OnChildrenCollectionChanged(object pSender, NotifyCollectionChangedEventArgs pEventArgs)
        {
            // Updating the HasChildren property.
            this.NotifyPropertyChanged("HasChildren");

            // Notifying the user.
            if (this.ChildrenCollectionChanged != null)
            {
                this.ChildrenCollectionChanged(pSender, pEventArgs);
            }
        }

        /// <summary>
        /// Delegate called when the visibility is changed.
        /// </summary>
        /// <param name="pNewValue">The new visibility.</param>
        protected override void OnVisibilityChanged(Visibility pNewValue)
        {
            // Updating the children visibility as well.
            foreach (AHierarchicalItemViewModel lChild in this.Children)
            {
                lChild.Visibility = pNewValue;
            }
        }

        /// <summary>
        /// Convert the item to the generic version.
        /// </summary>
        /// <typeparam name="T">The type of the owned object.</typeparam>
        /// <returns>The generic version of the item.</returns>
        public new AHierarchicalItemViewModel<T> ToGeneric<T>()
        {
            return (this as AHierarchicalItemViewModel<T>);
        }

        /// <summary>
        /// Convert the item to the generic version.
        /// </summary>
        /// <typeparam name="T">The type of the owned object.</typeparam>
        /// <returns>The generic version of the item.</returns>
        IHierarchicalItemViewModel<T> IHierarchicalItemViewModel.ToGeneric<T>()
        {
            return (this as IHierarchicalItemViewModel<T>);
        }

        /// <summary>
        /// Unregisters from the underlying model.
        /// </summary>
        protected override void UnregisterFromModel()
        {
            // Dispose the base class.
            base.UnregisterFromModel();

            // Removing this object from the parent children list.
            if (this.Parent != null)
            {
                this.Parent.mChildren.Remove(this);
            }

            // Disposing the children.
            while (this.mChildren.Count > 0)
            {
                this.mChildren[0].Dispose();
            }

            // Unbind the children.
            this.UnbindChildren();
        }

        /// <summary>
        /// Select this item and all the children items.
        /// </summary>
        /// <returns>The new selected items.</returns>
        IHierarchicalItemViewModel[] IHierarchicalItemViewModel.SelectAll()
        {
            List<IHierarchicalItemViewModel> lNewSelectedItem = new List<IHierarchicalItemViewModel>();
            if (this.IsSelected == false)
            {
                this.IsSelected = true;
                if (this.IsSelected)
                {
                    lNewSelectedItem.Add(this);
                }
            }

            foreach (IHierarchicalItemViewModel lChild in this.AllVisibleChildren)
            {
                lNewSelectedItem.AddRange(lChild.SelectAll());
            }

            return lNewSelectedItem.ToArray();
        }

        /// <summary>
        /// Unselect this item and all the children items.
        /// </summary>
        /// <returns>The old selected items.</returns>
        IHierarchicalItemViewModel[] IHierarchicalItemViewModel.UnSelectAll()
        {
            List<IHierarchicalItemViewModel> lOldSelectedItem = new List<IHierarchicalItemViewModel>();
            if (this.IsSelected)
            {
                this.IsSelected = false;
                if (this.IsSelected == false)
                {
                    lOldSelectedItem.Add(this);
                }
            }

            foreach (IHierarchicalItemViewModel lChild in this.AllVisibleChildren)
            {
                lOldSelectedItem.AddRange(lChild.UnSelectAll());
            }

            return lOldSelectedItem.ToArray();
        }

        /// <summary>
        /// Check this item and all its children.
        /// </summary>
        /// <returns>The new checked items.</returns>
        IHierarchicalItemViewModel[] IHierarchicalItemViewModel.CheckAll()
        {
            List<IHierarchicalItemViewModel> lNewCheckedItem = new List<IHierarchicalItemViewModel>();
            if (this.IsChecked == false)
            {
                this.IsChecked = true;
                if (this.IsChecked)
                {
                    lNewCheckedItem.Add(this);
                }
            }

            foreach (IHierarchicalItemViewModel lChild in this.AllVisibleChildren)
            {
                lNewCheckedItem.AddRange(lChild.CheckAll());
            }

            return lNewCheckedItem.ToArray();
        }

        /// <summary>
        /// Uncheck this item and all its children.
        /// </summary>
        /// <returns>The old checked items.</returns>
        IHierarchicalItemViewModel[] IHierarchicalItemViewModel.UncheckAll()
        {
            List<IHierarchicalItemViewModel> lOldCheckedItem = new List<IHierarchicalItemViewModel>();
            if (this.IsChecked)
            {
                this.IsChecked = false;
                if (this.IsChecked == false)
                {
                    lOldCheckedItem.Add(this);
                }
            }

            foreach (IHierarchicalItemViewModel lChild in this.AllVisibleChildren)
            {
                lOldCheckedItem.AddRange(lChild.UncheckAll());
            }

            return lOldCheckedItem.ToArray();
        }

        /// <summary>
        /// Method to call when new children are added to this view model.
        /// </summary>
        /// <param name="pChild">The child removed from the children list.</param>
        protected virtual void NotifyChildAdded(IHierarchicalItemViewModel pChild)
        {
            if (this.Parent != null)
            {
                this.Parent.NotifyChildAdded(pChild);
            }
        }

        /// <summary>
        /// Method to call when children are removed from this view model.
        /// </summary>
        /// <param name="pChild">The child added to the children list.</param>
        protected virtual void NotifyChildRemoved(IHierarchicalItemViewModel pChild)
        {
            if (this.Parent != null)
            {
                this.Parent.NotifyChildRemoved(pChild);
            }
        }

        /// <summary>
        /// Method to call when children are moved in this view model.
        /// </summary>
        /// <param name="pChild">The child added to the children list.</param>
        /// <param name="pOldIndex">The old index of the item.</param>
        /// <param name="pNewIndex">THe new index of the item.</param>
        protected virtual void NotifyChildMoved(IHierarchicalItemViewModel pChild, int pOldIndex, int pNewIndex)
        {
            if (this.Parent != null)
            {
                this.Parent.NotifyChildMoved(pChild, pOldIndex, pNewIndex);
            }
        }

        /// <summary>
        /// Delegate called when the properties of this view model is modified.
        /// </summary>
        /// <param name="pSender">The item view model event sender.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        protected virtual void NotifyItemViewModelModified(object pSender, PropertyChangedEventArgs pEventArgs)
        {
            if (this.Parent != null)
            {
                this.Parent.NotifyItemViewModelModified(this, pEventArgs);
            }
        }

        /// <summary>
        /// Delegate called when the expanded state is changed.
        /// </summary>
        /// <param name="pNewValue">The new expanded state.</param>
        protected virtual void OnIsExpandedChanged(bool pNewValue)
        {
            // Nothing to do.
        }

        /// <summary>
        /// Returns the children view models owning the given object.
        /// </summary>
        /// <param name="pOwnedObject">The analysed owned object.</param>
        /// <param name="pComparer">The owned object comparer used when searching for the view models.</param>
        /// <remarks>If the comparer is null, the comparison is made by reference.</remarks>
        /// <returns>The list of view models owning the object.</returns>
        public IEnumerable<IHierarchicalItemViewModel> GetViewModels(object pOwnedObject, IComparer pComparer = null)
        {
            List<IHierarchicalItemViewModel> lFoundViewModels = new List<IHierarchicalItemViewModel>();

            foreach (IHierarchicalItemViewModel lChild in this.Children)
            {
                // Checking the current child.
                if (pComparer == null)
                {
                    if (lChild.UntypedOwnedObject == pOwnedObject)
                    {
                        lFoundViewModels.Add(lChild);
                    }
                }
                else
                {
                    if (pComparer.Compare(lChild.UntypedOwnedObject, pOwnedObject) == 0)
                    {
                        lFoundViewModels.Add(lChild);
                    }
                }

                // Propagating the request.
                lFoundViewModels.AddRange(lChild.GetViewModels(pOwnedObject, pComparer));
            }

            return lFoundViewModels.ToArray();
        }

        #region Sorter

        /// <summary>
        /// Sets a predefined sorter.
        /// </summary>
        /// <param name="pKey">The p key.</param>
        /// <param name="pSortOrder">The p sort order.</param>
        public void EnableSort(SortKey pKey, SorterOrder pSortOrder)
        {
            if (pSortOrder == SorterOrder.Unsorted)
            {
                this.RemoveSorter();
            }
            else
            {
                switch (pKey)
                {
                    case SortKey.DisplayString:
                        {
                            this.SetSorter(GetDisplayStringAsSortKey, new StringComparer(pSortOrder));
                        }
                        break;

                    case SortKey.ChildCount:
                        {
                            this.SetSorter(GetChildCountAsSortKey, new IntComparer(pSortOrder));
                        }
                        break;
                }
            }
            
        }

        /// <summary>
        /// Gets the key sorter.
        /// </summary>
        /// <param name="pParentViewModel">The parent view model.</param>
        /// <param name="pViewModel">The view model.</param>
        /// <returns>The display string of the view model.</returns>
        private static object GetDisplayStringAsSortKey(IHierarchicalItemViewModel pParentViewModel, IHierarchicalItemViewModel pViewModel)
        {
            return pViewModel.DisplayString;
        }

        /// <summary>
        /// Gets the key sorter.
        /// </summary>
        /// <param name="pParentViewModel">The parent view model.</param>
        /// <param name="pViewModel">The view model.</param>
        /// <returns>The display string of the view model.</returns>
        private static object GetChildCountAsSortKey(IHierarchicalItemViewModel pParentViewModel, IHierarchicalItemViewModel pViewModel)
        {
            return pViewModel.Children.Count();
        }

        /// <summary>
        /// A ascending string comparer.
        /// </summary>
        public class StringComparer : IComparer<object>
        {
            #region Fields

            /// <summary>
            /// The m order
            /// </summary>
            private readonly int mOrder = 1;

            #endregion // Fields.

            #region Constructors

            /// <summary>
            /// Initializes a new instance of the <see cref="StringComparer"/> class.
            /// </summary>
            /// <param name="pSorterOrder">The sorter order.</param>
            public StringComparer(SorterOrder pSorterOrder)
            {
                if (pSorterOrder == SorterOrder.Descending)
                {
                    this.mOrder = -1;
                }
            }

            #endregion // Constructors.

            #region Methods

            /// <summary>
            /// Compares two strings.
            /// </summary>
            /// <param name="pFirst">The first argument.</param>
            /// <param name="pSecond">The second argument.</param>
            /// <returns>string.Compare</returns>
            int IComparer<object>.Compare(object pFirst, object pSecond)
            {
                return this.mOrder * string.Compare(pFirst.ToString(), pSecond.ToString(), StringComparison.InvariantCultureIgnoreCase);
            }

            #endregion // Methods.
        }

        /// <summary>
        /// A ascending int comparer.
        /// </summary>
        public class IntComparer : IComparer<object>
        {
            #region Fields

            /// <summary>
            /// The m order
            /// </summary>
            private readonly int mOrder = 1;

            #endregion // Fields.

            #region Constructors

            /// <summary>
            /// Initializes a new instance of the <see cref="StringComparer"/> class.
            /// </summary>
            /// <param name="pSorterOrder">The sorter order.</param>
            public IntComparer(SorterOrder pSorterOrder)
            {
                if (pSorterOrder == SorterOrder.Descending)
                {
                    this.mOrder = -1;
                }
            }

            #endregion // Constructors.

            #region Methods

            /// <summary>
            /// Compares two strings.
            /// </summary>
            /// <param name="pFirst">The first argument.</param>
            /// <param name="pSecond">The second argument.</param>
            /// <returns>string.Compare</returns>
            int IComparer<object>.Compare(object pFirst, object pSecond)
            {
                if (pFirst is int && pSecond is int)
                {
                    int lFirst = (int) pFirst;
                    int lSecond = (int) pSecond;
                    return this.mOrder * lFirst.CompareTo(lSecond);
                }

                return 0;
            }

            #endregion // Methods.
        }

        #endregion // Sorter.

        #endregion // Methods.

        #region Inner classes

        /// <summary>
        /// This class defines a view model collection implemented to handle the item index in the 
        /// parent view model for performance purpose.
        /// </summary>
        private class ViewModelCollection : ObservableCollection<AHierarchicalItemViewModel>
        {
            #region Fields

            /// <summary>
            /// The collection owner.
            /// </summary>
            private AHierarchicalItemViewModel mOwner;

            #endregion // Fields.

            #region Constructors

            /// <summary>
            /// Initializes a new instance of the <see cref="ViewModelCollection"/> class.
            /// </summary>
            /// <param name="pOwner">The owner of the collection.</param>
            public ViewModelCollection(AHierarchicalItemViewModel pOwner)
            {
                this.mOwner = pOwner;
            }

            #endregion // Constructors.

            #region Methods

            /// <summary>
            /// This method clears the items.
            /// </summary>
            protected override void ClearItems()
            {
                while (this.Count != 0)
                {
                    this.RemoveAt(this.Count - 1);
                }
            }

            /// <summary>
            /// This method inserts an item.
            /// </summary>
            /// <param name="pIndex">The insertion index.</param>
            /// <param name="pItem">The item to insert.</param>
            protected override void InsertItem(Int32 pIndex, AHierarchicalItemViewModel pItem)
            {
                if (pItem == null)
                {
                    throw new ArgumentNullException("pItem");
                }

                if (pItem.Parent != this.mOwner)
                {
                    // Removing the element from the old parent if any.
                    if (pItem.Parent != null)
                    {
                        pItem.Parent.mChildren.Remove(pItem);
                    }

                    // Updating the parenting info.
                    pItem.Parent = this.mOwner;
                    pItem.mIndex = pIndex;
                    pItem.Visibility = pItem.Parent.Visibility;

                    // Loading children if not load on demand.
                    if (pItem.LoadItemsOnDemand == false)
                    {
                        pItem.RegisterChildren();
                    }

                    // Updating the index of the item placed after the added one.
                    for (int lIndex = pIndex; lIndex < this.Count; lIndex++)
                    {
                        this[lIndex].mIndex++;
                    }

                    // Inserting the item.
                    base.InsertItem(pIndex, pItem);
                }
            }

            /// <summary>
            /// This method removes an item.
            /// </summary>
            /// <param name="pItemIndex">The item index.</param>
            protected override void RemoveItem(Int32 pItemIndex)
            {
                // Getting the item.
                AHierarchicalItemViewModel lItem = this[pItemIndex]; 

                // Invalidating the index.
                lItem.mIndex = -1;

                // Invalidating the parent.
                lItem.Parent = null;

                // Updating the index of the item placed after the removed one.
                for (int lIdx = pItemIndex + 1; lIdx < this.Count; lIdx++)
                {
                    this[lIdx].mIndex--;
                }

                // Removing it.
                base.RemoveItem(pItemIndex);
            }

            /// <summary>
            /// Method called when an item is moved.
            /// </summary>
            /// <param name="pOldIndex">The old index.</param>
            /// <param name="pNewIndex">The new index.</param>
            protected override void MoveItem(int pOldIndex, int pNewIndex)
            {
                // Moving the item.
                base.MoveItem(pOldIndex, pNewIndex);

                // Updating all the items.
                for (int lIdx = 0; lIdx < this.Count; lIdx++)
                {
                    this[lIdx].mIndex = lIdx;
                }
            }

            /// <summary>
            /// This method replace an item.
            /// </summary>
            /// <param name="pItemIndex">The item of the index to replace.</param>
            /// <param name="pItem">The item.</param>
            protected override void SetItem(Int32 pItemIndex, AHierarchicalItemViewModel pItem)
            {
                if (pItem == null)
                {
                    throw new ArgumentNullException("pItem");
                }

                this.RemoveAt(pItemIndex);
                this.InsertItem(pItemIndex, pItem);
            }

            #endregion // Methods.
        }

        #endregion // Inner classes.
    }
}

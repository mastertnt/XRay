using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Controls.Primitives;

namespace XTreeListView.ViewModel
{
    /// <summary>
    /// This interface defines an item in a hierarchical view model.
    /// </summary>
    /// <!-- DPE -->
    public interface IHierarchicalItemViewModel : IViewModel
    {
        #region Properties

        /// <summary>
        /// Gets or sets the parent of this object.
        /// </summary>
        IHierarchicalItemViewModel Parent 
        { 
            get; 
        }

        /// <summary>
        /// Gets the logical children items of this object.
        /// </summary>
        IEnumerable<IHierarchicalItemViewModel> Children
        {
            get;
        }

        /// <summary>
        /// Gets the visible children of this item.
        /// </summary>
        IEnumerable<IHierarchicalItemViewModel> AllVisibleChildren 
        { 
            get; 
        }

        /// <summary>
        /// Gets or sets the flag indicating if the children are loaded into the tree.
        /// </summary>
        bool ChildrenAreLoaded
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the flag indcating if the item has children. Can be used for load on demand implementation.
        /// </summary>
        bool HasChildren
        {
            get;
        }

        /// <summary>
        /// Gets or sets the flag indicating whether the item is expanded. Mainly used to display the children.
        /// </summary>
        bool IsExpanded 
        { 
            get; 
            set; 
        }

        /// <summary>
        /// Gets a value indicating whether this instance can be selected.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance can be selected; otherwise, <c>false</c>.
        /// </value>
        bool CanBeSelected
        {
            get;
        }

        /// <summary>
        /// Gets or sets the flag indicating whether the item is selected or not.
        /// </summary>
        bool IsSelected
        {
            get;
            set;
        }
        
        /// <summary>
        /// Gets or sets the width of the decorator part of the item (includes the expander, the icon and the check box).
        /// </summary>
        double DecoratorWidth 
        { 
            get; 
            set; 
        }

        #endregion // Properties.

        #region Methods

        /// <summary>
        /// Adds a child in the children list.
        /// </summary>
        /// <param name="pChild">The child to add.</param>
        void AddChild(IHierarchicalItemViewModel pChild);

        /// <summary>
        /// Insert a child at the given position.
        /// </summary>
        /// <param name="pIndex">The index where the hild has to be inserted.</param>
        /// <param name="pChild">The child to insert.</param>
        void InsertChild(Int32 pIndex, IHierarchicalItemViewModel pChild);

        /// <summary>
        /// Removes a child from the children list.
        /// </summary>
        /// <param name="pChild">The child to remove.</param>
        void RemoveChild(IHierarchicalItemViewModel pChild);

        /// <summary>
        /// Removes the child at the given position.
        /// </summary>
        /// <param name="pIndex">The position of the child to remove.</param>
        void RemoveChildAt(Int32 pIndex);

        /// <summary>
        /// Clears the children list.
        /// </summary>
        /// <param name="pDispose">Flag indicating if the children must be disposed or not.</param>
        void ClearChildren(bool pDispose);

        /// <summary>
        /// Add a sorter using the provided comparer.
        /// </summary>
        /// <param name="pKeySelector">The key selector.</param>
        /// <param name="pComparer">The key comparer.</param>
        void SetSorter(Func<IHierarchicalItemViewModel, object> pKeySelector, IComparer<object> pComparer);

        /// <summary>
        /// Removes the sorter from the view model.
        /// </summary>
        void RemoveSorter();

        /// <summary>
        /// Convert the item to the generic version.
        /// </summary>
        /// <typeparam name="T">The type of the owned object.</typeparam>
        /// <returns>The generic version of the item.</returns>
        new Generic.IHierarchicalItemViewModel<T> ToGeneric<T>();

        /// <summary>
        /// Select all the loaded children of this item.
        /// </summary>
        /// <returns>Returns the items selected during this process.</returns>
        /// <remarks>Items that were selected before this call are not a part of the resulting list.</remarks>
        IHierarchicalItemViewModel[] SelectAll();

        /// <summary>
        /// Unelect all the loaded children of this item.
        /// </summary>
        /// <returns>Returns the items unselected during this process.</returns>
        /// <remarks>Items that were unselected before this call are not a part of the resulting list.</remarks>
        IHierarchicalItemViewModel[] UnSelectAll();

        /// <summary>
        /// Check all the loaded children of this item.
        /// </summary>
        /// <returns>Returns the items checked during this process.</returns>
        /// <remarks>Items that were checked before this call are not a part of the resulting list.</remarks>
        IHierarchicalItemViewModel[] CheckAll();

        /// <summary>
        /// Uncheck all the loaded children of this item.
        /// </summary>
        /// <returns>Returns the items unchecked during this process.</returns>
        /// <remarks>Items that were unchecked before this call are not a part of the resulting list.</remarks>
        IHierarchicalItemViewModel[] UncheckAll();

        /// <summary>
        /// Returns the children view models owning the given object.
        /// </summary>
        /// <param name="pOwnedObject">The analysed owned object.</param>
        /// <param name="pComparer">The owned object comparer used when searching for the view models.</param>
        /// <remarks>If the comparer is null, the comparison is made by reference.</remarks>
        /// <returns>The list of view models owning the object.</returns>
        IEnumerable<IHierarchicalItemViewModel> GetViewModels(object pOwnedObject, IComparer pComparer = null);

        #endregion // Methods.
    }
}

﻿using System.Collections.Generic;
using System;

namespace XTreeListView.ViewModel
{
    /// <summary>
    /// This interface is the root node of a hierarchical view model.
    /// </summary>
    public interface IRootHierarchicalItemViewModel : IHierarchicalItemViewModel
    {
        #region Properties

        /// <summary>
        /// Gets the view model.
        /// </summary>
        IEnumerable<IHierarchicalItemViewModel> ViewModel 
        { 
            get; 
        }

        #endregion // Properties.

        #region Events

        /// <summary>
        /// Event called when some items are added.
        /// </summary>
        event TreeViewEventHandler ItemViewModelsAdded;

        /// <summary>
        /// Event called when some items are removed.
        /// </summary>
        event TreeViewEventHandler ItemViewModelsRemoved;

        /// <summary>
        /// Event called when some items are removed.
        /// </summary>
        event TreeViewItemEventHander ItemViewModelModified;

        /// <summary>
        /// Delegate called when an item is moved.
        /// </summary>
        event TreeViewItemMovedEventHandler ItemViewModelMoved;

        #endregion // Events.

        #region Methods

        /// <summary>
        /// Convert the item to the generic version.
        /// </summary>
        /// <typeparam name="TModel">The type of the owned object.</typeparam>
        /// <returns>The generic version of the item.</returns>
        new Generic.IRootHierarchicalItemViewModel<TModel> ToGeneric<TModel>();

        #endregion // Methods.
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace XTreeListView.ViewModel
{
    /// <summary>
    /// Delegate called when the selection is modified.
    /// </summary>
    /// <param name="pSender">The object sender.</param>
    /// <param name="pEventArgs">The event arguments.</param>
    public delegate void SelectionChangedEventHandler(Object pSender, SelectionChangedEventArgs pEventArgs);

    /// <summary>
    /// Delegate called on the tree view.
    /// </summary>
    /// <param name="pSender">The object sender (a tree list view)</param>
    /// <param name="pItems">The list of items.</param>
    public delegate void TreeViewEventHandler(Object pSender, IEnumerable<IHierarchicalItemViewModel> pItems);

    /// <summary>
    /// Delegate called on the tree view.
    /// </summary>
    /// <param name="pSender">The object sender (an item)</param>
    /// <param name="pEventArgs">The event arguments.</param>
    public delegate void TreeViewItemEventHander(Object pSender, PropertyChangedEventArgs pEventArgs);

    /// <summary>
    /// Delegate defining the entry point of a background work executed in a view model.
    /// </summary>
    /// <param name="pSender">The object sender.</param>
    /// <param name="pParameters">The work parameters.</param>
    public delegate void ParameterizedBackgroundWorkStart(Object pSender, Object pParameters);
}

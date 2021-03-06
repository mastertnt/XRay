﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Collections;
using System.Windows.Controls.Primitives;
using XTreeListView.Behaviors;
using XTreeListView.ViewModel;
using XTreeListView.Models;
using XTreeListView.Extensions;
using System.ComponentModel;
using System.Windows.Data;

namespace XTreeListView.Gui
{
    /// <summary>
    /// This control implements a tree based on a list view.
    /// </summary>
    /// <remarks>Implemented using the project "http://www.codeproject/KB/WPF/wpf_treelistview_control.aspx".</remarks>
    public class ExtendedListView : ListView
    {
        #region Dependencies

        /// <summary>
        /// Identifies the ViewModel dependency property.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof(IRootHierarchicalItemViewModel), typeof(ExtendedListView), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnViewModelChanged)));

        #endregion // Dependencies.

        #region Fields

        /// <summary>
        /// Stores the behavior responsible for resizing the columns of the list view grid view.
        /// </summary>
        public ColumnResizeBehavior mColumnResizeBehavior;

        /// <summary>
        /// Stores the behavior responsible for handling the user selection behavior.
        /// </summary>
        private SelectionBehavior mSelectionBehavior;

        ///// <summary>
        ///// Stores the behavior responsible for handling the user expand behavior.
        ///// </summary>
        private ExpandBehavior mExpandBehavior;

        #endregion // Fields.

        #region Constructors
        
        /// <summary>
        /// Initializes the <see cref="ExtendedListView"/> class.
        /// </summary>
        static ExtendedListView()
        {
            ExtendedListView.MultiColumnDefaultStyleKey = new ComponentResourceKey(typeof(ExtendedListView), "MultiColumnDefaultStyleKey");
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ExtendedListView"/> class.
        /// </summary>
        public ExtendedListView()
        {
            // Creating the rows collection.
            this.Rows = new RowsCollection(this);
            
            // Creating the models.
            this.SelectionModel = new SelectionModel(this);
            this.CheckModel = new CheckModel();
            this.ExpandModel = new ExpandModel(this);

            // Creating the behaviors.
            this.mSelectionBehavior = new SelectionBehavior(this);
            this.mExpandBehavior = new ExpandBehavior(this);
            this.mColumnResizeBehavior = new ColumnResizeBehavior(this);
        }

        #endregion // Constructors.

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
        /// Event called when an item is clicked.
        /// </summary>  
        public event TreeViewEventHandler ItemViewModelClicked;

        /// <summary>
        /// Event called when an item is double clicked.
        /// </summary>
        public event TreeViewEventHandler ItemViewModelDoubleClicked;

        #endregion // Events.

        #region Properties

        /// <summary>
        /// Stores the selection model.
        /// </summary>
        public SelectionModel SelectionModel
        {
            get;
            private set;
        }

        /// <summary>
        /// Stores the check model.
        /// </summary>
        public CheckModel CheckModel
        {
            get;
            private set;
        }

        /// <summary>
        /// Stores the expand model.
        /// </summary>
        public ExpandModel ExpandModel
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the rows corresponding to the items displayed in the list view.
        /// </summary>
        internal RowsCollection Rows 
        { 
            get; 
            private set; 
        }

        /// <summary>
        /// Gets or sets the view model of the list.
        /// </summary>
        public IRootHierarchicalItemViewModel ViewModel
        {
            get
            {
                return (IRootHierarchicalItemViewModel)GetValue(ViewModelProperty);
            }
            set
            {
                SetValue(ViewModelProperty, value);
            }
        }

        /// <summary>
        /// Gets the view as extended grid view.
        /// </summary>
        internal new ExtendedGridView View
        {
            get
            {
                return base.View as ExtendedGridView;
            }

            set
            {
                if (base.View != value)
                {
                    base.View = value;
                    if (base.View != null)
                    {
                        this.mColumnResizeBehavior.Activate();
                    }
                    else
                    {
                        this.mColumnResizeBehavior.Deactivate();
                    }
                }
            }
        }

        /// <summary>
        /// Gets the item style key when the tree is in multi column mode.
        /// </summary>
        public static ResourceKey MultiColumnDefaultStyleKey
        {
            get;
            private set;
        }

        #endregion // Properties.

        #region Methods

        /// <summary>
        /// This method creates the container for the item.
        /// In this case, the method creates a TreeListViewItem.
        /// </summary>
        /// <returns>a TreeListViewItem.</returns>
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new TreeListViewItem();
        }

        /// <summary>
        /// This method checks if the item is overriden.
        /// </summary>
        /// <param name="pItem">The native item.</param>
        /// <returns>True if the item has been overriden.</returns>
        protected override bool IsItemItsOwnContainerOverride(object pItem)
        {
            return pItem is TreeListViewItem;
        }

        /// <summary>
        /// This delagate is called when the view model is changed.
        /// </summary>
        /// <param name="pSender">The event sender.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private static void OnViewModelChanged(DependencyObject pSender, DependencyPropertyChangedEventArgs pEventArgs)
        {
            ExtendedListView lExtendedListView = pSender as ExtendedListView;
            if (lExtendedListView != null)
            {
                // Unloading the old view model.
                IRootHierarchicalItemViewModel lOldViewModel = pEventArgs.OldValue as IRootHierarchicalItemViewModel;
                if (lOldViewModel != null)
                {
                    // Unregistering from items modification events.
                    lOldViewModel.ItemViewModelsAdded -= lExtendedListView.OnItemViewModelsAdded;
                    lOldViewModel.ItemViewModelsRemoved -= lExtendedListView.OnItemViewModelsRemoved;
                    lOldViewModel.ItemViewModelMoved -= lExtendedListView.OnItemViewModelMoved;

                    // Initializing the view model.
                    lExtendedListView.DropChildrenItems(lOldViewModel, false);
                }

                // Loading the new view model.
                IRootHierarchicalItemViewModel lNewViewModel = pEventArgs.NewValue as IRootHierarchicalItemViewModel;
                if (lNewViewModel != null)
                {
                    // Registering on items modification events.
                    lNewViewModel.ItemViewModelsAdded += lExtendedListView.OnItemViewModelsAdded;
                    lNewViewModel.ItemViewModelsRemoved += lExtendedListView.OnItemViewModelsRemoved;
                    lNewViewModel.ItemViewModelMoved += lExtendedListView.OnItemViewModelMoved;

                    // Loading the first level items.
                    lExtendedListView.LoadsChildrenItems(lNewViewModel);
                }
            }
        }

        /// <summary>
        /// Scrolls the view to show the wanted item to the user.
        /// </summary>
        /// <param name="pItem">The item to bring.</param>
        /// <param name="pSelect">Flag indicating if the selected item must be selected.</param>
        /// <returns>True if the item is loaded, false otherwise.</returns>
        public bool ScrollIntoView(IHierarchicalItemViewModel pItem, bool pSelect)
        {
            if  (   (pItem != null)
                &&  (pItem.Parent != null)
                )
            {
                // Expand its parent to make pItem visible.
                this.ExpandModel.SetIsExpanded(pItem.Parent, true);

                // Showing the added item.
                this.ScrollIntoView(pItem);

                // Selecting the item if asked.
                if (pSelect)
                {
                    this.SelectionModel.Select(pItem);
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Loads the children of this item in the control.
        /// </summary>
        /// <param name="pViewModel">The view model containing the children.</param>
        internal void LoadsChildrenItems(IHierarchicalItemViewModel pViewModel)
        {
            if (pViewModel.ChildrenAreLoaded == false)
            {
                int lStartIndex = this.Rows.IndexOf(pViewModel);
                this.Rows.InsertRange(lStartIndex + 1, pViewModel.AllVisibleChildren.ToArray());

                pViewModel.ChildrenAreLoaded = true;
                pViewModel.AllVisibleChildren.Where(lItem => lItem.IsExpanded).ForEach(lItem => lItem.ChildrenAreLoaded = true);
            }
        }

        /// <summary>
        /// Removes the children from the rows.
        /// </summary>
        /// <param name="pViewModel">The view model containing the rows.</param>
        /// <param name="pIncludeParent">Flag indicating if the parent must be droped has well.</param>
        internal void DropChildrenItems(IHierarchicalItemViewModel pViewModel, bool pIncludeParent)
        {
            if (pViewModel.ChildrenAreLoaded)
            {
                int lStartIndex = this.Rows.IndexOf(pViewModel);
                int lCount = pViewModel.AllVisibleChildren.Count();

                if (pIncludeParent == false)
                {
                    lStartIndex++;
                }
                else
                {
                    lCount++;
                }

                // The item must be in the children list.
                if (lStartIndex != -1)
                {
                    this.Rows.RemoveRange(lStartIndex, lCount);
                }

                pViewModel.ChildrenAreLoaded = false;
                pViewModel.AllVisibleChildren.Where(lItem => lItem.IsExpanded).ForEach(lItem => lItem.ChildrenAreLoaded = false);
            }
        }

        /// <summary>
        /// Delegate called when a click is performed in the tree list view.
        /// </summary>
        /// <param name="pEventArgs">The event arguments.</param>
        internal void OnTreeListViewMouseDown(System.Windows.Input.MouseButtonEventArgs pEventArgs)
        {
            // Handling the selection.
            this.mSelectionBehavior.OnTreeListViewMouseDown(pEventArgs);
        }

        /// <summary>
        /// Delegate called when a key is pressed when the item get the focus.
        /// </summary>
        /// <param name="pItem">The clicked item.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        internal void OnItemKeyUp(IHierarchicalItemViewModel pItem, System.Windows.Input.KeyEventArgs pEventArgs)
        {
            // Handling the expand.
            this.mExpandBehavior.OnItemKeyUp(pItem, pEventArgs);
        }

        /// <summary>
        /// Delegate called when the mouse left button is down on an item.
        /// </summary>
        /// <param name="pItem">The clicked item.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        internal void OnItemMouseLeftButtonDown(IHierarchicalItemViewModel pItem, System.Windows.Input.MouseButtonEventArgs pEventArgs)
        {
            // Handling the selection.
            this.mSelectionBehavior.OnItemMouseLeftButtonDown(pItem, pEventArgs);
        }

        /// <summary>
        /// Delegate called when the mouse right button is down on an item.
        /// </summary>
        /// <param name="pItem">The clicked item.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        internal void OnItemMouseRightButtonDown(IHierarchicalItemViewModel pItem, System.Windows.Input.MouseButtonEventArgs pEventArgs)
        {
            // Handling the selection.
            this.mSelectionBehavior.OnItemMouseRightButtonDown(pItem, pEventArgs);
        }

        /// <summary>
        /// Delegate called when the mouse left button is up on an item.
        /// </summary>
        /// <param name="pItem">The clicked item.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        internal void OnItemMouseLeftButtonUp(IHierarchicalItemViewModel pItem, System.Windows.Input.MouseButtonEventArgs pEventArgs)
        {
            // Handling the selection.
            this.mSelectionBehavior.OnItemMouseLeftButtonUp(pItem, pEventArgs);
        }

        /// <summary>
        /// Delegate called when the mouse right button is up on an item.
        /// </summary>
        /// <param name="pItem">The clicked item.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        internal void OnItemMouseRightButtonUp(IHierarchicalItemViewModel pItem, System.Windows.Input.MouseButtonEventArgs pEventArgs)
        {
            // Handling the selection.
            this.mSelectionBehavior.OnItemMouseRightButtonUp(pItem, pEventArgs);
        }

        /// <summary>
        /// Delegate called when the mouse clicked on this item.
        /// </summary>
        /// <param name="pItem">The clicked item.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        internal void OnItemMouseClicked(IHierarchicalItemViewModel pItem, System.Windows.Input.MouseButtonEventArgs pEventArgs)
        {
            // Notification.
            if (this.ItemViewModelClicked != null)
            {
                this.ItemViewModelClicked(this, new IHierarchicalItemViewModel[] { pItem });
            }
        }

        /// <summary>
        /// Delegate called when the mouse double clicked on this item.
        /// </summary>
        /// <param name="pItem">The double clicked item.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        internal void OnItemMouseDoubleClicked(IHierarchicalItemViewModel pItem, System.Windows.Input.MouseButtonEventArgs pEventArgs)
        {
            // Handling the expand.
            this.mExpandBehavior.OnItemMouseDoubleClicked(pItem, pEventArgs);

            // Notification.
            if (this.ItemViewModelDoubleClicked != null)
            {
                this.ItemViewModelDoubleClicked(this, new IHierarchicalItemViewModel[] { pItem });
            }
        }

        /// <summary>
        /// Delegate called when the expander gets checked on the given item.
        /// </summary>
        /// <param name="pItem">The checked item.</param>
        internal void OnItemExpanderChecked(IHierarchicalItemViewModel pItem)
        {
            // Handling the expand.
            this.mExpandBehavior.OnItemExpanderChecked(pItem);
        }

        /// <summary>
        /// Delegate called when the expander gets unchecked on the given item.
        /// </summary>
        /// <param name="pItem">The unchecked item.</param>
        internal void OnItemExpanderUnchecked(IHierarchicalItemViewModel pItem)
        {
            // Handling the expand.
            this.mExpandBehavior.OnItemExpanderUnchecked(pItem);
        }

        /// <summary>
        /// Delegate called when items are added in the view model. 
        /// </summary>
        /// <param name="pSender">The modified root view model.</param>
        /// <param name="pItems">The added items.</param>
        private void OnItemViewModelsAdded(object pSender, IEnumerable<IHierarchicalItemViewModel> pItems)
        {
            // Updating the node loading in the list view.
            foreach (IHierarchicalItemViewModel lItem in pItems)
            {
                if (lItem.Parent.IsExpanded == true && lItem.Parent.ChildrenAreLoaded)
                {
                    // Computing the index of the item in the rows.
                    int lParentRowIndex = this.Rows.IndexOf(lItem.Parent);
                    int lChildIndex = lItem.Parent.AllVisibleChildren.ToList().IndexOf(lItem);
                    int lIndexInRows = lParentRowIndex + lChildIndex + 1;

                    // Adding the item in the rows.
                    if (lIndexInRows >= this.Rows.Count)
                    {
                        this.Rows.Add(lItem);
                    }
                    else
                    {
                        this.Rows.Insert(lIndexInRows, lItem);
                    }

                    // Recusrsive call on the visible children.
                    if (lItem.IsExpanded)
                    {
                        this.LoadsChildrenItems(lItem);
                    }
                }
            }

            // Forwarding the notification.
            if (this.ItemViewModelsAdded != null)
            {
                this.ItemViewModelsAdded(this, pItems);
            }
        }        

        /// <summary>
        /// Delegate called when items are removed from the view model. 
        /// </summary>
        /// <param name="pSender">The modified root view model.</param>
        /// <param name="pItems">The removed items.</param>
        private void OnItemViewModelsRemoved(object pSender, IEnumerable<IHierarchicalItemViewModel> pItems)
        {
            // Updating the node loading in the list view.
            foreach (IHierarchicalItemViewModel lItem in pItems)
            {
                // Removing the items from the selected and toggled list.
                this.SelectionModel.Unselect(lItem, true);
                this.CheckModel.Uncheck(lItem, true);

                // Removing the items from the tree view if they are displayed.
                if (lItem.IsExpanded)
                {
                    this.DropChildrenItems(lItem, true);
                }
                else
                {
                    this.Rows.Remove(lItem);
                }
            }

            // Forwarding the notification.
            if (this.ItemViewModelsRemoved != null)
            {
                this.ItemViewModelsRemoved(this, pItems);
            }
        }

        /// <summary>
        /// Delegate called when items are removed from the view model. 
        /// </summary>
        /// <param name="pSender">The modified root view model.</param>
        /// <param name="pItem">The removed item.</param>
        /// <param name="pOldIndex">The old index of the item.</param>
        /// <param name="pNewIndex">THe new index of the item.</param>
        private void OnItemViewModelMoved(object pSender, IHierarchicalItemViewModel pItem, int pOldIndex, int pNewIndex)
        {
            if (pItem != null)
            {
                // Computing the row indexes.
                int lOldRowIndex = pOldIndex;
                int lNewRowIndex = pNewIndex;
                if (pItem.Parent != null && pItem.Parent is IRootHierarchicalItemViewModel == false)
                {
                    int lParentRowIndex = this.Rows.IndexOf(pItem.Parent);
                    lOldRowIndex = lParentRowIndex + pOldIndex + 1;
                    lNewRowIndex = lParentRowIndex + pNewIndex + 1;
                }

                // Removing the items from the tree view if they are displayed.
                if (pItem.IsExpanded)
                {
                    this.DropChildrenItems(pItem, true);
                }
                else
                {
                    this.Rows.Remove(pItem);
                }

                // Adding the item in the rows.
                if (lNewRowIndex >= this.Rows.Count)
                {
                    this.Rows.Add(pItem);
                }
                else
                {
                    this.Rows.Insert(lNewRowIndex, pItem);
                }
            }
        }

        #endregion // Methods.
    }
}

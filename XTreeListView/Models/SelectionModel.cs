using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using XTreeListView.Gui;
using XTreeListView.ViewModel;

namespace XTreeListView.Models
{
    /// <summary>
    /// Class handling the selected items of the <see cref="ExtendedListView"/>
    /// </summary>
    public class SelectionModel
    {
        #region Fields

        /// <summary>
        /// Stores the parent of the selection model.
        /// </summary>
        private ExtendedListView mParent;

        /// <summary>
        /// Stores the list of the selected items.
        /// </summary>
        private ObservableCollection<IHierarchicalItemViewModel> mSelectedViewModels;

        /// <summary>
        /// Stores the flag indicating if the selection is allowed.
        /// </summary>
        private bool mCanSelect;

        #endregion // Fields.

        #region Properties

        /// <summary>
        /// Gets or sets the anchor used to select a set of items.
        /// </summary>
        public IHierarchicalItemViewModel Anchor
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the selected items.
        /// </summary>
        public IEnumerable<IHierarchicalItemViewModel> SelectedViewModels
        {
            get
            {
                return this.mSelectedViewModels;
            }
        }

        /// <summary>
        /// Gets the selected objects.
        /// </summary>
        public IEnumerable<object> SelectedObjects
        {
            get
            {
                return this.mSelectedViewModels.Select(lItem => lItem.UntypedOwnedObject).ToArray();
            }
        }

        /// <summary>
        /// Gets or sets the flag indicating the item selection mode.
        /// </summary>
        public TreeSelectionMode SelectionMode
        {
            get
            {
                if (this.mCanSelect == false)
                {
                    return TreeSelectionMode.NoSelection;
                }
                else
                {
                    if (this.mParent.SelectionMode == System.Windows.Controls.SelectionMode.Extended)
                    {
                        return TreeSelectionMode.MultiSelection;
                    }
                    else
                    {
                        return TreeSelectionMode.SingleSelection;
                    }
                }
            }
            set
            {
                switch (value)
                {
                    case TreeSelectionMode.NoSelection:
                        {
                            this.mCanSelect = false;
                        }
                        break;
                    case TreeSelectionMode.SingleSelection:
                        {
                            this.mCanSelect = true;
                            this.mParent.SelectionMode = System.Windows.Controls.SelectionMode.Single;
                        }
                        break;
                    case TreeSelectionMode.MultiSelection:
                        {
                            this.mCanSelect = true;
                            this.mParent.SelectionMode = System.Windows.Controls.SelectionMode.Extended;
                        }
                        break;
                }
            }
        }

        #endregion // Properties.

        #region Events

        /// <summary>
        /// Event raised when the selection is modified.
        /// </summary>
        public event SelectionChangedEventHandler SelectionChanged;

        #endregion // Events.

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectionModel"/> class.
        /// </summary>
        /// <param name="pParent">The model parent.</param>
        public SelectionModel(ExtendedListView pParent)
        {
            this.mParent = pParent;
            this.mCanSelect = true;
            this.mSelectedViewModels = new ObservableCollection<IHierarchicalItemViewModel>();

            this.Anchor = null;
        }

        #endregion // Constructors.

        #region Methods

        /// <summary>
        /// Select the given item.
        /// </summary>
        /// <param name="pItem">The selected item.</param>
        public void Select(IHierarchicalItemViewModel pItem)
        {
            if (this.SelectionMode != TreeSelectionMode.NoSelection)
            {
                this.InternalSelect(pItem, true);
            }
        }

        /// <summary>
        /// Adds the given to the selection.
        /// </summary>
        /// <param name="pItem">The item to add to the selection.</param>
        public void AddToSelection(IHierarchicalItemViewModel pItem)
        {
            if (this.SelectionMode != TreeSelectionMode.NoSelection)
            {
                this.InternalAddToSelection(pItem, true, true);
            }
        }

        /// <summary>
        /// Selects the given items.
        /// </summary>
        /// <param name="pItems">The selected items.</param>
        public void Select(IEnumerable<IHierarchicalItemViewModel> pItems)
        {
            if (this.SelectionMode != TreeSelectionMode.NoSelection)
            {
                this.InternalSelect(pItems, true, true);
            }
        }

        /// <summary>
        /// Selects a range of items.
        /// </summary>
        /// <param name="pFrom">The range start view model.</param>
        /// <param name="pTo">The range stop view model.</param>
        public void SelectRange(IHierarchicalItemViewModel pFrom, IHierarchicalItemViewModel pTo)
        {
            if (this.SelectionMode != TreeSelectionMode.NoSelection)
            {
                this.InternalSelectRange(pFrom, pTo, true);
            }
        }

        /// <summary>
        /// Select all the items.
        /// </summary>
        public void SelectAll()
        {
            if (this.SelectionMode != TreeSelectionMode.NoSelection)
            {
                this.InternalSelectAll(true);
            }
        }

        /// <summary>
        /// Unselect the given item.
        /// </summary>
        /// <param name="pItem">The item to unselect.</param>
        /// <param name="pUnselectChildren">Unselect the children as well.</param>
        public void Unselect(IHierarchicalItemViewModel pItem, bool pUnselectChildren)
        {
            if (pUnselectChildren)
            {
                this.InternalUnselectAndChildren(pItem, true);
            }
            else
            {
                this.InternalUnselect(pItem, true);
            }
        }

        /// <summary>
        /// Unselect all the selected items.
        /// </summary>
        public void UnselectAll()
        {
            this.InternalUnselectAll(true, true);
        }

        /// <summary>
        /// Select the given item.
        /// </summary>
        /// <param name="pItem">The selected item.</param>
        /// <param name="pNotify">Flag defining if the notification must be done.</param>
        private void InternalSelect(IHierarchicalItemViewModel pItem, bool pNotify)
        {
            if (pItem.CanBeSelected && (pItem.IsSelected == false || this.mSelectedViewModels.Count > 1))
            {
                // Update.
                IHierarchicalItemViewModel[] lOldSelection = this.SelectedViewModels.ToArray();
                this.InternalUnselectAll(true, false);
                this.InternalAddToSelection(pItem, true, false);

                // Notification.
                if (pNotify)
                {
                    this.NotifySelectionChanged(lOldSelection, new IHierarchicalItemViewModel[] { pItem });
                }
            }
        }

        /// <summary>
        /// Adds the given to the selection.
        /// </summary>
        /// <param name="pItem">The item to add to the selection.</param>
        /// <param name="pUpdatePivot">Flag to know if the pivot must be updated.</param>
        /// <param name="pNotify">Flag defining if the notification must be done.</param>
        private void InternalAddToSelection(IHierarchicalItemViewModel pItem, bool pUpdatePivot, bool pNotify)
        {
            if (pItem.CanBeSelected && pItem.IsSelected == false)
            {
                // Updating view model.
                pItem.IsSelected = true;

                // Updating the selected items list.
                this.mSelectedViewModels.Add(pItem);

                // Setting the pivot.
                if (pUpdatePivot)
                {
                    this.Anchor = pItem;
                }

                // Updating native selection handling.
                if (this.SelectionMode == TreeSelectionMode.SingleSelection)
                {
                    this.mParent.SelectedItem = pItem;
                }
                else
                {
                    this.mParent.SelectedItems.Add(pItem);
                }

                // Notification.
                if (pNotify)
                {
                    this.NotifySelectionChanged(new IHierarchicalItemViewModel[] { }, new IHierarchicalItemViewModel[] { pItem });
                }
            }
        }

        /// <summary>
        /// Selects the given items.
        /// </summary>
        /// <param name="pItems">The selected items.</param>
        /// <param name="pUpdatePivot">Flag to know if the pivot must be updated.</param>
        /// <param name="pNotify">Flag defining if the notification must be done.</param>
        private void InternalSelect(IEnumerable<IHierarchicalItemViewModel> pItems, bool pUpdatePivot, bool pNotify)
        {
            if (this.SelectionMode != TreeSelectionMode.SingleSelection || pItems.Count() == 1)
            {
                IHierarchicalItemViewModel[] lSelectableItems = pItems.Where(lItem => lItem.CanBeSelected).ToArray();
                IHierarchicalItemViewModel[] lUnselectedItems = lSelectableItems.Where(lItem => lItem.IsSelected == false).ToArray();
                if (lUnselectedItems.Any() || this.mParent.SelectedItems.Count != lSelectableItems.Count())
                {
                    // Update.
                    IHierarchicalItemViewModel[] lOldSelection = this.SelectedViewModels.ToArray();
                    this.InternalUnselectAll(pUpdatePivot, false);
                    foreach (IHierarchicalItemViewModel lItem in pItems)
                    {
                        this.InternalAddToSelection(lItem, pUpdatePivot, false);
                    }

                    // Notification.
                    if (pNotify)
                    {
                        IHierarchicalItemViewModel[] lCommonItems = lOldSelection.Intersect(pItems).ToArray();
                        IHierarchicalItemViewModel[] lItemsToRemove = lOldSelection.Except(lCommonItems).ToArray();
                        IHierarchicalItemViewModel[] lItemsToAdd = pItems.Except(lCommonItems).ToArray();

                        this.NotifySelectionChanged(lItemsToRemove, lItemsToAdd);
                    }
                }
            }
        }

        /// <summary>
        /// Selects a range of items.
        /// </summary>
        /// <param name="pFrom">The range start view model.</param>
        /// <param name="pTo">The range stop view model.</param>
        /// <param name="pNotify">Flag defining if the notification must be done.</param>
        private void InternalSelectRange(IHierarchicalItemViewModel pFrom, IHierarchicalItemViewModel pTo, bool pNotify)
        {
            if (this.SelectionMode != TreeSelectionMode.SingleSelection)
            {
                // Getting the indexes of the selection range.
                int lFromIndex = this.mParent.Rows.IndexOf(pFrom);
                int lToIndex = this.mParent.Rows.IndexOf(pTo);
                if (lFromIndex > lToIndex)
                {
                    // Swap values.
                    int lTemp = lFromIndex;
                    lFromIndex = lToIndex;
                    lToIndex = lTemp;
                }

                // Building the list of items to select.
                List<IHierarchicalItemViewModel> lItemsToSelect = new List<IHierarchicalItemViewModel>();
                for (int lIndex = lFromIndex; lIndex <= lToIndex; lIndex++)
                {
                    IHierarchicalItemViewModel lItem = this.mParent.Rows.ElementAtOrDefault(lIndex);
                    if (lItem != null)
                    {
                        lItemsToSelect.Add(lItem);
                    }
                }

                if (lItemsToSelect.Any())
                {
                    // Update.
                    this.InternalSelect(lItemsToSelect, false, pNotify);
                }
            }
        }

        /// <summary>
        /// Select all the items.
        /// </summary>
        /// <param name="pNotify">Flag defining if the notification must be done.</param>
        private void InternalSelectAll(bool pNotify)
        {
            if (this.SelectionMode != TreeSelectionMode.SingleSelection)
            {
                // Updating view model.
                List<IHierarchicalItemViewModel> lAddedItems = new List<IHierarchicalItemViewModel>();
                foreach (IHierarchicalItemViewModel lItem in this.mParent.ViewModel.Children)
                {
                    lAddedItems.AddRange(lItem.SelectAll());
                }

                if (lAddedItems.Any())
                {
                    // Updating the selected items list and native selection handling.
                    foreach (IHierarchicalItemViewModel lItem in lAddedItems)
                    {
                        this.mParent.SelectedItems.Add(lItem);
                        this.mSelectedViewModels.Add(lItem);
                    }

                    // Notification.
                    if (pNotify)
                    {
                        this.NotifySelectionChanged(new IHierarchicalItemViewModel[] { }, lAddedItems.ToArray());
                    }
                }
            }
        }

        /// <summary>
        /// Unselect the given item.
        /// </summary>
        /// <param name="pItem">The item to unselect.</param>
        /// <param name="pNotify">Flag defining if the notification must be done.</param>
        private void InternalUnselect(IHierarchicalItemViewModel pItem, bool pNotify)
        {
            if (pItem.IsSelected)
            {
                // Updating view model.
                pItem.IsSelected = false;

                // Updating the selected items list.
                this.mSelectedViewModels.Remove(pItem);

                // Updating the pivot.
                if (this.Anchor == pItem)
                {
                    this.Anchor = null;
                }

                // Updating native selection handling.
                if (this.SelectionMode == TreeSelectionMode.SingleSelection)
                {
                    this.mParent.SelectedItem = null;
                }
                else
                {
                    this.mParent.SelectedItems.Remove(pItem);
                }

                // Notification.
                if (pNotify)
                {
                    this.NotifySelectionChanged(new IHierarchicalItemViewModel[] { pItem }, new IHierarchicalItemViewModel[] { });
                }
            }
        }

        /// <summary>
        /// Unselect the given item and all its children.
        /// </summary>
        /// <param name="pItem">The item to unselect.</param>
        /// <param name="pNotify">Flag defining if the notification must be done.</param>
        private void InternalUnselectAndChildren(IHierarchicalItemViewModel pItem, bool pNotify)
        {
            if (pItem.IsSelected)
            {
                IHierarchicalItemViewModel[] lOldSelection = this.SelectedViewModels.ToArray();
                List<IHierarchicalItemViewModel> lRemovedItems = new List<IHierarchicalItemViewModel>();
                lRemovedItems.AddRange(pItem.UnSelectAll());

                if (lRemovedItems.Any())
                {
                    // Updating the selected items list.
                    foreach (IHierarchicalItemViewModel lItem in lRemovedItems)
                    {
                        this.mSelectedViewModels.Remove(lItem);
                    }

                    // Updating the pivot.
                    if (lRemovedItems.Contains(this.Anchor))
                    {
                        this.Anchor = null;
                    }

                    // Updating native selection handling.
                    if (this.SelectionMode == TreeSelectionMode.SingleSelection)
                    {
                        this.mParent.SelectedItem = null;
                    }
                    else
                    {
                        foreach (IHierarchicalItemViewModel lItem in lRemovedItems)
                        {
                            this.mParent.SelectedItems.Remove(lItem);
                        }
                    }

                    // Notification.
                    if (pNotify)
                    {
                        this.NotifySelectionChanged(lRemovedItems.ToArray(), new IHierarchicalItemViewModel[] { });
                    }
                }
            }
        }

        /// <summary>
        /// Unselect all the selected items.
        /// </summary>
        /// <param name="pCleanPivot">Flag defining if the pivot must be cleaned.</param>
        /// <param name="pNotify">Flag defining if the notification must be done.</param>
        private void InternalUnselectAll(bool pCleanPivot, bool pNotify)
        {
            if (this.SelectedViewModels.Any())
            {
                // Updating view model.
                IHierarchicalItemViewModel[] lOldSelection = this.SelectedViewModels.ToArray();
                foreach (IHierarchicalItemViewModel lItem in lOldSelection)
                {
                    lItem.UnSelectAll();
                }

                // Updating the selected items list.
                this.mSelectedViewModels.Clear();

                // Updating the pivot.
                if (pCleanPivot)
                {
                    this.Anchor = null;
                }

                // Updating native selection handling.
                if (this.SelectionMode == TreeSelectionMode.SingleSelection)
                {
                    this.mParent.SelectedItem = null;
                }
                else
                {
                    this.mParent.SelectedItems.Clear();
                }

                // Notification.
                if (pNotify)
                {
                    this.NotifySelectionChanged(lOldSelection, new IHierarchicalItemViewModel[] { });
                }
            }
        }

        /// <summary>
        /// Notifies a selection modification.
        /// </summary>
        /// <param name="pRemovedItems">The items removed from the selection.</param>
        /// <param name="pAddedItems">The items added to the selection.</param>
        private void NotifySelectionChanged(IHierarchicalItemViewModel[] pRemovedItems, IHierarchicalItemViewModel[] pAddedItems)
        {
            if (this.SelectionChanged != null)
            {
                SelectionChangedEventArgs lArgs = new SelectionChangedEventArgs(pRemovedItems, pAddedItems);
                this.SelectionChanged(this, lArgs);
            }
        }

        #endregion // Methods.
    }
}

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
        private ObservableCollection<IHierarchicalItemViewModel> mSelectedItemsViewModel;

        /// <summary>
        /// Stores the flag indicating if the selection is allowed.
        /// </summary>
        private bool mCanSelect;

        #endregion // Fields.

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectionModel"/> class.
        /// </summary>
        /// <param name="pParent">The model parent.</param>
        public SelectionModel(ExtendedListView pParent)
        {
            this.mParent = pParent;
            this.mCanSelect = true;
            this.mSelectedItemsViewModel = new ObservableCollection<IHierarchicalItemViewModel>();
        }

        #endregion // Constructors.

        #region Events

        /// <summary>
        /// Event raised when the selection is modified.
        /// </summary>
        public event SelectionChangedEventHandler SelectionChanged;

        #endregion // Events.

        #region Properties

        /// <summary>
        /// Gets the selected items.
        /// </summary>
        public IEnumerable<IHierarchicalItemViewModel> SelectedItemsViewModel
        {
            get
            {
                return this.mSelectedItemsViewModel;
            }
        }

        /// <summary>
        /// Gets the selected objects.
        /// </summary>
        public IEnumerable<object> SelectedObjects
        {
            get
            {
                return this.mSelectedItemsViewModel.Select(lItem => lItem.Selection).ToArray();
            }
        }

        /// <summary>
        /// Gets or sets the flag indicating the item selection mode.
        /// </summary>
        public TreeSelectionOptions SelectionOption
        {
            get
            {
                if (this.mCanSelect == false)
                {
                    return TreeSelectionOptions.NoSelection;
                }
                else
                {
                    if (this.mParent.SelectionMode == System.Windows.Controls.SelectionMode.Extended)
                    {
                        return TreeSelectionOptions.MultiSelection;
                    }
                    else
                    {
                        return TreeSelectionOptions.SingleSelection;
                    }
                }
            }
            set
            {
                switch (value)
                {
                    case TreeSelectionOptions.NoSelection:
                        {
                            this.mCanSelect = false;
                        }
                        break;
                    case TreeSelectionOptions.SingleSelection:
                        {
                            this.mCanSelect = true;
                            this.mParent.SelectionMode = System.Windows.Controls.SelectionMode.Single;
                        }
                        break;
                    case TreeSelectionOptions.MultiSelection:
                        {
                            this.mCanSelect = true;
                            this.mParent.SelectionMode = System.Windows.Controls.SelectionMode.Extended;
                        }
                        break;
                }
            }
        }

        #endregion // Properties.

        #region Methods

        /// <summary>
        /// Select the given item.
        /// </summary>
        /// <param name="pItem">The selected item.</param>
        public void Select(IHierarchicalItemViewModel pItem)
        {
            if (this.SelectionOption != TreeSelectionOptions.NoSelection)
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
            if (this.SelectionOption != TreeSelectionOptions.NoSelection)
            {
                this.InternalAddToSelection(pItem, true);
            }
        }

        /// <summary>
        /// Select all the items.
        /// </summary>
        public void SelectAll()
        {
            if (this.SelectionOption != TreeSelectionOptions.NoSelection)
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
            this.InternalUnselectAll(true);
        }

        /// <summary>
        /// Select the given item.
        /// </summary>
        /// <param name="pItem">The selected item.</param>
        /// <param name="pNotify">Flag defining if the notification must be done.</param>
        private void InternalSelect(IHierarchicalItemViewModel pItem, bool pNotify)
        {
            if (pItem.CanBeSelected && (pItem.IsSelected == false || this.mSelectedItemsViewModel.Count > 1))
            {
                // Update.
                IHierarchicalItemViewModel[] lOldSelection = this.SelectedItemsViewModel.ToArray();
                this.InternalUnselectAll(false);
                this.InternalAddToSelection(pItem, false);

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
        /// <param name="pNotify">Flag defining if the notification must be done.</param>
        private void InternalAddToSelection(IHierarchicalItemViewModel pItem, bool pNotify)
        {
            if (pItem.CanBeSelected && pItem.IsSelected == false)
            {
                // Updating view model.
                pItem.IsSelected = true;

                // Updating the selected items list.
                this.mSelectedItemsViewModel.Add(pItem);

                // Updating native selection handling.
                if (this.mParent.SelectionMode == System.Windows.Controls.SelectionMode.Single)
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
        /// Select all the items.
        /// </summary>
        /// <param name="pNotify">Flag defining if the notification must be done.</param>
        private void InternalSelectAll(bool pNotify)
        {
            if (this.mParent.SelectionMode != System.Windows.Controls.SelectionMode.Single)
            {
                // Updating view model.
                IHierarchicalItemViewModel[] lOldSelection = this.SelectedItemsViewModel.ToArray();
                List<IHierarchicalItemViewModel> lAddedItems = new List<IHierarchicalItemViewModel>();
                foreach (IHierarchicalItemViewModel lItem in this.mParent.ViewModel.ViewModel)
                {
                    lAddedItems.AddRange(lItem.SelectAll());
                }

                if (lAddedItems.Any())
                {
                    // Updating the selected items list and native selection handling.
                    foreach (IHierarchicalItemViewModel lItem in lAddedItems)
                    {
                        this.mParent.SelectedItems.Add(lItem);
                        this.mSelectedItemsViewModel.Add(lItem);
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
                this.mSelectedItemsViewModel.Remove(pItem);

                // Updating native selection handling.
                if (this.mParent.SelectionMode == System.Windows.Controls.SelectionMode.Single)
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
                IHierarchicalItemViewModel[] lOldSelection = this.SelectedItemsViewModel.ToArray();
                List<IHierarchicalItemViewModel> lRemovedItems = new List<IHierarchicalItemViewModel>();
                lRemovedItems.AddRange(pItem.UnSelectAll());

                if (lRemovedItems.Any())
                {
                    // Updating the selected items list.
                    foreach (IHierarchicalItemViewModel lItem in lRemovedItems)
                    {
                        this.mSelectedItemsViewModel.Remove(lItem);
                    }

                    // Updating native selection handling.
                    if (this.mParent.SelectionMode == System.Windows.Controls.SelectionMode.Single)
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
        /// <param name="pNotify">Flag defining if the notification must be done.</param>
        private void InternalUnselectAll(bool pNotify)
        {
            if (this.SelectedItemsViewModel.Any())
            {
                // Updating view model.
                IHierarchicalItemViewModel[] lOldSelection = this.SelectedItemsViewModel.ToArray();
                foreach (IHierarchicalItemViewModel lItem in lOldSelection)
                {
                    lItem.UnSelectAll();
                }

                // Updating the selected items list.
                this.mSelectedItemsViewModel.Clear();

                // Updating native selection handling.
                if (this.mParent.SelectionMode == System.Windows.Controls.SelectionMode.Single)
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

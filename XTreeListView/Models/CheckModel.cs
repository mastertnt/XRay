using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using XTreeListView.ViewModel;

namespace XTreeListView.Models
{
    /// <summary>
    /// Class handling the checked items in the ExtendedListView.
    /// </summary>
    public class CheckModel
    {
        #region Fields

        /// <summary>
        /// Stores the list of the selected items.
        /// </summary>
        private List<IHierarchicalItemViewModel> mCheckedItemsViewModel;

        #endregion // Fields.

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckModel"/> class.
        /// </summary>
        public CheckModel()
        {
            this.mCheckedItemsViewModel = new List<IHierarchicalItemViewModel>();
        }

        #endregion // Constructors.

        #region Events

        /// <summary>
        /// Event raised when an item gets toggled.
        /// </summary>
        public event TreeViewEventHandler ItemsViewModelToggled;

        #endregion // Events.

        #region Properties

        /// <summary>
        /// Gets the checked items.
        /// </summary>
        public IEnumerable<IHierarchicalItemViewModel> CheckedItemsViewModel
        {
            get
            {
                return this.mCheckedItemsViewModel;
            }
        }

        #endregion // Properties.

        #region Methods

        /// <summary>
        /// Check the item.
        /// </summary>
        /// <param name="pItem">The item to check.</param>
        /// <param name="pCheckChildren">Flag defining if the children have to be checked as well.</param>
        public void Check(IHierarchicalItemViewModel pItem, bool pCheckChildren)
        {
            if  (   (pItem.IsChecked == false && pItem.IsCheckable && pItem.IsCheckingEnabled)
                ||  (pCheckChildren)
                )
            {
                // Update.
                IHierarchicalItemViewModel[] lCheckedItems;
                if (pCheckChildren)
                {
                    lCheckedItems = pItem.CheckAll();
                }
                else
                {
                    pItem.IsChecked = true;
                    lCheckedItems = new IHierarchicalItemViewModel[] { pItem };
                }

                if (lCheckedItems.Any())
                {
                    this.mCheckedItemsViewModel.AddRange(lCheckedItems);

                    // Notification.
                    this.NotifyItemsToggled(lCheckedItems);
                }
            }
        }

        /// <summary>
        /// Uncheck the item.
        /// </summary>
        /// <param name="pItem">The item to uncheck.</param>
        /// <param name="pUncheckChildren">Flag defining if the children have to be unchecked as well.</param>
        public void Uncheck(IHierarchicalItemViewModel pItem, bool pUncheckChildren)
        {
            if  (   (pItem.IsChecked && pItem.IsCheckable && pItem.IsCheckingEnabled)
                ||  (pUncheckChildren)
                )
            {
                // Update.
                IHierarchicalItemViewModel[] lUncheckedItems;
                if (pUncheckChildren)
                {
                    lUncheckedItems = pItem.UncheckAll();
                }
                else
                {
                    pItem.IsChecked = false;
                    lUncheckedItems = new IHierarchicalItemViewModel[] { pItem };
                }

                if (lUncheckedItems.Any())
                {
                    foreach (IHierarchicalItemViewModel lItem in lUncheckedItems)
                    {
                        this.mCheckedItemsViewModel.Remove(lItem);
                    }

                    // Notification.
                    this.NotifyItemsToggled(lUncheckedItems);
                }
            }
        }

        /// <summary>
        /// Notifies a check modification.
        /// </summary>
        /// <param name="pToggledItem">The toogled item.</param>
        private void NotifyItemToggled(IHierarchicalItemViewModel pToggledItem)
        {
            if (this.ItemsViewModelToggled != null)
            {
                this.ItemsViewModelToggled(this, new IHierarchicalItemViewModel[] { pToggledItem });
            }
        }

        /// <summary>
        /// Notifies a check modification.
        /// </summary>
        /// <param name="pToggledItem">The toogled item.</param>
        private void NotifyItemsToggled(IHierarchicalItemViewModel[] pToggledItem)
        {
            if (this.ItemsViewModelToggled != null)
            {
                this.ItemsViewModelToggled(this, pToggledItem);
            }
        }

        #endregion // Methods.
    }
}

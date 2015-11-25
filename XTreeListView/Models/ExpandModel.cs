using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XTreeListView.Gui;
using XTreeListView.ViewModel;

namespace XTreeListView.Models
{
    /// <summary>
    /// Class handling the expand beahvior of the <see cref="ExtendedListView"/>.
    /// </summary>
    public class ExpandModel
    {
        #region Fields

        /// <summary>
        /// Stores the parent of the selection model.
        /// </summary>
        private ExtendedListView mParent;

        /// <summary>
        /// Stores the watchdog preventing for reentrancy.
        /// </summary>
        private bool mIsProcessingExpand;

        #endregion // Fields.

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpandModel"/> class.
        /// </summary>
        /// <param name="pParent">The model parent.</param>
        public ExpandModel(ExtendedListView pParent)
        {
            this.mParent = pParent;
        }

        #endregion // Constructors.

        #region Methods

        /// <summary>
        /// This method expands a node.
        /// </summary>
        /// <param name="pItem">The target node.</param>
        /// <param name="pValue">True to set the node as expanded.</param>
        public void SetIsExpanded(IHierarchicalItemViewModel pItem, bool pValue)
        {
            if (this.BeginProcessingExpand())
            {
                if (pItem.HasChildren)
                {
                    if (pValue)
                    {
                        pItem.IsExpanded = pValue;
                        this.mParent.LoadsChildrenItems(pItem);
                    }
                    else
                    {
                        // When collapsed, if any child is selected, then all the item are unselected and the collapsed item is selected.
                        if (this.mParent.SelectionModel.SelectedItemsViewModel.Any(lSelectedItem => pItem.AllVisibleChildren.Contains(lSelectedItem)))
                        {
                            this.mParent.SelectionModel.Select(pItem);
                        }

                        this.mParent.DropChildrenItems(pItem, false);
                        pItem.IsExpanded = pValue;
                    }
                }

                this.EndProcessingExpand();
            }
        }

        /// <summary>
        /// Toggles the expand state of the given item.
        /// </summary>
        /// <param name="pItem">The item to toggle expand.</param>
        public void ToggleExpand(IHierarchicalItemViewModel pItem)
        {
            if (pItem.HasChildren)
            {
                if (pItem.IsExpanded == false)
                {
                    this.SetIsExpanded(pItem, true);
                }
                else
                {
                    this.SetIsExpanded(pItem, false);
                }
            }
        }

        /// <summary>
        /// Allows the expand process by chacking the reentrancy. 
        /// </summary>
        /// <returns>True if the expand can be processed, false otherwise.</returns>
        private bool BeginProcessingExpand()
        {
            if (this.mIsProcessingExpand == false)
            {
                this.mIsProcessingExpand = true;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Notifies the end of the expand processing.
        /// </summary>
        private void EndProcessingExpand()
        {
            this.mIsProcessingExpand = false;
        }

        #endregion // Methods.
    }
}

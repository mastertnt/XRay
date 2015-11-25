using XTreeListView.Gui;
using XTreeListView.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XTreeListView.Behaviors
{
    /// <summary>
    /// Class handling the user selection behavior of an extended list view.
    /// </summary>
    public class ExpandBehavior
    {
        #region Fields

        /// <summary>
        /// Stores the handled list view.
        /// </summary>
        private ExtendedListView mParent;

        #endregion // Fields.

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpandBehavior"/> class.
        /// </summary>
        /// <param name="pParent">The behavior's parent.</param>
        public ExpandBehavior(ExtendedListView pParent)
        {
            this.mParent = pParent;
        }

        #endregion // Constructors.

        #region Methods

        /// <summary>
        /// Delegate called when a key is pressed when the item get the focus.
        /// </summary>
        /// <param name="pItem">The key up item.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        public void OnItemKeyUp(IHierarchicalItemViewModel pItem, System.Windows.Input.KeyEventArgs pEventArgs)
        {
            if (pEventArgs.KeyboardDevice.Modifiers == System.Windows.Input.ModifierKeys.None)
            {
                // Handling the selection when right arrow key is pressed.
                if (pEventArgs.Key == System.Windows.Input.Key.Right)
                {
                    if (pItem != null)
                    {
                        if (pItem.HasChildren)
                        {
                            if (pItem.IsExpanded)
                            {
                                // If not expanded, just expand it.
                                this.mParent.ExpandModel.SetIsExpanded(pItem, true);
                            }
                            else
                            {
                                // Otherwise, selecting the first child if any.
                                IHierarchicalItemViewModel lFirstChild = pItem.Children.FirstOrDefault();
                                if (lFirstChild != null)
                                {
                                    this.mParent.ScrollIntoView(lFirstChild, true);
                                }
                            }
                        }
                    }
                }

                // Handling the selection when left arrow key is pressed.
                if (pEventArgs.Key == System.Windows.Input.Key.Left)
                {
                    if (this.mParent.SelectionModel.SelectedItemsViewModel.Count() > 1)
                    {
                        // Selecting the first element with no selected parent.
                        IHierarchicalItemViewModel lFoundItem = this.mParent.SelectionModel.SelectedItemsViewModel.FirstOrDefault(lItem => (lItem.Parent == null) || (lItem.Parent.IsSelected == false));
                        if (lFoundItem != null)
                        {
                            this.mParent.ScrollIntoView(lFoundItem, true);
                        }
                    }
                    else
                    {
                        if (pItem != null)
                        {
                            // This item is the only one selected.
                            if (pItem.IsExpanded)
                            {
                                this.mParent.ExpandModel.SetIsExpanded(pItem, false);
                            }
                            else if
                                (   (pItem.Parent != null)
                                &&  (pItem.Parent is IRootHierarchicalItemViewModel) == false
                                )
                            {
                                this.mParent.ScrollIntoView(pItem.Parent, true);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Delegate called when the item expander gets checked.
        /// </summary>
        /// <param name="pItem">The checked item.</param>
        public void OnItemExpanderChecked(IHierarchicalItemViewModel pItem)
        {
            this.mParent.ExpandModel.SetIsExpanded(pItem, true);
        }

        /// <summary>
        /// Delegate called when the item expander gets unchecked.
        /// </summary>
        /// <param name="pItem">The unchecked item.</param>
        public void OnItemExpanderUnchecked(IHierarchicalItemViewModel pItem)
        {
            this.mParent.ExpandModel.SetIsExpanded(pItem, false);
        }

        /// <summary>
        /// Delegate called when the mouse double clicked on this item.
        /// </summary>
        /// <param name="pItem">The double clicked item.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        public void OnItemMouseDoubleClicked(IHierarchicalItemViewModel pItem, System.Windows.Input.MouseButtonEventArgs pEventArgs)
        {
            this.mParent.ExpandModel.ToggleExpand(pItem);
        }

        #endregion // Methods.
    }
}

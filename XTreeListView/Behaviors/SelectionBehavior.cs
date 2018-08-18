using XTreeListView.Gui;
using XTreeListView.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace XTreeListView.Behaviors
{
    /// <summary>
    /// Class handling the user selection behavior of an extended list view.
    /// </summary>
    public class SelectionBehavior
    {
        #region Fields

        /// <summary>
        /// Stores the handled list view.
        /// </summary>
        private ExtendedListView mParent;

        #endregion // Fields.

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectionBehavior"/> class.
        /// </summary>
        /// <param name="pParent">The behavior's parent.</param>
        public SelectionBehavior(ExtendedListView pParent)
        {
            this.mParent = pParent;
        }

        #endregion // Constructors.

        #region Methods

        /// <summary>
        /// Delegate called when the mouse left button is down on an item.
        /// </summary>
        /// <param name="pItem">The clicked item.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        public void OnItemMouseLeftButtonDown(IHierarchicalItemViewModel pItem, System.Windows.Input.MouseButtonEventArgs pEventArgs)
        {
            if (System.Windows.Input.Keyboard.IsKeyDown(Key.LeftCtrl) || System.Windows.Input.Keyboard.IsKeyDown(Key.RightCtrl))
            {
                if (this.mParent.SelectionModel.SelectionMode == TreeSelectionMode.MultiSelection)
                {
                    if (pItem.CanBeSelected)
                    {
                        if (pItem.IsSelected == false)
                        {
                            this.mParent.SelectionModel.AddToSelection(pItem);
                        }
                        else
                        {
                            this.mParent.SelectionModel.Unselect(pItem, false);
                        }
                    }
                }
            }
            else if (System.Windows.Input.Keyboard.IsKeyDown(Key.LeftShift) || System.Windows.Input.Keyboard.IsKeyDown(Key.RightShift))
            {
                if (this.mParent.SelectionModel.SelectionMode == TreeSelectionMode.MultiSelection)
                {
                    if (pItem.CanBeSelected)
                    {
                        if (this.mParent.SelectionModel.Anchor == null)
                        {
                            this.mParent.SelectionModel.Select(pItem);
                        }
                        else
                        {
                            this.mParent.SelectionModel.SelectRange(this.mParent.SelectionModel.Anchor, pItem);
                        }
                    }
                }
            }
            else
            {
                // Default behavior.
                if (this.mParent.SelectionModel.SelectionMode == TreeSelectionMode.SingleSelection)
                {
                    if (pItem.CanBeSelected)
                    {
                        this.mParent.SelectionModel.Select(pItem);
                    }
                }
                else if (this.mParent.SelectionModel.SelectionMode == TreeSelectionMode.MultiSelection)
                {
                    if (pItem.CanBeSelected)
                    {
                        this.mParent.SelectionModel.Select(pItem);
                    }
                }
            }
        }

        /// <summary>
        /// Delegate called when the mouse right button is down on an item.
        /// </summary>
        /// <param name="pItem">The clicked item.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        public void OnItemMouseRightButtonDown(IHierarchicalItemViewModel pItem, System.Windows.Input.MouseButtonEventArgs pEventArgs)
        {
            if (pItem.CanBeSelected)
            {
                if (pItem.IsSelected == false)
                {
                    this.mParent.SelectionModel.Select(pItem);
                }
            }
            else
            {
                this.mParent.SelectionModel.UnselectAll();
            }
        }

        /// <summary>
        /// Delegate called when the mouse left button is up on an item.
        /// </summary>
        /// <param name="pItem">The clicked item.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        public void OnItemMouseLeftButtonUp(IHierarchicalItemViewModel pItem, System.Windows.Input.MouseButtonEventArgs pEventArgs)
        {
            if (    System.Windows.Input.Keyboard.Modifiers == System.Windows.Input.ModifierKeys.None
               &&   pItem.CanBeSelected)
            {
                if (pItem.IsSelected && this.mParent.SelectionModel.SelectedViewModels.Count() > 1)
                {
                    this.mParent.SelectionModel.Select(pItem);
                }
            }
        }

        /// <summary>
        /// Delegate called when the mouse right button is up on an item.
        /// </summary>
        /// <param name="pItem">The clicked item.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        public void OnItemMouseRightButtonUp(IHierarchicalItemViewModel pItem, System.Windows.Input.MouseButtonEventArgs pEventArgs)
        {
            // Nothing to do.
        }

        /// <summary>
        /// Delegate called when a click is performed in the tree list view.
        /// </summary>
        /// <param name="pEventArgs">The event arguments.</param>
        public void OnTreeListViewMouseDown(System.Windows.Input.MouseButtonEventArgs pEventArgs)
        {
            this.mParent.SelectionModel.UnselectAll();
        }

        #endregion // Methods.
    }
}

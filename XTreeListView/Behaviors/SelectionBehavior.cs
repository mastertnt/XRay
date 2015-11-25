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
            if (System.Windows.Input.Keyboard.Modifiers == System.Windows.Input.ModifierKeys.None)
            {
                if (this.mParent.SelectionModel.SelectionOption == TreeSelectionOptions.SingleSelection)
                {
                    this.mParent.SelectionModel.Select(pItem);
                }
                else if (this.mParent.SelectionModel.SelectionOption == TreeSelectionOptions.MultiSelection)
                {
                    this.mParent.SelectionModel.Select(pItem);
                }
            }
            else if (System.Windows.Input.Keyboard.Modifiers == System.Windows.Input.ModifierKeys.Control)
            {
                if (this.mParent.SelectionModel.SelectionOption == TreeSelectionOptions.MultiSelection)
                {
                    this.mParent.SelectionModel.AddToSelection(pItem);
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
            this.mParent.SelectionModel.Select(pItem);
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

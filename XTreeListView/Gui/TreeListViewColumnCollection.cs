using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace XTreeListView.Gui
{
    /// <summary>
    /// Class defining a set of <see cref="TreeListViewColumn"/>.
    /// </summary>
    public sealed class TreeListViewColumnCollection : ObservableCollection<TreeListViewColumn>
    {
        #region Fields

        /// <summary>
        /// Stores the collection owner.
        /// </summary>
        private TreeListView mOwner;

        #endregion // Fields.

        #region Properties

        /// <summary>
        /// Gets or sets the view.
        /// </summary>
        private ExtendedGridView View
        {
            get
            {
                if (this.mOwner.InnerListView != null)
                {
                    return this.mOwner.InnerListView.View;
                }

                return null;
            }

            set
            {
                if (this.mOwner.InnerListView == null)
                {
                    return;
                }

                if (this.mOwner.InnerListView.View != null)
                {
                    this.mOwner.InnerListView.View.Columns.CollectionChanged -= this.OnGridViewColumnsCollectionChanged;
                }

                this.mOwner.InnerListView.View = value;

                if (this.mOwner.InnerListView.View != null)
                {
                    this.mOwner.InnerListView.View.Columns.CollectionChanged += this.OnGridViewColumnsCollectionChanged;
                }
            }
        }

        #endregion // Properties.

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeListView"/> class.
        /// </summary>
        /// <param name="pOwner">The collection owner.</param>
        internal TreeListViewColumnCollection(TreeListView pOwner)
        {
            this.mOwner = pOwner;
        }

        #endregion // Constructors.

        #region Methods

        /// <summary>
        /// Inserts an item at the given index.
        /// </summary>
        /// <param name="pIndex">The index of the item.</param>
        /// <param name="pItem">The item to insert.</param>
        protected override void InsertItem(int pIndex, TreeListViewColumn pItem)
        {
            if (this.mOwner.InnerListView != null && this.Count == 0)
            {
                // The first column is added.
                this.View = new ExtendedGridView();
            }

            // Calling base method.
            base.InsertItem(pIndex, pItem);

            // Synchronizing the grid view.
            this.SynchronizeGridViewColumns();
        }

        /// <summary>
        /// Moves an item from the first to the second index position.
        /// </summary>
        /// <param name="pOldIndex">The old position.</param>
        /// <param name="pNewIndex">The new position.</param>
        protected override void MoveItem(int pOldIndex, int pNewIndex)
        {
            // Calling base method.
            base.MoveItem(pOldIndex, pNewIndex);

            // Synchronizing the grid view.
            this.SynchronizeGridViewColumns();
        }

        /// <summary>
        /// Removes the item stores at the given position.
        /// </summary>
        /// <param name="pIndex">The posiiton of the item to remove.</param>
        protected override void RemoveItem(int pIndex)
        {
            if (this.mOwner.InnerListView != null && this.Count == 1)
            {
                // The last column is removed.
                this.View = null;
            }

            // Calling base method.
            base.RemoveItem(pIndex);

            // Synchronizing the grid view.
            this.SynchronizeGridViewColumns();
        }

        /// <summary>
        /// Replaces the item stored at the given position.
        /// </summary>
        /// <param name="pIndex">The index of the item to replace.</param>
        /// <param name="pItem">The new item.</param>
        protected override void SetItem(int pIndex, TreeListViewColumn pItem)
        {
            // Calling base method.
            base.SetItem(pIndex, pItem);

            // Synchronizing the grid view.
            this.SynchronizeGridViewColumns();
        }

        /// <summary>
        /// Clears the collection.
        /// </summary>
        protected override void ClearItems()
        {
            // Calling base method.
            base.ClearItems();

            // Synchronizing the grid view.
            this.SynchronizeGridViewColumns();
        }

        /// <summary>
        /// Synchronizes the grid view columns.
        /// </summary>
        private void SynchronizeGridViewColumns()
        {
            if (this.View != null)
            {
                this.View.Columns.CollectionChanged -= this.OnGridViewColumnsCollectionChanged;
                this.View.SynchronizeColumns(this);
                this.View.Columns.CollectionChanged += this.OnGridViewColumnsCollectionChanged;
            }
        }

        /// <summary>
        /// Delegate called when the columns collection of the grid view is modified.
        /// </summary>
        /// <param name="pSender">The modified grid view.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private void OnGridViewColumnsCollectionChanged(object pSender, NotifyCollectionChangedEventArgs pEventArgs)
        {
            switch (pEventArgs.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    // Nothing to do.
                    break;

                case NotifyCollectionChangedAction.Remove:
                    // Nothing to do.
                    break;

                case NotifyCollectionChangedAction.Replace:
                    // Nothing to do.
                    break;

                case NotifyCollectionChangedAction.Move:
                    {
                        Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            // Updating this list.
                            this.Move(pEventArgs.OldStartingIndex, pEventArgs.NewStartingIndex);
                        }));
                    }

                    break;

                case NotifyCollectionChangedAction.Reset:
                    // Nothing to do.
                    break;
            }
        }

        /// <summary>
        /// Method called when the parent tree list view template is applied.
        /// </summary>
        internal void OnParentTreeListViewTemplateApplied()
        {
            if (this.Count > 0)
            {
                // Setting the grid view.
                this.View = new ExtendedGridView();

                // Synchronizing the grid view.
                this.SynchronizeGridViewColumns();
            }
        }

        #endregion // Methods.
    }
}

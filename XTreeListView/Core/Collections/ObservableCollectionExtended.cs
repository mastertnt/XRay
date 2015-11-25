using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace XTreeListView.Core.Collections
{
    /// <summary>
    /// This class extends observable collection to add useful methods.
    /// </summary>
    public class ObservableCollectionExtended<T> : ObservableCollection<T>
    {
        #region Methods

        /// <summary>
        /// This method removes a range of items.
        /// </summary>
        /// <param name="pStartIndex">The start index of the deletion.</param>
        /// <param name="pItemCount">The number of elements to remove.</param>
        public void RemoveRange(int pStartIndex, int pItemCount)
        {
            this.CheckReentrancy();
            List<T> lItems = this.Items as List<T>;
            lItems.RemoveRange(pStartIndex, pItemCount);
            this.NotifyReset();
        }

        /// <summary>
        /// This method inserts items at a given index.
        /// </summary>
        /// <param name="pStartIndex">The start index of the insertion.</param>
        /// <param name="pItems">The items to add.</param>
        public void InsertRange(int pStartIndex, IEnumerable<T> pItems)
        {
            this.CheckReentrancy();
            List<T> lItems = this.Items as List<T>;
            lItems.InsertRange(pStartIndex, pItems);
            this.NotifyReset();
        }

        /// <summary>
        /// This method notifies a reset made on the collection.
        /// </summary>
        private void NotifyReset()
        {
            this.OnPropertyChanged(new PropertyChangedEventArgs("Count"));
            this.OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        #endregion // Methods.
    }
}

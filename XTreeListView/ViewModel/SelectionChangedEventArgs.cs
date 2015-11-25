using System;
using System.Collections;
using System.Collections.Generic;

namespace XTreeListView.ViewModel
{
    /// <summary>
    /// This class defines the arguments of the selection changed event.
    /// </summary>
    /// <!-- DPE -->
    public class SelectionChangedEventArgs : EventArgs
    {
        #region Fields

        /// <summary>
        /// Gets the list of added items compared from the previous selection.
        /// </summary>
        private IEnumerable mAddedItems;

        /// <summary>
        /// Gets the list of removed items compared from the previous selection.
        /// </summary>
        private IEnumerable mRemovedItems;

        #endregion // Fields.

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectionChangedEventArgs"/> class.
        /// </summary>
        /// <param name="pAddedItems">The added items from the previous selection.</param>
        /// <param name="pRemovedItems">The removed items from the previous selection.</param>
        public SelectionChangedEventArgs(IEnumerable pRemovedItems, IEnumerable pAddedItems)
        {
            this.mRemovedItems = pRemovedItems;
            this.mAddedItems = pAddedItems;
        }

        #endregion // Constructors.

        #region Properties

        /// <summary>
        /// Gets the list of added items compared from the previous selection.
        /// </summary>
        public IEnumerable<IViewModel> AddedItems
        {
            get
            {
                if
                    (this.mAddedItems != null)
                {
                    foreach
                        (IViewModel lItem in this.mAddedItems)
                    {
                        yield return lItem;
                    }
                }
            }
        }

        /// <summary>
        /// Gets the list of removed items compared from the previous selection.
        /// </summary>
        public IEnumerable<IViewModel> RemovedItems
        {
            get
            {
                if
                    (this.mRemovedItems != null)
                {
                    foreach
                        (IViewModel lItem in this.mRemovedItems)
                    {
                        yield return lItem;
                    }
                }
            }
        }

        #endregion // Properties.
    }
}

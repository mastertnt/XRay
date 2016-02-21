using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace XGraph.ViewModels
{
    /// <summary>
    /// Class defining a port view model collection.
    /// </summary>
    public class PortViewModelCollection : ObservableCollection<PortViewModel>
    {
        #region Fields

        /// <summary>
        /// Stores the node view model owner.
        /// </summary>
        private NodeViewModel mOwner;

        #endregion // Fields.

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PortViewModelCollection"/> class.
        /// </summary>
        /// <param name="pOwner">The collection owner.</param>
        public PortViewModelCollection(NodeViewModel pOwner)
        {
            this.mOwner = pOwner;
        }

        #endregion // Constructors.

        #region Methods

        /// <summary>
        /// Insert a new item in the collection.
        /// </summary>
        /// <param name="pIndex"></param>
        /// <param name="pItem"></param>
        protected override void InsertItem(int pIndex, PortViewModel pItem)
        {
            base.InsertItem(pIndex, pItem);

            pItem.ParentNode = this.mOwner;
        }

        /// <summary>
        /// Removes an item from the collection.
        /// </summary>
        /// <param name="pIndex">The index of the item to remove.</param>
        protected override void RemoveItem(int pIndex)
        {
            this[pIndex].ParentNode = null;

            base.RemoveItem(pIndex);
        }

        #endregion // Methods.
    }
}

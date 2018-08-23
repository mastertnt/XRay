using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using XTreeListView.ViewModel;

namespace XTreeListView.Gui
{
    /// <summary>
    /// Class defining the list view rows collection.
    /// </summary>
    public class RowsCollection : IList<IHierarchicalItemViewModel>
    {
        #region Fields

        /// <summary>
        /// Stores the collection owner.
        /// </summary>
        private ExtendedListView mOwner;

        /// <summary>
        /// Stores the source collection.
        /// </summary>
        private ObservableCollection<IHierarchicalItemViewModel> mSource;

        /// <summary>
        /// Stores the collection view source.
        /// </summary>
        private CollectionViewSource mViewSource;

        #endregion // Fields.

        #region Properties
        
        /// <summary>
        /// Gets or sets the item stores at the given index.
        /// </summary>
        /// <param name="pIndex">The item index.</param>
        /// <returns>The found item.</returns>
        public IHierarchicalItemViewModel this[int pIndex]
        {
            get
            {
                return this.mSource[pIndex];
            }

            set
            {
                this.mSource[pIndex] = value;
            }
        }

        /// <summary>
        /// Gets the number of items in the collection.
        /// </summary>
        public int Count
        {
            get
            {
                return this.mSource.Count;
            }
        }

        /// <summary>
        /// Gets the flag indicating if the collection is read only.
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        #endregion // Properties.

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RowsCollection"/> class.
        /// </summary>
        /// <param name="pOwner">The collection owner.</param>
        public RowsCollection(ExtendedListView pOwner)
        {
            this.mOwner = pOwner;

            // Creating the source collection.
            this.mSource = new ObservableCollection<IHierarchicalItemViewModel>();
            this.mViewSource = new CollectionViewSource();
            this.mViewSource.Source = this.mSource;

            // Bind it the owner items source property.
            Binding lItemsSourceBinding = new Binding();
            lItemsSourceBinding.Source = this.mViewSource;
            this.mOwner.SetBinding(ExtendedListView.ItemsSourceProperty, lItemsSourceBinding);
        }

        #endregion // Constructors.

        #region Methods

        /// <summary>
        /// Inserts items at a given index.
        /// </summary>
        /// <param name="pIndex">The start index of the insertion.</param>
        /// <param name="pCollection">The items to add.</param>
        public void InsertRange(int pIndex, IEnumerable<IHierarchicalItemViewModel> pCollection)
        {
            using (this.mViewSource.DeferRefresh())
            {
                using (IEnumerator<IHierarchicalItemViewModel> lEnumerator = pCollection.GetEnumerator())
                {
                    while (lEnumerator.MoveNext())
                    {
                        this.Insert(pIndex++, lEnumerator.Current);
                    }
                }
            }
        }

        /// <summary>
        /// Removes a range of items.
        /// </summary>
        /// <param name="pIndex">The start index of the deletion.</param>
        /// <param name="pCount">The number of elements to remove.</param>
        public void RemoveRange(int pIndex, int pCount)
        {
            if (pCount > 0)
            {
                using (this.mViewSource.DeferRefresh())
                {
                    int lRemovedCount = 0;
                    while (lRemovedCount != pCount)
                    {
                        this.RemoveAt(pIndex);
                        lRemovedCount++;
                    }
                }
            }
        }

        /// <summary>
        /// Returns the index of the item in the collection.
        /// </summary>
        /// <param name="pItem">The item of interest.</param>
        /// <returns>The item index.</returns>
        public int IndexOf(IHierarchicalItemViewModel pItem)
        {
            return this.mSource.IndexOf(pItem);
        }

        /// <summary>
        /// Inserts item at a given index.
        /// </summary>
        /// <param name="pIndex">The index of the insertion.</param>
        /// <param name="pItem">The item to add.</param>
        public void Insert(int pIndex, IHierarchicalItemViewModel pItem)
        {
            this.mSource.Insert(pIndex, pItem);
        }

        /// <summary>
        /// Removes the item stored at the given index.
        /// </summary>
        /// <param name="pIndex">The index of the item to remove.</param>
        public void RemoveAt(int pIndex)
        {
            this.mSource.RemoveAt(pIndex);
        }

        /// <summary>
        /// Adds an item to the collection.
        /// </summary>
        /// <param name="pItem">The added item.</param>
        public void Add(IHierarchicalItemViewModel pItem)
        {
            this.mSource.Add(pItem);
        }

        /// <summary>
        /// Clears the collection.
        /// </summary>
        public void Clear()
        {
            this.mSource.Clear();
        }

        /// <summary>
        /// Verifies if the item is in the collection.
        /// </summary>
        /// <param name="pItem">The item to verify.</param>
        /// <returns>True if the item is a part of the collection, false otherwise.</returns>
        public bool Contains(IHierarchicalItemViewModel pItem)
        {
            return this.mSource.Contains(pItem);
        }

        /// <summary>
        /// Copies the entire collection to a compatible one-dimensional System.Array, starting at the specified index of the target array.
        /// </summary>
        /// <param name="pArray">The target array.</param>
        /// <param name="pArrayIndex">The starting index in the array.</param>
        public void CopyTo(IHierarchicalItemViewModel[] pArray, int pArrayIndex)
        {
            this.mSource.CopyTo(pArray, pArrayIndex);
        }

        /// <summary>
        /// Removes the given item from the collection.
        /// </summary>
        /// <param name="pItem">The item to remove.</param>
        /// <returns>True if the item has been removed, false otherwise.</returns>
        public bool Remove(IHierarchicalItemViewModel pItem)
        {
            return this.mSource.Remove(pItem);
        }

        /// <summary>
        /// Returns the collection enumerator.
        /// </summary>
        /// <returns>The enumerator.</returns>
        public IEnumerator<IHierarchicalItemViewModel> GetEnumerator()
        {
            return this.mSource.GetEnumerator();
        }

        /// <summary>
        /// Returns the collection enumerator.
        /// </summary>
        /// <returns>The enumerator.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion // Methods.
    }
}

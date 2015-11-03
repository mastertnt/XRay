using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XSystem.Collections
{
    /// <summary>
    /// This class can be used to make a cache with fixed size.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class CacheDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        #region Fields

        /// <summary>
        /// This field stores the values.
        /// </summary>
        private readonly Dictionary<TKey, TValue> mInternal = new Dictionary<TKey, TValue>();

        /// <summary>
        /// This field stores the latest pKeys.
        /// </summary>
        private readonly List<TKey> mLatestKeys = new List<TKey>();

        #endregion // Fields.

        #region Properties

        /// <summary>
        /// Gets an <see cref="T:System.Collections.Generic.ICollection`1" /> containing the values in the <see cref="T:System.Collections.Generic.IDictionary`2" />.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.Generic.ICollection`1" /> containing the values in the object that implements <see cref="T:System.Collections.Generic.IDictionary`2" />.</returns>
        public ICollection<TValue> Values
        {
            get
            {
                return this.mInternal.Values;
            }
        }
        
        /// <summary>
        /// Gets or sets the element with the specified key.
        /// </summary>
        /// <param name="pKey">The key.</param>
        /// <returns></returns>
        public TValue this[TKey pKey]
        {
            get
            {
                return this.mInternal[pKey];
            }
            set
            {
                if (this.ContainsKey(pKey) == false)
                {
                    this.Add(pKey, value);
                }
                else
                {
                    this.mInternal[pKey] = value;

                    // Refresh the LRU keys.
                    this.mLatestKeys.Remove(pKey);
                    this.mLatestKeys.Add(pKey);
                }
            }
        }

        /// <summary>
        /// Gets an <see cref="T:System.Collections.Generic.ICollection`1" /> containing the keys of the <see cref="T:System.Collections.Generic.IDictionary`2" />.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.Generic.ICollection`1" /> containing the keys of the object that implements <see cref="T:System.Collections.Generic.IDictionary`2" />.</returns>
        public ICollection<TKey> Keys
        {
            get
            {
                return this.mInternal.Keys;
            }
        }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <returns>The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1" />.</returns>
        public int Count
        {
            get
            {
                return this.mInternal.Count;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only.
        /// </summary>
        /// <returns>true if the <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only; otherwise, false.</returns>
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the capacity.
        /// </summary>
        /// <value>
        /// The capacity.
        /// </value>
        public int Capacity
        {
            get; 
            private set;
        }

        #endregion // Properties.

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheDictionary{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="pCapacity">The maximum capacity.</param>
        public CacheDictionary(int pCapacity)
        {
            this.Capacity = pCapacity;
        }

        #endregion // Constructors.

        #region Methods

        /// <summary>
        /// Adds an element with the provided key and value to the <see cref="T:System.Collections.Generic.IDictionary`2" />.
        /// </summary>
        /// <param name="pKey">The object to use as the key of the element to add.</param>
        /// <param name="pValue">The object to use as the value of the element to add.</param>
        public void Add(TKey pKey, TValue pValue)
        {
            this.mLatestKeys.Add(pKey);
            this.mInternal.Add(pKey, pValue);
            if (this.mLatestKeys.Count > this.Capacity)
            {
                this.Remove(this.mLatestKeys.FirstOrDefault());
            }
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.IDictionary`2" /> contains an element with the specified key.
        /// </summary>
        /// <param name="pKey">The key to locate in the <see cref="T:System.Collections.Generic.IDictionary`2" />.</param>
        /// <returns>
        /// true if the <see cref="T:System.Collections.Generic.IDictionary`2" /> contains an element with the key; otherwise, false.
        /// </returns>
        public bool ContainsKey(TKey pKey)
        {
            return this.mInternal.ContainsKey(pKey);
        }

        /// <summary>
        /// Removes the element with the specified key from the <see cref="T:System.Collections.Generic.IDictionary`2" />.
        /// </summary>
        /// <param name="pKey">The key of the element to remove.</param>
        /// <returns>
        /// true if the element is successfully removed; otherwise, false.  This method also returns false if <paramref name="pKey" /> was not found in the original <see cref="T:System.Collections.Generic.IDictionary`2" />.
        /// </returns>
        public bool Remove(TKey pKey)
        {
            bool lResult = this.mInternal.Remove(pKey);
            if (lResult)
            {
                this.mLatestKeys.Remove(pKey);
            }
            return lResult;
        }

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <param name="pKey">The key whose value to get.</param>
        /// <param name="pValue">When this method returns, the value associated with the specified key, if the key is found; otherwise, the default value for the type of the <paramref name="pValue" /> parameter. This parameter is passed uninitialized.</param>
        /// <returns>
        /// true if the object that implements <see cref="T:System.Collections.Generic.IDictionary`2" /> contains an element with the specified key; otherwise, false.
        /// </returns>
        public bool TryGetValue(TKey pKey, out TValue pValue)
        {
            return this.mInternal.TryGetValue(pKey, out pValue);
        }


        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <param name="pItem">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        public void Add(KeyValuePair<TKey, TValue> pItem)
        {
            this.Add(pItem.Key, pItem.Value);
        }

        /// <summary>
        /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        public void Clear()
        {
            this.mInternal.Clear();
            this.mLatestKeys.Clear();
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1" /> contains a specific value.
        /// </summary>
        /// <param name="pItem">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        /// <returns>
        /// true if <paramref name="pItem" /> is found in the <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, false.
        /// </returns>
        public bool Contains(KeyValuePair<TKey, TValue> pItem)
        {
            return this.mInternal.Contains(pItem);
        }

        /// <summary>
        /// Copies to.
        /// </summary>
        /// <param name="pArray">The array.</param>
        /// <param name="pArrayIndex">Index of the array.</param>
        public void CopyTo(KeyValuePair<TKey, TValue>[] pArray, int pArrayIndex)
        {
            // Nothing to do.
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <param name="pItem">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        /// <returns>
        /// true if <paramref name="pItem" /> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, false. This method also returns false if <paramref name="pItem" /> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </returns>
        public bool Remove(KeyValuePair<TKey, TValue> pItem)
        {
            return this.Remove(pItem.Key);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return this.mInternal.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.mInternal.GetEnumerator();
        }

        #endregion // Methods.
    }
}

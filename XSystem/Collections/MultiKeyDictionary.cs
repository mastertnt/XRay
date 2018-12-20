using System.Collections.Generic;

namespace XSystem.Collections
{
    /// <summary>
    ///     This class implement a Dictionary that can reference several value for a unique key.
    /// </summary>
    /// <typeparam name="TKey">The key type.</typeparam>
    /// <typeparam name="TValue">The value type.</typeparam>
    public class MultiKeyDictionary<TKey, TValue> where TValue : class
    {
        #region Member

        /// <summary>
        ///     The multimap.
        /// </summary>
        private readonly Dictionary<TKey, List<TValue>> mMultiMap;

        #endregion // Member

        #region Properties

        /// <summary>
        ///     Returns the keys.
        /// </summary>
        public IEnumerable<TKey> Keys => this.mMultiMap.Keys;

        #endregion // Properties

        #region Methods

        /// <summary>
        ///     Constructor.
        /// </summary>
        public MultiKeyDictionary()
        {
            this.mMultiMap = new Dictionary<TKey, List<TValue>>();
        }

        /// <summary>
        ///     Add a value on the key reference.
        /// </summary>
        /// <param name="pKey">The key.</param>
        /// <param name="pValue">The added value.</param>
        public void Add(TKey pKey, TValue pValue)
        {
            List<TValue> lList;
            if (this.mMultiMap.TryGetValue(pKey, out lList))
            {
                if (pValue != null && !lList.Exists(pVal => pVal.Equals(pValue)))
                {
                    lList.Add(pValue);
                }
            }
            else
            {
                this.mMultiMap[pKey] = new List<TValue>();
                if (pValue != null)
                {
                    this.mMultiMap[pKey].Add(pValue);
                }
            }
        }

        /// <summary>
        ///     Removes the pair [key,value] from the dictionary.
        /// </summary>
        /// <param name="pKey">the key to remove.</param>
        /// <param name="pValue">the value to remove.</param>
        public void Remove(TKey pKey, TValue pValue)
        {
            if (this.ContainsKey(pKey))
            {
                this.mMultiMap[pKey].Remove(pValue);
            }
        }

        /// <summary>
        ///     Removes all pairs with the given key
        /// </summary>
        /// <param name="pKey">the key to remove.</param>
        public void Remove(TKey pKey)
        {
            if (this.ContainsKey(pKey))
            {
                this.mMultiMap.Remove(pKey);
            }
        }

        /// <summary>
        ///     Returns the values registered on a given key.
        /// </summary>
        /// <param name="pKey">The key.</param>
        /// <returns>The mapped values.</returns>
        public List<TValue> this[TKey pKey]
        {
            get
            {
                List<TValue> lList;
                if (this.mMultiMap.TryGetValue(pKey, out lList))
                {
                    return lList;
                }

                this.mMultiMap[pKey] = new List<TValue>();
                return this.mMultiMap[pKey];
            }
        }

        /// <summary>
        ///     This methods checks if a value is associated with a key.
        /// </summary>
        /// <param name="pKey">The key to look for.</param>
        /// <returns>True if a value is associated with the key.</returns>
        public bool ContainsKey(TKey pKey)
        {
            return this.mMultiMap.ContainsKey(pKey);
        }

        /// <summary>
        ///     Try to get the values associated to a key.
        /// </summary>
        /// <param name="pKey">The key</param>
        /// <param name="pValue">The associated values.</param>
        /// <returns>True if the value has been reached successfully.</returns>
        public bool TryGetValue(TKey pKey, out List<TValue> pValue)
        {
            return this.mMultiMap.TryGetValue(pKey, out pValue);
        }

        #endregion // Methods
    }
}
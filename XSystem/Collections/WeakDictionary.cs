using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace XSystem.Collections
{
    /// <summary>
    ///     This class implements a dictionary that contains weak reference to the values.
    ///     A weak reference is a reference on an object wich doesn't prevent the garbage
    ///     collector from delete the object.
    ///     This class can typically be used as a cache.
    ///     Once in a while the Add() method triggers a check on the values and removes
    ///     invalid (collected) WeakReferences from the dictionary.
    ///     Values are checked by small chunks of cNbItemsToCheck once every cNbAddBeforeCleanup
    ///     calls to Add()
    /// </summary>
    /// <typeparam name="TKey">The key type.</typeparam>
    /// <typeparam name="TValue">The value type.</typeparam>
    /// <!-- DPE -->
    public sealed class WeakDictionary<TKey, TValue>
    {
        #region Fields

        /// <summary>
        ///     The number of Add calls before triggering a full cleanup.
        /// </summary>
        private const int cNbAddBeforeFullCleanup = 10000;

        /// <summary>
        ///     The used dictionary.
        /// </summary>
        private readonly Dictionary<TKey, WeakReference> mCache;

        /// <summary>
        ///     The number of Add() calls since the last full cleanup.
        /// </summary>
        private int mNbAddSinceLastFullCleanup;

        /// <summary>
        ///     Use this as a lock to access the mCache member
        /// </summary>
        private readonly object mCacheLock = new object();

        /// <summary>
        ///     A reference to the cleanup thread
        /// </summary>
        private Thread mCleanupThread;

        /// <summary>
        ///     A reference to the event that triggers the cleanup
        /// </summary>
        private AutoResetEvent mCleanupEvent = new AutoResetEvent(false);

        /// <summary>
        ///     Gets a value indicating if the cleanup process is running
        /// </summary>
        private bool mIsCleaningUp;

        #endregion // Fields

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="WeakDictionary{TKey, TValue}" /> class.
        /// </summary>
        public WeakDictionary() : this(EqualityComparer<TKey>.Default)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="WeakDictionary{TKey, TValue}" /> class.
        /// </summary>
        /// <param name="pComparer">The key comparer used.</param>
        public WeakDictionary(IEqualityComparer<TKey> pComparer)
        {
            this.mCache = new Dictionary<TKey, WeakReference>(pComparer);
        }

        #endregion // Constructors

        #region Properties

        /// <summary>
        ///     Gets the keys list of the registered object.
        /// </summary>
        public List<TKey> Keys
        {
            get
            {
                List<TKey> lKeys;
                lock (this.mCacheLock)
                {
                    lKeys = this.mCache.Keys.ToList();
                }

                return lKeys;
            }
        }

        /// <summary>
        ///     Gets the value list of the registered object.
        /// </summary>
        public List<TValue> Values
        {
            get
            {
                List<TValue> lValues;
                lock (this.mCacheLock)
                {
                    lValues = (from lWeakReference in this.mCache.Values select (TValue) lWeakReference.Target).ToList();
                }

                return lValues;
            }
        }

        /// <summary>
        ///     Gets the key-value pairs in this dictionary
        /// </summary>
        public IEnumerable<KeyValuePair<TKey, WeakReference>> KeyValues
        {
            get
            {
                List<KeyValuePair<TKey, WeakReference>> lKeyValues;
                lock (this.mCacheLock)
                {
                    lKeyValues = this.mCache.ToList();
                }

                return lKeyValues;
            }
        }

        #endregion // Properties.

        #region Methods

        /// <summary>
        ///     Clears all entries of this dictionary
        /// </summary>
        public void Clear()
        {
            lock (this.mCacheLock)
            {
                this.mCache.Clear();
            }
        }

        /// <summary>
        ///     Adds a new pair in the dictionary.
        /// </summary>
        /// <param name="pKey">The added key.</param>
        /// <param name="pValue">The corresponding added value.</param>
        public void Add(TKey pKey, TValue pValue)
        {
            lock (this.mCacheLock)
            {
                this.mCache.Add(pKey, new WeakReference(pValue));
            }

            this.IncrementAddCount();
        }

        /// <summary>
        ///     Returns a value indicating wheather or not the key has already been registered in the dictionary
        /// </summary>
        /// <param name="pKey">The key</param>
        /// <returns>True if the key is in the dictionary, false otherwise.</returns>
        public bool ContainsKey(TKey pKey)
        {
            bool lContainsKey;
            lock (this.mCacheLock)
            {
                lContainsKey = this.mCache.ContainsKey(pKey);
            }

            return lContainsKey;
        }

        /// <summary>
        ///     Returns or modifies the value registered on a given key.
        /// </summary>
        /// <param name="pKey">The key.</param>
        /// <returns>The mapped values.</returns>
        public TValue this[TKey pKey]
        {
            get
            {
                var lValue = default(TValue);
                if (this.TryGetValue(pKey, out lValue))
                {
                    return lValue;
                }

                return default(TValue);
            }

            set
            {
                lock (this.mCacheLock)
                {
                    this.mCache[pKey] = new WeakReference(value);
                }

                this.IncrementAddCount();
            }
        }

        /// <summary>
        ///     Try to get the values associated to a key.
        /// </summary>
        /// <param name="pKey">The reference key.</param>
        /// <param name="pValue">The associated value.</param>
        /// <returns>True if the value has been reached successfully.</returns>
        public bool TryGetValue(TKey pKey, out TValue pValue)
        {
            WeakReference lWeakRef;
            bool lFoundKey;
            lock (this.mCacheLock)
            {
                lFoundKey = this.mCache.TryGetValue(pKey, out lWeakRef);
            }

            if (lFoundKey)
            {
                if (lWeakRef.IsAlive == false)
                {
                    pValue = default(TValue);
                    return false;
                }

                pValue = (TValue) lWeakRef.Target;
                return true;
            }

            pValue = default(TValue);
            return false;
        }

        /// <summary>
        ///     Removes the pair referenced by the given key from the map.
        /// </summary>
        /// <param name="pKey">The reference key.</param>
        /// <returns>True of the object as been removed, false otherwise.</returns>
        public bool Remove(TKey pKey)
        {
            bool lRemoved;
            lock (this.mCacheLock)
            {
                lRemoved = this.mCache.Remove(pKey);
            }

            return lRemoved;
        }

        /// <summary>
        ///     Increments the Add COunt and triggers the full cleanup if needed.
        /// </summary>
        private void IncrementAddCount()
        {
            // Perform a full cleanup in background if needed
            ++this.mNbAddSinceLastFullCleanup;
            if (this.mNbAddSinceLastFullCleanup > cNbAddBeforeFullCleanup)
            {
                this.mNbAddSinceLastFullCleanup = 0;
                if (this.mIsCleaningUp == false)
                {
                    this.TryToCleanup();
                }
            }
        }

        /// <summary>
        ///     Tries to cleanup the dictionary.
        /// </summary>
        private void TryToCleanup()
        {
            if (this.mCleanupThread == null || this.mCleanupThread.IsAlive == false)
            {
                this.mCleanupThread = new Thread(this.CleanupThread);
                this.mCleanupThread.Start();
            }
        }

        /// <summary>
        ///     Entry point for the cleanup thread
        /// </summary>
        public void CleanupThread()
        {
            // Start the cleanup process.
            this.mIsCleaningUp = true;

            // Cleaning up...
            this.FullCleanup();

            // Ended the cleanup process.
            this.mIsCleaningUp = false;
        }

        /// <summary>
        ///     Performs a full cleanup on the mCache Dictionary. I.e remove
        ///     keys with garbage collected values.
        /// </summary>
        public void FullCleanup()
        {
            //Console.WriteLine("Start cleanup...");
            var lNbRemoved = 0;
            List<TKey> lKeysToCheck;
            lock (this.mCacheLock)
            {
                lKeysToCheck = this.mCache.Keys.ToList();
            }

            foreach (var lKey in lKeysToCheck)
            {
                lock (this.mCacheLock)
                {
                    WeakReference lWeakRef;
                    bool lKeyFound;
                    lock (this.mCacheLock)
                    {
                        lKeyFound = this.mCache.TryGetValue(lKey, out lWeakRef);
                    }

                    if (lKeyFound)
                    {
                        if (lWeakRef.IsAlive == false)
                        {
                            lock (this.mCacheLock)
                            {
                                this.mCache.Remove(lKey);
                                ++lNbRemoved;
                            }
                        }
                    }
                }
            }

            //Console.WriteLine("Cleanup finished {0}", lNbRemoved);
        }

        #endregion // Methods
    }
}
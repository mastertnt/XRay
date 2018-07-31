using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using XMetadata.MetadataDescriptors;

namespace XMetadata
{
    /// <summary>
    /// Definition of the <see cref="MetadataCollection"/> class.
    /// </summary>
    public class MetadataCollection : ICollection<IMetadata>
    {
        #region Fields

        /// <summary>
        /// Stores the set of metadata by name.
        /// </summary>
        private Dictionary<string, IMetadata> mMetadata;

        #endregion // Fields.

        #region Properties

        /// <summary>
        /// Gets the metadata count
        /// </summary>
        public int Count
        {
            get
            {
                return this.mMetadata.Count;
            }
        }

        /// <summary>
        /// Gets the flag indicating whether the collection is read only or not.
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the metadata corresponding to the given identifier.
        /// </summary>
        /// <param name="pId">The identifier of the metadata to look for.</param>
        /// <returns>The metadata if found, null otherwise.</returns>
        public IMetadata this[string pId]
        {
            get
            {
                if (string.IsNullOrEmpty(pId))
                {
                    return null;
                }

                IMetadata lMetadata;
                if (this.mMetadata.TryGetValue(pId, out lMetadata))
                {
                    return lMetadata;
                }

                return null;
            }
        }

        #endregion // Properties.

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MetadataCollection"/> class.
        /// </summary>
        public MetadataCollection()
        {
            this.mMetadata = new Dictionary<string, IMetadata>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MetadataCollection"/> class.
        /// </summary>
        /// <param name="pInitialList">The initial list of behaviors.</param>
        public MetadataCollection(IEnumerable<IMetadata> pInitialList)
            : this()
        {
            if (pInitialList != null)
            {
                foreach (IMetadata lMetadata in pInitialList)
                {
                    this.mMetadata[lMetadata.Id] = lMetadata;
                }
            }
        }

        #endregion // Constructors.

        #region Methods

        /// <summary>
        /// Adds a new metadata.
        /// </summary>
        /// <param name="pItem">The item to add.</param>
        public void Add(IMetadata pItem)
        {
            if (pItem == null)
            {
                return;
            }

            // Overwrite dupplicates
            this.mMetadata[pItem.Id] = pItem;
        }

        /// <summary>
        /// Removes a metadata
        /// </summary>
        /// <param name="pItem">The item to remove</param>
        /// <returns>True if successful, false otherwise.</returns>
        public bool Remove(IMetadata pItem)
        {
            if (pItem == null)
            {
                return false;
            }

            return this.mMetadata.Remove(pItem.Id);
        }

        /// <summary>
        /// Clear the metadata
        /// </summary>
        public void Clear()
        {
            this.mMetadata.Clear();
        }

        /// <summary>
        /// Checks whether the given metadata is contained or not.
        /// </summary>
        /// <param name="pItem">The metadata to look for</param>
        /// <returns>True of contained, false otherwise.</returns>
        public bool Contains(IMetadata pItem)
        {
            if (pItem == null)
            {
                return false;
            }

            return this.mMetadata.ContainsKey(pItem.Id);
        }

        /// <summary>
        /// Copies the set of metadata into the given array starting at the given array index location.
        /// </summary>
        /// <param name="pArray">The array to fill</param>
        /// <param name="pArrayIndex">The starting array index.</param>
        public void CopyTo(IMetadata[] pArray, int pArrayIndex)
        {
            this.mMetadata.Values.CopyTo(pArray, pArrayIndex);
        }

        /// <summary>
        /// Gets the metadata enumerator
        /// </summary>
        /// <returns>The enumerator</returns>
        public IEnumerator<IMetadata> GetEnumerator()
        {
            return this.mMetadata.Values.GetEnumerator();
        }

        /// <summary>
        /// Gets the metadata enumerator
        /// </summary>
        /// <returns>The enumerator</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion // Methods.
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using XSystem.Dynamic;

namespace XMetadata.MetadataDescriptors
{
    /// <summary>
    /// Definition of the <see cref="MetadataSetType"/> class.
    /// </summary>
    public class MetadataSetType : IMetadataSetType
    {
        #region Fields

        /// <summary>
        /// Stores the tactical metadata target tactical data type name. (e.g: Area2D, Area3D, Point, Polyline, ...)
        /// </summary>
        private string mTargetType;

        /// <summary>
        /// Stores the set of metadata.
        /// </summary>
        private MetadataCollection mMetadata;

        /// <summary>
        /// Stores the nested types if any.
        /// </summary>
        private Dictionary<string, MetadataSetType> mNestedTypes;

        /// <summary>
        /// Stores the nested types by target if any.
        /// </summary>
        private Dictionary<string, MetadataSetType> mNestedTypesByTarget;

        #endregion // Fields.

        #region Properties

        /// <summary>
        /// Gets or sets the object's identifier. Persists over the time.
        /// </summary>
        public string Id
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the parent type.
        /// </summary>
        public IMetadataSetType ParentType
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets the set of nested types if any.
        /// </summary>
        public IEnumerable<IMetadataSetType> NestedTypes
        {
            get
            {
                return this.mNestedTypes.Values;
            }
        }

        /// <summary>
        /// Gets the nested type by its identifier.
        /// </summary>
        /// <param name="pIdentifier">The type identifier/name</param>
        /// <returns>The type if any, Null otherwise.</returns>
        public IMetadataSetType this[TypeKey pIdentifier]
        {
            get
            {
                MetadataSetType lNestedType = null;
                if (this.mNestedTypes.TryGetValue(pIdentifier, out lNestedType))
                {
                    return lNestedType;
                }

                return null;
            }
        }

        /// <summary>
        /// Gets the nested type by its target identifier.
        /// </summary>
        /// <param name="pIdentifier">The target identifier/name</param>
        /// <returns>The type if any, Null otherwise.</returns>
        public IMetadataSetType this[TargetKey pIdentifier]
        {
            get
            {
                MetadataSetType lNestedType = null;
                if (this.mNestedTypesByTarget.TryGetValue(pIdentifier, out lNestedType))
                {
                    return lNestedType;
                }

                return null;
            }
        }

        /// <summary>
        /// Gets or sets the tactical metadata target tactical data type name. (e.g: Area2D, Area3D, Point, Polyline, ...)
        /// </summary>
        [ReadOnly(true)]
        public string TargetType
        {
            get
            {
                return this.mTargetType;
            }
            set
            {
                this.mTargetType = value;
            }
        }

        /// <summary>
        /// Gets the set of metadata.
        /// </summary>
        public IEnumerable<IMetadata> Metadata
        {
            get
            {
                return this.mMetadata;
            }
        }

        /// <summary>
        /// Gets the metadata corresponding to the given Identifier.
        /// </summary>
        /// <param name="pId"></param>
        /// <returns>The metadata, null otherwise.</returns>
        public IMetadata this[string pId]
        {
            get
            {
                return this.mMetadata[pId];
            }
        }

        #endregion // Properties.

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MetadataSetType"/> class.
        /// </summary>
        /// <param name="pId">The type identifier.</param>
        public MetadataSetType(string pId)
        {
            this.Id = pId;
            this.mMetadata = new MetadataCollection();
            this.mNestedTypes = new Dictionary<string, MetadataSetType>();
            this.mNestedTypesByTarget = new Dictionary<string, MetadataSetType>();
        }

        #endregion // Constructors.

        #region Methods

        /// <summary>
        /// Creates an instance of the tactical data defined by this metadata type.
        /// </summary>
        /// <param name="pAttributeCreator">The creator used to affect attributes to the metadata properties.</param>
        /// <returns>The new tactical data instance.</returns>
        public MetadataSet CreateInstance(AAttributesCreator pAttributeCreator)
        {
            MetadataSet lInstance = null;
            try
            {
                PropertyDescriptorCollection lCollection = new PropertyDescriptorCollection(null);

                foreach (IMetadata lMetadata in this.mMetadata)
                {
                    Attribute[] lAttributes = null;
                    if (pAttributeCreator != null)
                    {
                        lAttributes = pAttributeCreator.Create(lMetadata);
                    }

                    DynamicTypedObjectPropertyDescriptor lProperty = lMetadata.CreateInstance(lAttributes);
                    if (lProperty != null)
                    {
                        lCollection.Add(lProperty);
                    }
                }

                lInstance = new MetadataSet(lCollection, this);

                // Initializes
                foreach (IMetadata lMetadata in this.mMetadata)
                {
                    lInstance.TrySetMember(lMetadata.Id, lMetadata.GetDefautValue());
                }
            }
            catch
            {
                return null;
            }

            return lInstance;
        }

        /// <summary>
        /// Adds a nested type.
        /// </summary>
        /// <param name="pType"></param>
        public void AddNestedType(MetadataSetType pType)
        {
            if (pType == null)
            {
                return;
            }

            pType.ParentType = this;
            this.mNestedTypes[pType.Id] = pType;
            this.mNestedTypesByTarget[pType.mTargetType] = pType;
        }

        /// <summary>
        /// Removes a nested type.
        /// </summary>
        /// <param name="pType"></param>
        public bool RemoveNestedType(MetadataSetType pType)
        {
            if (pType == null)
            {
                return false;
            }

            pType.ParentType = null;
            this.mNestedTypesByTarget.Remove(pType.mTargetType);
            return this.mNestedTypes.Remove(pType.Id);
        }

        /// <summary>
        /// Adds a new metadata to the type.
        /// </summary>
        /// <param name="pMetadata">The metadata to add.</param>
        public void AddMetadata(IMetadata pMetadata)
        {
            this.mMetadata.Add(pMetadata);
        }

        /// <summary>
        /// Removes a metadata from the type.
        /// </summary>
        /// <param name="pMetadata">The metadata to remove.</param>
        /// <returns>True if successful, false otherwise.</returns>
        public bool RemoveMetadata(IMetadata pMetadata)
        {
            return this.mMetadata.Remove(pMetadata);
        }

        /// <summary>
        /// Returns the type of the metadata as a string.s
        /// </summary>
        /// <returns>The type as string.</returns>
        public string GetTypeAsString()
        {
            return MetadataManager.Instance.BuildTypeAsString(this);
        }

        /// <summary>
        /// Returns the parent type path as a string.
        /// </summary>
        /// <returns>The parent type path.</returns>
        public string GetParentTypeAsString()
        {
            return MetadataManager.Instance.BuildParentTypeAsString(this);
        }

        #endregion // Methods.
    }
}

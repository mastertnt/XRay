using System;
using System.ComponentModel;
using System.Xml.Serialization;
using XSystem.Dynamic;

namespace XMetadata.MetadataDescriptors
{
    /// <summary>
    /// Definition of the <see cref="AMetadata{T}"/> class.
    /// </summary>
    public abstract class AMetadata<T> : IMetadata
    {
        #region Fields

        /// <summary>
        /// Stores the metadata system type.
        /// </summary>
        private Type mType;

        /// <summary>
        /// Stores the flag indicating whether the metadata is optional or not.
        /// </summary>
        private bool mIsOptional;

        #endregion // Fields.

        #region Properties

        /// <summary>
        /// Gets the metadata id.
        /// </summary>
        public string Id
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the system type of this metadata.
        /// </summary>
        public Type Type
        {
            get
            {
                if (this.mType == null)
                {
                    this.mType = typeof(T);
                }

                return this.mType;
            }
        }

        /// <summary>
        /// Gets or sets the flag indicating whether the metadata is optional or not.
        /// </summary>
        [XmlIgnore]
        [Browsable(false)]
        public bool IsOptional
        {
            get
            {
                return this.mIsOptional;
            }
            set
            {
                if ( this.mIsOptional != value )
                {
                    this.mIsOptional = value;
                }
            }
        }

        #endregion // Properties.

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AMetadata{T}"/> class.
        /// </summary>
        /// <param name="pId">The metadata identifier.</param>
        protected AMetadata(string pId)
        {
            this.Id = pId;
        }

        #endregion // Constructors.

        #region Methods

        /// <summary>
        /// Gets the default value.
        /// </summary>
        /// <returns>The default value.</returns>
        public abstract object GetDefautValue();

        /// <summary>
        /// Creates an instance of the object defined in this metadata 
        /// </summary>
        /// <param name="pAttributes">The attributes to give to the instance.</param>
        /// <returns>The new instance.</returns>
        public virtual DynamicTypedObjectPropertyDescriptor CreateInstance(Attribute[] pAttributes)
        {
            return new DynamicTypedObjectPropertyDescriptor(this.Id, this.Type, pAttributes);
        }

        #endregion // Methods.
    }
}

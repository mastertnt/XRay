using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using XMetadata.MetadataDescriptors;
using XSystem.Dynamic;

namespace XMetadata
{
    /// <summary>
    /// Class defining a specific tactical metadata set.
    /// </summary>
    public class MetadataSet : DynamicTypedObject
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MetadataSet"/> class.
        /// </summary>
        /// <param name="pPropertyDescriptors">The properties of the metadata set.</param>
        /// <param name="pType">The parent type.</param>
        public MetadataSet(PropertyDescriptorCollection pPropertyDescriptors, IMetadataSetType pType)
            : base(pPropertyDescriptors, pType.GetTypeAsString())
        {
            this.Type = pType;
        }

        #endregion // Constructors.

        #region Properties

        /// <summary>
        /// Gets or sets the metadata set type.
        /// </summary>
        public IMetadataSetType Type
        {
            get;
            private set;
        }

        #endregion // Properties.

        #region Methods

        /// <summary>
        /// Duplicates the metadata of tactical data.
        /// </summary>
        /// <returns>The cloned metadata set.</returns>
        public MetadataSet Clone()
        {
            MetadataSet lMetadataClone = new MetadataSet(this.PropertyDescriptors, this.Type);

            for (int lCount = 0; lCount < lMetadataClone.PropertyDescriptors.Count; lCount++)
            {
                object lValue = this.PropertyDescriptors[lCount].GetValue(this);
                MemberDescriptor lComponent = this.PropertyDescriptors[lCount] as MemberDescriptor;
                string lMemberName = lComponent.Name;
                lMetadataClone.TrySetMember(lMemberName, lValue);
            }

            return lMetadataClone;
        }

        #endregion // Methods.
    }
}

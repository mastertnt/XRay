using System;
using XSystem.Dynamic;

namespace XMetadata.MetadataDescriptors
{
    /// <summary>
    /// Definition of the <see cref="IMetadata"/> interface
    /// </summary>
    public interface IMetadata
    {
        #region Properties

        /// <summary>
        /// Gets the metadata id.
        /// </summary>
        string Id
        {
            get;
        }

        /// <summary>
        /// Gets the system type of this metadata.
        /// </summary>
        Type Type
        {
            get;
        }

        /// <summary>
        /// Gets or sets the flag indicating whether the metadata is optional or not.
        /// </summary>
        bool IsOptional
        {
            get;
            set;
        }

        #endregion // Properties.

        #region Methods

        /// <summary>
        /// Gets the default value.
        /// </summary>
        /// <returns>The default value.</returns>
        object GetDefautValue();

        /// <summary>
        /// Creates an instance of the object defined in this metadata 
        /// </summary>
        /// <param name="pAttributes">The attributes to give to the instance.</param>
        /// <returns>The new instance.</returns>
        DynamicTypedObjectPropertyDescriptor CreateInstance(Attribute[] pAttributes);

        #endregion // Methods.
    }
}

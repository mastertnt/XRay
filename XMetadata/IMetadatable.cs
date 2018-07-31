using XMetadata.MetadataDescriptors;

namespace XMetadata
{
    /// <summary>
    /// Definition of the <see cref="IMetadatable"/> interface.
    /// </summary>
    public interface IMetadatable
    {
        #region Properties

        /// <summary>
        /// Gets or sets the user type.
        /// </summary>
        string UserType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the metadatable's type owning metadata description.
        /// </summary>
        IMetadataSetType Type
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the metadata.
        /// </summary>
        MetadataSet Metadata
        {
            get;
            set;
        }

        #endregion // Properties.
    }
}

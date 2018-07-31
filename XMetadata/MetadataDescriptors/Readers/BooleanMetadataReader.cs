using XMetadata.MetadataDescriptors;

namespace XMetadata.MetadataDescriptors.Readers
{
    /// <summary>
    /// Definition of the <see cref="BooleanMetadataReader"/> class.
    /// </summary>
    public class BooleanMetadataReader : AMetadataReader
    {
        #region Properties

        /// <summary>
        /// Gets the metadata reader type.
        /// </summary>
        public override sealed string Type
        {
            get
            {
                return "bool";
            }
        }

        #endregion // Properties.

        #region Methods

        /// <summary>
        /// Creates the meta data object.
        /// </summary>
        /// <param name="pId">The metadata identifier.</param>
        /// <returns>The new metadata.</returns>
        public override IMetadata Create(string pId)
        {
            return new BooleanMetadata(pId);
        }

        #endregion // Methods.
    }
}

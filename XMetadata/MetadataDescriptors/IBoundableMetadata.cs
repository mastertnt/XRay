namespace XMetadata.MetadataDescriptors
{
    /// <summary>
    /// Definition of the <see cref="IBoundableMetadata"/> interface.
    /// </summary>
    public interface IBoundableMetadata : IMetadata
    {
        #region Properties

        /// <summary>
        /// Gets or sets the Min metadata data.
        /// </summary>
        object Min
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Max metadata data.
        /// </summary>
        object Max
        {
            get;
            set;
        }

        #endregion // Properties.
    }
}

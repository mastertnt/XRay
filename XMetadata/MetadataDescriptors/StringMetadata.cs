
namespace XMetadata.MetadataDescriptors
{
    /// <summary>
    /// Definition of the <see cref="StringMetadata"/> class.
    /// </summary>
    public class StringMetadata : AMetadata<string>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StringMetadata"/> class.
        /// </summary>
        /// <param name="pId">The metadata identifier.</param>
        public StringMetadata(string pId)
            : base(pId)
        {
            
        }

        #endregion // Constructors.

        #region Methods

        /// <summary>
        /// Gets the default value.
        /// </summary>
        /// <returns>The default value.</returns>
        public override object GetDefautValue()
        {
            return string.Empty;
        }

        #endregion // Methods.
    }
}


namespace XMetadata.MetadataDescriptors
{
    /// <summary>
    /// Definition of the <see cref="BooleanMetadata"/> class.
    /// </summary>
    public class BooleanMetadata : AMetadata<bool>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BooleanMetadata"/> class.
        /// </summary>
        /// <param name="pId">The metadata identifier.</param>
        public BooleanMetadata(string pId)
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
            return false;
        }

        #endregion // Methods.
    }
}

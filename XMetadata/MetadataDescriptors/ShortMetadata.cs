
namespace XMetadata.MetadataDescriptors
{
    /// <summary>
    /// Definition of the <see cref="ShortMetadata"/> class.
    /// </summary>
    public class ShortMetadata : ABoundableMetadata<short>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ShortMetadata"/> class.
        /// </summary>
        /// <param name="pId">The metadata identifier.</param>
        public ShortMetadata(string pId)
            : base(pId)
        {
            this.Min = short.MinValue;
            this.Max = short.MaxValue;
        }

        #endregion // Constructors.

        #region Methods

        /// <summary>
        /// Gets the default value.
        /// </summary>
        /// <returns>The default value.</returns>
        public override object GetDefautValue()
        {
            return 0;
        }

        #endregion // Methods.
    }
}


namespace XMetadata.MetadataDescriptors
{
    /// <summary>
    /// Definition of the <see cref="UShortMetadata"/> class.
    /// </summary>
    public class UShortMetadata : ABoundableMetadata<ushort>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UShortMetadata"/> class.
        /// </summary>
        /// <param name="pId">The metadata identifier.</param>
        public UShortMetadata(string pId)
            : base(pId)
        {
            this.Min = ushort.MinValue;
            this.Max = ushort.MaxValue;
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


namespace XMetadata.MetadataDescriptors
{
    /// <summary>
    /// Definition of the <see cref="UIntMetadata"/> class.
    /// </summary>
    public class UIntMetadata : ABoundableMetadata<uint>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UIntMetadata"/> class.
        /// </summary>
        /// <param name="pId">The metadata identifier.</param>
        public UIntMetadata(string pId)
            : base(pId)
        {
            this.Min = uint.MinValue;
            this.Max = uint.MaxValue;
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


namespace XMetadata.MetadataDescriptors
{
    /// <summary>
    /// Definition of the <see cref="UIntMetadata"/> class.
    /// </summary>
    public class ULongMetadata : ABoundableMetadata<ulong>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ULongMetadata"/> class.
        /// </summary>
        /// <param name="pId">The metadata identifier.</param>
        public ULongMetadata(string pId)
            : base(pId)
        {
            this.Min = ulong.MinValue;
            this.Max = ulong.MaxValue;
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

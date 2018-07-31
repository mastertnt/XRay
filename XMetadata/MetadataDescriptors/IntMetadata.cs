
namespace XMetadata.MetadataDescriptors
{
    /// <summary>
    /// Definition of the <see cref="IntMetadata"/> class.
    /// </summary>
    public class IntMetadata : ABoundableMetadata<int>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="IntMetadata"/> class.
        /// </summary>
        /// <param name="pId">The metadata identifier.</param>
        public IntMetadata(string pId)
            : base(pId)
        {
            this.Min = int.MinValue;
            this.Max = int.MaxValue;
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

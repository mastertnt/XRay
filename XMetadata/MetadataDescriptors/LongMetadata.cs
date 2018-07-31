
namespace XMetadata.MetadataDescriptors
{
    /// <summary>
    /// Definition of the <see cref="LongMetadata"/> class.
    /// </summary>
    public class LongMetadata : ABoundableMetadata<long>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LongMetadata"/> class.
        /// </summary>
        /// <param name="pId">The metadata identifier.</param>
        public LongMetadata(string pId)
            : base(pId)
        {
            this.Min = long.MinValue;
            this.Max = long.MaxValue;
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

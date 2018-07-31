
namespace XMetadata.MetadataDescriptors
{
    /// <summary>
    /// Definition of the <see cref="DoubleMetadata"/> class.
    /// </summary>
    public class DoubleMetadata : ABoundableMetadata<double>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DoubleMetadata"/> class.
        /// </summary>
        /// <param name="pId">The metadata identifier.</param>
        public DoubleMetadata(string pId)
            : base(pId)
        {
            this.Min = double.MinValue;
            this.Max = double.MaxValue;
        }

        #endregion // Constructors.

        #region Methods

        /// <summary>
        /// Gets the default value.
        /// </summary>
        /// <returns>The default value.</returns>
        public override object GetDefautValue()
        {
            return 0.0;
        }

        #endregion // Methods.
    }
}


namespace XMetadata.MetadataDescriptors
{
    /// <summary>
    /// Definition of the <see cref="FloatMetadata"/> class.
    /// </summary>
    public class FloatMetadata : ABoundableMetadata<float>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FloatMetadata"/> class.
        /// </summary>
        /// <param name="pId">The metadata identifier.</param>
        public FloatMetadata(string pId)
            : base(pId)
        {
            this.Min = float.MinValue;
            this.Max = float.MaxValue;
        }

        #endregion // Constructors.

        #region Methods

        /// <summary>
        /// Gets the default value.
        /// </summary>
        /// <returns>The default value.</returns>
        public override object GetDefautValue()
        {
            return 0.0f;
        }

        #endregion // Methods.
    }
}

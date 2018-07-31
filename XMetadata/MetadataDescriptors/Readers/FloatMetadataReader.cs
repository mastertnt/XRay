using System.Globalization;
using System.Xml.Linq;

namespace XMetadata.MetadataDescriptors.Readers
{
    /// <summary>
    /// Definition of the <see cref="FloatMetadataReader"/> class.
    /// </summary>
    public class FloatMetadataReader : AMetadataReader
    {
        #region Properties

        /// <summary>
        /// Gets the metadata reader type.
        /// </summary>
        public override sealed string Type
        {
            get
            {
                return "float";
            }
        }

        #endregion // Properties.

        #region Methods

        /// <summary>
        /// Creates the meta data object.
        /// </summary>
        /// <param name="pId">The metadata identifier.</param>
        /// <returns>The new metadata.</returns>
        public override IMetadata Create(string pId)
        {
            return new FloatMetadata(pId);
        }

        /// <summary>
        /// Internal metadata read process.
        /// </summary>
        /// <param name="pToFill"></param>
        /// <param name="pElement"></param>
        protected override void InternalRead(ref IMetadata pToFill, XElement pElement)
        {
            ABoundableMetadata<float> lToFill = pToFill as ABoundableMetadata<float>;

            XAttribute lXMin = pElement.Attribute(cMetadataMinTag);
            if (lXMin != null)
            {
                float lMin;
                if (float.TryParse(lXMin.Value, NumberStyles.Float, CultureInfo.InvariantCulture, out lMin))
                {
                    lToFill.Min = lMin;
                }
            }

            XAttribute lXMax = pElement.Attribute(cMetadataMaxTag);
            if (lXMax != null)
            {
                float lMax;
                if (float.TryParse(lXMax.Value, NumberStyles.Float, CultureInfo.InvariantCulture, out lMax))
                {
                    lToFill.Max = lMax;
                }
            }
        }

        #endregion // Methods.
    }
}

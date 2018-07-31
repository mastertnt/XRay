using System.Globalization;
using System.Xml.Linq;
using XMetadata.MetadataDescriptors;

namespace XMetadata.MetadataDescriptors.Readers
{
    /// <summary>
    /// Definition of the <see cref="DoubleMetadataReader"/> class.
    /// </summary>
    public class DoubleMetadataReader : AMetadataReader
    {
        #region Properties

        /// <summary>
        /// Gets the metadata reader type.
        /// </summary>
        public override sealed string Type
        {
            get
            {
                return "double";
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
            return new DoubleMetadata(pId);
        }

        /// <summary>
        /// Internal metadata read process.
        /// </summary>
        /// <param name="pToFill"></param>
        /// <param name="pElement"></param>
        protected override void InternalRead(ref IMetadata pToFill, XElement pElement)
        {
            ABoundableMetadata<double> lToFill = pToFill as ABoundableMetadata<double>;

            XAttribute lXMin = pElement.Attribute(cMetadataMinTag);
            if (lXMin != null)
            {
                double lMin;
                if (double.TryParse(lXMin.Value, NumberStyles.Float, CultureInfo.InvariantCulture, out lMin))
                {
                    lToFill.Min = lMin;
                }
            }

            XAttribute lXMax = pElement.Attribute(cMetadataMaxTag);
            if (lXMax != null)
            {
                double lMax;
                if (double.TryParse(lXMax.Value, NumberStyles.Float, CultureInfo.InvariantCulture, out lMax))
                {
                    lToFill.Max = lMax;
                }
            }
        }

        #endregion // Methods.
    }
}

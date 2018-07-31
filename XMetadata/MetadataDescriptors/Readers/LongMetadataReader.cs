using System.Xml.Linq;

namespace XMetadata.MetadataDescriptors.Readers
{
    /// <summary>
    /// Definition of the <see cref="LongMetadataReader"/> class.
    /// </summary>
    public class LongMetadataReader : AMetadataReader
    {
        #region Properties

        /// <summary>
        /// Gets the metadata reader type.
        /// </summary>
        public override sealed string Type
        {
            get
            {
                return "long";
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
            return new LongMetadata(pId);
        }

        /// <summary>
        /// Internal metadata read process.
        /// </summary>
        /// <param name="pToFill"></param>
        /// <param name="pElement"></param>
        protected override void InternalRead(ref IMetadata pToFill, XElement pElement)
        {
            ABoundableMetadata<long> lToFill = pToFill as ABoundableMetadata<long>;

            XAttribute lXMin = pElement.Attribute(cMetadataMinTag);
            if (lXMin != null)
            {
                long lMin;
                if (long.TryParse(lXMin.Value, out lMin))
                {
                    lToFill.Min = lMin;
                }
            }

            XAttribute lXMax = pElement.Attribute(cMetadataMaxTag);
            if (lXMax != null)
            {
                long lMax;
                if (long.TryParse(lXMax.Value, out lMax))
                {
                    lToFill.Max = lMax;
                }
            }
        }

        #endregion // Methods.
    }
}

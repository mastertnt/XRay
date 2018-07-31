using System.Xml.Linq;

namespace XMetadata.MetadataDescriptors.Readers
{
    /// <summary>
    /// Definition of the <see cref="IntMetadataReader"/> class.
    /// </summary>
    public class IntMetadataReader : AMetadataReader
    {
        #region Properties

        /// <summary>
        /// Gets the metadata reader type.
        /// </summary>
        public override sealed string Type
        {
            get
            {
                return "int";
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
            return new IntMetadata(pId);
        }

        /// <summary>
        /// Internal metadata read process.
        /// </summary>
        /// <param name="pToFill"></param>
        /// <param name="pElement"></param>
        protected override void InternalRead(ref IMetadata pToFill, XElement pElement)
        {
            ABoundableMetadata<int> lToFill = pToFill as ABoundableMetadata<int>;

            XAttribute lXMin = pElement.Attribute(cMetadataMinTag);
            if (lXMin != null)
            {
                int lMin;
                if (int.TryParse(lXMin.Value, out lMin))
                {
                    lToFill.Min = lMin;
                }
            }

            XAttribute lXMax = pElement.Attribute(cMetadataMaxTag);
            if (lXMax != null)
            {
                int lMax;
                if (int.TryParse(lXMax.Value, out lMax))
                {
                    lToFill.Max = lMax;
                }
            }
        }

        #endregion // Methods.
    }
}

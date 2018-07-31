using System.Xml.Linq;

namespace XMetadata.MetadataDescriptors.Readers
{
    /// <summary>
    /// Definition of the <see cref="ListMetadataReader"/> class.
    /// </summary>
    public class ListMetadataReader : AMetadataReader
    {
        #region Properties

        /// <summary>
        /// Gets the metadata reader type.
        /// </summary>
        public override sealed string Type
        {
            get
            {
                return "metaList";
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
            return new ListMetadata(pId);
        }

        /// <summary>
        /// Internal metadata read process.
        /// </summary>
        /// <param name="pToFill"></param>
        /// <param name="pElement"></param>
        protected override void InternalRead(ref IMetadata pToFill, XElement pElement)
        {
            ListMetadata lToFill = pToFill as ListMetadata;

            foreach (XElement lXMetadata in pElement.Descendants(MetadataManager.cMetadataTag))
            {
                XAttribute lXType = lXMetadata.Attribute(MetadataManager.cTypeTag);
                if (lXType == null)
                {
                    continue;
                }

                IMetadata lMetadata = null;
                AMetadataReader lReader = MetadataManager.Instance.GetReader(lXType.Value);
                lReader.Read(out lMetadata, lXMetadata);

                lToFill.AddMetadata(lMetadata);
            }
        }

        #endregion // Methods.
    }
}

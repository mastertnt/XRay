using System.Xml.Linq;
using XMetadata.MetadataDescriptors;
using XMetadata.MetadataDescriptors.Readers;

namespace XMetadata.Metadata.Readers
{
    /// <summary>
    /// Definition of the <see cref="MetadataTypeReader"/> class.
    /// </summary>
    public class MetadataTypeReader
    {
        #region Methods

        /// <summary>
        /// Reads a new metadata element.
        /// </summary>
        /// <param name="pMetadataType">The metadata type to fill.</param>
        /// <param name="pElement">The XML element the metadata is extracted from.</param>
        /// <returns>True if successful, false otherwise.</returns>
        public bool Read(ref MetadataSetType pMetadataType, XElement pElement)
        {
            XAttribute lXNestedPropertyName = pElement.Attribute(MetadataManager.cMetadataPropertyNameTag);
            XAttribute lXNestedTypeId = pElement.Attribute(MetadataManager.cMetadataIdTag);
            if (lXNestedPropertyName == null ||
                 lXNestedTypeId == null)
            {
                return false;
            }

            string lNestedTargetType = lXNestedPropertyName.Value;
            string lNestedTypeId = lXNestedTypeId.Value;
            MetadataSetType lNestedType = new MetadataSetType(lNestedTypeId);
            lNestedType.TargetType = lNestedTargetType;

            foreach (XElement lXMetadata in pElement.Elements(MetadataManager.cMetadataTag))
            {
                XAttribute lXType = lXMetadata.Attribute(MetadataManager.cTypeTag);
                if (lXType == null)
                {
                    continue;
                }

                AMetadataReader lReader = MetadataManager.Instance.GetReader(lXType.Value);
                if (lReader != null)
                {
                    lReader.Read(ref lNestedType, lXMetadata);
                }
            }

            // Check for nested types too.
            foreach (XElement lXNestedType in pElement.Elements(MetadataManager.cMetadataTypeTag))
            {
                MetadataTypeReader lNestedReader = new MetadataTypeReader();
                lNestedReader.Read(ref lNestedType, lXNestedType);
            }

            pMetadataType.AddNestedType(lNestedType);

            return true;
        }

        #endregion // Methods.
    }
}

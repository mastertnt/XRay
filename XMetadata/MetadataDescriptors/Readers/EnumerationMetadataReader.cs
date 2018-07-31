using System.Collections.Generic;
using System.Xml.Linq;

namespace XMetadata.MetadataDescriptors.Readers
{
    /// <summary>
    /// Definition of the <see cref="EnumerationMetadataReader"/> class.
    /// </summary>
    public class EnumerationMetadataReader : AMetadataReader
    {
        #region Properties

        /// <summary>
        /// Gets the metadata reader type.
        /// </summary>
        public override sealed string Type
        {
            get
            {
                return "enumeration";
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
            return new EnumMetadata(pId);
        }

        /// <summary>
        /// Internal metadata read process.
        /// </summary>
        /// <param name="pToFill"></param>
        /// <param name="pElement"></param>
        protected override void InternalRead(ref IMetadata pToFill, XElement pElement)
        {
            string lValue = pElement.Value;
            if (string.IsNullOrEmpty(lValue))
            {
                return;
            }

            EnumMetadata lToFill = pToFill as EnumMetadata;

            int lLastValue = 0;
            List<Enum> lEnumList = new List<Enum>();
            string[] lValues = lValue.Trim().Split(';');
            foreach (string lEnumValue in lValues)
            {
                Enum lNewEnumValue = new Enum();
                lNewEnumValue.Name = lEnumValue.Trim();
                if (string.IsNullOrEmpty(lNewEnumValue.Name))
                {
                    continue;
                }

                lNewEnumValue.Value = lLastValue++;
                lEnumList.Add(lNewEnumValue);
            }

            lToFill.Enumeration = lEnumList;
        }

        #endregion // Methods.
    }
}

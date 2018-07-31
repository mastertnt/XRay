using System.Xml.Linq;

namespace XMetadata.MetadataDescriptors.Readers
{
    /// <summary>
    /// Definition of the <see cref="AMetadataReader"/> abstract class.
    /// </summary>
    public abstract class AMetadataReader
    {
        #region Fields

        /// <summary>
        /// Stores the constant metadata min tag.
        /// </summary>
        protected const string cMetadataMinTag = "min";

        /// <summary>
        /// Stores the constant metadata max tag.
        /// </summary>
        protected const string cMetadataMaxTag = "max";

        /// <summary>
        /// Stores the constant metadata type tag.
        /// </summary>
        protected const string cMetadataTypeTag = "type";

        /// <summary>
        /// Stores the constant metadata isOptional tag.
        /// </summary>
        protected const string cMetadataIsOptionalTag = "isOptional";

        #endregion // Fields.

        #region Properties

        /// <summary>
        /// Gets the metadata reader type.
        /// </summary>
        public abstract string Type
        {
            get;
        }

        #endregion // Properties.

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AMetadataReader"/> class.
        /// </summary>
        protected AMetadataReader()
        {
        }

        #endregion // Constructors.

        #region Methods

        /// <summary>
        /// Creates the meta data object.
        /// </summary>
        /// <param name="pId">The metadata identifier.</param>
        /// <returns>The new metadata.</returns>
        public abstract IMetadata Create(string pId);

        /// <summary>
        /// Reads a new metadata element.
        /// </summary>
        /// <param name="pMetadataType">The metadata type to fill.</param>
        /// <param name="pElement">The XML element the metadata is extracted from.</param>
        /// <returns>True if successful, false otherwise.</returns>
        public bool Read(ref MetadataSetType pMetadataType, XElement pElement)
        {
            IMetadata lMetadata = null;
            this.Read(out lMetadata, pElement);

            pMetadataType.AddMetadata(lMetadata);

            return true;
        }

        /// <summary>
        /// Reads a new meta data element.
        /// </summary>
        /// <param name="pMetadata">The metadata to fill.</param>
        /// <param name="pElement">The xml element containing the informations.</param>
        /// <returns></returns>
        public bool Read(out IMetadata pMetadata, XElement pElement)
        {
            XAttribute lXId = pElement.Attribute(MetadataManager.cMetadataIdTag);

            pMetadata = this.Create(lXId.Value);

            XAttribute lXOptional = pElement.Attribute(cMetadataIsOptionalTag);
            if (lXOptional != null)
            {
                bool lIsOptional = false;
                if (bool.TryParse(lXOptional.Value, out lIsOptional))
                {
                    pMetadata.IsOptional = lIsOptional;
                }
            }

            this.InternalRead(ref pMetadata, pElement);

            return true;
        }

        /// <summary>
        /// Internal metadata read process.
        /// </summary>
        /// <param name="pToFill">The metadata to fill.</param>
        /// <param name="pElement">The xml element containing the informations.</param>
        protected virtual void InternalRead(ref IMetadata pToFill, XElement pElement)
        {
            // Nothing to do.
        }

        #endregion // Methods.
    }
}

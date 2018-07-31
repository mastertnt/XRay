using System.Collections.Generic;
using System.Linq;

namespace XMetadata.MetadataDescriptors
{
    /// <summary>
    /// Definition of the <see cref="EnumMetadata"/> class.
    /// </summary>
    public class EnumMetadata : AMetadata<string>
    {
        #region Fields

        /// <summary>
        /// Stores the set of enum of the enumeration.
        /// </summary>
        private IEnumerable<Enum> mEnumeration;

        #endregion // Fields.

        #region Properties

        /// <summary>
        /// Gets or sets the enumeration.
        /// </summary>
        public IEnumerable<Enum> Enumeration
        {
            get
            {
                return this.mEnumeration;
            }
            set
            {
                this.mEnumeration = value;
            }
        }

        #endregion // Properties.

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumMetadata"/> class.
        /// </summary>
        /// <param name="pId">The metadata identifier.</param>
        public EnumMetadata(string pId)
            : base(pId)
        {

        }

        #endregion // Constructors.

        #region Methods

        /// <summary>
        /// Gets the default value.
        /// </summary>
        /// <returns>The default value.</returns>
        public override object GetDefautValue()
        {
            Enum lFirst = this.Enumeration.FirstOrDefault();
            if (lFirst != null)
            {
                return lFirst.Name;
            }

            return string.Empty;
        }

        #endregion // Methods.
    }
}

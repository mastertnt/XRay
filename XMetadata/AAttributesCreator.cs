using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XMetadata.MetadataDescriptors;

namespace XMetadata
{
    /// <summary>
    /// Class defining an attribute creator.
    /// </summary>
    public abstract class AAttributesCreator
    {
        #region Methods

        /// <summary>
        /// Creates the attributes for the given metadata.
        /// </summary>
        /// <param name="pMetadata">The metadata to visit.</param>
        /// <returns>The generated attributes.</returns>
        public abstract Attribute[] Create(IMetadata pMetadata);

        #endregion // Methods.
    }
}

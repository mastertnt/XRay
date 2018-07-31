using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace XSerialization.Defaults
{
    /// <summary>
    /// This class defines the default serialization contract.
    /// </summary>
    public class DefaultObjectSerializationContract : AObjectSerializationContract<object>
    {
        #region Methods

        /// <summary>
        /// This method determines whether this instance can manage the specified type.
        /// </summary>
        /// <param name="pObjectType">The object type to test.</param>
        /// <param name="pSerializationContext">The serialization context.</param>
        /// <returns>The depth of inheritance or -1 if the contract cannot support.</returns>
        public override SupportPriority CanManage(Type pObjectType, IXSerializationContext pSerializationContext)
        {
            SupportPriority lPriority = base.CanManage(pObjectType, pSerializationContext);
            return new SupportPriority(SupportLevel.Default, lPriority.SubPriority);
        }

        #endregion // Methods.
    }
}

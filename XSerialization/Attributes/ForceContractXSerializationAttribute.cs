using System;

namespace XSerialization.Attributes
{
    /// <summary>
    /// This attribute can be used to force a contract type on a property.
    /// </summary>
    public class ForceContractXSerializationAttribute : XSerializationAttribute
    {
        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="pContractType">The contract type.</param>
        public ForceContractXSerializationAttribute(Type pContractType)
            : base(pContractType)
        {
            
        }

        #endregion // Constructors.
    }
}

using System;
using System.Globalization;
using System.Xml.Linq;

namespace XSerialization.Primitives
{
    /// <summary>
    /// This abstract class is used by primitive types.
    /// </summary>
    /// <typeparam name="TPrimitiveType">The type of the primitive type.</typeparam>
    public abstract class PrimitiveTypeSerializationContract<TPrimitiveType> : ATypeSerializationContract<TPrimitiveType> where TPrimitiveType : new()
    {
        #region Properties

        /// <summary>
        /// Flag to know if an external creation is necessary.
        /// </summary>
        public override bool NeedCreate
        {
            get
            {
                return true;
            }
        }

        #endregion // Properties.

        #region Methods

        /// <summary>
        /// This method determines whether this instance can manage the specified object.
        /// </summary>
        /// <param name="pType">The type to test.</param>
        /// <param name="pSerializationContext">The serialization context.</param>
        /// <returns>The depth of inheritance or -1 if the contract cannot support.</returns>
        public override SupportPriority CanManage(Type pType, IXSerializationContext pSerializationContext)
        {
            if (pType.IsPrimitive)
            {
                if (pType == this.SupportedType)
                {
                    return new SupportPriority(SupportLevel.Type, 0);
                }
            }

            return SupportPriority.CANNOT_SUPPORT;
        }

        /// <summary>
        /// This method creates the specified element.
        /// </summary>
        /// <param name="pElement">The element.</param>
        /// <param name="pSerializationContext">The serialization context.</param>
        /// <returns>The created object.</returns>
        public override object Create(XElement pElement, IXSerializationContext pSerializationContext)
        {
            return new TPrimitiveType();
        }

        /// <summary>
        /// This method writes the specified object.
        /// </summary>
        /// <param name="pObject">The object to serialize.</param>
        /// <param name="pParentElement">The parent element.</param>
        /// <param name="pSerializationContext">The serialization context.</param>
        /// <returns>The modified parent element</returns>
        public override XElement Write(object pObject, XElement pParentElement, IXSerializationContext pSerializationContext)
        {
            pParentElement.Value = Convert.ToString(pObject, CultureInfo.InvariantCulture);
            return pParentElement;
        }

        #endregion // Methods.
    }
}

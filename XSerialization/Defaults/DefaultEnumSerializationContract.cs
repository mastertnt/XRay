using System.Xml.Linq;
using System;
using XSystem;

namespace XSerialization.Defaults
{
    /// <summary>
    /// This class defines the serialization contract for enum.
    /// </summary>
    public class DefaultEnumSerializationContract : IXSerializationContract
    {
        /// <summary>
        /// Stores the object type.
        /// </summary>
        /// <remarks>
        /// Used for retro compatibility.
        /// </remarks>
        private Type mObjectType = null;

        /// <summary>
        /// Flag to know if an external creation is necessary.
        /// </summary>
        public bool NeedCreate
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// This method determines whether this type can manage the specified object.
        /// </summary>
        /// <param name="pObjectType">The object type to test.</param>
        /// <param name="pSerializationContext">The serialization context.</param>
        /// <returns>The depth of inheritance or -1 if the contract cannot support.</returns>
        public virtual SupportPriority CanManage(Type pObjectType, IXSerializationContext pSerializationContext)
        {
            if (pObjectType.IsEnum)
            {
                this.mObjectType = pObjectType;
                return new SupportPriority(SupportLevel.Default, 0);
            }
            return SupportPriority.CANNOT_SUPPORT;
        }

        /// <summary>
        /// This method determines whether this instance can manage the specified object.
        /// </summary>
        /// <param name="pObject">The object to test.</param>
        /// <param name="pSerializationContext">The serialization context.</param>
        /// <returns>The depth of inheritance or -1 if the contract cannot support.</returns>
        public virtual SupportPriority CanManage(object pObject, IXSerializationContext pSerializationContext)
        {
            if (pObject != null)
            {
                if (pObject.GetType().IsEnum)
                {
                    this.mObjectType = pObject.GetType();
                    return new SupportPriority(SupportLevel.Type, 0);
                }
            }
            return SupportPriority.CANNOT_SUPPORT;
        }

        /// <summary>
        /// This method determines whether this instance can manage the specified element.
        /// </summary>
        /// <param name="pElement">The element to test.</param>
        /// <param name="pSerializationContext">The serialization context.</param>
        /// <returns>The depth of inheritance or -1 if the contract cannot support.</returns>
        public virtual SupportPriority CanManage(XElement pElement, IXSerializationContext pSerializationContext)
        {
            Type lObjectType = AppDomain.CurrentDomain.GetTypeByFullName(pElement.Name.LocalName);
            if (lObjectType != null)
            {
                this.mObjectType = lObjectType;
                return this.CanManage(lObjectType, pSerializationContext);
            }
            return SupportPriority.CANNOT_SUPPORT;
        }

        /// <summary>
        /// This method creates the specified element.
        /// </summary>
        /// <param name="pElement">The element.</param>
        /// <param name="pSerializationContext">The serialization context.</param>
        /// <returns>The created object.</returns>
        public virtual object Create(XElement pElement, IXSerializationContext pSerializationContext)
        {
            XElement lTypeElement = pElement.Element(XConstants.TYPE_TAG);
            Type lRetrievedType = null;
            string lValueAsString = null;
            if (lTypeElement != null)
            {
                lRetrievedType = pSerializationContext.ResolveType(lTypeElement);
                XElement lValueElement = pElement.Element(XConstants.VALUE_TAG);
                if (lRetrievedType != null && lValueElement != null)
                {
                    lValueAsString = lValueElement.Value;
                }
            }
            else
            {
                lRetrievedType = this.mObjectType;
                lValueAsString = pElement.Value;
            }

            if (lRetrievedType != null && lValueAsString != null)
            {
                return Enum.Parse(lRetrievedType, lValueAsString);
            }

            return null;
        }

        /// <summary>
        /// This method reads the specified element.
        /// </summary>
        /// <param name="pObjectToInitialize">The object to initialize</param>
        /// <param name="pElement">The element.</param>
        /// <param name="pSerializationContext">The serialization context.</param>
        /// <returns>The initialized object if the input object is valid.</returns>
        public virtual object Read(object pObjectToInitialize, XElement pElement, IXSerializationContext pSerializationContext)
        {
            return pObjectToInitialize;
        }

        /// <summary>
        /// This method writes the specified object.
        /// </summary>
        /// <param name="pObject">The object to serialize.</param>
        /// <param name="pParentElement">The parent element.</param>
        /// <param name="pSerializationContext">The serialization context.</param>
        /// <returns>The modified parent element</returns>
        public virtual XElement Write(object pObject, XElement pParentElement, IXSerializationContext pSerializationContext)
        {
            pParentElement.Add(pSerializationContext.ReferenceType(pObject.GetType()));

            XElement lValuelement = new XElement(XConstants.VALUE_TAG);
            lValuelement.Value = pObject.ToString();
            pParentElement.Add(lValuelement);

            return pParentElement;
        }
    }
}

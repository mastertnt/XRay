using System;
using System.Reflection;
using System.Xml.Linq;
using XSystem;

namespace XSerialization
{
    /// <summary>
    /// This class defines a serialization contract based on attribute.
    /// </summary>
    public class AttributeSerializationContract : IAttributeXSerializationContract
    {
        /// <summary>
        /// Gets the supported type.
        /// </summary>
        public Type SupportedAttribute
        {
            get
            {
                return typeof(XSerializationAttribute);
            }
        }

        /// <summary>
        /// Gets or sets the decorated contract.
        /// </summary>
        public IXSerializationContract SubContract { get; set; }

        /// <summary>
        /// Flag to know if an external creation is necessary.
        /// </summary>
        public bool NeedCreate
        {
            get
            {
                return this.SubContract.NeedCreate;
            }
        }

        /// <summary>
        /// This method determines whether this type can manage the specified object.
        /// </summary>
        /// <param name="pObjectType">The object type to test.</param>
        /// <param name="pSerializationContext">The serialization context.</param>
        /// <returns>The depth of inheritance or -1 if the contract cannot support.</returns>
        public SupportPriority CanManage(Type pObjectType, IXSerializationContext pSerializationContext)
        {
           return SupportPriority.CANNOT_SUPPORT;
        }

        /// <summary>
        /// This method determines whether this instance can manage the specified object.
        /// </summary>
        /// <param name="pObject">The object to test.</param>
        /// <param name="pSerializationContext">The serialization context.</param>
        /// <returns>The depth of inheritance or -1 if the contract cannot support.</returns>
        public SupportPriority CanManage(object pObject, IXSerializationContext pSerializationContext)
        {
            if (pObject is Attribute)
            {
                return this.CanManage(pObject as Attribute, pSerializationContext);
            }
            return SupportPriority.CANNOT_SUPPORT;
        }

        /// <summary>
        /// This method determines whether this instance can manage the specified object.
        /// </summary>
        /// <param name="pAttribute">The attribute to test.</param>
        /// <param name="pSerializationContext">The serialization context.</param>
        /// <returns>The depth of inheritance or -1 if the contract cannot support.</returns>
        public virtual SupportPriority CanManage(Attribute pAttribute, IXSerializationContext pSerializationContext)
        {
            if (this.SupportedAttribute.IsInstanceOfType(pAttribute))
            {
                return new SupportPriority(SupportLevel.Attribute, pAttribute.GetType().DistanceTo(this.SupportedAttribute));
            }

            return SupportPriority.CANNOT_SUPPORT;
        }

        /// <summary>
        /// This method determines whether this instance can manage the specified object.
        /// </summary>
        /// <param name="pElement">The element to test.</param>
        /// <param name="pSerializationContext">The serialization context.</param>
        /// <returns>The depth of inheritance or -1 if the contract cannot support.</returns>
        public virtual SupportPriority CanManage(XElement pElement, IXSerializationContext pSerializationContext)
        {
            return SupportPriority.CANNOT_SUPPORT;
        }

        /// <summary>
        /// Creates the specified element.
        /// </summary>
        /// <param name="pElement">The element.</param>
        /// <param name="pSerializationContext">The serialization context.</param>
        /// <returns></returns>
        public object Create(XElement pElement, IXSerializationContext pSerializationContext)
        {
            return this.SubContract.Create(pElement, pSerializationContext);
        }

        /// <summary>
        /// Reads the specified element.
        /// </summary>
        /// <param name="pObjectToInitialize"></param>
        /// <param name="pParentElement">The parent element.</param>
        /// <param name="pSerializationContext">The serialization context.</param>
        /// <returns></returns>
        public object Read(object pObjectToInitialize, XElement pParentElement, IXSerializationContext pSerializationContext)
        {
            PropertyInfo lPropertyInfo = pObjectToInitialize as PropertyInfo;
// ReSharper disable once PossibleNullReferenceException
            if (pParentElement.Element(lPropertyInfo.Name) != null)
            {
                object lValue = lPropertyInfo.GetValue(pSerializationContext.CurrentObject, null);
                XElement lPropertyElement = pParentElement.Element(lPropertyInfo.Name);
                return this.SubContract.Read(lValue, lPropertyElement, pSerializationContext);
            }
            return pObjectToInitialize;
        }

        /// <summary>
        /// Writes the specified object.
        /// </summary>
        /// <param name="pObject">The object.</param>
        /// <param name="pParentElement">The parent element.</param>
        /// <param name="pSerializationContext">The serialization context.</param>
        /// <returns></returns>
        public XElement Write(object pObject, XElement pParentElement, IXSerializationContext pSerializationContext)
        {
            PropertyInfo lPropertyInfo = pObject as PropertyInfo;

            //Create Element named after the property
// ReSharper disable once PossibleNullReferenceException
            XElement lPropElement = new XElement(lPropertyInfo.Name);
            XElement lXResult = this.SubContract.Write(lPropertyInfo.GetValue(pSerializationContext.CurrentObject, null), lPropElement, pSerializationContext);

            //TOCHECK : Only add property element if it is not empty (for NoWrite contract which returns null).
            if (lXResult != null)
            {
                pParentElement.Add(lPropElement); 
            }
            return pParentElement;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using XSerialization.Attributes;
using XSystem;

namespace XSerialization.Defaults
{
    /// <summary>
    /// This class defines the default serialization contract for property info.
    /// </summary>
    public class DefaultPropertyInfoSerializationContract : ATypeSerializationContract<PropertyInfo>
    {
        #region Methods

        /// <summary>
        /// This method determines whether this instance can manage the specified object.
        /// </summary>
        /// <param name="pObjectType">The type to test.</param>
        /// <param name="pSerializationContext">The serialization context.</param>
        /// <returns>The depth of inheritance or -1 if the contract cannot support.</returns>
        public override SupportPriority CanManage(Type pObjectType, IXSerializationContext pSerializationContext)
        {
            return base.CanManage(pObjectType, pSerializationContext);
        }

        /// <summary>
        /// This method determines whether this instance can manage the specified object.
        /// </summary>
        /// <param name="pObject">The object to test.</param>
        /// <param name="pSerializationContext">The serialization context.</param>
        /// <returns>The depth of inheritance or -1 if the contract cannot support.</returns>
        public override SupportPriority CanManage(object pObject, IXSerializationContext pSerializationContext)
        {
            if (pObject is PropertyInfo)
            {
                PropertyInfo lPropertyInfo = pObject as PropertyInfo;
                if (lPropertyInfo.CanRead && lPropertyInfo.CanWrite)
                {
                    object[] lAttributes = lPropertyInfo.GetCustomAttributes(typeof(XInternalSerializationAttribute), true);
                    if (!lAttributes.Any())
                    {
                        ForceWriteXSerializationAttribute lForceWriteAttribute = lAttributes.OfType<ForceWriteXSerializationAttribute>().FirstOrDefault();
                        if (lForceWriteAttribute == null)
                        {
                            return new SupportPriority(SupportLevel.Type, 0);
                        }
                    }
                }
            }
            return SupportPriority.CANNOT_SUPPORT;
        }

        /// <summary>
        /// This method creates the specified element.
        /// </summary>
        /// <param name="pObjectElement">The element.</param>
        /// <param name="pSerializationContext">The serialization context.</param>
        /// <returns>The created object.</returns>
        public override object Create(XElement pObjectElement, IXSerializationContext pSerializationContext)
        {
            Type lRetrievedType = AppDomain.CurrentDomain.GetTypeByFullName(pObjectElement.Value);
            if (lRetrievedType != null)
            {
                return Activator.CreateInstance(lRetrievedType, true);
            }
            return null;
        }

        /// <summary>
        /// This method reads the specified p object to initialize.
        /// </summary>
        /// <param name="pObjectToInitialize">The object to initialize.</param>
        /// <param name="pElement">The element.</param>
        /// <param name="pSerializationContext">The serialization context.</param>
        /// <returns>The initialized object</returns>
        public override object Read(object pObjectToInitialize, XElement pElement, IXSerializationContext pSerializationContext)
        {
            PropertyInfo lPropertyInfo = pObjectToInitialize as PropertyInfo;

            // Look for a sub-element with the good local name.
            IEnumerable<XElement> lElements = pElement.Elements(lPropertyInfo.Name);
            XElement lPropertyElement = lElements.FirstOrDefault();
            if (lPropertyElement == null)
            {
                IEnumerable<XElement> lDescendants = pElement.Descendants(lPropertyInfo.Name);
                lPropertyElement = lDescendants.FirstOrDefault();
            }
            if (lPropertyElement != null)
            {
                IXSerializationContract lSerializationContract = pSerializationContext.SelectContract(lPropertyElement, lPropertyInfo.PropertyType);
                if (lSerializationContract != null)
                {
                    object lReadObject = lSerializationContract.Read(lPropertyInfo.GetValue(pSerializationContext.CurrentObject, null), lPropertyElement, pSerializationContext);
                    lPropertyInfo.SetValue(pSerializationContext.CurrentObject, lReadObject, null);
                    return pObjectToInitialize;
                }
            }

            return null;
        }

        /// <summary>
        /// Writes the specified p object.
        /// </summary>
        /// <param name="pObject">The p object.</param>
        /// <param name="pParentElement">The p parent element.</param>
        /// <param name="pSerializationContext">The p serialization context.</param>
        /// <returns></returns>
        public override XElement Write(object pObject, XElement pParentElement, IXSerializationContext pSerializationContext)
        {
            PropertyInfo lPropertyInfo = pObject as PropertyInfo;
            XElement lObjectElement = new XElement(lPropertyInfo.Name);
            object lPropertyValue = lPropertyInfo.GetValue(pSerializationContext.CurrentObject, null);
            IXSerializationContract lSerializationContract = pSerializationContext.SelectContract(lObjectElement, lPropertyInfo.PropertyType);
            if (lPropertyValue == null)
            {
                lSerializationContract = pSerializationContext.SelectContract(lObjectElement, null);
            }
            if (lSerializationContract != null)
            {
                lSerializationContract.Write(lPropertyValue, lObjectElement, pSerializationContext);
            }
            pParentElement.Add(lObjectElement);
            return pParentElement;
        }

        #endregion // Methods.
    }
}

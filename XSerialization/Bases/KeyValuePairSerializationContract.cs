using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using XSystem;

namespace XSerialization.Bases
{
    /// <summary>
    /// This contract manages a KeyValuePair serialization.
    /// </summary>
    public class KeyValuePairSerializationContract : IXSerializationContract
    {
        /// <summary>
        /// This method checks if the object type can be managed by the contract.
        /// </summary>
        /// <param name="pObjectType">The object type to manage.</param>
        /// <param name="pSerializationContext">The context of serialization</param>
        /// <returns>A support priority</returns>
        /// <remarks>
        /// The object can be a type, a property info, ...
        /// </remarks>
        public SupportPriority CanManage(Type pObjectType, IXSerializationContext pSerializationContext)
        {
            if (pObjectType.Name == typeof(KeyValuePair<,>).Name)
            {
                return new SupportPriority(SupportLevel.Type, 0);
            }
            return SupportPriority.CANNOT_SUPPORT;
        }

        /// <summary>
        /// This method checks if the object can be managed by the contract.
        /// </summary>
        /// <param name="pObject">The object to manage.</param>
        /// <param name="pSerializationContext">The context of serialization</param>
        /// <returns>A support priority</returns>
        /// <remarks>
        /// The object can be a type, a property info, ...
        /// </remarks>
        public SupportPriority CanManage(object pObject, IXSerializationContext pSerializationContext)
        {
            if (pObject != null)
            {
// ReSharper disable once OperatorIsCanBeUsed
                return this.CanManage(pObject.GetType(), pSerializationContext);

            }
            return SupportPriority.CANNOT_SUPPORT;
        }

        /// <summary>
        /// This method checks if the object can be managed by the contract.
        /// </summary>
        /// <param name="pParentElement">The element to manage.</param>
        /// <param name="pSerializationContext">The context of serialization</param>
        /// <returns>A support priority</returns>
        /// <remarks>
        /// The object can be a type, a property info, ...
        /// </remarks>
        public SupportPriority CanManage(XElement pParentElement, IXSerializationContext pSerializationContext)
        {
            return SupportPriority.CANNOT_SUPPORT;
        }

        /// <summary>
        /// Flag to know if an external creation is necessary.
        /// </summary>
        public bool NeedCreate
        {
            get { return true; }
        }

        /// <summary>
        /// This method creates the specified element.
        /// </summary>
        /// <param name="pParentElement">The element.</param>
        /// <param name="pSerializationContext">The serialization context.</param>
        /// <returns>
        /// The created object.
        /// </returns>
        public object Create(XElement pParentElement, IXSerializationContext pSerializationContext)
        {
            XElement lTypeElement = pParentElement.Element(XConstants.TYPE_TAG);
            if (lTypeElement != null)
            {
                Type lRetrievedType = pSerializationContext.ResolveType(lTypeElement);
                if (lRetrievedType != null)
                {
                    XElement lKeyElement = pParentElement.Descendants(XConstants.KEY_TAG).FirstOrDefault();
                    IXSerializationContract lKeyContract = pSerializationContext.SelectContract(lKeyElement, null, lRetrievedType.GetGenericArguments()[0], null);
                    object lKeyObject = null;
                    if (lKeyContract.NeedCreate)
                    {
                        lKeyObject = lKeyContract.Create(lKeyElement, pSerializationContext);
                    }
                    else
                    {
                        try
                        {
                            lKeyObject = Activator.CreateInstance(lRetrievedType.GetGenericArguments()[0], true);
                        }
                        catch
                        {
                        }

                    }
                    lKeyObject = lKeyContract.Read(lKeyObject, lKeyElement, pSerializationContext);

                    XElement lValueElement = pParentElement.Descendants(XConstants.VALUE_TAG).FirstOrDefault();
                    IXSerializationContract lValueContract = pSerializationContext.SelectContract(lValueElement, null, lRetrievedType.GetGenericArguments()[1], null);
                    object lValueObject = null;
                    if (lValueContract.NeedCreate)
                    {
                        lValueObject = lValueContract.Create(lValueElement, pSerializationContext);
                    }
                    else
                    {
                        try
                        {
                            lValueObject = Activator.CreateInstance(lRetrievedType.GetGenericArguments()[1], true);
                        }
                        catch
                        {
                        }

                    }
                    lValueObject = lValueContract.Read(lValueObject, lValueElement, pSerializationContext);
                    return Activator.CreateInstance(lRetrievedType, new object[] {lKeyObject, lValueObject});
                }
            }
            return null;
        }

        /// <summary>
        /// THis method deserialized an X element in to an object.
        /// </summary>
        /// <param name="pObjectToInitialize"></param>
        /// <param name="pParentElement">The element to convert.</param>
        /// <param name="pSerializationContext">The context of serialization</param>
        /// <returns>The initialized object</returns>
        public object Read(object pObjectToInitialize, XElement pParentElement, IXSerializationContext pSerializationContext)
        {
            // The key value pair is initialized with its constructor.
            return pObjectToInitialize;
        }

        /// <summary>
        /// THis method serializes the object in to an XElement.
        /// </summary>
        /// <param name="pObject">The object to serialize.</param>
        /// <param name="pParentElement"></param>
        /// <param name="pSerializationContext">The context of serialization</param>
        /// <returns>
        /// The modified XElement.
        /// </returns>
        public XElement Write(object pObject, XElement pParentElement, IXSerializationContext pSerializationContext)
        {
            pParentElement.Add(pSerializationContext.ReferenceType(pObject.GetType()));

            IXSerializationContract lKeyContract = pSerializationContext.SelectContract(null, null, pObject.GetPropertyType("Key"), null);
            if (lKeyContract != null)
            {
                XElement lKeyElement = new XElement(XConstants.KEY_TAG);
                lKeyContract.Write(pObject.GetPropertyValue("Key"), lKeyElement, pSerializationContext);
                pParentElement.Add(lKeyElement);
            }

            IXSerializationContract lValueContract = pSerializationContext.SelectContract(null, null, pObject.GetPropertyType("Value"), null);
            if (lValueContract != null)
            {
                XElement lValueElement = new XElement(XConstants.VALUE_TAG);
                lValueContract.Write(pObject.GetPropertyValue("Value"), lValueElement, pSerializationContext);
                pParentElement.Add(lValueElement);
            }
            return pParentElement;
        }
    }
}

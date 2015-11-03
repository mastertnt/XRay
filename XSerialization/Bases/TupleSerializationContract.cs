using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using XSystem;

namespace XSerialization.Bases
{
    /// <summary>
    /// This contract manages a Tuple serialization.
    /// </summary>
    public class TupleSerializationContract : IXSerializationContract
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
            if (pObjectType.Name.StartsWith(typeof (Tuple).Name))
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
                    List<object> lParameters = new List<object>();
                    for (int lGenericIndex = 0; lGenericIndex < lRetrievedType.GetGenericArguments().Count(); lGenericIndex++)
                    {
                        string lItemName = XConstants.ITEM_TAG + (lGenericIndex + 1);
                        XElement lItemElement = pParentElement.Descendants(lItemName).FirstOrDefault();
                        IXSerializationContract lItemContract = pSerializationContext.SelectContract(lItemElement, null, lRetrievedType.GetGenericArguments()[lGenericIndex], null);
                        object lItemObject = null;
                        if (lItemContract.NeedCreate)
                        {
                            lItemObject = lItemContract.Create(lItemElement, pSerializationContext);
                        }
                        else
                        {
                            try
                            {
                                lItemObject = Activator.CreateInstance(lRetrievedType.GetGenericArguments()[lGenericIndex], true);
                            }
                            catch
                            {
                            }

                        }
                        lItemObject = lItemContract.Read(lItemObject, lItemElement, pSerializationContext);
                        lParameters.Add(lItemObject);
                    }

                    return Activator.CreateInstance(lRetrievedType, lParameters.ToArray());
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
            // The tuple is initialized with its constructor.
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

            for (int lGenericIndex = 0; lGenericIndex < pObject.GetType().GetGenericArguments().Count(); lGenericIndex++)
            {
                string lItemName = XConstants.ITEM_TAG + (lGenericIndex + 1);
                IXSerializationContract lKeyContract = pSerializationContext.SelectContract(null, null, pObject.GetPropertyType(lItemName), null);
                if (lKeyContract != null)
                {
                    XElement lKeyElement = new XElement(lItemName);
                    lKeyContract.Write(pObject.GetPropertyValue(lItemName), lKeyElement, pSerializationContext);
                    pParentElement.Add(lKeyElement);
                }
            }

            return pParentElement;
        }
    }
}

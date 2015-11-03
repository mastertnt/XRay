using System;
using System.Xml.Linq;
using XSystem;

namespace XSerialization.Bases
{
    /// <summary>
    /// This contract manages a generic Nullable serialization contract.
    /// </summary>
    /// <!-- nby -->
    public class NullableSerializationContract : IXSerializationContract
    {
        /// <summary>
        /// Flag to know if an external creation is necessary.
        /// </summary>
        public bool NeedCreate
        {
            get { return false; }
        }

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
            if (pObjectType.Name.StartsWith(typeof(Nullable).Name))
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
        /// <param name="pObjectElement">The element to manage.</param>
        /// <param name="pSerializationContext">The context of serialization</param>
        /// <returns>A support priority</returns>
        /// <remarks>
        /// The object can be a type, a property info, ...
        /// </remarks>
        public SupportPriority CanManage(XElement pObjectElement, IXSerializationContext pSerializationContext)
        {
            return SupportPriority.CANNOT_SUPPORT;
        }

        /// <summary>
        /// This method creates the specified element.
        /// </summary>
        /// <param name="pParentElement">The element.</param>
        /// <param name="pSerializationContext">The serialization context.</param>
        /// <returns>
        /// The created object.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public object Create(XElement pParentElement, IXSerializationContext pSerializationContext)
        {
            return null;
        }

        
        /// <summary>
        /// THis method deserialized an X element in to an object.
        /// </summary>
        /// <param name="pObjectToInitialize"></param>
        /// <param name="pParentElement">The parent element to convert.</param>
        /// <param name="pSerializationContext">The context of serialization</param>
        /// <returns>The initialized object</returns>
        public object Read(object pObjectToInitialize, XElement pParentElement, IXSerializationContext pSerializationContext)
        {
            XElement lTypeElement = pParentElement.Element(XConstants.TYPE_TAG);
            XElement lValueElement = pParentElement.Element(XConstants.VALUE_TAG);
            if (lTypeElement != null && lValueElement != null)
            {
                Type lRetrievedType = pSerializationContext.ResolveType(lTypeElement);
                if (lRetrievedType != null)
                {
                    IXSerializationContract lSerializationContract = pSerializationContext.SelectContract(lValueElement, lRetrievedType.GetGenericArguments()[0]);
                    if (lSerializationContract != null)
                    {
                        Type lValueType = lRetrievedType.GetGenericArguments()[0];
                        object lValue = lValueType.DefaultValue();
                        lValue = lSerializationContract.Read(lValue, lValueElement, pSerializationContext);
                        return Activator.CreateInstance(lRetrievedType, lValue);
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// THis method serializes the object in to an XElement.
        /// </summary>
        /// <param name="pObject">The object to serialize.</param>
        /// <param name="pParentElement">The parent element</param>
        /// <param name="pSerializationContext">The context of serialization</param>
        /// <returns>
        /// The modified XElement.
        /// </returns>
        public XElement Write(object pObject, XElement pParentElement, IXSerializationContext pSerializationContext)
        {
            if (pObject != null)
            {
                Type lNullableType = typeof(Nullable<>).MakeGenericType(pObject.GetType());
                pParentElement.Add(pSerializationContext.ReferenceType(lNullableType));
            }

            XElement lValueElement = new XElement(XConstants.VALUE_TAG);
            IXSerializationContract lSerializationContract = pSerializationContext.SelectContract(pParentElement, pObject);
            if (lSerializationContract != null)
            {
                lSerializationContract.Write(pObject, lValueElement, pSerializationContext);
            }
            pParentElement.Add(lValueElement);
            
            return pParentElement;
        }
    }
}

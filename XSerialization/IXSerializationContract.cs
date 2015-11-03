using System;
using System.Xml.Linq;

namespace XSerialization
{
    /// <summary>
    /// This interface declares a serialization contract.
    /// </summary>
    public interface IXSerializationContract
    {
        /// <summary>
        /// Flag to know if an external creation is necessary.
        /// </summary>
        bool NeedCreate { get; }

        /// <summary>
        /// This method checks if the object type can be managed by the contract.
        /// </summary>
        /// <param name="pObjectType">The object type to manage.</param>
        /// <remarks>The object can be a type, a property info, ... </remarks>
        /// <param name="pSerializationContext">The context of serialization</param>
        /// <returns>The support priority or SupportPriority.CANNOT_SUPPORT</returns>
        SupportPriority CanManage(Type pObjectType, IXSerializationContext pSerializationContext);

        /// <summary>
        /// This method checks if the object can be managed by the contract.
        /// </summary>
        /// <param name="pObject">The object to manage.</param>
        /// <remarks>The object can be a type, a property info, ... </remarks>
        /// <param name="pSerializationContext">The context of serialization</param>
        /// <returns>The support priority or SupportPriority.CANNOT_SUPPORT</returns>
        SupportPriority CanManage(object pObject, IXSerializationContext pSerializationContext);

        /// <summary>
        /// This method checks if the object can be managed by the contract.
        /// </summary>
        /// <param name="pParentElement">The element to manage.</param>
        /// <remarks>The object can be a type, a property info, ... </remarks>
        /// <param name="pSerializationContext">The context of serialization</param>
        /// <returns>The support priority or SupportPriority.CANNOT_SUPPORT</returns>
        SupportPriority CanManage(XElement pParentElement, IXSerializationContext pSerializationContext);

        /// <summary>
        /// This method creates the specified element.
        /// </summary>
        /// <param name="pParentElement">The element.</param>
        /// <param name="pSerializationContext">The serialization context.</param>
        /// <returns>The created object.</returns>
        object Create(XElement pParentElement, IXSerializationContext pSerializationContext);

        /// <summary>
        /// THis method deserializes an X element in to an object.
        /// </summary>
        /// <param name="pObjectToInitialize"></param>
        /// <param name="pParentElement">The element to convert.</param>
        /// <param name="pSerializationContext">The context of serialization</param>
        /// <returns>The initialized object</returns>
        object Read(object pObjectToInitialize, XElement pParentElement, IXSerializationContext pSerializationContext);

        /// <summary>
        /// This method serializes the object in to an XElement.
        /// </summary>
        /// <param name="pObject">The object to serialize.</param>
        /// <param name="pParentElement">The parent element.</param>
        /// <param name="pSerializationContext">The context of serialization</param>
        /// <returns>The modified parent.</returns>
        XElement Write(object pObject, XElement pParentElement, IXSerializationContext pSerializationContext);
    }
}

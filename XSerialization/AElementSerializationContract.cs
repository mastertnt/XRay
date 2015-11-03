using System;
using System.Collections.Generic;
using System.Xml.Linq;
using XSystem;

namespace XSerialization
{
    /// <summary>
    /// This class defines a serialization contract based on XElement.
    /// </summary>
    public abstract class AElementSerializationContract : IElementXSerializationContract
    {
        #region Properties

        /// <summary>
        /// Flag to know if an external creation is necessary.
        /// </summary>    
        public virtual bool NeedCreate
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the element name.
        /// </summary>
        public string ElementName
        {
            get;
            private set;
        }

        #endregion // Properties.

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AElementSerializationContract"/> class.
        /// </summary>
        /// <param name="pElementName">The element name.</param>
        protected AElementSerializationContract(string pElementName)
        {
            this.ElementName = pElementName;
        }

        #endregion // Constructors.

        #region Methods

        /// <summary>
        /// This method checks if the object type can be managed by the contract.
        /// </summary>
        /// <param name="pObjectType">The object type to manage.</param>
        /// <remarks>The object can be a type, a property info, ... </remarks>
        /// <param name="pSerializationContext">The context of serialization</param>
        /// <returns>The support priority or SupportPriority.CANNOT_SUPPORT</returns>
        public virtual SupportPriority CanManage(Type pObjectType, IXSerializationContext pSerializationContext)
        {
            return SupportPriority.CANNOT_SUPPORT;
        }

        /// <summary>
        /// This method checks if the object can be managed by the contract.
        /// </summary>
        /// <param name="pObject">The object to manage.</param>
        /// <remarks>The object can be a type, a property info, ... </remarks>
        /// <param name="pSerializationContext">The context of serialization</param>
        /// <returns>The support priority or SupportPriority.CANNOT_SUPPORT</returns>
        public virtual SupportPriority CanManage(object pObject, IXSerializationContext pSerializationContext)
        {
            return SupportPriority.CANNOT_SUPPORT;
        }

        /// <summary>
        /// This method checks if the object can be managed by the contract.
        /// </summary>
        /// <param name="pParentElement">The element to manage.</param>
        /// <remarks>The object can be a type, a property info, ... </remarks>
        /// <param name="pSerializationContext">The context of serialization</param>
        /// <returns>The support priority or SupportPriority.CANNOT_SUPPORT</returns>
        public virtual SupportPriority CanManage(XElement pParentElement, IXSerializationContext pSerializationContext)
        {
            if (pParentElement.Name == this.ElementName)
            {
                return new SupportPriority(SupportLevel.Element, 0);
            }

            return SupportPriority.CANNOT_SUPPORT;
        }

        /// <summary>
        /// This method creates the specified element.
        /// </summary>
        /// <param name="pObjectElement">The object element.</param>
        /// <param name="pSerializationContext">The serialization context.</param>
        /// <returns>The created object.</returns>
        public virtual object Create(XElement pObjectElement, IXSerializationContext pSerializationContext)
        {
            return null;
        }

        /// <summary>
        /// This method reads the specified element.
        /// </summary>
        /// <param name="pObjectToInitialize">The object to initialize</param>
        /// <param name="pElement">The element.</param>
        /// <param name="pSerializationContext">The serialization context.</param>
        /// <returns>The initialized object if the input object is valid.</returns>
        public abstract object Read(object pObjectToInitialize, XElement pElement, IXSerializationContext pSerializationContext);

        /// <summary>
        /// This method writes the specified object.
        /// </summary>
        /// <param name="pObject">The object to serialize.</param>
        /// <param name="pParentElement">The parent element.</param>
        /// <param name="pSerializationContext">The serialization context.</param>
        /// <returns>The modified parent element</returns>
        public abstract XElement Write(object pObject, XElement pParentElement, IXSerializationContext pSerializationContext);

        #endregion // Methods.
    }
}

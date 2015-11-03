using System;
using System.Globalization;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using System.Xml.Linq;
using XSerialization.Values;
using XSystem;

namespace XSerialization.Decorators
{
    /// <summary>
    /// This decorator is used to give basics behavior to the serialization contracts.
    /// </summary>
    public class SerializationContractDecorator : IXSerializationContract
    {
        #region Properties

        /// <summary>
        /// Gets the decorated contract.
        /// </summary>
        /// <value>
        /// The decorated contract.
        /// </value>
        public IXSerializationContract DecoratedContract
        {
            get;
            private set;
        }

                /// <summary>
        /// Gets the decorated contract.
        /// </summary>
        /// <value>
        /// The decorated contract.
        /// </value>
        public XSerializer XSerializer
        {
            get;
            private set;
        }

        /// <summary>
        /// Flag to know if an external creation is necessary.
        /// </summary>
        public bool NeedCreate
        {
            get { return this.DecoratedContract.NeedCreate; }
        }

        #endregion // Properties

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SerializationContractDecorator" /> class.
        /// </summary>
        /// <param name="pDecoratedContract">The p decorated contract.</param>
        /// <param name="pXSerializer">The calling x serializer.</param>
        public SerializationContractDecorator(IXSerializationContract pDecoratedContract, XSerializer pXSerializer)
        {
            this.DecoratedContract = pDecoratedContract;
            this.XSerializer = pXSerializer;
        }

        #endregion // Constructors.

        #region Methods

        /// <summary>
        /// This method checks if the object type can be managed by the contract.
        /// </summary>
        /// <param name="pObjectType">The object type to manage.</param>
        /// <param name="pSerializationContext">The context of serialization</param>
        /// <returns>
        /// The support priority or SupportPriority.CANNOT_SUPPORT
        /// </returns>
        /// <remarks>
        /// The object can be a type, a property info, ...
        /// </remarks>
        public SupportPriority CanManage(Type pObjectType, IXSerializationContext pSerializationContext)
        {
            return SupportPriority.CANNOT_SUPPORT;
        }

        /// <summary>
        /// This method checks if the object can be managed by the contract.
        /// </summary>
        /// <param name="pObject">The object to manage.</param>
        /// <param name="pSerializationContext">The context of serialization</param>
        /// <returns>
        /// The support priority or SupportPriority.CANNOT_SUPPORT
        /// </returns>
        /// <remarks>
        /// The object can be a type, a property info, ...
        /// </remarks>
        public SupportPriority CanManage(object pObject, IXSerializationContext pSerializationContext)
        {
            return SupportPriority.CANNOT_SUPPORT;
        }

        /// <summary>
        /// This method checks if the object can be managed by the contract.
        /// </summary>
        /// <param name="pParentElement">The element to manage.</param>
        /// <param name="pSerializationContext">The context of serialization</param>
        /// <returns>
        /// The support priority or SupportPriority.CANNOT_SUPPORT
        /// </returns>
        /// <remarks>
        /// The object can be a type, a property info, ...
        /// </remarks>
        public SupportPriority CanManage(XElement pParentElement, IXSerializationContext pSerializationContext)
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
        public object Create(XElement pParentElement, IXSerializationContext pSerializationContext)
        {
            return this.DecoratedContract.Create(pParentElement, pSerializationContext);
        }

        /// <summary>
        /// THis method deserializes an X element in to an object.
        /// </summary>
        /// <param name="pObjectToInitialize"></param>
        /// <param name="pParentElement">The element to convert.</param>
        /// <param name="pSerializationContext">The context of serialization</param>
        /// <returns>
        /// The initialized object
        /// </returns>
        public object Read(object pObjectToInitialize, XElement pParentElement, IXSerializationContext pSerializationContext)
        {
            try
            {
                if (this.NeedCreate)
                {
                    pObjectToInitialize = this.Create(pParentElement, pSerializationContext);
                }

                int lReadReference = XConstants.NO_REFERENCED_OBJECT;
                XAttribute lAttribute = pParentElement.Attribute(XConstants.ID_ATTRIBUTE);
                if (lAttribute != null)
                {
                    lReadReference = Convert.ToInt32(lAttribute.Value.Trim(), CultureInfo.InvariantCulture);
                }
                bool lCanBeCurrent = this.CanBeCurrentObject(pObjectToInitialize);
                if (lCanBeCurrent)
                {
                    pSerializationContext.PushObject(pObjectToInitialize, lReadReference);
                }
                object lInitializedObject = this.DecoratedContract.Read(pObjectToInitialize, pParentElement, pSerializationContext);
                if (lCanBeCurrent)
                {
                    pSerializationContext.PopObject();
                }
                return lInitializedObject;
            }
            catch
            {
                IXmlLineInfo lInfo = pParentElement;
                pSerializationContext.PushError(new XSerializationError(XErrorType.Parsing, lInfo.LineNumber, lInfo.LinePosition, pSerializationContext.CurrentFile, string.Empty));
            }
            return pObjectToInitialize;
        }

        /// <summary>
        /// This method serializes the object in to an XElement.
        /// </summary>
        /// <param name="pObject">The object to serialize.</param>
        /// <param name="pParentElement">The parent element.</param>
        /// <param name="pSerializationContext">The context of serialization</param>
        /// <returns>
        /// The modified parent.
        /// </returns>
        public XElement Write(object pObject, XElement pParentElement, IXSerializationContext pSerializationContext)
        {
            try
            {
                int lReference = XConstants.NOT_YET_REFERENCED_OBJECT;
                string lExternalReference = string.Empty;

                if (pSerializationContext.ExternalReferenceResolver.HasExternalReference(pObject) && pSerializationContext.CurrentObject != null)
                {
                    lExternalReference = pSerializationContext.ExternalReferenceResolver.GetExternalReference(pObject);
                }

                bool lCanBeCurrent = this.CanBeCurrentObject(pObject);
                bool lCanBeReferenced = this.CanBeInternallyReferenced(pObject);
                if (lCanBeCurrent)
                {
                    if (lCanBeReferenced)
                    {
                        lReference = pSerializationContext.GetObjectReference(pObject); 
                    }
                    pSerializationContext.PushObject(pObject);
                }
                
                XElement lModifiedElement;
                if (string.IsNullOrEmpty(lExternalReference) == false)
                {
                    pParentElement.Add(new XComment("Serialized with XSerialization.Values.ExternalReferenceSerializationContract"));
                    lModifiedElement = new ExternalReferenceSerializationContract().Write(pObject, pParentElement, pSerializationContext);
                }
                else if (lReference != XConstants.NOT_YET_REFERENCED_OBJECT)
                {
                    pParentElement.Add(new XComment("Serialized with XSerialization.Values.InternalReferenceSerializationContract"));
                    lModifiedElement = new InternalReferenceSerializationContract().Write(pObject, pParentElement, pSerializationContext);
                }
                else
                {
                    pParentElement.Add(new XComment("Serialized with " + this.DecoratedContract.GetType().XmlFullname()));
                    lModifiedElement = this.DecoratedContract.Write(pObject, pParentElement, pSerializationContext);
                }
                 
                if (lCanBeCurrent)
                {
                    pSerializationContext.PopObject();
                }
                return lModifiedElement;
            }
            catch
            {
                IXmlLineInfo lInfo = pParentElement;
                pSerializationContext.PushError(new XSerializationError(XErrorType.Parsing, lInfo.LineNumber, lInfo.LinePosition, pSerializationContext.CurrentFile, string.Empty));
            }
            return pParentElement;
        }

        /// <summary>
        /// This method checks if the object can be a current object.
        /// </summary>
        /// <param name="pObject">The object to check.</param>
        /// <returns>true if the object can be current, false otherwise.</returns>
        private bool CanBeCurrentObject(object pObject)
        {
            return (pObject != null
                    && pObject.GetType().IsSimple() == false
                    && pObject.GetType().IsStruct() == false
                    && (pObject is PropertyInfo) == false
                    && (pObject is Attribute) == false);
        }

        /// <summary>
        /// Determines whether objecte can be internally referenced
        /// </summary>
        /// <param name="pObject">The object.</param>
        /// <returns></returns>
        private bool CanBeInternallyReferenced(object pObject)
        {
            return this.XSerializer.InternalReferenceExcludedType.Contains(pObject.GetType().FullName) == false;
        }

        #endregion // Methods.
    }
}

using System;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Win32.SafeHandles;
using XSystem;
using XSerialization.Values;

namespace XSerialization.Attributes
{
    /// <summary>
    /// This contract is used to serialize/deserialize fields.
    /// </summary>
    public class FieldSerializationContract : IAttributeXSerializationContract
    {
        #region Properties

        /// <summary>
        /// Gets or sets the attribute.
        /// </summary>
        /// <value>
        /// The attribute.
        /// </value>
        public XSerializationAttribute Attribute
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the attribute.
        /// </summary>
        /// <value>
        /// The attribute.
        /// </value>
        public ForceFieldContractXSerializationAttribute TypedAttribute
        {
            get
            {
                return this.Attribute as ForceFieldContractXSerializationAttribute;
            }
        }

        /// <summary>
        /// Gets the supported type.
        /// </summary>
        public Type SupportedAttribute
        {
            get
            {
                return typeof(ForceFieldContractXSerializationAttribute);
            }
        }

        /// <summary>
        /// Flag to know if an external creation is necessary.
        /// </summary>
        public bool NeedCreate
        {
            get
            {
                return false;
            }
        }

        #endregion // Properties.

        #region Methods

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
            return null;
        }

        /// <summary>
        /// This method determines whether this type can manage the specified object.
        /// </summary>
        /// <param name="pObjectType">The object type to test.</param>
        /// <param name="pSerializationContext">The serialization context.</param>
        /// <returns>The depth of inheritance or -1 if the contract cannot support.</returns>
        public virtual SupportPriority CanManage(Type pObjectType, IXSerializationContext pSerializationContext)
        {
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
            if (pObject is Attribute)
            {
                return this.CanManage((Attribute)pObject, pSerializationContext);
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
                this.Attribute = pAttribute as XSerializationAttribute;
                if (this.Attribute is ForceFieldContractXSerializationAttribute)
                {
                    ForceFieldContractXSerializationAttribute lForceFieldContract = this.Attribute as ForceFieldContractXSerializationAttribute;
                    if (this.GetType() == lForceFieldContract.SupportedContract)
                    {
                        this.Attribute = lForceFieldContract;
                        return new SupportPriority(SupportLevel.Attribute, 0);
                    }
                }
            }

            this.Attribute = null;
            return SupportPriority.CANNOT_SUPPORT;
        }

        /// <summary>
        /// This method determines whether this instance can manage the specified object.
        /// </summary>
        /// <param name="pParentElement">The parent element.</param>
        /// <param name="pSerializationContext">The serialization context.</param>
        /// <returns>The depth of inheritance or -1 if the contract cannot support.</returns>
        public virtual SupportPriority CanManage(XElement pParentElement, IXSerializationContext pSerializationContext)
        {
            return SupportPriority.CANNOT_SUPPORT;
        }

        /// <summary>
        /// This method reads the specified element.
        /// </summary>
        /// <param name="pObjectToInitialize">The object to initialize (it is the property value of the parent object)</param>
        /// <param name="pParentElement">The parent element (it is the property name of the parent object).</param>
        /// <param name="pSerializationContext">The serialization context.</param>
        /// <returns>The initialized object if the input object is valid.</returns>
        public virtual object Read(object pObjectToInitialize, XElement pParentElement, IXSerializationContext pSerializationContext)
        {
            PropertyInfo lPropertyInfo = pObjectToInitialize as PropertyInfo;
            if (lPropertyInfo != null && pSerializationContext.CurrentObject != null)
            {
                if (pParentElement.Elements(lPropertyInfo.Name).Elements("Field").Any())
                {
                    XElement lFieldElement = pParentElement.Elements(lPropertyInfo.Name).Elements("Field").FirstOrDefault(pElement => pElement.Attribute("fieldName") != null && pElement.Attribute("fieldName").Value == this.TypedAttribute.FieldName);
                    Type lFieldType;
                    object lFieldObject = pSerializationContext.CurrentObject.GetFieldValue(this.TypedAttribute.FieldName, out lFieldType);
                    IXSerializationContract lSerializationContract = pSerializationContext.SelectContract(null, lFieldType);
                    if (lSerializationContract != null)
                    {
                        object lReadFieldObject = lSerializationContract.Read(lFieldObject, lFieldElement, pSerializationContext);
                        pSerializationContext.CurrentObject.SetFieldValue(this.TypedAttribute.FieldName, lReadFieldObject);
                    }

                    // Check if a initial synchronisation method exits.
                    if (string.IsNullOrWhiteSpace(this.TypedAttribute.SyncFieldMethod) == false)
                    {
                        MethodInfo lSynchronizationMethod = pSerializationContext.CurrentObject.GetType().GetMethod(this.TypedAttribute.SyncFieldMethod, BindingFlags.NonPublic | BindingFlags.Instance);
                        if (lSynchronizationMethod != null)
                        {
                            lSynchronizationMethod.Invoke(pSerializationContext.CurrentObject, null);
                        }
                    }
                }
                else if (pParentElement.Elements(lPropertyInfo.Name).Any())
                {
                   object lValue = new InternalReferenceSerializationContract().Read(pObjectToInitialize, pParentElement.Elements(lPropertyInfo.Name).FirstOrDefault(), pSerializationContext);
                   pSerializationContext.CurrentObject.SetPropertyValue(lPropertyInfo.Name, lValue);
                }
            }
            else
            {
                IXmlLineInfo lInfo = pParentElement;
                pSerializationContext.PushError(new XSerializationError(XErrorType.Parsing, lInfo.LineNumber, lInfo.LinePosition, pSerializationContext.CurrentFile, "Field serialization contract must be called with a property info and not null object as parameter"));
            }

            return pObjectToInitialize;
        }

        /// <summary>
        /// This method writes the specified object.
        /// </summary>
        /// <param name="pObject">The object to serialize (it is the property value of the parent object)</param>
        /// <param name="pParentElement">The parent element (it is the property name of the parent object).</param>
        /// <param name="pSerializationContext">The serialization context.</param>
        /// <returns>The modified parent element</returns>
        public virtual XElement Write(object pObject, XElement pParentElement, IXSerializationContext pSerializationContext)
        {
            Type lFieldType;
            object lFieldObject = pSerializationContext.CurrentObject.GetFieldValue(this.TypedAttribute.FieldName, out lFieldType);

            XElement lFieldElement = new XElement("Field");
            lFieldElement.SetAttributeValue("fieldName", this.TypedAttribute.FieldName);
            IXSerializationContract lSerializationContract = pSerializationContext.SelectContract(null, lFieldObject);
            if (lSerializationContract != null)
            {
                lSerializationContract.Write(lFieldObject, lFieldElement, pSerializationContext);
            }
            pParentElement.Add(lFieldElement);

            this.Attribute = null;
            return pParentElement;
        }

        #endregion // Methods.
    }
}

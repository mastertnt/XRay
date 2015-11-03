using System;
using System.Reflection;
using System.Xml.Linq;

namespace XSerialization
{
    /// <summary>
    /// This class defines a serialization contract based on property info.
    /// </summary>
    public abstract class APropertyInfoSerializationContract : IPropertyInfoSerializationContract
    {
        /// <summary>
        /// Gets the supported declaring type.
        /// </summary>
        public abstract Type DeclaringType
        {
            get;
        }

        /// <summary>
        /// Gets the supported property type.
        /// </summary>
        public abstract Type PropertyType
        {
            get;
        }

        /// <summary>
        /// Gets the supported property name.
        /// </summary>
        public abstract string PropertyName
        {
            get;
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

        /// <summary>
        /// This method determines whether this type can manage the specified object.
        /// </summary>
        /// <param name="pObjectType">The object type to test.</param>
        /// <param name="pSerializationContext">The serialization context.</param>
        /// <returns>The depth of inheritance or -1 if the contract cannot support.</returns>
        public SupportPriority CanManage(Type pObjectType, IXSerializationContext pSerializationContext)
        {
            if (pObjectType == typeof(PropertyInfo))
            {
                return new SupportPriority(SupportLevel.Type, 0);
            }
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
            if (pObject is PropertyInfo)
            {
                return this.CanManage(pObject as PropertyInfo, pSerializationContext);
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
        /// This method determines whether this instance can manage the specified object.
        /// </summary>
        /// <param name="pPropertyInfo">The property info to test.</param>
        /// <param name="pSerializationContext">The serialization context.</param>
        /// <returns>The depth of inheritance or -1 if the contract cannot support.</returns>
        public SupportPriority CanManage(PropertyInfo pPropertyInfo, IXSerializationContext pSerializationContext)
        {
            if (pPropertyInfo.Name == this.PropertyName && this.DeclaringType.IsAssignableFrom(pPropertyInfo.DeclaringType) && this.PropertyType == pPropertyInfo.PropertyType)
            {
                return new SupportPriority(SupportLevel.PropertyInfo, 0);
            }
            return SupportPriority.CANNOT_SUPPORT;
        }

        /// <summary>
        /// Creates the specified element.
        /// </summary>
        /// <param name="pElement">The element.</param>
        /// <param name="pSerializationContext">The serialization context.</param>
        /// <returns></returns>
        public abstract object Create(XElement pElement, IXSerializationContext pSerializationContext);

        /// <summary>
        /// Reads the specified element.
        /// </summary>
        /// <param name="pObjectToInitialize"></param>
        /// <param name="pElement">The element.</param>
        /// <param name="pSerializationContext">The serialization context.</param>
        /// <returns></returns>
        public abstract object Read(object pObjectToInitialize, XElement pElement, IXSerializationContext pSerializationContext);

        /// <summary>
        /// Writes the specified object.
        /// </summary>
        /// <param name="pObject">The object.</param>
        /// <param name="pParentElement"></param>
        /// <param name="pSerializationContext">The serialization context.</param>
        /// <returns></returns>
        public abstract XElement Write(object pObject, XElement pParentElement, IXSerializationContext pSerializationContext);
    }
}

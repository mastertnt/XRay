using System;
using System.Collections.Generic;
using System.Xml.Linq;
using XSystem;

namespace XSerialization
{
    /// <summary>
    /// This class defines a serialization contract based on type.
    /// </summary>
    public abstract class ATypeSerializationContract<TType> : ITypeXSerializationContract
    {
        /// <summary>
        /// This dictionary stores SupportPriority of the SupportedType by Type.
        /// This works because SupportPriority objects cannot be modified.
        /// </summary>
        private Dictionary<Type, SupportPriority> mSupportPriorityForType = new Dictionary<Type, SupportPriority>();

        /// <summary>
        /// Gets the supported type.
        /// </summary>
        public virtual Type SupportedType
        {
            get { return typeof (TType); }
        }

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
        /// This method determines whether this instance can manage the specified object.
        /// </summary>
        /// <param name="pObject">The object to test.</param>
        /// <param name="pSerializationContext">The serialization context.</param>
        /// <returns>The depth of inheritance or -1 if the contract cannot support.</returns>
        public virtual SupportPriority CanManage(object pObject, IXSerializationContext pSerializationContext)
        {
            if (pObject != null)
            {
                return this.CanManage(pObject.GetType(), pSerializationContext);
            }
            return SupportPriority.CANNOT_SUPPORT;
        }

        /// <summary>
        /// This method determines whether this instance can manage the specified object.
        /// </summary>
        /// <param name="pType">The type to test.</param>
        /// <param name="pSerializationContext">The serialization context.</param>
        /// <returns>The depth of inheritance or -1 if the contract cannot support.</returns>
        public virtual SupportPriority CanManage(Type pType, IXSerializationContext pSerializationContext)
        {
            // Cache SupportPriority by type
            SupportPriority lSupportPriority;
            if
                (this.mSupportPriorityForType.TryGetValue(pType, out lSupportPriority) == false)
            {
                int lDepthOfInheritance = pType.DistanceTo(this.SupportedType);

                if
                    (lDepthOfInheritance == -1)
                {
                    lSupportPriority = SupportPriority.CANNOT_SUPPORT;
                }
                else if
                    (this.SupportedType.IsInterface)
                {
                    lSupportPriority = new SupportPriority(SupportLevel.Interface, lDepthOfInheritance);
                }
                else
                {
                    lSupportPriority = new SupportPriority(SupportLevel.Type, lDepthOfInheritance);
                }

                this.mSupportPriorityForType[pType] = lSupportPriority;
            }

            return lSupportPriority;
        }

        /// <summary>
        /// This method determines whether this instance can manage the specified element.
        /// </summary>
        /// <param name="pObjectElement">The object element.</param>
        /// <param name="pSerializationContext">The serialization context.</param>
        /// <returns>The depth of inheritance or -1 if the contract cannot support.</returns>
        public virtual SupportPriority CanManage(XElement pObjectElement, IXSerializationContext pSerializationContext)
        {
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
            XElement lTypeElement = pObjectElement.Element(XConstants.TYPE_TAG);
            if (lTypeElement != null)
            {
                Type lRetrievedType = pSerializationContext.ResolveType(lTypeElement);
                if (lRetrievedType != null)
                {
                    return Activator.CreateInstance(lRetrievedType, true);
                }
            }
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
    }
}

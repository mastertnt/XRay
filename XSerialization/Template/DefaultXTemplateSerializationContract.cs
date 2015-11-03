using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XSerialization.Template
{
    /// <summary>
    /// Class defining the default serialization contract for the <see cref="IXTemplate"/>.
    /// </summary>
    public class DefaultXTemplateSerializationContract : ATypeSerializationContract<IXTemplate>
    {
        /// <summary>
        /// Flag to know if an external creation is necessary.
        /// </summary>
        public override bool NeedCreate
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// This method determines whether this instance can manage the specified object.
        /// </summary>
        /// <param name="pType">The type to test.</param>
        /// <param name="pSerializationContext">The serialization context.</param>
        /// <returns>The depth of inheritance or -1 if the contract cannot support.</returns>
        public override SupportPriority CanManage(Type pType, IXSerializationContext pSerializationContext)
        {
            SupportPriority lInitialPriority = base.CanManage(pType, pSerializationContext);
            if (lInitialPriority.Level == SupportLevel.NotSupported)
            {
                return lInitialPriority;
            }
            return new SupportPriority(SupportLevel.Default, lInitialPriority.SubPriority);
        }

        /// <summary>
        /// This method reads the specified element.
        /// </summary>
        /// <param name="pObjectToInitialize">The object to initialize</param>
        /// <param name="pElement">The element.</param>
        /// <param name="pSerializationContext">The serialization context.</param>
        /// <returns>The initialized object if the input object is valid.</returns>
        public override object Read(object pObjectToInitialize, System.Xml.Linq.XElement pElement, IXSerializationContext pSerializationContext)
        {
            IXTemplate lTemplate = pObjectToInitialize as IXTemplate;
            if (lTemplate != null)
            {
                Type lTemplatedObjectType = pSerializationContext.ResolveType(pElement.Element(XConstants.TYPE_TAG));
                if (lTemplatedObjectType != null)
                {
                    IXSerializationContract lContract = pSerializationContext.SelectContract(null, lTemplatedObjectType);
                    object lTemplatedObject = null;
                    if (lContract != null)
                    {
                        lTemplatedObject = lContract.Read(lTemplatedObject, pElement, pSerializationContext);
                    }  

                    if (lTemplatedObject != null)
                    {
                        lTemplate.InitializeFrom(lTemplatedObject);
                    }
                }
            }

            return pObjectToInitialize;
        }

        /// <summary>
        /// This method writes the specified object.
        /// </summary>
        /// <param name="pObject">The object to serialize.</param>
        /// <param name="pParentElement">The parent element.</param>
        /// <param name="pSerializationContext">The serialization context.</param>
        /// <returns>The modified parent element</returns>
        public override System.Xml.Linq.XElement Write(object pObject, System.Xml.Linq.XElement pParentElement, IXSerializationContext pSerializationContext)
        {
            IXTemplate lTemplate = pObject as IXTemplate;
            if (lTemplate != null)
            {
                // Creating the templated object.
                object lTemplatedObject = lTemplate.Create();
                if (lTemplatedObject != null)
                {
                    IXSerializationContract lContract = pSerializationContext.SelectContract(null, lTemplatedObject);
                    if (lContract != null)
                    {
                        lContract.Write(lTemplatedObject, pParentElement, pSerializationContext);
                    }
                }
            }

            return pParentElement;
        }
    }
}

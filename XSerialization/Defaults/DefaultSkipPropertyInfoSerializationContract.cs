using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace XSerialization.Defaults
{
    class DefaultSkipPropertyInfoSerializationContract : DefaultPropertyInfoSerializationContract
    {
        /// <summary>
        /// This method determines whether this instance can manage the specified object.
        /// </summary>
        /// <param name="pObject">The object to test.</param>
        /// <param name="pSerializationContext">The serialization context.</param>
        /// <returns>The depth of inheritance or -1 if the contract cannot support.</returns>
        public override SupportPriority CanManage(object pObject, IXSerializationContext pSerializationContext)
        {
            if (pObject is PropertyInfo)
            {
                PropertyInfo lPropertyInfo = pObject as PropertyInfo;
                if (lPropertyInfo.CanWrite == false)
                {
                    object[] lAttributes = lPropertyInfo.GetCustomAttributes(typeof(XInternalSerializationAttribute), true);
                    if (!lAttributes.Any())
                    {
                        return new SupportPriority(SupportLevel.Type, 0);
                    }
                }
            }
            return SupportPriority.CANNOT_SUPPORT;
        }

        /// <summary>
        /// This method reads the specified object to initialize.
        /// </summary>
        /// <param name="pObjectToInitialize">The object to initialize.</param>
        /// <param name="pParentElement">The parent element.</param>
        /// <param name="pSerializationContext">The serialization context.</param>
        /// <returns>The initialized object</returns>
        public override object Read(object pObjectToInitialize, XElement pParentElement, IXSerializationContext pSerializationContext)
        {
            return pObjectToInitialize;
        }

        /// <summary>
        /// Writes the specified p object.
        /// </summary>
        /// <param name="pObject">The p object.</param>
        /// <param name="pParentElement">The p parent element.</param>
        /// <param name="pSerializationContext">The p serialization context.</param>
        /// <returns></returns>
        public override XElement Write(object pObject, XElement pParentElement, IXSerializationContext pSerializationContext)
        {   
            return pParentElement;
        }
    }
}

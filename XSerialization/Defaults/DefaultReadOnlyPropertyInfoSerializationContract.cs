using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using XSerialization.Attributes;

namespace XSerialization.Defaults
{
    /// <summary>
    /// Contract allowing one to save an object which will not be read.
    /// (for properties with no set)
    /// </summary>
    public class DefaultReadOnlyPropertyInfoSerializationContract : DefaultPropertyInfoSerializationContract
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

                object[] lAttributes = lPropertyInfo.GetCustomAttributes(typeof (XInternalSerializationAttribute), true);
                if (lAttributes.Any())
                {
                    ForceWriteXSerializationAttribute lForceWriteAttribute = lAttributes.OfType<ForceWriteXSerializationAttribute>().FirstOrDefault();
                    if (lPropertyInfo.CanRead && lForceWriteAttribute != null)
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
            PropertyInfo lPropertyInfo = pObjectToInitialize as PropertyInfo;

            // Look for a sub-element with the good local name.
// ReSharper disable once PossibleNullReferenceException
            IEnumerable<XElement> lElements = pParentElement.Elements(lPropertyInfo.Name);
            XElement lPropertyElement = lElements.FirstOrDefault();
            if (lPropertyElement == null)
            {
                IEnumerable<XElement> lDescendants = pParentElement.Descendants(lPropertyInfo.Name);
                lPropertyElement = lDescendants.FirstOrDefault();
            }
            if (lPropertyElement != null)
            {
                IXSerializationContract lSerializationContract = pSerializationContext.SelectContract(lPropertyElement, lPropertyInfo.PropertyType);
                if (lSerializationContract != null)
                {
                    object lReadObject = lSerializationContract.Read(lPropertyInfo.GetValue(pSerializationContext.CurrentObject, null), lPropertyElement, pSerializationContext);
                    return pObjectToInitialize;
                }
            }

            return null;
        }
    }
}

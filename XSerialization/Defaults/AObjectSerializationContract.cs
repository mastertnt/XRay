using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace XSerialization.Defaults
{
    /// <summary>
    /// This class defines the serialization contract for an object.
    /// </summary>
    public abstract class AObjectSerializationContract<TType> : ATypeSerializationContract<TType>
    {
        #region Properties

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

        #endregion // Properties

        #region Methods

        /// <summary>
        /// This method reads the specified object to initialize.
        /// </summary>
        /// <param name="pObjectToInitialize">The object to initialize.</param>
        /// <param name="pElement">The element.</param>
        /// <param name="pSerializationContext">The serialization context.</param>
        /// <returns>The initialized object</returns>
        public override object Read(object pObjectToInitialize, XElement pElement, IXSerializationContext pSerializationContext)
        {
            if (pObjectToInitialize != null)
            {
                //Get public propertyinfo and the private one bearing a XSerializationAttribute attribute.
                PropertyInfo[] lPublicPropertyInfos = pObjectToInitialize.GetType().GetProperties();

                // Heed non public properties bearing a IXSerializationAttribute
                PropertyInfo[] lNonPublicPropertyInfos = pObjectToInitialize.GetType().GetProperties(BindingFlags.Instance | BindingFlags.NonPublic);
                IEnumerable<PropertyInfo> lFilteredPropertyInfos = lPublicPropertyInfos.Union(lNonPublicPropertyInfos.Where(pElt => pElt.GetCustomAttributes(typeof(IXSerializationAttribute), true).Any()));
                foreach (PropertyInfo lPropertyInfo in lFilteredPropertyInfos)
                {
                    IXSerializationContract lSerializationContract = pSerializationContext.SelectContract(pElement, lPropertyInfo);
                    if (lSerializationContract != null)
                    {
                        lSerializationContract.Read(lPropertyInfo, pElement, pSerializationContext);
                    }
                }
            }
            
            return pObjectToInitialize;
        }

        /// <summary>
        /// Writes the specified object.
        /// </summary>
        /// <param name="pObject">The object.</param>
        /// <param name="pParentElement">The parent element.</param>
        /// <param name="pSerializationContext">The serialization context.</param>
        /// <returns></returns>
        public override XElement Write(object pObject, XElement pParentElement, IXSerializationContext pSerializationContext)
        {
            pParentElement.Add(pSerializationContext.ReferenceType(pObject.GetType()));
            
            // Get public propertyinfo and the private one bearing a XSerializationAttribute attribute.
            PropertyInfo[] lPublicPropertyInfos = pObject.GetType().GetProperties();
            PropertyInfo[] lNonPublicPropertyInfos = pObject.GetType().GetProperties(BindingFlags.Instance | BindingFlags.NonPublic);
            IEnumerable<PropertyInfo> lFilteredPropertyInfos = lPublicPropertyInfos.Union(lNonPublicPropertyInfos.Where(pElt => pElt.GetCustomAttributes(typeof(IXSerializationAttribute), true).Any()));
            pParentElement.SetAttributeValue(XConstants.ID_ATTRIBUTE, pSerializationContext.GetObjectReference(pObject));
            foreach (PropertyInfo lPropertyInfo in lFilteredPropertyInfos)
            {
                IXSerializationContract lSerializationContract = pSerializationContext.SelectContract(null, lPropertyInfo);
                if (lSerializationContract != null)
                {
                    lSerializationContract.Write(lPropertyInfo, pParentElement, pSerializationContext);
                }
            }

            return pParentElement;
        }

        #endregion // Methods.
    }
}

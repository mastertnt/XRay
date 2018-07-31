using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using XSerialization.Attributes;

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
                PropertyInfo[] lNonPublicPropertyInfos = pObjectToInitialize.GetType().GetProperties(BindingFlags.Instance | BindingFlags.NonPublic);
                IEnumerable<PropertyInfo> lFilteredPropertyInfos = lPublicPropertyInfos.Union(lNonPublicPropertyInfos.Where(pElt => pElt.GetCustomAttributes(typeof(IXSerializationAttribute), true).Any()));
                
                // The order is given by the XML, not by the properties.
                foreach (var lElement in pElement.Elements())
                {
                    PropertyInfo lPropertyInfo = lFilteredPropertyInfos.FirstOrDefault(pProp => pProp.Name == lElement.Name);
                    if (lPropertyInfo != null)
                    {
                        IXSerializationContract lSerializationContract = pSerializationContext.SelectContract(null, lPropertyInfo);
                        if (lSerializationContract != null)
                        {
                            lSerializationContract.Read(lPropertyInfo, pElement, pSerializationContext);
                        } 
                    }
                    else
                    {
                        IXSerializationContract lSerializationContract = pSerializationContext.SelectContract(lElement, null);
                        if (lSerializationContract != null)
                        {
                            lSerializationContract.Read(null, lElement, pSerializationContext);
                        } 
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
            PropertyInfo[] lFilteredPropertyInfos = lPublicPropertyInfos.Union(lNonPublicPropertyInfos.Where(pElt => pElt.GetCustomAttributes(typeof(IXSerializationAttribute), true).Any())).ToArray();
            PropertyInfo[] lSortedFilteredPropertyInfos = lFilteredPropertyInfos.Select(pX => new { Property = pX, Attribute = (OrderXSerializationAttribute)Attribute.GetCustomAttribute(pX, typeof(OrderXSerializationAttribute), true) }).OrderBy(pX => pX.Attribute != null ? pX.Attribute.Order : Int32.MaxValue).ThenBy(pX => pX.Property.Name).Select(pX => pX.Property).ToArray();
            pParentElement.SetAttributeValue(XConstants.ID_ATTRIBUTE, pSerializationContext.GetObjectReference(pObject));
            foreach (PropertyInfo lPropertyInfo in lSortedFilteredPropertyInfos)
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

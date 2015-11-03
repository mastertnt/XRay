using System;
using System.Collections;
using System.Xml.Linq;

namespace XSerialization.Collections
{
    /// <summary>
    /// This class defines a serialization contract for object list.
    /// </summary>
    public class DictionarySerializationContract : ATypeSerializationContract<IDictionary>
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
        /// Determines whether this instance can manage the specified object.
        /// </summary>
        /// <param name="pObject">The object.</param>
        /// <param name="pSerializationContext">The serialization context.</param>
        /// <returns></returns>
        public override SupportPriority CanManage(object pObject, IXSerializationContext pSerializationContext)
        {
            if (this.SupportedType.IsInstanceOfType(pObject))
            {
                return new SupportPriority(SupportLevel.Type, 0);
            }
            return SupportPriority.CANNOT_SUPPORT;
        }

        /// <summary>
        /// This method reads the specified element.
        /// </summary>
        /// <param name="pObjectToInitialize">The object to initialize</param>
        /// <param name="pParentElement">The element.</param>
        /// <param name="pSerializationContext">The serialization context.</param>
        /// <returns>The initialized object if the input object is valid.</returns>
        public override object Read(object pObjectToInitialize, XElement pParentElement, IXSerializationContext pSerializationContext)
        {
            if (pObjectToInitialize != null)
            {
                IDictionary lDictionary = pObjectToInitialize as IDictionary;
                if (lDictionary != null)
                {
                    foreach (XElement lChild in pParentElement.Elements(XConstants.ITEM_TAG))
                    {
                        IXSerializationContract lKeyContract = pSerializationContext.SelectContract(lChild.Element(XConstants.KEY_TAG), null, lDictionary.GetType().GetGenericArguments()[0], null);
                        object lKeyObject = null;
                        lKeyObject = lKeyContract.Read(lKeyObject, lChild.Element(XConstants.KEY_TAG), pSerializationContext);

                        Type lValueType = lDictionary.GetType().GetGenericArguments()[1];
                        XElement lValueTypeElement = lChild.Element(XConstants.TYPE_TAG);
                        if (lValueTypeElement != null)
                        {
                            lValueType = pSerializationContext.ResolveType(lValueTypeElement);
                        }

                        IXSerializationContract lValueContract = pSerializationContext.SelectContract(lChild.Element(XConstants.VALUE_TAG), null, lValueType, null);
                        object lValueObject = null;
                        lValueObject = lValueContract.Read(lValueObject, lChild.Element(XConstants.VALUE_TAG), pSerializationContext);
                        lDictionary.Add(lKeyObject, lValueObject);
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
        public override XElement Write(object pObject, XElement pParentElement, IXSerializationContext pSerializationContext)
        {
            // Store the type.
            pParentElement.Add(pSerializationContext.ReferenceType(pObject.GetType()));

            // Store all items.
            IDictionary lDictionary = pObject as IDictionary;
            if (lDictionary != null)
            {
                foreach (var lKey in lDictionary.Keys)
                {
                    XElement lEntryElement = new XElement(XConstants.ITEM_TAG);
                    IXSerializationContract lKeyContract = pSerializationContext.SelectContract(lEntryElement, lKey);
                    if (lKeyContract != null)
                    {
                        XElement lKeyElement = new XElement(XConstants.KEY_TAG);
                        lKeyContract.Write(lKey, lKeyElement, pSerializationContext);
                        lEntryElement.Add(lKeyElement);
                    }

                    object lValue = lDictionary[lKey];
                    IXSerializationContract lValueContract = pSerializationContext.SelectContract(lEntryElement, lValue);
                    if (lValueContract != null)
                    {
                        // Add the type of the value.
                        lEntryElement.Add(pSerializationContext.ReferenceType(lValue.GetType()));
                        XElement lValuelement = new XElement(XConstants.VALUE_TAG);
                        lValueContract.Write(lValue, lValuelement, pSerializationContext);
                        lEntryElement.Add(lValuelement);                        
                    }

                    
                    pParentElement.Add(lEntryElement);
                }
            }
            
            return pParentElement;
        }
    }
}

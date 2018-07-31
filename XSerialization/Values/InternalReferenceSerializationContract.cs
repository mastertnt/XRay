using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace XSerialization.Values
{
    /// <summary>
    /// This class implements a contract to read/write internal references.
    /// </summary>
    public class InternalReferenceSerializationContract : IXSerializationContract
    {
        /// <summary>
        /// Flag to know if an external creation is necessary.
        /// </summary>
        public bool NeedCreate
        {
            get
            {
                return true;
            }
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
            // Look for : <ParentElement ref="00001"><ParentElement>
            if (pParentElement.Attribute(XConstants.REFERENCE_ATTRIBUTE) != null && pParentElement.Name != XConstants.TYPE_TAG)
            {
                return new SupportPriority(SupportLevel.Element, 0);
            }
            return SupportPriority.CANNOT_SUPPORT;
        }

        /// <summary>
        /// This method creates the specified element.
        /// </summary>
        /// <param name="pParentElement">The parent element.</param>
        /// <param name="pSerializationContext">The serialization context.</param>
        /// <returns>The created object.</returns>
        public virtual object Create(XElement pParentElement, IXSerializationContext pSerializationContext)
        {
            return null;
        }

        /// <summary>
        /// This method reads the specified element.
        /// </summary>
        /// <param name="pObjectToInitialize">The object to initialize</param>
        /// <param name="pParentElement">The element.</param>
        /// <param name="pSerializationContext">The serialization context.</param>
        /// <returns>The initialized object if the input object is valid.</returns>
        public virtual object Read(object pObjectToInitialize, XElement pParentElement, IXSerializationContext pSerializationContext)
        {
            object lResult = null;
            XAttribute lAttribute = pParentElement.Attribute(XConstants.REFERENCE_ATTRIBUTE);
            if (lAttribute != null)
            {
                try
                {
                    int lReference = Convert.ToInt32(lAttribute.Value.Trim(), CultureInfo.InvariantCulture);
                    lResult = pSerializationContext.GetObjectByReference(lReference);
                    if (lResult == null) // If ref not found in cache yet, attempt to retrieve it ourselves
                    {
                        lResult = this.AttemptReferenceRetrieval(pObjectToInitialize, pParentElement, lReference, pSerializationContext);
                    }
                }
                catch (FormatException)
                {
                    IXmlLineInfo lInfo = pParentElement;
                    pSerializationContext.PushError(new XSerializationError(XErrorType.Parsing, lInfo.LineNumber, lInfo.LinePosition, pSerializationContext.CurrentFile, string.Empty));
                }
                catch (OverflowException)
                {
                    IXmlLineInfo lInfo = pParentElement;
                    pSerializationContext.PushError(new XSerializationError(XErrorType.NumberOverflow, lInfo.LineNumber, lInfo.LinePosition, pSerializationContext.CurrentFile, string.Empty));
                }
            }

            return lResult;
        }

        /// <summary>
        /// Attempt to retrieve the missing reference (last chance)
        /// </summary>
        /// <param name="pObjectToInitialize">The object to initialize</param>
        /// <param name="pParentElement"></param>
        /// <param name="pReference">The reference to look for.</param>
        /// <param name="pSerializationContext">The serialization context.</param>
        /// <returns>The extracted object, null otherwise.</returns>
        private object AttemptReferenceRetrieval(object pObjectToInitialize, XElement pParentElement, int pReference, IXSerializationContext pSerializationContext)
        {
            object lResult = null;
            XElement lXRoot = this.GetRoot( pParentElement );
            if ( lXRoot != null )
            {
                IEnumerable<XElement> lXElements = lXRoot.Descendants( pParentElement.Name );
                if ( lXElements != null &&
                     lXElements.Any() )
                {
                    foreach ( XElement lXElement in lXElements )
                    {
                        XAttribute lXId = lXElement.Attribute( XConstants.ID_ATTRIBUTE );
                        if ( lXId != null )
                        {
                            int lId = Convert.ToInt32(lXId.Value.Trim(), CultureInfo.InvariantCulture);
                            if ( lId == pReference )
                            {
                                XElement lXType = lXElement.Element( XConstants.TYPE_TAG );
                                if ( lXType != null )
                                {
                                    Type lType = pSerializationContext.ResolveType( lXType );
                                    IXSerializationContract lContract = pSerializationContext.SelectContract( lXElement, null, lType, pObjectToInitialize );
                                    if ( lContract != null )
                                    {
                                        if ( lContract.NeedCreate )
                                        {
                                            pObjectToInitialize = lContract.Create( lXElement, pSerializationContext );
                                        }

                                        lResult = lContract.Read( pObjectToInitialize, lXElement, pSerializationContext );
                                        if ( lResult != null )
                                        {
                                            pSerializationContext.PushObject( lResult, pReference ); // Cache it into ref by obj caches.
                                            pSerializationContext.PopObject(); // But remove it from current object stack then to come back to previous current objects stack state.
                                        }
                                    }
                                }
                                break;
                            }
                        }
                    }
                }
            }

            return lResult;
        }

        /// <summary>
        /// Retrieves the given XElement root XElement
        /// </summary>
        /// <param name="pElement"></param>
        /// <returns>The root element.</returns>
        private XElement GetRoot(XElement pElement)
        {
            if ( pElement == null )
            {
                return null;
            }

            XElement lRoot = null;
            XElement lCurrent = pElement;
            while ( lCurrent != null )
            {
                lRoot = lCurrent;
                lCurrent = lCurrent.Parent;
            }

            return lRoot;
        }

        /// <summary>
        /// This method writes the specified object.
        /// </summary>
        /// <param name="pObject">The object to serialize.</param>
        /// <param name="pParentElement">The parent element.</param>
        /// <param name="pSerializationContext">The serialization context.</param>
        /// <returns>The modified parent element</returns>
        public virtual XElement Write(object pObject, XElement pParentElement, IXSerializationContext pSerializationContext)
        {
            pParentElement.SetAttributeValue(XConstants.REFERENCE_ATTRIBUTE, pSerializationContext.GetObjectReference(pObject));
            return pParentElement;
        }
    }
}

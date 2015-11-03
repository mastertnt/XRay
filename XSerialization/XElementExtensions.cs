using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using XSystem;

namespace XSerialization
{
    /// <summary>
    /// This class stores extensions methods for System.Xml.Linq.XElement
    /// </summary>
    public static  class XElementExtensions
    {

        /// <summary>
        /// Converts an XElement (Type) to a System.Type.
        /// </summary>
        /// <param name="pElement">The element to convert.</param>
        /// <returns>The retrieved type.</returns>
        public static Type ToType(this XElement pElement)
        {
            try
            {
                if (pElement.Name.LocalName == XConstants.QUALIFIED_TYPE_TAG)
                {
                    return AppDomain.CurrentDomain.GetTypeByAssemblyQualifiedName(pElement.Value);
                }

                XElement lRootElement;
                XElement lParentElement;
                if (pElement.Name.LocalName == XConstants.TYPE_TAG)
                {
                    lRootElement = pElement.Descendants().FirstOrDefault();
                    lParentElement = pElement;
                }
                else
                {
                    lRootElement = pElement;
                    lParentElement = pElement.Parent;
                }
                
                Type lComputedType = null; 
// ReSharper disable once PossibleNullReferenceException
                XElement lGenericTypeElement = lParentElement.Element(XConstants.GENERIC_DEFINITION_TAG);
                if (lGenericTypeElement != null)
                {
                    List<Type> lArgTypes = new List<Type>();
                    string lRebuildType = lGenericTypeElement.Attribute(XConstants.TYPE_ATTRIBUTE).Value + "`" + lGenericTypeElement.Elements().Count();
// ReSharper disable once LoopCanBeConvertedToQuery
                    foreach (XElement lGenericArgElement in lGenericTypeElement.Elements())
                    {
                        lArgTypes.Add(lGenericArgElement.ToType());
                    }
                    Type lGeneric = null;
                    if (lRebuildType == "Nullable`1")
                    {
                        lGeneric = typeof (Nullable<>);
                    }
                    else
                    {
                        lGeneric = AppDomain.CurrentDomain.GetTypeByFullName(lRebuildType);
                    }
                    if (lGeneric != null)
                    {
                        lComputedType = lGeneric.MakeGenericType(lArgTypes.ToArray());
                    }
                }
                else if (lRootElement != null)
                {
// ReSharper disable once PossibleNullReferenceException
                    lComputedType = AppDomain.CurrentDomain.GetTypeByFullName(lRootElement.Name.LocalName);
                }
                return lComputedType;
            }
// ReSharper disable once EmptyGeneralCatchClause
            catch
            {
                
            }
            return null;
        }
    }
}

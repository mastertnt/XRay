using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace XSerialization
{
    /// <summary>
    /// This class stores extensions methods for System.Type
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// This method cleans the full name for XML serialization.
        /// </summary>
        /// <param name="pType">The type to clean.</param>
        /// <returns>the xml full name.</returns>
        public static string XmlFullname(this Type pType)
        {
            if (pType.FullName.Contains("`"))
            {
                string[] lFullname = pType.Name.Split(new char[] {'`'}, StringSplitOptions.RemoveEmptyEntries);
                string lResult = pType.Namespace + "." + lFullname[0];
                return lResult;
            }
            return pType.FullName;
        }

        /// <summary>
        /// To the fullname with assembly.
        /// </summary>
        /// <param name="pType">Type of the p.</param>
        /// <returns></returns>
        internal static string ToFullnameWithAssembly(this Type pType)
        {
            return pType.FullName + XConstants.ASSEMBLY_SEPARATOR + pType.Assembly.GetName().Name;
        }

        /// <summary>
        /// Build a tree of XElement to describe a type.
        /// </summary>
        /// <param name="pType"></param>
        /// <returns></returns>
        internal static XElement ToElement(this Type pType)
        {
            XElement lTypeElement = new XElement(XConstants.QUALIFIED_TYPE_TAG);
            lTypeElement.SetValue(pType.AssemblyQualifiedName);
            return lTypeElement;
        }

        /// <summary>
        /// Removes the version part of a type full name.
        /// </summary>
        /// <param name="pTypeFullName">The type full name to parse.</param>
        /// <returns>The type full name without any version information.</returns>
        public static string RemoveVersionFromFullName(string pTypeFullName)
        {
            // Removing all the parts containing the version informations.
            List<string> lParts = new List<string>(pTypeFullName.Split(new char[] { ',' }).Where(pPart => pPart.Contains(" Version=") == false));

            // Builing the new type full name.
            if (lParts.Any())
            {
                StringBuilder lTypeBuilder = new StringBuilder(lParts[0]);
                lParts.RemoveAt(0);
                foreach(string lPart in lParts)
                {
                    lTypeBuilder.Append(',');
                    lTypeBuilder.Append(lPart);
                }

                return lTypeBuilder.ToString();
            }

            return string.Empty;
        }
    }
}

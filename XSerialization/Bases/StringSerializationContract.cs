using System;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

namespace XSerialization.Bases
{
    /// <summary>
    /// This class defines a serialization contract for string.
    /// </summary>
    public class StringSerializationContract : ATypeSerializationContract<String>
    {

        /// <summary>
        /// Gets a value indicating whether [need create].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [need create]; otherwise, <c>false</c>.
        /// </value>
        public override bool NeedCreate
        {
            get { return true; }
        }

        /// <summary>
        /// Creates the specified element.
        /// </summary>
        /// <param name="pElement">The element.</param>
        /// <param name="pSerializationContext">The serialization context.</param>
        /// <returns></returns>
        public override object Create(XElement pElement, IXSerializationContext pSerializationContext)
        {
            return string.Empty;
        }

        /// <summary>
        /// This method reads the specified element.
        /// </summary>
        /// <param name="pObjectToInitialize">The object to initialize</param>
        /// <param name="pElement">The element.</param>
        /// <param name="pSerializationContext">The serialization context.</param>
        /// <returns>The initialized object if the input object is valid.</returns>
        public override object Read(object pObjectToInitialize, XElement pElement, IXSerializationContext pSerializationContext)
        {
            string lValue = (string)(pObjectToInitialize);
            XElement lSubElement = pElement.Descendants().FirstOrDefault();
            if (lSubElement == null)
            {
                lValue = pElement.Value;
            }
            else
            {
                if (lSubElement.Name.LocalName == XConstants.NULL_TAG)
                {
                    lValue = null;
                }
                else
                {
                    lValue = lSubElement.Value;
                }
            }
            
            return lValue;
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
            pParentElement.Value = Convert.ToString(pObject, CultureInfo.InvariantCulture);
            return pParentElement;
        }
    }
}

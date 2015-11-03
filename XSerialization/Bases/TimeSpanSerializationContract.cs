using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Globalization;

namespace XSerialization.Bases
{
    /// <summary>
    /// This class defines a serialization contract for TimeSpan.
    /// </summary>
    public class TimeSpanSerializationContract : ATypeSerializationContract<TimeSpan>
    {
        /// <summary>
        /// Creates the specified element.
        /// </summary>
        /// <param name="pElement">The element.</param>
        /// <param name="pSerializationContext">The serialization context.</param>
        /// <returns></returns>
        public override object Create(XElement pElement, IXSerializationContext pSerializationContext)
        {
            return new TimeSpan();
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
            if (pObjectToInitialize == null) return null;
            TimeSpan lValue = (TimeSpan)(pObjectToInitialize);
            try
            {
                lValue = TimeSpan.Parse(pElement.Value.Trim());
            }
            catch (FormatException)
            {
                Console.WriteLine("Input string is not a sequence of digits.");
            }
            catch (OverflowException)
            {
                Console.WriteLine("The number cannot fit in {0}.", this.SupportedType.Name);
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

using System;
using System.Globalization;
using System.Xml;
using System.Xml.Linq;

namespace XSerialization.Primitives
{
    /// <summary>
    /// This class defines a serialization contract for UInt16.
    /// </summary>
    public class UInt16SerializationContract : PrimitiveTypeSerializationContract<UInt16>
    {
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
            if (pElement == null) return null;
            UInt16 lValue = (UInt16)(pObjectToInitialize);
            try
            {
                lValue = Convert.ToUInt16(pElement.Value.Trim(), CultureInfo.InvariantCulture);
            }
            catch (FormatException)
            {
                IXmlLineInfo lInfo = pElement;
                pSerializationContext.PushError(new XSerializationError(XErrorType.Parsing, lInfo.LineNumber, lInfo.LinePosition, pSerializationContext.CurrentFile, string.Empty));
            }
            catch (OverflowException)
            {
                IXmlLineInfo lInfo = pElement;
                pSerializationContext.PushError(new XSerializationError(XErrorType.NumberOverflow, lInfo.LineNumber, lInfo.LinePosition, pSerializationContext.CurrentFile, string.Empty));
            }
            return lValue;
        }
    }
}

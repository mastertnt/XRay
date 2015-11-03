using System;
using System.Xml.Linq;

namespace XSerialization
{
    /// <summary>
    /// An interface for all serialization contract based on XElement.
    /// </summary>
    public interface IElementXSerializationContract : IXTypedSerializationContract<XElement>
    {
        /// <summary>
        /// Gets the element name.
        /// </summary>
        string ElementName
        {
            get;
        }
    }
}

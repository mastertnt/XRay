using System;
using XSerialization.Attributes;

namespace XSerialization
{
    /// <summary>
    /// This interface defines serialization contract based on attribute.
    /// </summary>
    public interface IAttributeXSerializationContract : IXTypedSerializationContract<Type>
    {
        /// <summary>
        /// Gets or sets the attribute.
        /// </summary>
        /// <value>
        /// The attribute.
        /// </value>
        XSerializationAttribute Attribute
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the supported attribute
        /// </summary>
        Type SupportedAttribute { get; }
    }
}

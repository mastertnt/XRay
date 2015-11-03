using System;

namespace XSerialization
{
    /// <summary>
    /// This interface defines serialization contract based on attribute.
    /// </summary>
    public interface IAttributeXSerializationContract : IXTypedSerializationContract<Type>
    {
        /// <summary>
        /// Gets the supported attribute
        /// </summary>
        Type SupportedAttribute { get; }
    }
}

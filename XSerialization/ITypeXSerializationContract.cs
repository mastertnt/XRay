using System;

namespace XSerialization
{
    /// <summary>
    /// An interface for all serialization contract based on type.
    /// </summary>
    public interface ITypeXSerializationContract : IXTypedSerializationContract<Type>
    {
        /// <summary>
        /// Gets the supported type.
        /// </summary>
        Type SupportedType { get; }
    }
}

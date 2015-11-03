using System;
using System.Reflection;

namespace XSerialization
{
    /// <summary>
    /// This interface defines a serialization contract for a property info.
    /// </summary>
    interface IPropertyInfoSerializationContract : IXTypedSerializationContract<PropertyInfo>
    {
        /// <summary>
        /// Gets the supported declaring type.
        /// </summary>
        Type DeclaringType
        {
            get;
        }

        /// <summary>
        /// Gets the supported property type.
        /// </summary>
        Type PropertyType
        {
            get;
        }

        /// <summary>
        /// Gets the supported property name.
        /// </summary>
        string PropertyName
        {
            get;
        }
    }
}

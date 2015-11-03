using System;

namespace XSerialization
{
    /// <summary>
    /// Base attribute for serialization.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class XSerializationAttribute : Attribute, IXSerializationAttribute
    {
        #region Properties

        /// <summary>
        /// Gets the contract type to use.
        /// </summary>
        public Type SupportedContract
        {
            get;
            private set;
        }

        #endregion // Properties.

        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        protected XSerializationAttribute(Type pContractType)
        {
            this.SupportedContract = pContractType;
        }

        #endregion // Constructors.
    }

    /// <summary>
    /// Base attribute for serialization.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class XInternalSerializationAttribute : Attribute, IXSerializationAttribute
    {
        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        protected XInternalSerializationAttribute()
        {
        }

        #endregion // Constructors.
    }
}

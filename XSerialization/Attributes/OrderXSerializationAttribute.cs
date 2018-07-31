using System;

namespace XSerialization.Attributes
{
    /// <summary>
    /// This attribute can be used to skip the serialization on a property.
    /// </summary>
    public sealed class OrderXSerializationAttribute : Attribute
    {
        #region Properties

        /// <summary>
        /// Gets the order.
        /// </summary>
        /// <value>
        /// The order.
        /// </value>
        public int Order
        {
            get;
            private set;
        }

        #endregion // Properties.

        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public OrderXSerializationAttribute(int pOrder)
        {
            this.Order = pOrder;
        }

        #endregion // Constructors.
    }
}

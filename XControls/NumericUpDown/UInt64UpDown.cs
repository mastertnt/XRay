using System;
using System.Windows;

namespace XControls
{
    /// <summary>
    /// Class defining an UInt32 editor.
    /// </summary>
    public class UInt64UpDown : ANativeNumericUpDown<UInt64>
    {
        #region Constructors

        /// <summary>
        /// Initializes the <see cref="UInt64UpDown"/> class.
        /// </summary>
        static UInt64UpDown()
        {
            UInt64UpDown.UpdateMetadata(typeof(UInt64UpDown), 1, UInt64.MinValue, UInt64.MaxValue);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UInt64UpDown"/> class.
        /// </summary>
        public UInt64UpDown()
            : base(UInt64.Parse, Decimal.ToUInt64, (v1, v2) => v1 < v2, (v1, v2) => v1 > v2)
        {
        }

        #endregion // Constructors.

        #region Methods

        /// <summary>
        /// Increments the value.
        /// </summary>
        /// <param name="pValue">The value to increment.</param>
        /// <param name="pIncrement">The increment step.</param>
        /// <returns>The incremented value.</returns>
        protected override UInt64 CustomIncrementValue(UInt64 pValue, UInt64 pIncrement)
        {
            return (UInt64)(pValue + pIncrement);
        }

        /// <summary>
        /// Decrements the value.
        /// </summary>
        /// <param name="pValue">The value to decrement.</param>
        /// <param name="pIncrement">The decrement step.</param>
        /// <returns>The decremented value.</returns>
        protected override UInt64 CustomDecrementValue(UInt64 pValue, UInt64 pIncrement)
        {
            if (pValue == 0)
            {
                return pValue;
            }

            return (UInt64)(pValue - pIncrement);
        }

        #endregion // Methods.
    }
}

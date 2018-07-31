using System;
using System.Windows;

namespace XControls
{
    /// <summary>
    /// Class defining an UInt32 editor.
    /// </summary>
    public class UInt32UpDown : ANativeNumericUpDown<UInt32>
    {
        #region Constructors

        /// <summary>
        /// Initializes the <see cref="UInt32UpDown"/> class.
        /// </summary>
        static UInt32UpDown()
        {
            UpdateMetadata(typeof(UInt32UpDown), 1, UInt32.MinValue, UInt32.MaxValue);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UInt32UpDown"/> class.
        /// </summary>
        public UInt32UpDown()
            : base(UInt32.Parse, Decimal.ToUInt32, (v1, v2) => v1 < v2, (v1, v2) => v1 > v2)
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
        protected override UInt32 CustomIncrementValue(UInt32 pValue, UInt32 pIncrement)
        {
            return (UInt32)(pValue + pIncrement);
        }

        /// <summary>
        /// Decrements the value.
        /// </summary>
        /// <param name="pValue">The value to decrement.</param>
        /// <param name="pIncrement">The decrement step.</param>
        /// <returns>The decremented value.</returns>
        protected override UInt32 CustomDecrementValue(UInt32 pValue, UInt32 pIncrement)
        {
            if (pValue == 0)
            {
                return pValue;
            }

            return (UInt32)(pValue - pIncrement);
        }

        #endregion // Methods.
    }
}

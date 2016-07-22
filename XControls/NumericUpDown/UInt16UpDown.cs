using System;
using System.Windows;

namespace XControls
{
    /// <summary>
    /// Class defining an UInt16 editor.
    /// </summary>
    public class UInt16UpDown : ANativeNumericUpDown<UInt16>
    {
        #region Constructors

        /// <summary>
        /// Initializes the <see cref="UInt16UpDown"/> class.
        /// </summary>
        static UInt16UpDown()
        {
            UInt16UpDown.UpdateMetadata(typeof(UInt16UpDown), 1, UInt16.MinValue, UInt16.MaxValue);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UInt16UpDown"/> class.
        /// </summary>
        public UInt16UpDown()
            : base(UInt16.Parse, Decimal.ToUInt16, (v1, v2) => v1 < v2, (v1, v2) => v1 > v2)
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
        protected override UInt16 CustomIncrementValue(UInt16 pValue, UInt16 pIncrement)
        {
            return (UInt16)(pValue + pIncrement);
        }

        /// <summary>
        /// Decrements the value.
        /// </summary>
        /// <param name="pValue">The value to decrement.</param>
        /// <param name="pIncrement">The decrement step.</param>
        /// <returns>The decremented value.</returns>
        protected override UInt16 CustomDecrementValue(UInt16 pValue, UInt16 pIncrement)
        {
            if (pValue == 0)
            {
                return pValue;
            }

            return (UInt16)(pValue - pIncrement);
        }

        #endregion // Methods.
    }
}

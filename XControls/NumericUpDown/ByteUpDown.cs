using System;
using System.Windows;

namespace XControls
{
    /// <summary>
    /// Class defining a byte editor.
    /// </summary>
    public class ByteUpDown : ANativeNumericUpDown<byte>
    {
        #region Constructors

        /// <summary>
        /// Initializes the <see cref="ByteUpDown"/> class.
        /// </summary>
        static ByteUpDown()
        {
            ByteUpDown.UpdateMetadata(typeof(ByteUpDown), (byte)1, byte.MinValue, byte.MaxValue);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ByteUpDown"/> class.
        /// </summary>
        public ByteUpDown()
            : base(Byte.Parse, Decimal.ToByte, (v1, v2) => v1 < v2, (v1, v2) => v1 > v2)
        {
        }

        #endregion //Constructors

        #region Methods

        /// <summary>
        /// Increments the value.
        /// </summary>
        /// <param name="pValue">The value to increment.</param>
        /// <param name="pIncrement">The increment step.</param>
        /// <returns>The incremented value.</returns>
        protected override byte CustomIncrementValue(byte pValue, byte pIncrement)
        {
            return (byte)(pValue + pIncrement);
        }

        /// <summary>
        /// Decrements the value.
        /// </summary>
        /// <param name="pValue">The value to decrement.</param>
        /// <param name="pIncrement">The decrement step.</param>
        /// <returns>The decremented value.</returns>
        protected override byte CustomDecrementValue(byte pValue, byte pIncrement)
        {
            return (byte)(pValue - pIncrement);
        }

        #endregion //Base Class Overrides
    }
}

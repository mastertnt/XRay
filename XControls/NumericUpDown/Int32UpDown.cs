using System;
using System.Windows;

namespace XControls
{
    /// <summary>
    /// Class defining an Int32 editor.
    /// </summary>
    public class Int32UpDown : ANativeNumericUpDown<Int32>
    {
        #region Constructors

        /// <summary>
        /// Initializes the <see cref="Int32UpDown"/> class.
        /// </summary>
        static Int32UpDown()
        {
            Int32UpDown.UpdateMetadata(typeof(Int32UpDown), 1, Int32.MinValue, Int32.MaxValue);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Int32UpDown"/> class.
        /// </summary>
        public Int32UpDown()
            : base(Int32.Parse, Decimal.ToInt32, (v1, v2) => v1 < v2, (v1, v2) => v1 > v2)
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
        protected override int CustomIncrementValue(Int32 pValue, Int32 pIncrement)
        {
            return pValue + pIncrement;
        }

        /// <summary>
        /// Decrements the value.
        /// </summary>
        /// <param name="pValue">The value to decrement.</param>
        /// <param name="pIncrement">The decrement step.</param>
        /// <returns>The decremented value.</returns>
        protected override int CustomDecrementValue(Int32 pValue, Int32 pIncrement)
        {
            return pValue - pIncrement;
        }

        #endregion // Methods.
    }
}

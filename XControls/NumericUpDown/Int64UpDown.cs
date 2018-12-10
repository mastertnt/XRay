using System;

namespace XControls.NumericUpDown
{
    /// <summary>
    /// Class defining an Int64 editor.
    /// </summary>
    public class Int64UpDown : ANativeNumericUpDown<Int64>
    {
        #region Constructors

        /// <summary>
        /// Initializes the <see cref="Int64UpDown"/> class.
        /// </summary>
        static Int64UpDown()
        {
            UpdateMetadata(typeof(Int64UpDown), 1, Int64.MinValue, Int64.MaxValue);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Int64UpDown"/> class.
        /// </summary>
        public Int64UpDown()
            : base(Int64.Parse, Decimal.ToInt64, (v1, v2) => v1 < v2, (v1, v2) => v1 > v2)
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
        protected override Int64 CustomIncrementValue(Int64 pValue, Int64 pIncrement)
        {
            return pValue + pIncrement;
        }

        /// <summary>
        /// Decrements the value.
        /// </summary>
        /// <param name="pValue">The value to decrement.</param>
        /// <param name="pIncrement">The decrement step.</param>
        /// <returns>The decremented value.</returns>
        protected override Int64 CustomDecrementValue(Int64 pValue, Int64 pIncrement)
        {
            return pValue - pIncrement;
        }

        #endregion // Methods.
    }
}

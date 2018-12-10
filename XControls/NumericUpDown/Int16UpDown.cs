using System;

namespace XControls.NumericUpDown
{
    /// <summary>
    /// Class defining an Int16 editor.
    /// </summary>
    public class Int16UpDown : ANativeNumericUpDown<Int16>
    {
        #region Constructors

        /// <summary>
        /// Initializes the <see cref="Int16UpDown"/> class.
        /// </summary>
        static Int16UpDown()
        {
            UpdateMetadata(typeof(Int16UpDown), 1, Int16.MinValue, Int16.MaxValue);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Int16UpDown"/> class.
        /// </summary>
        public Int16UpDown()
            : base(Int16.Parse, Decimal.ToInt16, (v1, v2) => v1 < v2, (v1, v2) => v1 > v2)
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
        protected override Int16 CustomIncrementValue(Int16 pValue, Int16 pIncrement)
        {
            return (Int16)(pValue + pIncrement);
        }

        /// <summary>
        /// Decrements the value.
        /// </summary>
        /// <param name="pValue">The value to decrement.</param>
        /// <param name="pIncrement">The decrement step.</param>
        /// <returns>The decremented value.</returns>
        protected override Int16 CustomDecrementValue(Int16 pValue, Int16 pIncrement)
        {
            return (Int16)(pValue - pIncrement);
        }

        #endregion // Methods.
    }
}

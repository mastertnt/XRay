using System;

namespace XControls.NumericUpDown
{
    /// <summary>
    /// Class defining a decimal editor.
    /// </summary>
    public class DecimalUpDown : ANativeNumericUpDown<decimal>
    {
        #region Constructors

        /// <summary>
        /// Initializes the <see cref="DecimalUpDown"/> class.
        /// </summary>
        static DecimalUpDown()
        {
            UpdateMetadata(typeof(DecimalUpDown), 1m, decimal.MinValue, decimal.MaxValue);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DecimalUpDown"/> class.
        /// </summary>
        public DecimalUpDown()
            : base(Decimal.Parse, (d) => d, (v1, v2) => v1 < v2, (v1, v2) => v1 > v2)
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
        protected override decimal CustomIncrementValue(decimal pValue, decimal pIncrement)
        {
            return pValue + pIncrement;
        }

        /// <summary>
        /// Decrements the value.
        /// </summary>
        /// <param name="pValue">The value to decrement.</param>
        /// <param name="pIncrement">The decrement step.</param>
        /// <returns>The decremented value.</returns>
        protected override decimal CustomDecrementValue(decimal pValue, decimal pIncrement)
        {
            return pValue - pIncrement;
        }

        #endregion // Methods.
    }
}

using System;
using System.Windows;
using System.Globalization;
using System.IO;

namespace XControls
{
    /// <summary>
    /// Class defining a double editor.
    /// </summary>
    public class DoubleUpDown : ANativeNumericUpDown<double>
    {
        #region Dependencies

        /// <summary>
        /// Identifies the AllowInputSpecialValues property.
        /// </summary>
        public static readonly DependencyProperty AllowInputSpecialValuesProperty = DependencyProperty.Register("AllowInputSpecialValues", typeof(AllowedSpecialValues), typeof(DoubleUpDown), new UIPropertyMetadata(AllowedSpecialValues.None));
        
        #endregion // Dependencies.

        #region Constructors

        /// <summary>
        /// Initializes the <see cref="DoubleUpDown"/> class.
        /// </summary>
        static DoubleUpDown()
        {
            UpdateMetadata(typeof(DoubleUpDown), 1d, double.NegativeInfinity, double.PositiveInfinity);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DoubleUpDown"/> class.
        /// </summary>
        public DoubleUpDown()
            : base(Double.Parse, Decimal.ToDouble, (v1, v2) => v1 < v2, (v1, v2) => v1 > v2)
        {
        }

        #endregion // Constructors.

        #region Properties

        /// <summary>
        /// Gets or sets the flags defining the allowed special values.
        /// </summary>
        public AllowedSpecialValues AllowInputSpecialValues
        {
            get 
            { 
                return (AllowedSpecialValues) this.GetValue(AllowInputSpecialValuesProperty); 
            }
            set 
            {
                this.SetValue(AllowInputSpecialValuesProperty, value); 
            }
        }

        #endregion // Properties.

        #region Methods

        /// <summary>
        /// Coerce the value.
        /// </summary>
        /// <param name="pBaseValue">The value to coerce.</param>
        /// <returns>The coerced value.</returns>
        protected override double? OnCoerceValue(double? pBaseValue)
        {
            return this.CoerceSpecialValue(pBaseValue);
        }

        /// <summary>
        /// Coerce the value.
        /// </summary>
        /// <param name="pBaseValue">The value to coerce.</param>
        /// <returns>The coerced value.</returns>
        protected override double? OnCoerceIncrement(double? pBaseValue)
        {
            if (pBaseValue.HasValue && double.IsNaN(pBaseValue.Value))
            {
                throw new ArgumentException("NaN is invalid for Increment.");
            }

            return base.OnCoerceIncrement(pBaseValue);
        }

        /// <summary>
        /// Coerce the value.
        /// </summary>
        /// <param name="pBaseValue">The value to coerce.</param>
        /// <returns>The coerced value.</returns>
        protected override double? OnCoerceMaximum(double? pBaseValue)
        {
            if (pBaseValue.HasValue && double.IsNaN(pBaseValue.Value))
            {
                throw new ArgumentException("NaN is invalid for Maximum.");
            }

            return base.OnCoerceMaximum(pBaseValue);
        }

        /// <summary>
        /// Coerce the value.
        /// </summary>
        /// <param name="pBaseValue">The value to coerce.</param>
        /// <returns>The coerced value.</returns>
        protected override double? OnCoerceMinimum(double? pBaseValue)
        {
            if (pBaseValue.HasValue && double.IsNaN(pBaseValue.Value))
            {
                throw new ArgumentException("NaN is invalid for Minimum.");
            }

            return base.OnCoerceMinimum(pBaseValue);
        }

        /// <summary>
        /// Increments the value.
        /// </summary>
        /// <param name="pValue">The value to increment.</param>
        /// <param name="pIncrement">The increment step.</param>
        /// <returns>The incremented value.</returns>
        protected override double CustomIncrementValue(double pValue, double pIncrement)
        {
            return pValue + pIncrement;
        }

        /// <summary>
        /// Decrements the value.
        /// </summary>
        /// <param name="pValue">The value to decrement.</param>
        /// <param name="pIncrement">The decrement step.</param>
        /// <returns>The decremented value.</returns>
        protected override double CustomDecrementValue(double pValue, double pIncrement)
        {
            return pValue - pIncrement;
        }

        /// <summary>
        /// Sets the valid spin direction depending on the value.s
        /// </summary>
        /// <param name="pValue">The value to evaluate.</param>
        protected override void SetValidSpinDirection(double? pValue)
        {
            if (pValue.HasValue && double.IsInfinity(pValue.Value) && (this.Spinner != null))
            {
                this.Spinner.ValidSpinDirections = ValidSpinDirections.None;
            }
            else
            {
                base.SetValidSpinDirection(pValue);
            }
        }

        /// <summary>
        /// Converts the text to its corresponding value.
        /// </summary>
        /// <param name="pText">The text to convert.</param>
        /// <returns>The corresponding value.</returns>
        protected override double? ConvertTextToValue(string pText)
        {
            double? lResult = base.ConvertTextToValue(pText);
            return this.CoerceSpecialValue(lResult);
        }

        /// <summary>
        /// Coerce the value taking in account the special value constraints.
        /// </summary>
        /// <param name="pValue">The value to coerce.</param>
        /// <returns>The coerced value.</returns>
        private double? CoerceSpecialValue(double? pValue)
        {
            if (pValue != null)
            {
                if (double.IsNaN(pValue.Value))
                {
                    if (this.TestInputSpecialValue(this.AllowInputSpecialValues, AllowedSpecialValues.NaN) == false)
                    {
                        return null;
                    }
                }  
                else if (double.IsPositiveInfinity(pValue.Value))
                {
                    if (this.TestInputSpecialValue(this.AllowInputSpecialValues, AllowedSpecialValues.PositiveInfinity) == false)
                    {
                        return null;
                    }
                }
                else if (double.IsNegativeInfinity(pValue.Value))
                {
                    if (this.TestInputSpecialValue(this.AllowInputSpecialValues, AllowedSpecialValues.NegativeInfinity) == false)
                    {
                        return null;
                    }
                }
            }

            return pValue;
        }

        #endregion // Methods.
    }
}

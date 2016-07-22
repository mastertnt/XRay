/*************************************************************************************

   Extended WPF Toolkit

   Copyright (C) 2007-2013 Xceed Software Inc.

   This program is provided to you under the terms of the Microsoft Public
   License (Ms-PL) as published at http://wpftoolkit.codeplex.com/license 

   For more features, controls, and fast professional support,
   pick up the Plus Edition at http://xceed.com/wpf_toolkit

   Stay informed: follow @datagrid on Twitter or Like http://facebook.com/datagrids

  ***********************************************************************************/

using System;
using System.Windows;
using System.Globalization;
using System.IO;

namespace XControls
{
    public class DoubleUpDown : ANativeNumericUpDown<double>
    {
        #region Constructors

        static DoubleUpDown()
        {
            UpdateMetadata(typeof(DoubleUpDown), 1d, double.NegativeInfinity, double.PositiveInfinity);
        }

        public DoubleUpDown()
            : base(Double.Parse, Decimal.ToDouble, (v1, v2) => v1 < v2, (v1, v2) => v1 > v2)
        {
        }

        #endregion //Constructors

        #region Properties


        #region AllowInputSpecialValues

        public static readonly DependencyProperty AllowInputSpecialValuesProperty =
            DependencyProperty.Register("AllowInputSpecialValues", typeof(AllowedSpecialValues), typeof(DoubleUpDown), new UIPropertyMetadata(AllowedSpecialValues.None));

        public AllowedSpecialValues AllowInputSpecialValues
        {
            get { return (AllowedSpecialValues)GetValue(AllowInputSpecialValuesProperty); }
            set { SetValue(AllowInputSpecialValuesProperty, value); }
        }

        #endregion //AllowInputSpecialValues

        #endregion

        #region Base Class Overrides

        protected override double? OnCoerceValue(double? pNewValue)
        {
            return this.CoerceSpecialValue(pNewValue);
        }

        protected override double? OnCoerceIncrement(double? baseValue)
        {
            if (baseValue.HasValue && double.IsNaN(baseValue.Value))
                throw new ArgumentException("NaN is invalid for Increment.");

            return base.OnCoerceIncrement(baseValue);
        }

        protected override double? OnCoerceMaximum(double? baseValue)
        {
            if (baseValue.HasValue && double.IsNaN(baseValue.Value))
                throw new ArgumentException("NaN is invalid for Maximum.");

            return base.OnCoerceMaximum(baseValue);
        }

        protected override double? OnCoerceMinimum(double? baseValue)
        {
            if (baseValue.HasValue && double.IsNaN(baseValue.Value))
                throw new ArgumentException("NaN is invalid for Minimum.");

            return base.OnCoerceMinimum(baseValue);
        }

        protected override double CustomIncrementValue(double value, double increment)
        {
            return value + increment;
        }

        protected override double CustomDecrementValue(double value, double increment)
        {
            return value - increment;
        }

        protected override void SetValidSpinDirection(double? pValue)
        {
            if (pValue.HasValue && double.IsInfinity(pValue.Value) && (Spinner != null))
            {
                Spinner.ValidSpinDirections = ValidSpinDirections.None;
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

        #endregion
    }
}

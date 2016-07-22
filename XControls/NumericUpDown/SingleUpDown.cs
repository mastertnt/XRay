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
    public class SingleUpDown : ANativeNumericUpDown<float>
    {
        #region Constructors

        static SingleUpDown()
        {
            UpdateMetadata(typeof(SingleUpDown), 1f, float.NegativeInfinity, float.PositiveInfinity);
        }

        public SingleUpDown()
            : base(Single.Parse, Decimal.ToSingle, (v1, v2) => v1 < v2, (v1, v2) => v1 > v2)
        {
        }

        #endregion //Constructors

        #region Properties

        #region AllowInputSpecialValues

        public static readonly DependencyProperty AllowInputSpecialValuesProperty =
            DependencyProperty.Register("AllowInputSpecialValues", typeof(AllowedSpecialValues), typeof(SingleUpDown), new UIPropertyMetadata(AllowedSpecialValues.None));

        public AllowedSpecialValues AllowInputSpecialValues
        {
            get { return (AllowedSpecialValues)GetValue(AllowInputSpecialValuesProperty); }
            set { SetValue(AllowInputSpecialValuesProperty, value); }
        }

        #endregion //AllowInputSpecialValues

        #endregion

        #region Base Class Overrides

        protected override float? OnCoerceValue(float? pNewValue)
        {
            return this.CoerceSpecialValue(pNewValue);
        }

        protected override float? OnCoerceIncrement(float? baseValue)
        {
            if (baseValue.HasValue && float.IsNaN(baseValue.Value))
                throw new ArgumentException("NaN is invalid for Increment.");

            return base.OnCoerceIncrement(baseValue);
        }

        protected override float? OnCoerceMaximum(float? baseValue)
        {
            if (baseValue.HasValue && float.IsNaN(baseValue.Value))
                throw new ArgumentException("NaN is invalid for Maximum.");

            return base.OnCoerceMaximum(baseValue);
        }

        protected override float? OnCoerceMinimum(float? baseValue)
        {
            if (baseValue.HasValue && float.IsNaN(baseValue.Value))
                throw new ArgumentException("NaN is invalid for Minimum.");

            return base.OnCoerceMinimum(baseValue);
        }

        protected override float CustomIncrementValue(float value, float increment)
        {
            return value + increment;
        }

        protected override float CustomDecrementValue(float value, float increment)
        {
            return value - increment;
        }

        protected override void SetValidSpinDirection(float? pValue)
        {
            if (pValue.HasValue && float.IsInfinity(pValue.Value) && (Spinner != null))
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
        protected override float? ConvertTextToValue(string pText)
        {
            float? lResult = base.ConvertTextToValue(pText);
            return this.CoerceSpecialValue(lResult);
        }

        /// <summary>
        /// Coerce the value taking in account the special value constraints.
        /// </summary>
        /// <param name="pValue">The value to coerce.</param>
        /// <returns>The coerced value.</returns>
        private float? CoerceSpecialValue(float? pValue)
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

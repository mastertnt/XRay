using System;
using System.Windows;
using System.Globalization;
using System.IO;
using XControls.Primitives;
using XControls.Core;

namespace XControls
{
    /// <summary>
    /// Class defining a native numeric editor.
    /// </summary>
    /// <typeparam name="T">The type of the edited value.</typeparam>
    public abstract class ANativeNumericUpDown<T> : ANumericUpDown<T?> where T : struct, IFormattable, IComparable<T>
    {
        #region Delegates

        /// <summary>
        /// Converts a value form the given text.
        /// </summary>
        /// <param name="pText">The text to parse.</param>
        /// <param name="pStyle">The number style.</param>
        /// <param name="pProvider">The format provider.</param>
        /// <returns>The parsed value.</returns>
        protected delegate T FromText(string pText, NumberStyles pStyle, IFormatProvider pProvider);
        
        /// <summary>
        /// Converts a value form a decimal value.
        /// </summary>
        /// <param name="pDecimal">The decimal value.</param>
        /// <returns>The converted value.</returns>
        protected delegate T FromDecimal(decimal pDecimal);

        #endregion // Delegates.

        #region Fields

        /// <summary>
        /// Stores the value converter from text.
        /// </summary>
        private FromText mFromText;

        /// <summary>
        /// Stores the value converter from decimal.
        /// </summary>
        private FromDecimal mFromDecimal;

        /// <summary>
        /// Stores the lower than comparator.
        /// </summary>
        private Func<T, T, bool> mFromLowerThan;

        /// <summary>
        /// Stores the greater than comparator.
        /// </summary>
        private Func<T, T, bool> mFromGreaterThan;

        /// <summary>
        /// Stores the last defined value. Property used when the <see cref="IsNullableValue"/> property is set to true.
        /// </summary>
        private T? mLastDefinedValue;

        #endregion // Fields.

        #region Dependencies

        /// <summary>
        /// Identifies the IsNullableValue dependency property.
        /// </summary>
        public static readonly DependencyProperty IsNullableValueProperty = DependencyProperty.Register("IsNullableValue", typeof(bool), typeof(ANativeNumericUpDown<T>), new FrameworkPropertyMetadata(false));

        /// <summary>
        /// Identifies the InfiniteValue dependency property.
        /// </summary>
        public static readonly DependencyProperty InfiniteValueProperty = DependencyProperty.Register("InfiniteValue", typeof(T?), typeof(ANativeNumericUpDown<T>), new FrameworkPropertyMetadata(null));

        /// <summary>
        /// Identifies the ParsingNumberStyle dependency property.
        /// </summary>
        public static readonly DependencyProperty ParsingNumberStyleProperty = DependencyProperty.Register("ParsingNumberStyle", typeof(NumberStyles), typeof(ANativeNumericUpDown<T>), new UIPropertyMetadata(NumberStyles.Number));

        /// <summary>
        /// Identifies the AutoReverse dependency property.
        /// </summary>
        public static readonly DependencyProperty AutoReverseProperty = DependencyProperty.Register("AutoReverse", typeof(bool), typeof(ANativeNumericUpDown<T>), new FrameworkPropertyMetadata(false));

        /// <summary>
        /// Identifies the AllowMinusZero dependency property.
        /// </summary>
        public static readonly DependencyProperty AllowMinusZeroProperty = DependencyProperty.Register("AllowMinusZero", typeof(bool), typeof(ANativeNumericUpDown<T>), new FrameworkPropertyMetadata(false));

        #endregion // Dependencies.

        #region Properties

        /// <summary>
        /// Gets the value indicating if the edited value can be set to null.
        /// </summary>
        /// <remarks>
        /// If true, the value can be set to null only at the initialization.
        /// Once the value is defined, it will never be able set it to null again.
        /// </remarks>
        public bool IsNullableValue
        {
            get
            {
                return (bool)GetValue(IsNullableValueProperty);
            }
            set
            {
                SetValue(IsNullableValueProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the infinite value.
        /// </summary>
        /// <remarks>
        /// If the infinite value is null, the corresponding infinite button in the template is not displayed.
        /// </remarks>
        public T? InfiniteValue
        {
            get
            {
                return (T?)GetValue(InfiniteValueProperty);
            }
            set
            {
                SetValue(InfiniteValueProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the number style to use during value parsing.
        /// </summary>
        public NumberStyles ParsingNumberStyle
        {
            get 
            { 
                return (NumberStyles)GetValue(ParsingNumberStyleProperty); 
            }
            set 
            { 
                SetValue(ParsingNumberStyleProperty, value); 
            }
        }

        /// <summary>
        /// Gets the value indicating if the value can be set to min when max is incremented.
        /// </summary>
        public bool AutoReverse
        {
            get
            {
                return (bool)GetValue(AutoReverseProperty);
            }
            set
            {
                SetValue(AutoReverseProperty, value);
            }
        }

        /// <summary>
        /// Gets the falg indicating if the minus zero can be entered in the control.
        /// </summary>
        public bool AllowMinusZero
        {
            get
            {
                return (bool)GetValue(AllowMinusZeroProperty);
            }
            set
            {
                SetValue(AllowMinusZeroProperty, value);
            }
        }

        #endregion // Properties.

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ANativeNumericUpDown"/> class.
        /// </summary>
        /// <param name="pFromText">The value converter from text.</param>
        /// <param name="pFromDecimal">The value converter from decimal.</param>
        /// <param name="pFromLowerThan">The lower than comparator.</param>
        /// <param name="pFromGreaterThan">The greater than comparator.</param>
        protected ANativeNumericUpDown(FromText pFromText, FromDecimal pFromDecimal, Func<T, T, bool> pFromLowerThan, Func<T, T, bool> pFromGreaterThan)
        {
            if (pFromText == null)
            {
                throw new ArgumentNullException("parseMethod");
            }

            if (pFromDecimal == null)
            {
                throw new ArgumentNullException("fromDecimal");
            }

            if (pFromLowerThan == null)
            {
                throw new ArgumentNullException("fromLowerThan");
            }

            if (pFromGreaterThan == null)
            {
                throw new ArgumentNullException("fromGreaterThan");
            }

            this.mFromText = pFromText;
            this.mFromDecimal = pFromDecimal;
            this.mFromLowerThan = pFromLowerThan;
            this.mFromGreaterThan = pFromGreaterThan;
        }

        #endregion // Constructors.

        #region Methods

        /// <summary>
        /// Parse the text defining a physical value.
        /// </summary>
        /// <param name="pText">The text to parse.</param>
        /// <param name="pCultureInfo">The culture info.</param>
        /// <returns>The parsed value as decimal.</returns>
        protected T ParsePhysicalValue(string pText, IFormatProvider pCultureInfo)
        {
            // Handling the unit symbol case.
            pText = pText.Replace(this.UnitSymbol, null);
            return this.mFromText(pText, NumberStyles.Any, pCultureInfo);
        }

        /// <summary>
        /// Updates the class metadata.
        /// </summary>
        /// <param name="pType">The type of the concrete editor.</param>
        /// <param name="pIncrement">The increment value.</param>
        /// <param name="pMinValue">The minimum value.</param>
        /// <param name="pMaxValue">The maximum value.</param>
        protected static void UpdateMetadata(Type pType, T? pIncrement, T? pMinValue, T? pMaxValue)
        {
            DefaultStyleKeyProperty.OverrideMetadata(pType, new FrameworkPropertyMetadata(pType));
            UpdateMetadataCommon(pType, pIncrement, pMinValue, pMaxValue);
        }

        /// <summary>
        /// Updates the class metadata.
        /// </summary>
        /// <param name="pType">The type of the concrete editor.</param>
        /// <param name="pIncrement">The increment value.</param>
        /// <param name="pMinValue">The minimum value.</param>
        /// <param name="pMaxValue">The maximum value.</param>
        private static void UpdateMetadataCommon(Type pType, T? pIncrement, T? pMinValue, T? pMaxValue)
        {
            IncrementProperty.OverrideMetadata(pType, new FrameworkPropertyMetadata(pIncrement));
            MinimumProperty.OverrideMetadata(pType, new FrameworkPropertyMetadata(pMinValue));
            MaximumProperty.OverrideMetadata(pType, new FrameworkPropertyMetadata(pMaxValue));
        }

        /// <summary>
        /// Test if the special value is allowed in this editor.
        /// </summary>
        /// <param name="pAllowedValues">The allowed values.</param>
        /// <param name="pValueToCompare">The value to test.</param>
        /// <returns>True if the special value is allowed, false otherwise.</returns>
        protected bool TestInputSpecialValue(AllowedSpecialValues pAllowedValues, AllowedSpecialValues pValueToCompare)
        {
            return ((pAllowedValues & pValueToCompare) == pValueToCompare);
        }

        private bool IsLowerThan(T? pValue1, T? pValue2)
        {
            if (pValue1 == null || pValue2 == null)
            {
                return false;
            }

            return this.mFromLowerThan(pValue1.Value, pValue2.Value);
        }

        private bool IsGreaterThan(T? pValue1, T? pValue2)
        {
            if (pValue1 == null || pValue2 == null)
            {
                return false;
            }

            return this.mFromGreaterThan(pValue1.Value, pValue2.Value);
        }

        private bool HandleNullSpin()
        {
            T? lValue = this.ConvertTextToValue(this.Text);
            if (lValue.HasValue == false)
            {
                T? lForcedValue = (this.DefaultValue.HasValue) ? this.DefaultValue.Value : default( T );
                lForcedValue = this.CoerceValueMinMax(lForcedValue.Value);
                if (this.UpdateValueOnEnterKey == false)
                {
                    // Updating directly the value.
                    this.Value = lForcedValue;
                }
                else
                {
                    // Updating the temp value only.
                    this.TempValue = lForcedValue;
                }

                return true;
            }
            else if (this.Increment.HasValue == false)
            {
                return true;
            }

            return false;
        }

        internal bool IsValid( T? value )
        {
          return !IsLowerThan( value, Minimum ) && !IsGreaterThan( value, Maximum );
        }

        /// <summary>
        /// Method called when the maximum value is modified.
        /// </summary>
        /// <param name="pOldValue">The old maximum value.</param>
        /// <param name="pNewValue">The new maximum value.</param>
        protected override void OnMaximumChanged(T? pOldValue, T? pNewValue)
        {
            base.OnMaximumChanged(pOldValue, pNewValue);
            if (this.IsInitialized)
            {
                if (this.Value.HasValue)
                {
                    this.Value = this.CoerceValueMinMaxInfinite(this.Value.Value);
                    this.SyncTextAndValueProperties(false, null, true);
                }
            }
        }

        /// <summary>
        /// Method called when the minimum value is modified.
        /// </summary>
        /// <param name="pOldValue">The old minimum value.</param>
        /// <param name="pNewValue">The new minimum value.</param>
        protected virtual void OnMinimumChanged(T pOldValue, T pNewValue)
        {
            base.OnMinimumChanged(pOldValue, pNewValue);
            if (this.IsInitialized)
            {
                if (this.Value.HasValue)
                {
                    this.Value = this.CoerceValueMinMaxInfinite(this.Value.Value);
                    this.SyncTextAndValueProperties(false, null, true);
                }
            }
        }

        private T? CoerceValueMinMaxInfinite(T pValue)
        {
            if (this.InfiniteValue != null && (this.IsGreaterThan(pValue, this.Maximum) || object.Equals(this.InfiniteValue.Value, pValue)))
            {
                return this.InfiniteValue;
            }

            if (IsLowerThan(pValue, Minimum))
                return Minimum;
            else if (IsGreaterThan(pValue, Maximum))
                return Maximum;
            else
                return pValue;
        }

        private T? CoerceValueMinMax( T value )
        {
          if( IsLowerThan( value, Minimum ) )
            return Minimum;
          else if( IsGreaterThan( value, Maximum ) )
            return Maximum;
          else
            return value;
        }

        protected override T? OnCoerceValue(T? newValue)
        {
            if (newValue == null && this.mLastDefinedValue != null && this.IsNullableValue == false)
            {
                return this.mLastDefinedValue;
            }

            return newValue;
        }

        protected override void OnValueChanged(T? oldValue, T? newValue)
        {
            base.OnValueChanged(oldValue, newValue);

            this.mLastDefinedValue = newValue;
            this.mTempValue = newValue;
        }

        /// <summary>
        /// Stores the value used to display the value when the control is in UpdateValueOnEnterKey mode.
        /// </summary>
        private T? mTempValue;

        /// <summary>
        /// Gets or sets the value used to display the value when the control is in <see cref="UpdateValueOnEnterKey"/> mode.
        /// </summary>
        protected T? TempValue
        {
            get
            {
                return this.mTempValue;
            }

            set
            {
                this.mTempValue = value;
                this.Text = this.ConvertRawValueToText(this.mTempValue);
                this.TextBox.Text = this.Text;
            }
        }

        protected override void OnIncrement()
        {
            T? lValue = null;
            if (this.HandleNullSpin() == false)
            {
                if (this.UpdateValueOnEnterKey == false)
                {
                    // Updating directly the value.
                    if (this.AutoReverse && this.Value.Value.Equals(this.Maximum.Value))
                    {
                        lValue = this.Minimum;
                    }
                    else
                    {
                        lValue = this.IncrementValue(this.Value.Value, this.Increment.Value);
                    }
                    this.Value = this.CoerceValueMinMaxInfinite(lValue.Value);
                }
                else
                {
                    // Updating the temp value only.
                    if (this.AutoReverse && this.Value.Value.Equals(this.Maximum.Value))
                    {
                        lValue = this.Minimum;
                    }
                    else
                    {
                        lValue = this.IncrementValue(this.TempValue.Value, this.Increment.Value);
                    }
                    this.TempValue = this.CoerceValueMinMaxInfinite(lValue.Value);
                }
            }

            // Nothing to do as disabling an up down button will result in loosing the focus...
            //this.SetValidSpinDirection(lValue);
        }

        protected override void OnDecrement()
        {
            T? lValue = null;
            if  (this.HandleNullSpin() == false)
            {
                if (this.UpdateValueOnEnterKey == false)
                {
                    // Updating directly the value.
                    if (this.AutoReverse && this.Value.Value.Equals(this.Minimum.Value))
                    {
                        lValue = this.Maximum;
                    }
                    else
                    {
                        lValue = this.DecrementValue(this.Value.Value, this.Increment.Value);
                    }
                    this.Value = this.CoerceValueMinMax(lValue.Value);
                }
                else
                {
                    // Updating the temp value only.
                    if (this.AutoReverse && this.Value.Value.Equals(this.Minimum.Value))
                    {
                        lValue = this.Maximum;
                    }
                    else
                    {
                        lValue = this.DecrementValue(this.TempValue.Value, this.Increment.Value);
                    }
                    this.TempValue = this.CoerceValueMinMax(lValue.Value);
                }
            }

            // Nothing to do as disabling an up down button will result in loosing the focus...
            //this.SetValidSpinDirection(lValue);
        }

        /// <summary>
        /// Converts the text to the corresponding value.
        /// </summary>
        /// <param name="pText">The text to convert.</param>
        /// <returns>The converted value.</returns>
        protected override T? ConvertTextToValue(string pText)
        {
            T? lResult = null;

            // The value is not valid.
            if (String.IsNullOrEmpty(pText))
            {
                return lResult;
            }

            // The value is the infinite.
            if (this.InfiniteValue.HasValue && pText == Constants.INFINITY_SYMBOL.ToString())
            {
                return this.InfiniteValue;
            }

            // Negative sign can be entered alone in the control. It's valid but it does not change he value.
            if (pText == CultureInfo.InvariantCulture.NumberFormat.NegativeSign)
            {
                return this.Value;
            }

            // Since the conversion from Value to text using a FormartString may not be parsable,
            // we verify that the already existing text is not the exact same value.
            string lCurrentValueText = this.ConvertValueToText();
            if (object.Equals(lCurrentValueText, pText))
            {
                return this.Value;
            }

            // Ensuring the good decimal separator is used.
            pText = pText.Replace(",", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator);

            //If the value is approximated, remove the ~ prefix as it cannot be parsed.
            //The prefix is to be restored just before returning the the string.
            if (pText.StartsWith(Constants.APPROXIMATION_SYMBOL))
            {
                pText = pText.Replace(Constants.APPROXIMATION_SYMBOL, String.Empty);
            }

            try
            {
                lResult = this.ContainsUnitSymbol(this.BuildFormatString())
                    ? this.ParsePhysicalValue(pText, CultureInfo.InvariantCulture)
                    : this.mFromText(pText, this.ParsingNumberStyle, CultureInfo.InvariantCulture);

                return this.GetClippedMinMaxInfiniteValue();
            }
            catch
            {
                return this.Value;
            }
        }

        protected override void OnSpin(SpinEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException("e");
            }

            if (e.Direction == SpinDirection.Increase)
            {
                if (e.GoToInfinite)
                {
                    if (this.UpdateValueOnEnterKey == false)
                    {
                        this.Value = this.InfiniteValue;
                    }
                    else
                    {
                        this.TempValue = this.InfiniteValue;
                    }
                }
                else
                {
                    this.DoIncrement();
                }
            }
            else
            {
                this.DoDecrement();
            }
        }

        /// <summary>
        /// Converts the raw value to text (not formatted).
        /// </summary>
        /// <param name="pValue">The value.</param>
        /// <returns></returns>
        protected override string ConvertRawValueToText(T? pValue)
        {
            if (pValue == null)
            {
                return string.Empty;
            }

            if (this.InfiniteValue.HasValue && object.Equals(pValue, this.InfiniteValue))
            {
                return Constants.INFINITY_SYMBOL.ToString();
            }

            string lNewText = pValue.Value.ToString("0.###############", CultureInfo);

            // Handling the case a negative sign has been specified in the text in front of a zero value (default(T) returns the 0 value strongly typed).
            if (this.AllowMinusZero && this.Text != null && this.Text != Constants.INFINITY_SYMBOL.ToString())
            {
                T? lCurrentValue = this.GetClippedMinMaxInfiniteValue();
                if (this.Text.StartsWith(CultureInfo.InvariantCulture.NumberFormat.NegativeSign)
                    && lCurrentValue != null && lCurrentValue.Equals(default(T)) && this.IsLowerThan(this.Minimum, default(T))
                    && pValue != null && pValue.Equals(default(T)))
                {
                    lNewText = CultureInfo.InvariantCulture.NumberFormat.NegativeSign + lNewText;
                }
            }

            return lNewText;
        }

        /// <summary>
        /// Converts the value to text.
        /// </summary>
        /// <param name="pValue">The p value.</param>
        /// <returns></returns>
        protected override string ConvertValueToText(T? pValue)
        {
            if (pValue == null)
            {
                return string.Empty;
            }

            if (this.InfiniteValue.HasValue && object.Equals(pValue, this.InfiniteValue))
            {
                return Constants.INFINITY_SYMBOL.ToString();
            }

            string lFormatString = this.BuildFormatString();
            string lNewText = string.Format(CultureInfo.InvariantCulture, lFormatString, pValue);

            T lValue = this.ContainsUnitSymbol(lFormatString)
                        ? this.ParsePhysicalValue(lNewText, CultureInfo.InvariantCulture)
                        : this.mFromText(lNewText, NumberStyles.Any, CultureInfo.InvariantCulture);

            // Limit the number of digit to 15 before comparison ("0.###############").
            T lRawValue = this.mFromText(pValue.Value.ToString("0.###############", CultureInfo.InvariantCulture), NumberStyles.Any, CultureInfo.InvariantCulture);

            if (lValue.CompareTo(lRawValue) != 0)
            {
                lNewText = string.Format("{0}{1}", Constants.APPROXIMATION_SYMBOL, lNewText);
            }

            // Handling the case a negative sign has been specified in the text in front of a zero value (default(T) returns the 0 value strongly typed).
            if (this.AllowMinusZero && this.Text != null && this.Text != Constants.INFINITY_SYMBOL.ToString())
            {
                T? lCurrentValue = this.GetClippedMinMaxInfiniteValue();
                if (this.Text.StartsWith(CultureInfo.InvariantCulture.NumberFormat.NegativeSign)
                    && lCurrentValue != null && lCurrentValue.Equals(default(T)) && this.IsLowerThan(this.Minimum, default(T))
                    && pValue != null && pValue.Equals(default(T)))
                {
                    lNewText = CultureInfo.InvariantCulture.NumberFormat.NegativeSign + lNewText;
                }
            }

            return lNewText;
        }

        protected override void SetValidSpinDirection(T? pValue)
        {
          ValidSpinDirections validDirections = ValidSpinDirections.None;

          // Null increment always prevents spin.
          if( (this.Increment != null) && !IsReadOnly )
          {
              if (IsLowerThan(pValue, Maximum) || !pValue.HasValue || !Maximum.HasValue)
              validDirections = validDirections | ValidSpinDirections.Increase;

              if (IsGreaterThan(pValue, Minimum) || !pValue.HasValue || !Minimum.HasValue)
              validDirections = validDirections | ValidSpinDirections.Decrease;
          }

          if( Spinner != null )
            Spinner.ValidSpinDirections = validDirections;
        }

        /// <summary>
        /// Coerce the text entered by the user.
        /// </summary>
        /// <param name="pBaseText">The text to coerce.</param>
        /// <returns>The coerced text.</returns>
        protected override string CoerceText(string pBaseText)
        {
            if (string.IsNullOrEmpty(pBaseText))
            {
                return string.Empty;
            }

            // The value is the infinite.
            if (this.InfiniteValue.HasValue && pBaseText == Constants.INFINITY_SYMBOL.ToString())
            {
                return pBaseText;
            }

            // Ensuring the good decimal separator is used.
            string lText = pBaseText.Replace(",", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator);

            //If the value is approximated, remove the ~ prefix as it cannot be parsed.
            //The prefix is to be restored just before return ning the the string.
            bool lIsApproximated = false;
            if (lText.StartsWith(Constants.APPROXIMATION_SYMBOL))
            {
                lText = lText.Replace(Constants.APPROXIMATION_SYMBOL, String.Empty);
                lIsApproximated = true;
            }

            try
            {
                T? result = this.ContainsUnitSymbol(this.BuildFormatString())
                          ? this.ParsePhysicalValue(lText, CultureInfo.InvariantCulture)
                          : this.mFromText(lText, this.ParsingNumberStyle, CultureInfo.InvariantCulture);

                //If the value is approximated, add ~ prefix
                if (lIsApproximated)
                {
                    lText = string.Format("{0}{1}", Constants.APPROXIMATION_SYMBOL, lText);
                }

                return lText;
            }
            catch
            {
                return this.Text;
            }
        }

        private T? GetClippedMinMaxInfiniteValue()
        {
            if (string.IsNullOrEmpty(this.Text))
            {
                return null;
            }

            // Ensuring the good decimal separator is used.
            string lText = this.Text.Replace(",", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator);

            //If the value is approximated, remove the ~ prefix as it cannot be parsed.
            //The prefix is to be restored just before return ning the the string.
            bool lIsApproximated = false;
            if (lText.StartsWith(Constants.APPROXIMATION_SYMBOL))
            {
                lText = lText.Replace(Constants.APPROXIMATION_SYMBOL, String.Empty);
                lIsApproximated = true;
            }

            try
            {
                T? result = this.ContainsUnitSymbol(this.BuildFormatString())
                          ? this.ParsePhysicalValue(lText, CultureInfo.InvariantCulture)
                          : this.mFromText(lText, this.ParsingNumberStyle, CultureInfo.InvariantCulture);

                // If the value is approximated, add ~ prefix.
                if (lIsApproximated)
                {
                    lText = string.Format("{0}{1}", Constants.APPROXIMATION_SYMBOL, lText);
                }

                return this.CoerceValueMinMaxInfinite(result.Value);
            }
            catch
            {
                return this.Value;
            }
        }

        private T IncrementValue(T pValue, T pIncrement)
        {
            if (this.InfiniteValue != null && (object.Equals(pValue, this.Maximum) || object.Equals(pValue, this.InfiniteValue)))
            {
                return this.InfiniteValue.Value;
            }

            return this.CustomIncrementValue(pValue, pIncrement);
        }

        protected abstract T CustomIncrementValue(T pValue, T pIncrement);

        private T DecrementValue(T pValue, T pIncrement)
        {
            if (this.InfiniteValue != null && object.Equals(pValue, this.InfiniteValue))
            {
                return this.Maximum.Value;
            }

            return this.CustomDecrementValue(pValue, pIncrement);
        }

        protected abstract T CustomDecrementValue(T pValue, T pIncrement);

        #endregion // Methods.
    }
}

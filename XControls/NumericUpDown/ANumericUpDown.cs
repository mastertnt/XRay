using System;
using System.Globalization;
using System.Windows;
using XControls.Core;
using XControls.Primitives;

namespace XControls
{
    /// <summary>
    /// Class defining a base numeric up down editor.
    /// </summary>
    /// <typeparam name="T">The type of the numeric to edit.</typeparam>
    public abstract class ANumericUpDown<T> : UpDownBase<T>
    {
        #region Dependencies

        /// <summary>
        /// Identifies the AutoSelectBehavior property.
        /// </summary>
        public static readonly DependencyProperty AutoSelectBehaviorProperty = DependencyProperty.Register("AutoSelectBehavior", typeof(AutoSelectBehavior), typeof(ANumericUpDown<T>), new UIPropertyMetadata(AutoSelectBehavior.OnFocus));

        /// <summary>
        /// Identifies the AutoMoveFocus property.
        /// </summary>
        public static readonly DependencyProperty AutoMoveFocusProperty = DependencyProperty.Register("AutoMoveFocus", typeof(bool), typeof(ANumericUpDown<T>), new UIPropertyMetadata(false));

        /// <summary>
        /// Identifies the FormatString property.
        /// </summary>
        public static readonly DependencyProperty FormatStringProperty = DependencyProperty.Register("FormatString", typeof(string), typeof(ANumericUpDown<T>), new UIPropertyMetadata(string.Empty, OnFormatStringChanged));

        /// <summary>
        /// Identifies the NumberDecimalDigits property.
        /// </summary>
        public static readonly DependencyProperty NumberDecimalDigitsProperty = DependencyProperty.Register("NumberDecimalDigits", typeof(int), typeof(ANumericUpDown<T>), new UIPropertyMetadata(0, OnNumberDecimalDigitsChanged));

        /// <summary>
        /// Identifies the UnitSymbol property.
        /// </summary>
        public static readonly DependencyProperty UnitSymbolProperty = DependencyProperty.Register("UnitSymbol", typeof(string), typeof(ANumericUpDown<T>), new UIPropertyMetadata(string.Empty, OnUnitSymbolChanged));
        
        /// <summary>
        /// Identifies the Increment property.
        /// </summary>
        public static readonly DependencyProperty IncrementProperty = DependencyProperty.Register("Increment", typeof(T), typeof(ANumericUpDown<T>), new PropertyMetadata(default(T), OnIncrementChanged, OnCoerceIncrement));

        #endregion // Dependencies.

        #region Fields

        /// <summary>
        /// Stores the string set by the user without the unit symbol.
        /// </summary>
        private string mBaseWatermark;

        #endregion // Fields.

        #region Properties

        /// <summary>
        /// Gets or sets the flag indicating when the automatic selection can be done.
        /// </summary>
        public AutoSelectBehavior AutoSelectBehavior
        {
            get
            {
                return (AutoSelectBehavior)GetValue(AutoSelectBehaviorProperty);
            }
            set
            {
                SetValue(AutoSelectBehaviorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the flag indicating if the focus can be moved using the arrow keys.
        /// </summary>
        public bool AutoMoveFocus
        {
            get
            {
                return (bool)GetValue(AutoMoveFocusProperty);
            }
            set
            {
                SetValue(AutoMoveFocusProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the format string.
        /// </summary>
        public string FormatString
        {
            get
            {
                return (string)GetValue(FormatStringProperty);
            }
            set
            {
                SetValue(FormatStringProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the step to use to increment the value.
        /// </summary>
        public T Increment
        {
            get
            {
                return (T)GetValue(IncrementProperty);
            }
            set
            {
                SetValue(IncrementProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the number of decimal digits.
        /// </summary>
        public int NumberDecimalDigits
        {
            get
            {
                return (int)GetValue(NumberDecimalDigitsProperty);
            }
            set
            {
                SetValue(NumberDecimalDigitsProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the unit symbol.
        /// </summary>
        public string UnitSymbol
        {
            get
            {
                return (string)GetValue(UnitSymbolProperty);
            }
            set
            {
                SetValue(UnitSymbolProperty, value);
            }
        }

        #endregion // Properties.

        #region Methods

        /// <summary>
        /// Delegate called when the format string changed.
        /// </summary>
        /// <param name="pObject">The modified control.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private static void OnFormatStringChanged(DependencyObject pObject, DependencyPropertyChangedEventArgs pEventArgs)
        {
            ANumericUpDown<T> lNumericUpDown = pObject as ANumericUpDown<T>;
            if (lNumericUpDown != null)
            {
                lNumericUpDown.OnFormatStringChanged((string)pEventArgs.OldValue, (string)pEventArgs.NewValue);
            }
        }

        /// <summary>
        /// Delegate called when the format string changed.
        /// </summary>
        /// <param name="pObject">The modified control.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private static void OnNumberDecimalDigitsChanged(DependencyObject pObject, DependencyPropertyChangedEventArgs pEventArgs)
        {
            ANumericUpDown<T> lNumericUpDown = pObject as ANumericUpDown<T>;
            if (lNumericUpDown != null)
            {
                lNumericUpDown.SyncTextAndValueProperties();
            }
        }

        /// <summary>
        /// Delegate called when the unit symbol changed.
        /// </summary>
        /// <param name="pObject">The modified control.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private static void OnUnitSymbolChanged(DependencyObject pObject, DependencyPropertyChangedEventArgs pEventArgs)
        {
            ANumericUpDown<T> lNumericUpDown = pObject as ANumericUpDown<T>;
            if (lNumericUpDown != null)
            {
                lNumericUpDown.SyncTextAndValueProperties();

                // Foce the watermark refresh.
                lNumericUpDown.ForceWatermarkRefresh();
            }
        }

        /// <summary>
        /// Builds and returns the format string depending on the number of decimal, unit symbol... properties.
        /// </summary>
        protected string BuildFormatString()
        {
            string lFormat = string.Empty;

            if (string.IsNullOrEmpty(this.FormatString) == false)
            {
                // Using the format string to format the number.
                lFormat = "{0:" + this.FormatString + "}";
            }
            else
            {
                // Using the the number of digits to format the number.
                lFormat = "{0:0";
                if (this.NumberDecimalDigits > 0)
                {
                    lFormat += ".";
                    for (int lIter = 0; lIter < this.NumberDecimalDigits; lIter++)
                    {
                        lFormat += "0";
                    }
                }
                lFormat += "}";
            }

            // Adding the unit symbol if any.
            if (string.IsNullOrEmpty(this.UnitSymbol) == false)
            {
                lFormat += " " + this.UnitSymbol;
            }

            return lFormat;
        }

        /// <summary>
        /// Delegate called when the format string changed.
        /// </summary>
        /// <param name="pOldValue">The old value.</param>
        /// <param name="pNewValue">The new value.</param>
        protected virtual void OnFormatStringChanged(string pOldValue, string pNewValue)
        {
            this.SyncTextAndValueProperties();
        }

        /// <summary>
        /// Synchronizes the text and the value.
        /// </summary>
        private void SyncTextAndValueProperties()
        {
            if (this.IsInitialized)
            {
                this.SyncTextAndValueProperties(false, null);
            }
        }

        /// <summary>
        /// Delegate called when the increment value changed.
        /// </summary>
        /// <param name="pObject">The modified control.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private static void OnIncrementChanged(DependencyObject pObject, DependencyPropertyChangedEventArgs pEventArgs)
        {
            ANumericUpDown<T> lNumericUpDown = pObject as ANumericUpDown<T>;
            if (lNumericUpDown != null)
            {
                lNumericUpDown.OnIncrementChanged((T)pEventArgs.OldValue, (T)pEventArgs.NewValue);
            }
        }

        /// <summary>
        /// Deleaget calles when the increment value changed.
        /// </summary>
        /// <param name="pOldValue">The old value.</param>
        /// <param name="pNewValue">The new value.</param>
        protected virtual void OnIncrementChanged(T pOldValue, T pNewValue)
        {
            if (this.IsInitialized)
            {
                this.SetValidSpinDirection();
            }
        }

        /// <summary>
        /// Delegate called when the increment value is coerced.
        /// </summary>
        /// <param name="pObject">The modified control.</param>
        /// <param name="pBaseValue">The value to coerce.</param>
        /// <returns>The coerced value.</returns>
        private static object OnCoerceIncrement(DependencyObject pObject, object pBaseValue)
        {
            ANumericUpDown<T> lNumericUpDown = pObject as ANumericUpDown<T>;
            if (lNumericUpDown != null)
            {
                return lNumericUpDown.OnCoerceIncrement((T)pBaseValue);
            }

            return pBaseValue;
        }

        /// <summary>
        /// Delegate called when the increment value is coerced.
        /// </summary>
        /// <param name="pBaseValue">The value to coerce.</param>
        /// <returns>The coerced value.</returns>
        protected virtual T OnCoerceIncrement(T pBaseValue)
        {
            return pBaseValue;
        }

        /// <summary>
        /// Verifies if the given text contains the percent symbol letter.
        /// </summary>
        /// <param name="pStringToTest">The string to test.</param>
        /// <returns>Flag to know if the symbol is in the string.</returns>
        protected bool ContainsUnitSymbol(string pStringToTest)
        {
            return (string.IsNullOrEmpty(this.UnitSymbol) == false && pStringToTest.Contains(this.UnitSymbol));
        }

        /// <summary>
        /// Coerce the Watermark.
        /// </summary>
        /// <param name="pBaseValue">The Watermark to coerce.</param>
        /// <returns>The coerced Watermark.</returns>
        protected override string OnCoerceWatermark(string pBaseValue)
        {
            // Saving the base watermark.
            this.mBaseWatermark = (string)pBaseValue;

            // Taking the unit symbol in account if any.
            if (string.IsNullOrEmpty(this.mBaseWatermark) == false && string.IsNullOrEmpty(this.UnitSymbol) == false)
            {
                return string.Format(Constants.WATERMARK_WITH_SYMBOL_STRING_FORMAT, pBaseValue, this.UnitSymbol);
            }

            return pBaseValue;
        }

        /// <summary>
        /// Force the watermark refresh to display the unit.
        /// </summary>
        private void ForceWatermarkRefresh()
        {
            if (this.IsInitialized || string.IsNullOrEmpty(this.mBaseWatermark) == false)
            {
                string lBaseWatermark = this.mBaseWatermark;
                this.Watermark = null;
                this.Watermark = lBaseWatermark;
            }
        }

        /// <summary>
        /// Displays the raw value as text in the text box.
        /// </summary>
        protected override void CustomDisplayRawValueAsText()
        {
            string lRawNewText = this.ConvertRawValueToText(this.Value);
            string lNewText = this.ConvertValueToText(this.Value);

            // Remove the ~ prefix and the symbol : we want to compare the values as string.
            if (lNewText.StartsWith(Constants.APPROXIMATION_SYMBOL))
            {
                lNewText = lNewText.Replace(Constants.APPROXIMATION_SYMBOL, String.Empty);
            }
            if (string.IsNullOrEmpty(this.UnitSymbol) == false && lNewText.EndsWith(this.UnitSymbol))
            {
                lNewText = lNewText.Replace(this.UnitSymbol, String.Empty);
                lNewText = lNewText.Trim();
            }

            // Keep longer of both text to keep insignificant digit that are displayed because of number format : 
            // 1.123456789 displayed as ~1.1 : dislay "1.123456789"
            // 245 displayed as 245.0 (format string) : display 245.0 (ConvertRawValueToText would return "245")
            if (lNewText.Length < lRawNewText.Length)
            {
                lNewText = lRawNewText;
            }
            if (object.Equals(this.Text, lNewText) == false)
            {
                this.Text = lNewText;
            }

            // Sync Text and textBox.
            if (this.TextBox != null)
            {
                this.TextBox.Text = this.Text;
            }
        }

        #endregion // Methods.
    }
}

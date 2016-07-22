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
using System.Globalization;
using System.Windows;
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
        public static readonly DependencyProperty FormatStringProperty = DependencyProperty.Register("FormatString", typeof(string), typeof(ANumericUpDown<T>), new UIPropertyMetadata(String.Empty, OnFormatStringChanged));

        /// <summary>
        /// Identifies the NumberDecimalDigits property.
        /// </summary>
        public static readonly DependencyProperty NumberDecimalDigitsProperty = DependencyProperty.Register("NumberDecimalDigits", typeof(int), typeof(ANumericUpDown<T>), new UIPropertyMetadata(0, OnNumberDecimalDigitsChanged));

        /// <summary>
        /// Identifies the Increment property.
        /// </summary>
        public static readonly DependencyProperty IncrementProperty = DependencyProperty.Register("Increment", typeof(T), typeof(ANumericUpDown<T>), new PropertyMetadata(default(T), OnIncrementChanged, OnCoerceIncrement));
        
        #endregion // Dependencies.

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
                // Updating the format.
                string lFormat = "0";

                int lValue = (int)pEventArgs.NewValue;
                if (lValue > 0)
                {
                    lFormat += ".";
                    for (int lIter = 0; lIter < lValue; lIter++)
                    {
                        lFormat += "0";
                    }
                }

                lNumericUpDown.FormatString = lFormat;
            }
        }

        /// <summary>
        /// Delegate called when the format string changed.
        /// </summary>
        /// <param name="pOldValue">The old value.</param>
        /// <param name="pNewValue">The new value.</param>
        protected virtual void OnFormatStringChanged(string pOldValue, string pNewValue)
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
        /// Parse the text defining a percent value.
        /// </summary>
        /// <param name="pText">The text to parse.</param>
        /// <param name="pCultureInfo">The culture info.</param>
        /// <returns>The parsed value as decimal.</returns>
        protected static decimal ParsePercent(string pText, IFormatProvider pCultureInfo)
        {
            NumberFormatInfo lInfo = NumberFormatInfo.GetInstance(pCultureInfo);

            pText = pText.Replace(lInfo.PercentSymbol, null);

            decimal lResult = Decimal.Parse(pText, NumberStyles.Any, lInfo);
            lResult = lResult / 100;

            return lResult;
        }

        #endregion //Methods
    }
}

using System;
using System.Windows;
using System.Windows.Input;
using XControls.Core.Converters;

namespace XControls.TextBox
{
    /// <summary>
    ///     Defines a textbox which can edit a value constrained to a unique type.
    /// </summary>
    /// <!-- DPE -->
    public partial class ConstrainedTextBox : System.Windows.Controls.TextBox
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DS_Class1" /> class.
        /// </summary>
        public ConstrainedTextBox()
        {
            this.InitializeComponent();

            this.mUseMax = false;
            this.mUseMin = false;

            // Handling textbox events.
            this.PreviewTextInput += this.OnPreviewTextInput;
            this.GotFocus += this.OnGotFocus;
            this.LostFocus += this.OnLostFocus;
            this.KeyUp += this.OnKeyUp;
        }

        #endregion // Constructors.

        #region Fields

        /// <summary>
        ///     Hold the previous value when the textbox get focused.
        ///     Manly useful when paste is performed with bad value.
        /// </summary>
        private string mPreviousValue;

        /// <summary>
        ///     Tells whether the min value should be used as lower bound.
        /// </summary>
        private bool mUseMin;

        /// <summary>
        ///     Tells whether the max value should be used as lower bound.
        /// </summary>
        private bool mUseMax;

        #endregion // Fields.

        #region Finalizers (Destructors)

        #endregion // Finalizers (Destructors).

        #region Delegates

        #endregion // Delegates.

        #region Events

        #endregion // Events.

        #region Dependency properties

        /// <summary>
        ///     The dependency property which owned the ValueType property.
        /// </summary>
        public static readonly DependencyProperty ValueTypeProperty = DependencyProperty.Register("ValueType", typeof(Type), typeof(ConstrainedTextBox), new PropertyMetadata(null, null));

        /// <summary>
        ///     Identifies the MaxValue dependency property.
        /// </summary>
        public static readonly DependencyProperty MaxValueProperty = DependencyProperty.Register("MaxValue", typeof(double), typeof(ConstrainedTextBox), new FrameworkPropertyMetadata(double.MaxValue, OnMaxValueChanged));

        /// <summary>
        ///     Identifies the MinValue dependency property.
        /// </summary>
        public static readonly DependencyProperty MinValueProperty = DependencyProperty.Register("MinValue", typeof(double), typeof(ConstrainedTextBox), new FrameworkPropertyMetadata(double.MinValue, OnMinValueChanged));

        #endregion // Dependency properties.

        #region Properties

        /// <summary>
        ///     Gets or sets the type of the value the editor is editing.
        /// </summary>
        public Type ValueType
        {
            get => (Type) this.GetValue(ValueTypeProperty);
            set => this.SetValue(ValueTypeProperty, value);
        }


        /// <summary>
        ///     Gets or sets the minimum value.
        /// </summary>
        public double MinValue
        {
            get => (double) this.GetValue(MinValueProperty);
            set => this.SetValue(MinValueProperty, value);
        }

        /// <summary>
        ///     Gets or sets the maxmimum value.
        /// </summary>
        public double MaxValue
        {
            get => (double) this.GetValue(MaxValueProperty);
            set => this.SetValue(MaxValueProperty, value);
        }

        #endregion // Properties.

        #region Indexers

        #endregion // Indexers.

        #region Methods

        /// <summary>
        ///     Method called when an input is about to be entered.
        /// </summary>
        /// <param name="pSender">The object sender.</param>
        /// <param name="pArgs">The event arguments.</param>
        private void OnPreviewTextInput(object pSender, TextCompositionEventArgs pArgs)
        {
            try
            {
                var lText = this.Text;
                if (this.SelectedText != string.Empty)
                {
                    lText = this.Text.Replace(this.SelectedText, "");
                }

                // Build the Preview Text
                var lPreviewText = lText.Insert((pSender as System.Windows.Controls.TextBox).CaretIndex, pArgs.Text);
                if (lPreviewText.Length != 0)
                {
                    if (char.IsNumber(lPreviewText[lPreviewText.Length - 1]) == false && this.ValueType != typeof(string))
                    {
                        lPreviewText += "0";
                    }
                }

                if (!this.IsTextSuitable(lPreviewText))
                {
                    pArgs.Handled = true;
                }
            }
            catch // Managed
            {
                // The value doesn't have the good type, reinitializing the textbox with the old value.
                pArgs.Handled = true;
            }
        }

        /// <summary>
        ///     This delagate is called when the min value is changed.
        /// </summary>
        /// <param name="pSender">The event sender.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private static void OnMinValueChanged(DependencyObject pObject, DependencyPropertyChangedEventArgs pEventArgs)
        {
            var lControl = pObject as ConstrainedTextBox;
            if (lControl != null)
            {
                lControl.mUseMin = true;
            }
        }

        /// <summary>
        ///     This delagate is called when the max value is changed.
        /// </summary>
        /// <param name="pSender">The event sender.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private static void OnMaxValueChanged(DependencyObject pObject, DependencyPropertyChangedEventArgs pEventArgs)
        {
            var lControl = pObject as ConstrainedTextBox;
            if (lControl != null)
            {
                lControl.mUseMax = true;
            }
        }

        /// <summary>
        ///     Method called when the textbox got the focus.
        /// </summary>
        /// <param name="pSender">The object sender.</param>
        /// <param name="pArgs">The event arguments.</param>
        private void OnGotFocus(object pSender, RoutedEventArgs pArgs)
        {
            // Saving the previous value.
            this.mPreviousValue = this.Text;
            this.SelectAll();
        }

        /// <summary>
        ///     Determines whether pNewText matches the constraints (type & bounds).
        /// </summary>
        /// <param name="pNewText">The text to test.</param>
        /// <returns>
        ///     <c>true</c> if pNewText matches the constraints; otherwise, <c>false</c>.
        /// </returns>
        private bool IsTextSuitable(string pNewText)
        {
            try
            {
                if (this.ValueType != typeof(double))
                {
                    // Verify the value is of type ValueType.
                    Convert.ChangeType(pNewText, this.ValueType);
                }
                else
                {
                    // In case of double, try to convert using DoubleToStringWithCulture.
                    // It allows to customize double parsing.
                    DoubleToStringWithCulture lCulture = new DoubleToStringWithCulture();
                    object lValue = lCulture.ConvertBack(pNewText, null, null, null); // Only first parameter is used.
                    if (lValue == null)
                    {
                        // The value doesn't have the good type,return false.
                        return false;
                    }
                }

                //
                if ((this.ValueType == typeof(double) || this.ValueType == typeof(int) || this.ValueType == typeof(float) || this.ValueType == typeof(long)) && (this.mUseMax || this.mUseMin))
                {
                    // In case of double, try to convert using DoubleToStringWithCulture.
                    // It allows to customize double parsing.
                    DoubleToStringWithCulture lCulture = new DoubleToStringWithCulture();
                    object lValue = lCulture.ConvertBack(pNewText, null, null, null); // Only first parameter is used.

                    // Check bounds
                    var lValueAsDouble = (double) lValue;

                    if (this.mUseMax && lValueAsDouble > this.MaxValue)
                    {
                        // The value doesn't match bounds.
                        return false;
                    }

                    if (this.mUseMin && lValueAsDouble < this.MinValue)
                    {
                        // The value doesn't match bounds.
                        return false;
                    }
                }
            }
            catch // Managed
            {
                // The value doesn't have the good type.
                return false;
            }

            return true;
        }

        /// <summary>
        ///     Method called when the textbox lost the focus.
        /// </summary>
        /// <param name="pSender">The object sender.</param>
        /// <param name="pArgs">The event arguments.</param>
        private void OnLostFocus(object pSender, RoutedEventArgs pArgs)
        {
            try
            {
                if (this.Text.Length != 0)
                {
                    if (char.IsNumber(this.Text[this.Text.Length - 1]) == false && this.ValueType != typeof(string))
                    {
                        this.Text += "0";
                    }
                }

                if (!this.IsTextSuitable(this.Text))
                {
                    this.Text = this.mPreviousValue;
                }
            }
            catch // Managed
            {
                // The value doesn't have the good type, reinitializing the textbox 
                // with the old value.
                this.Text = this.mPreviousValue;
            }
        }

        /// <summary>
        ///     Method called when a key is unpressed on the textbox.
        /// </summary>
        /// <param name="pSender">The object sender.</param>
        /// <param name="pArgs">The event arguments.</param>
        private void OnKeyUp(object pSender, KeyEventArgs pArgs)
        {
            if (pArgs.Key == Key.Enter)
            {
                var lRequest = new TraversalRequest(FocusNavigationDirection.Next);
                this.MoveFocus(lRequest);
            }
        }

        #endregion // Methods.

        #region Inner Structs

        #endregion // Inner Structs.

        #region Inner Classes

        #endregion // Inner Classes.
    }
}
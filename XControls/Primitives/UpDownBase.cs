using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using XControls.Core;
using XControls.Core.Input;
using System.Globalization;
using System.Windows.Threading;

namespace XControls.Primitives
{
    [TemplatePart(Name = PART_TextBox, Type = typeof(TextBox))]
    [TemplatePart(Name = PART_Spinner, Type = typeof(ASpinner))]
    public abstract class UpDownBase<T> : InputBase, IValidateInput
    {
        #region Dependencies

        /// <summary>
        /// Identifies the FocusNavigationDirection dependency property.
        /// </summary>
        public static readonly DependencyProperty FocusNavigationDirectionProperty = DependencyProperty.Register("FocusNavigationDirection", typeof(FocusNavigationDirection), typeof(UpDownBase<T>), new FrameworkPropertyMetadata(FocusNavigationDirection.Next));

        /// <summary>
        /// Identifies the HandlesSpecialKeys dependency property.
        /// </summary>
        public static readonly DependencyProperty HandlesSpecialKeysProperty = DependencyProperty.Register("HandlesSpecialKeys", typeof(bool), typeof(UpDownBase<T>), new FrameworkPropertyMetadata(true));

        #endregion // Dependencies.

        #region Members

        /// <summary>
        /// Name constant for control template part.
        /// </summary>
        internal const string PART_TextBox = "PART_TextBox";
        internal const string PART_Spinner = "PART_Spinner";

        internal bool _isTextChangedFromUI;

        /// <summary>
        /// Flags if the Text and Value properties are in the process of being sync'd
        /// </summary>
        private bool _isSyncingTextAndValueProperties;
        private bool _internalValueSet;

        #endregion //Members

        #region Properties

        protected ASpinner Spinner
        {
            get;
            private set;
        }
        protected TextBox TextBox
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the strategy to redirect the focus.
        /// </summary>
        public FocusNavigationDirection FocusNavigationDirection
        {
            get
            {
                return (FocusNavigationDirection)GetValue(FocusNavigationDirectionProperty);
            }
            set
            {
                SetValue(FocusNavigationDirectionProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the flag indicating if the enter, tab and cancel keys must be handled by the control.
        /// </summary>
        public bool HandlesSpecialKeys
        {
            get
            {
                return (bool)GetValue(HandlesSpecialKeysProperty);
            }
            set
            {
                SetValue(HandlesSpecialKeysProperty, value);
            }
        }

        #region AllowSpin

        public static readonly DependencyProperty AllowSpinProperty = DependencyProperty.Register("AllowSpin", typeof(bool), typeof(UpDownBase<T>), new UIPropertyMetadata(true));
        public bool AllowSpin
        {
            get
            {
                return (bool)GetValue(AllowSpinProperty);
            }
            set
            {
                SetValue(AllowSpinProperty, value);
            }
        }

        #endregion //AllowSpin

        #region ButtonSpinnerLocation

        public static readonly DependencyProperty ButtonSpinnerLocationProperty = DependencyProperty.Register("ButtonSpinnerLocation", typeof(Location), typeof(UpDownBase<T>), new UIPropertyMetadata(Location.Right));
        public Location ButtonSpinnerLocation
        {
            get
            {
                return (Location)GetValue(ButtonSpinnerLocationProperty);
            }
            set
            {
                SetValue(ButtonSpinnerLocationProperty, value);
            }
        }

        #endregion //ButtonSpinnerLocation

        #region DisplayDefaultValueOnEmptyText

        public static readonly DependencyProperty DisplayDefaultValueOnEmptyTextProperty = DependencyProperty.Register("DisplayDefaultValueOnEmptyText", typeof(bool), typeof(UpDownBase<T>), new UIPropertyMetadata(false, OnDisplayDefaultValueOnEmptyTextChanged));
        public bool DisplayDefaultValueOnEmptyText
        {
            get
            {
                return (bool)GetValue(DisplayDefaultValueOnEmptyTextProperty);
            }
            set
            {
                SetValue(DisplayDefaultValueOnEmptyTextProperty, value);
            }
        }

        private static void OnDisplayDefaultValueOnEmptyTextChanged(DependencyObject source, DependencyPropertyChangedEventArgs args)
        {
            ((UpDownBase<T>)source).OnDisplayDefaultValueOnEmptyTextChanged((bool)args.OldValue, (bool)args.NewValue);
        }

        private void OnDisplayDefaultValueOnEmptyTextChanged(bool oldValue, bool newValue)
        {
            if (this.IsInitialized && string.IsNullOrEmpty(Text))
            {
                this.SyncTextAndValueProperties(false, Text);
            }
        }

        #endregion //DisplayDefaultValueOnEmptyText

        #region DefaultValue

        public static readonly DependencyProperty DefaultValueProperty = DependencyProperty.Register("DefaultValue", typeof(T), typeof(UpDownBase<T>), new UIPropertyMetadata(default(T), OnDefaultValueChanged));
        public T DefaultValue
        {
            get
            {
                return (T)GetValue(DefaultValueProperty);
            }
            set
            {
                SetValue(DefaultValueProperty, value);
            }
        }

        private static void OnDefaultValueChanged(DependencyObject source, DependencyPropertyChangedEventArgs args)
        {
            ((UpDownBase<T>)source).OnDefaultValueChanged((T)args.OldValue, (T)args.NewValue);
        }

        private void OnDefaultValueChanged(T oldValue, T newValue)
        {
            if (this.IsInitialized && string.IsNullOrEmpty(Text))
            {
                this.SyncTextAndValueProperties(true, Text);
            }
        }

        #endregion //DefaultValue

        #region Maximum

        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register("Maximum", typeof(T), typeof(UpDownBase<T>), new UIPropertyMetadata(default(T), OnMaximumChanged, OnCoerceMaximum));
        public T Maximum
        {
            get
            {
                return (T)GetValue(MaximumProperty);
            }
            set
            {
                SetValue(MaximumProperty, value);
            }
        }

        private static void OnMaximumChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            UpDownBase<T> upDown = o as UpDownBase<T>;
            if (upDown != null)
                upDown.OnMaximumChanged((T)e.OldValue, (T)e.NewValue);
        }

        protected virtual void OnMaximumChanged(T oldValue, T newValue)
        {
            if (this.IsInitialized)
            {
                this.SetValidSpinDirection();
            }
        }

        private static object OnCoerceMaximum(DependencyObject d, object baseValue)
        {
            UpDownBase<T> upDown = d as UpDownBase<T>;
            if (upDown != null)
                return upDown.OnCoerceMaximum((T)baseValue);

            return baseValue;
        }

        protected virtual T OnCoerceMaximum(T baseValue)
        {
            return baseValue;
        }

        #endregion //Maximum

        #region Minimum

        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register("Minimum", typeof(T), typeof(UpDownBase<T>), new UIPropertyMetadata(default(T), OnMinimumChanged, OnCoerceMinimum));
        public T Minimum
        {
            get
            {
                return (T)GetValue(MinimumProperty);
            }
            set
            {
                SetValue(MinimumProperty, value);
            }
        }

        private static void OnMinimumChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            UpDownBase<T> upDown = o as UpDownBase<T>;
            if (upDown != null)
                upDown.OnMinimumChanged((T)e.OldValue, (T)e.NewValue);
        }

        protected virtual void OnMinimumChanged(T oldValue, T newValue)
        {
            if (this.IsInitialized)
            {
                this.SetValidSpinDirection();
            }
        }

        private static object OnCoerceMinimum(DependencyObject d, object baseValue)
        {
            UpDownBase<T> upDown = d as UpDownBase<T>;
            if (upDown != null)
                return upDown.OnCoerceMinimum((T)baseValue);

            return baseValue;
        }

        protected virtual T OnCoerceMinimum(T baseValue)
        {
            return baseValue;
        }

        #endregion //Minimum

        #region MouseWheelActiveTrigger

        /// <summary>
        /// Identifies the MouseWheelActiveTrigger dependency property
        /// </summary>
        public static readonly DependencyProperty MouseWheelActiveTriggerProperty = DependencyProperty.Register("MouseWheelActiveTrigger", typeof(MouseWheelActiveTrigger), typeof(UpDownBase<T>), new UIPropertyMetadata(MouseWheelActiveTrigger.FocusedMouseOver));

        /// <summary>
        /// Get or set when the mouse wheel event should affect the value.
        /// </summary>
        public MouseWheelActiveTrigger MouseWheelActiveTrigger
        {
            get
            {
                return (MouseWheelActiveTrigger)GetValue(MouseWheelActiveTriggerProperty);
            }
            set
            {
                SetValue(MouseWheelActiveTriggerProperty, value);
            }
        }

        #endregion //MouseWheelActiveTrigger

        #region MouseWheelActiveOnFocus

        [Obsolete("Use MouseWheelActiveTrigger property instead")]
        public static readonly DependencyProperty MouseWheelActiveOnFocusProperty = DependencyProperty.Register("MouseWheelActiveOnFocus", typeof(bool), typeof(UpDownBase<T>), new UIPropertyMetadata(true, OnMouseWheelActiveOnFocusChanged));

        [Obsolete("Use MouseWheelActiveTrigger property instead")]
        public bool MouseWheelActiveOnFocus
        {
            get
            {
#pragma warning disable 618
                return (bool)GetValue(MouseWheelActiveOnFocusProperty);
#pragma warning restore 618
            }
            set
            {
#pragma warning disable 618
                SetValue(MouseWheelActiveOnFocusProperty, value);
#pragma warning restore 618
            }
        }

        private static void OnMouseWheelActiveOnFocusChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            UpDownBase<T> upDownBase = o as UpDownBase<T>;
            if (upDownBase != null)
                upDownBase.MouseWheelActiveTrigger = ((bool)e.NewValue)
                  ? MouseWheelActiveTrigger.FocusedMouseOver
                  : MouseWheelActiveTrigger.MouseOver;
        }

        #endregion //MouseWheelActiveOnFocus

        #region ShowButtonSpinner

        public static readonly DependencyProperty ShowButtonSpinnerProperty = DependencyProperty.Register("ShowButtonSpinner", typeof(bool), typeof(UpDownBase<T>), new UIPropertyMetadata(true));
        public bool ShowButtonSpinner
        {
            get
            {
                return (bool)GetValue(ShowButtonSpinnerProperty);
            }
            set
            {
                SetValue(ShowButtonSpinnerProperty, value);
            }
        }

        #endregion //ShowButtonSpinner

        #region UpdateValueOnEnterKey

        public static readonly DependencyProperty UpdateValueOnEnterKeyProperty = DependencyProperty.Register("UpdateValueOnEnterKey", typeof(bool), typeof(UpDownBase<T>), new FrameworkPropertyMetadata(false));
        public bool UpdateValueOnEnterKey
        {
            get
            {
                return (bool)GetValue(UpdateValueOnEnterKeyProperty);
            }
            set
            {
                SetValue(UpdateValueOnEnterKeyProperty, value);
            }
        }

        #endregion //UpdateValueOnEnterKey

        #region Value

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(T), typeof(UpDownBase<T>), new FrameworkPropertyMetadata(default(T), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnValueChanged, OnCoerceValue, false, UpdateSourceTrigger.PropertyChanged));
        public T Value
        {
            get
            {
                return (T)GetValue(ValueProperty);
            }
            set
            {
                SetValue(ValueProperty, value);
            }
        }

        private void SetValueInternal(T value)
        {
            _internalValueSet = true;
            try
            {
                this.Value = value;
            }
            finally
            {
                _internalValueSet = false;
            }
        }

        /// <summary>
        /// Delegate called to coearce the value.
        /// </summary>
        /// <param name="pObject">The used control.</param>
        /// <param name="pBasevalue">The value to coerce.</param>
        /// <returns>The coerced value.</returns>
        private static object OnCoerceValue(DependencyObject pObject, object pBasevalue)
        {
            UpDownBase<T> lUpDownBase = pObject as UpDownBase<T>;
            if (lUpDownBase != null)
            {
                return lUpDownBase.OnCoerceValue((T)pBasevalue);
            }

            return pBasevalue;
        }

        /// <summary>
        /// Delegate called to coearce the value.
        /// </summary>
        /// <param name="pNewValue">The value to coerce.</param>
        /// <returns>The coerced value.</returns>
        protected virtual T OnCoerceValue(T pNewValue)
        {
            return pNewValue;
        }

        private static void OnValueChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            UpDownBase<T> upDownBase = o as UpDownBase<T>;
            if (upDownBase != null)
                upDownBase.OnValueChanged((T)e.OldValue, (T)e.NewValue);
        }

        protected virtual void OnValueChanged(T oldValue, T newValue)
        {
            if (!_internalValueSet && this.IsInitialized)
            {
                SyncTextAndValueProperties(false, null, true);
            }

            SetValidSpinDirection();

            this.RaiseValueChangedEvent(oldValue, newValue);
        }

        #endregion //Value

        #endregion //Properties

        #region Constructors

        internal UpDownBase()
        {
            this.AddHandler(Mouse.PreviewMouseDownOutsideCapturedElementEvent, new RoutedEventHandler(this.HandleClickOutsideOfControlWithMouseCapture), true);
        }

        #endregion //Constructors

        #region Base Class Overrides

        protected override void OnAccessKey(AccessKeyEventArgs e)
        {
            if (TextBox != null)
                TextBox.Focus();

            base.OnAccessKey(e);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (TextBox != null)
            {
                TextBox.LostFocus -= this.TextBox_LostFocus;
                TextBox.TextChanged -= this.TextBox_TextChanged;
                TextBox.GotFocus -= this.TextBox_GotFocus;
                TextBox.RemoveHandler(Mouse.PreviewMouseDownEvent, new MouseButtonEventHandler(this.TextBox_PreviewMouseDown));
            }

            TextBox = GetTemplateChild(PART_TextBox) as TextBox;

            if (TextBox != null)
            {
                TextBox.Text = Text;
                TextBox.LostFocus += this.TextBox_LostFocus;
                TextBox.TextChanged += this.TextBox_TextChanged;
                TextBox.GotFocus += this.TextBox_GotFocus;
                TextBox.AddHandler(Mouse.PreviewMouseDownEvent, new MouseButtonEventHandler(this.TextBox_PreviewMouseDown), true);
            }

            if (Spinner != null)
                Spinner.Spin -= OnSpinnerSpin;

            Spinner = GetTemplateChild(PART_Spinner) as ASpinner;

            if (Spinner != null)
                Spinner.Spin += OnSpinnerSpin;

            SetValidSpinDirection();
        }

        /// <summary>
        /// Handles the GotFocus event of the TextBox control.
        /// </summary>
        /// <param name="pSender">The source of the event.</param>
        /// <param name="pRoutedEventArgs">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void TextBox_GotFocus(object pSender, RoutedEventArgs pRoutedEventArgs)
        {
            this.DisplayRawValueAsText();

            //Selectthe whole text when the TB gets the focus
            if (this.TextBox != null && string.IsNullOrEmpty(this.TextBox.Text) == false)
            {
                this.TextBox.SelectionStart = 0;
                this.TextBox.SelectionLength = this.TextBox.Text.Length; 
            }
        }

        /// <summary>
        /// Displays the raw value as text in the text box.
        /// </summary>
        protected abstract void CustomDisplayRawValueAsText();

        /// <summary>
        /// Displays the raw value as text in the text box.
        /// </summary>
        private void DisplayRawValueAsText()
        {
            this._isSyncingTextAndValueProperties = true;
            this.CustomDisplayRawValueAsText();
            this._isSyncingTextAndValueProperties = false;
        }


        protected override void OnPreviewKeyDown(KeyEventArgs pEventArgs)
        {
            if (this.HandlesSpecialKeys)
            {
                switch (pEventArgs.Key)
                {
                    case Key.Enter:
                    case Key.Tab:
                        {
                            if (this.CommitInput())
                            {
                                pEventArgs.Handled = this.MoveFocus();
                            }
                            break;
                        }
                    case Key.Escape:
                        {
                            bool lCancelSuccess = this.CancelInput();
                            pEventArgs.Handled = lCancelSuccess;
                            break;
                        }
                }
            }
        }

        private bool CanMoveFocus(FocusNavigationDirection pDirection)
        {
            QueryMoveFocusEventArgs lEventArgs = new QueryMoveFocusEventArgs(pDirection, false);
            this.RaiseEvent(lEventArgs);
            return lEventArgs.CanMoveFocus;
        }

        private bool MoveFocus()
        {
            if (this.CanMoveFocus(this.FocusNavigationDirection))
            {
                // Using the explicit navigation description.
                bool lResult = this.TextBox.MoveFocus(new TraversalRequest(this.FocusNavigationDirection));
                if (lResult == false)
                {
                    // Using the default one.
                    lResult = this.TextBox.MoveFocus(new TraversalRequest(System.Windows.Input.FocusNavigationDirection.Next));
                }

                return lResult;
            }

            return false;
        }


        protected override void OnTextChanged(string oldValue, string newValue)
        {
            base.OnTextChanged(oldValue, newValue);

            if (this.IsInitialized)
            {
                if (this.UpdateValueOnEnterKey == false)
                {
                    SyncTextAndValueProperties(true, Text);
                }

                if (this.IsKeyboardFocusWithin == false)
                {
                    // Focus is not on the control. Text property has been set from the code behind and not using the GUI.
                    SyncTextAndValueProperties(true, Text);
                }
            }
        }

        protected override void OnCultureInfoChanged(CultureInfo oldValue, CultureInfo newValue)
        {
            if (IsInitialized)
            {
                SyncTextAndValueProperties(false, null);
            }
        }

        protected override void OnReadOnlyChanged(bool oldValue, bool newValue)
        {
            SetValidSpinDirection();
        }

        #endregion //Base Class Overrides

        #region Event Handlers

        private void TextBox_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            if (this.MouseWheelActiveTrigger == Primitives.MouseWheelActiveTrigger.Focused)
            {
                //Capture the spinner when user clicks on the control.
                if (Mouse.Captured != this.Spinner)
                {
                    //Delay the capture to let the DateTimeUpDown select a new DateTime part.
                    this.Dispatcher.BeginInvoke(DispatcherPriority.Input, new Action(() =>
                    {
                        Mouse.Capture(this.Spinner);
                    }
                    ));
                }
            }
        }

        private void HandleClickOutsideOfControlWithMouseCapture(object sender, RoutedEventArgs e)
        {
            if (Mouse.Captured is ASpinner)
            {
                //Release the captured spinner when user clicks away from spinner.
                this.Spinner.ReleaseMouseCapture();
            }
        }

        private void OnSpinnerSpin(object sender, SpinEventArgs e)
        {
            if (AllowSpin && !IsReadOnly)
            {
                var activeTrigger = this.MouseWheelActiveTrigger;
                bool spin = !e.UsingMouseWheel;
                spin |= (activeTrigger == MouseWheelActiveTrigger.MouseOver);
                spin |= (TextBox.IsFocused && (activeTrigger == MouseWheelActiveTrigger.FocusedMouseOver));
                spin |= (TextBox.IsFocused && (activeTrigger == MouseWheelActiveTrigger.Focused) && (Mouse.Captured is ASpinner));

                if (spin)
                {
                    OnSpin(e);
                }
            }
        }

        #endregion //Event Handlers

        #region Events

        public event InputValidationErrorEventHandler InputValidationError;

        #region ValueChanged Event

        //Due to a bug in Visual Studio, you cannot create event handlers for generic T args in XAML, so I have to use object instead.
        public static readonly RoutedEvent ValueChangedEvent = EventManager.RegisterRoutedEvent("ValueChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<object>), typeof(UpDownBase<T>));
        public event RoutedPropertyChangedEventHandler<object> ValueChanged
        {
            add
            {
                AddHandler(ValueChangedEvent, value);
            }
            remove
            {
                RemoveHandler(ValueChangedEvent, value);
            }
        }

        #endregion

        #endregion //Events

        #region Methods

        protected virtual void OnSpin(SpinEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException("e");
            }

            if (e.Direction == SpinDirection.Increase)
            {
                this.DoIncrement();
            }
            else
            {
                this.DoDecrement();
            }
        }

        /// <summary>
        /// Raises the value changed event.
        /// </summary>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        protected virtual void RaiseValueChangedEvent(T oldValue, T newValue)
        {
            RoutedPropertyChangedEventArgs<object> args = new RoutedPropertyChangedEventArgs<object>(oldValue, newValue);
            args.RoutedEvent = ValueChangedEvent;
            RaiseEvent(args);
        }

        /// <summary>
        /// Déclenche l'événement <see cref="E:System.Windows.FrameworkElement.Initialized" />.Cette méthode est appelée chaque fois qu'<see cref="P:System.Windows.FrameworkElement.IsInitialized" /> a la valeur true  en interne.
        /// </summary>
        /// <param name="e"><see cref="T:System.Windows.RoutedEventArgs" /> qui contient les données d'événement.</param>
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            // When both Value and Text are initialized, Value has priority.
            // To be sure that the value is not initialized, it should
            // have no local value, no binding, and equal to the default value.
            bool updateValueFromText =
              (this.ReadLocalValue(ValueProperty) == DependencyProperty.UnsetValue)
              && (BindingOperations.GetBinding(this, ValueProperty) == null)
              && (object.Equals(this.Value, ValueProperty.DefaultMetadata.DefaultValue));

            this.SyncTextAndValueProperties(updateValueFromText, Text, !updateValueFromText);
        }

        /// <summary>
        /// Performs an increment if conditions allow it.
        /// </summary>
        internal void DoDecrement()
        {
            if (Spinner == null || (Spinner.ValidSpinDirections & ValidSpinDirections.Decrease) == ValidSpinDirections.Decrease)
            {
                OnDecrement();
            }
        }

        /// <summary>
        /// Performs a decrement if conditions allow it.
        /// </summary>
        internal void DoIncrement()
        {
            if (Spinner == null || (Spinner.ValidSpinDirections & ValidSpinDirections.Increase) == ValidSpinDirections.Increase)
            {
                OnIncrement();
            }
        }

        /// <summary>
        /// Handles the TextChanged event of the TextBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TextChangedEventArgs"/> instance containing the event data.</param>
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!this._isSyncingTextAndValueProperties)
            {
                if (this.TextBox != null && this.TextBox.Text.StartsWith(Constants.APPROXIMATION_SYMBOL))
                {
                    this.DisplayRawValueAsText();
                }
 
                if (!this.IsKeyboardFocusWithin)
                    return;

                if (this.UpdateValueOnEnterKey == false)
                {
                    this.TryUpdateTextProperty();
                } 
            }
        }

        private void TryUpdateTextProperty()
        {
            try
            {
                if (this.TextBox != null)
                {
                    _isTextChangedFromUI = true;
                    Text = this.TextBox.Text;
                }
            }
            finally
            {
                _isTextChangedFromUI = false;
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (this.UpdateValueOnEnterKey == false)
            {
                CommitInput();
            }
        }

        protected override void OnIsKeyboardFocusWithinChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnIsKeyboardFocusWithinChanged(e);

            bool lNewValue = Convert.ToBoolean(e.NewValue);
            if (lNewValue == false)
            {
                this.CommitInput();
            }
        }

        private void RaiseInputValidationError(Exception e)
        {
            if (InputValidationError != null)
            {
                InputValidationErrorEventArgs args = new InputValidationErrorEventArgs(e);
                InputValidationError(this, args);
                if (args.ThrowException)
                {
                    throw args.Exception;
                }
            }
        }

        public virtual bool CommitInput()
        {
            //Nothing to commit if the text has not changed
            string lText = ConvertValueToText();

            // Ensuring the good decimal separator is used.
            lText = lText.Replace(",", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator);

            if (this.Text == lText && this.TextBox.Text == lText)
            {
                return true;
            }

            if (this.UpdateValueOnEnterKey)
            {
                this.TryUpdateTextProperty();
            }

            return this.SyncTextAndValueProperties(true, Text);
        }

        public virtual bool CancelInput()
        {
            return this.SyncTextAndValueProperties(false, Text);
        }

        protected bool SyncTextAndValueProperties(bool updateValueFromText, string text)
        {
            return this.SyncTextAndValueProperties(updateValueFromText, text, false);
        }

        protected bool SyncTextAndValueProperties(bool updateValueFromText, string text, bool forceTextUpdate)
        {
            if (_isSyncingTextAndValueProperties)
                return true;

            _isSyncingTextAndValueProperties = true;
            bool parsedTextIsValid = true;
            try
            {
                if (updateValueFromText)
                {
                    if (string.IsNullOrEmpty(text))
                    {
                        // An empty input sets the value to the default value.
                        this.SetValueInternal(this.DefaultValue);
                    }
                    else
                    {
                        try
                        {
                            T newValue = this.ConvertTextToValue(text);
                            if (!object.Equals(newValue, this.Value))
                            {
                                this.SetValueInternal(newValue);
                            }
                        }
                        catch (Exception e)
                        {
                            parsedTextIsValid = false;

                            // From the UI, just allow any input.
                            if (!_isTextChangedFromUI)
                            {
                                // This call may throw an exception. 
                                // See RaiseInputValidationError() implementation.
                                this.RaiseInputValidationError(e);
                            }
                        }
                    }
                }

                // Do not touch the ongoing text input from user.
                if (!_isTextChangedFromUI)
                {
                    // Don't replace the empty Text with the non-empty representation of DefaultValue.
                    bool shouldKeepEmpty = !forceTextUpdate && string.IsNullOrEmpty(Text) && object.Equals(Value, DefaultValue) && !this.DisplayDefaultValueOnEmptyText;
                    if (!shouldKeepEmpty)
                    {
                        string newText = ConvertValueToText();
                        if (!object.Equals(this.Text, newText))
                        {
                            Text = newText;
                        }
                    }

                    // Sync Text and textBox
                    if (TextBox != null)
                        TextBox.Text = Text;
                }

                if (_isTextChangedFromUI && !parsedTextIsValid)
                {
                    // Text input was made from the user and the text
                    // repesents an invalid value. Disable the spinner
                    // in this case.
                    if (Spinner != null)
                    {
                        Spinner.ValidSpinDirections = ValidSpinDirections.None;
                    }
                }
                else
                {
                    this.SetValidSpinDirection();
                }
            }
            finally
            {
                _isSyncingTextAndValueProperties = false;
            }
            return parsedTextIsValid;
        }

        protected string ConvertValueToText()
        {
            return this.ConvertValueToText(this.Value);
        }

        protected void SetValidSpinDirection()
        {
            // Nothing to do as disabling an up down button will result in loosing the focus...
            // this.SetValidSpinDirection(this.Value);
        }

        #region Abstract

        /// <summary>
        /// Converts the formatted text to a value.
        /// </summary>
        protected abstract T ConvertTextToValue(string text);

        /// <summary>
        /// Converts the value to formatted text.
        /// </summary>
        /// <returns></returns>
        protected abstract string ConvertValueToText(T pValue);

        /// <summary>
        /// Converts the raw value to text (not formatted).
        /// </summary>
        /// <param name="pValue">The value.</param>
        /// <returns></returns>
        protected abstract string ConvertRawValueToText(T pValue);

        /// <summary>
        /// Called by OnSpin when the spin direction is SpinDirection.Increase.
        /// </summary>
        protected abstract void OnIncrement();

        /// <summary>
        /// Called by OnSpin when the spin direction is SpinDirection.Descrease.
        /// </summary>
        protected abstract void OnDecrement();

        /// <summary>
        /// Sets the valid spin directions.
        /// </summary>
        protected abstract void SetValidSpinDirection(T pValue);

        #endregion //Abstract

        #endregion //Methods
    }
}

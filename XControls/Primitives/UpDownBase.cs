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
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="XControls.Primitives.InputBase" />
    /// <seealso cref="XControls.Core.Input.IValidateInput" />
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
        /// <summary>
        /// The part spinner
        /// </summary>
        internal const string PART_Spinner = "PART_Spinner";

        /// <summary>
        /// The is text changed from UI
        /// </summary>
        internal bool _isTextChangedFromUI;

        /// <summary>
        /// Flags if the Text and Value properties are in the process of being sync'd
        /// </summary>
        private bool _isSyncingTextAndValueProperties;
        /// <summary>
        /// The internal value set
        /// </summary>
        private bool _internalValueSet;

        #endregion //Members

        #region Properties

        /// <summary>
        /// Gets the spinner.
        /// </summary>
        /// <value>
        /// The spinner.
        /// </value>
        protected ASpinner Spinner
        {
            get;
            private set;
        }
        /// <summary>
        /// Gets the text box.
        /// </summary>
        /// <value>
        /// The text box.
        /// </value>
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
                return (FocusNavigationDirection) this.GetValue(FocusNavigationDirectionProperty);
            }
            set
            {
                this.SetValue(FocusNavigationDirectionProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the flag indicating if the enter, tab and cancel keys must be handled by the control.
        /// </summary>
        public bool HandlesSpecialKeys
        {
            get
            {
                return (bool) this.GetValue(HandlesSpecialKeysProperty);
            }
            set
            {
                this.SetValue(HandlesSpecialKeysProperty, value);
            }
        }

        #region AllowSpin

        /// <summary>
        /// The allow spin property
        /// </summary>
        public static readonly DependencyProperty AllowSpinProperty = DependencyProperty.Register("AllowSpin", typeof(bool), typeof(UpDownBase<T>), new UIPropertyMetadata(true));
        /// <summary>
        /// Gets or sets a value indicating whether [allow spin].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [allow spin]; otherwise, <c>false</c>.
        /// </value>
        public bool AllowSpin
        {
            get
            {
                return (bool) this.GetValue(AllowSpinProperty);
            }
            set
            {
                this.SetValue(AllowSpinProperty, value);
            }
        }

        #endregion //AllowSpin

        #region ButtonSpinnerLocation

        /// <summary>
        /// The button spinner location property
        /// </summary>
        public static readonly DependencyProperty ButtonSpinnerLocationProperty = DependencyProperty.Register("ButtonSpinnerLocation", typeof(Location), typeof(UpDownBase<T>), new UIPropertyMetadata(Location.Right));
        /// <summary>
        /// Gets or sets the button spinner location.
        /// </summary>
        /// <value>
        /// The button spinner location.
        /// </value>
        public Location ButtonSpinnerLocation
        {
            get
            {
                return (Location) this.GetValue(ButtonSpinnerLocationProperty);
            }
            set
            {
                this.SetValue(ButtonSpinnerLocationProperty, value);
            }
        }

        #endregion //ButtonSpinnerLocation

        #region DisplayDefaultValueOnEmptyText

        /// <summary>
        /// The display default value on empty text property
        /// </summary>
        public static readonly DependencyProperty DisplayDefaultValueOnEmptyTextProperty = DependencyProperty.Register("DisplayDefaultValueOnEmptyText", typeof(bool), typeof(UpDownBase<T>), new UIPropertyMetadata(false, OnDisplayDefaultValueOnEmptyTextChanged));
        /// <summary>
        /// Gets or sets a value indicating whether [display default value on empty text].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [display default value on empty text]; otherwise, <c>false</c>.
        /// </value>
        public bool DisplayDefaultValueOnEmptyText
        {
            get
            {
                return (bool) this.GetValue(DisplayDefaultValueOnEmptyTextProperty);
            }
            set
            {
                this.SetValue(DisplayDefaultValueOnEmptyTextProperty, value);
            }
        }

        /// <summary>
        /// Called when [display default value on empty text changed].
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnDisplayDefaultValueOnEmptyTextChanged(DependencyObject source, DependencyPropertyChangedEventArgs args)
        {
            ((UpDownBase<T>)source).OnDisplayDefaultValueOnEmptyTextChanged((bool)args.OldValue, (bool)args.NewValue);
        }

        /// <summary>
        /// Called when [display default value on empty text changed].
        /// </summary>
        /// <param name="oldValue">if set to <c>true</c> [old value].</param>
        /// <param name="newValue">if set to <c>true</c> [new value].</param>
        private void OnDisplayDefaultValueOnEmptyTextChanged(bool oldValue, bool newValue)
        {
            if (this.IsInitialized && string.IsNullOrEmpty(this.Text))
            {
                this.SyncTextAndValueProperties(false, this.Text);
            }
        }

        #endregion //DisplayDefaultValueOnEmptyText

        #region DefaultValue

        /// <summary>
        /// The default value property
        /// </summary>
        public static readonly DependencyProperty DefaultValueProperty = DependencyProperty.Register("DefaultValue", typeof(T), typeof(UpDownBase<T>), new UIPropertyMetadata(default(T), OnDefaultValueChanged));
        /// <summary>
        /// Gets or sets the default value.
        /// </summary>
        /// <value>
        /// The default value.
        /// </value>
        public T DefaultValue
        {
            get
            {
                return (T) this.GetValue(DefaultValueProperty);
            }
            set
            {
                this.SetValue(DefaultValueProperty, value);
            }
        }

        /// <summary>
        /// Called when [default value changed].
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnDefaultValueChanged(DependencyObject source, DependencyPropertyChangedEventArgs args)
        {
            ((UpDownBase<T>)source).OnDefaultValueChanged((T)args.OldValue, (T)args.NewValue);
        }

        /// <summary>
        /// Called when [default value changed].
        /// </summary>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private void OnDefaultValueChanged(T oldValue, T newValue)
        {
            if (this.IsInitialized && string.IsNullOrEmpty(this.Text))
            {
                this.SyncTextAndValueProperties(true, this.Text);
            }
        }

        #endregion //DefaultValue

        #region Maximum

        /// <summary>
        /// The maximum property
        /// </summary>
        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register("Maximum", typeof(T), typeof(UpDownBase<T>), new UIPropertyMetadata(default(T), OnMaximumChanged, OnCoerceMaximum));
        /// <summary>
        /// Gets or sets the maximum.
        /// </summary>
        /// <value>
        /// The maximum.
        /// </value>
        public T Maximum
        {
            get
            {
                return (T) this.GetValue(MaximumProperty);
            }
            set
            {
                this.SetValue(MaximumProperty, value);
            }
        }

        /// <summary>
        /// Called when [maximum changed].
        /// </summary>
        /// <param name="o">The o.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        private static void OnMaximumChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            UpDownBase<T> upDown = o as UpDownBase<T>;
            if (upDown != null)
                upDown.OnMaximumChanged((T)e.OldValue, (T)e.NewValue);
        }

        /// <summary>
        /// Called when [maximum changed].
        /// </summary>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        protected virtual void OnMaximumChanged(T oldValue, T newValue)
        {
            if (this.IsInitialized)
            {
                this.SetValidSpinDirection();
            }
        }

        /// <summary>
        /// Called when [coerce maximum].
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="baseValue">The base value.</param>
        /// <returns></returns>
        private static object OnCoerceMaximum(DependencyObject d, object baseValue)
        {
            UpDownBase<T> upDown = d as UpDownBase<T>;
            if (upDown != null)
                return upDown.OnCoerceMaximum((T)baseValue);

            return baseValue;
        }

        /// <summary>
        /// Called when [coerce maximum].
        /// </summary>
        /// <param name="baseValue">The base value.</param>
        /// <returns></returns>
        protected virtual T OnCoerceMaximum(T baseValue)
        {
            return baseValue;
        }

        #endregion //Maximum

        #region Minimum

        /// <summary>
        /// The minimum property
        /// </summary>
        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register("Minimum", typeof(T), typeof(UpDownBase<T>), new UIPropertyMetadata(default(T), OnMinimumChanged, OnCoerceMinimum));
        /// <summary>
        /// Gets or sets the minimum.
        /// </summary>
        /// <value>
        /// The minimum.
        /// </value>
        public T Minimum
        {
            get
            {
                return (T) this.GetValue(MinimumProperty);
            }
            set
            {
                this.SetValue(MinimumProperty, value);
            }
        }

        /// <summary>
        /// Called when [minimum changed].
        /// </summary>
        /// <param name="o">The o.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnMinimumChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            UpDownBase<T> upDown = o as UpDownBase<T>;
            if (upDown != null)
                upDown.OnMinimumChanged((T)e.OldValue, (T)e.NewValue);
        }

        /// <summary>
        /// Called when [minimum changed].
        /// </summary>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        protected virtual void OnMinimumChanged(T oldValue, T newValue)
        {
            if (this.IsInitialized)
            {
                this.SetValidSpinDirection();
            }
        }

        /// <summary>
        /// Called when [coerce minimum].
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="baseValue">The base value.</param>
        /// <returns></returns>
        private static object OnCoerceMinimum(DependencyObject d, object baseValue)
        {
            UpDownBase<T> upDown = d as UpDownBase<T>;
            if (upDown != null)
                return upDown.OnCoerceMinimum((T)baseValue);

            return baseValue;
        }

        /// <summary>
        /// Called when [coerce minimum].
        /// </summary>
        /// <param name="baseValue">The base value.</param>
        /// <returns></returns>
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
                return (MouseWheelActiveTrigger) this.GetValue(MouseWheelActiveTriggerProperty);
            }
            set
            {
                this.SetValue(MouseWheelActiveTriggerProperty, value);
            }
        }

        #endregion //MouseWheelActiveTrigger

        #region MouseWheelActiveOnFocus

        /// <summary>
        /// The mouse wheel active on focus property
        /// </summary>
        [Obsolete("Use MouseWheelActiveTrigger property instead")]
        public static readonly DependencyProperty MouseWheelActiveOnFocusProperty = DependencyProperty.Register("MouseWheelActiveOnFocus", typeof(bool), typeof(UpDownBase<T>), new UIPropertyMetadata(true, OnMouseWheelActiveOnFocusChanged));

        /// <summary>
        /// Gets or sets a value indicating whether [mouse wheel active on focus].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [mouse wheel active on focus]; otherwise, <c>false</c>.
        /// </value>
        [Obsolete("Use MouseWheelActiveTrigger property instead")]
        public bool MouseWheelActiveOnFocus
        {
            get
            {
#pragma warning disable 618
                return (bool) this.GetValue(MouseWheelActiveOnFocusProperty);
#pragma warning restore 618
            }
            set
            {
#pragma warning disable 618
                this.SetValue(MouseWheelActiveOnFocusProperty, value);
#pragma warning restore 618
            }
        }

        /// <summary>
        /// Called when [mouse wheel active on focus changed].
        /// </summary>
        /// <param name="o">The o.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
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

        /// <summary>
        /// The show button spinner property
        /// </summary>
        public static readonly DependencyProperty ShowButtonSpinnerProperty = DependencyProperty.Register("ShowButtonSpinner", typeof(bool), typeof(UpDownBase<T>), new UIPropertyMetadata(true));
        /// <summary>
        /// Gets or sets a value indicating whether [show button spinner].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [show button spinner]; otherwise, <c>false</c>.
        /// </value>
        public bool ShowButtonSpinner
        {
            get
            {
                return (bool) this.GetValue(ShowButtonSpinnerProperty);
            }
            set
            {
                this.SetValue(ShowButtonSpinnerProperty, value);
            }
        }

        #endregion //ShowButtonSpinner

        #region UpdateValueOnEnterKey

        /// <summary>
        /// The update value on enter key property
        /// </summary>
        public static readonly DependencyProperty UpdateValueOnEnterKeyProperty = DependencyProperty.Register("UpdateValueOnEnterKey", typeof(bool), typeof(UpDownBase<T>), new FrameworkPropertyMetadata(false));
        /// <summary>
        /// Gets or sets a value indicating whether [update value on enter key].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [update value on enter key]; otherwise, <c>false</c>.
        /// </value>
        public bool UpdateValueOnEnterKey
        {
            get
            {
                return (bool) this.GetValue(UpdateValueOnEnterKeyProperty);
            }
            set
            {
                this.SetValue(UpdateValueOnEnterKeyProperty, value);
            }
        }

        #endregion //UpdateValueOnEnterKey

        #region Value

        /// <summary>
        /// The value property
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(T), typeof(UpDownBase<T>), new FrameworkPropertyMetadata(default(T), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnValueChanged, OnCoerceValue, false, UpdateSourceTrigger.PropertyChanged));
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public T Value
        {
            get
            {
                return (T) this.GetValue(ValueProperty);
            }
            set
            {
                this.SetValue(ValueProperty, value);
            }
        }

        private void SetValueInternal(T value)
        {
            this._internalValueSet = true;
            try
            {
                this.Value = value;
            }
            finally
            {
                this._internalValueSet = false;
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

        /// <summary>
        /// Called when [value changed].
        /// </summary>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        protected virtual void OnValueChanged(T oldValue, T newValue)
        {
            if (!this._internalValueSet && this.IsInitialized)
            {
                this.SyncTextAndValueProperties(false, null, true);
            }

            this.SetValidSpinDirection();

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

        /// <summary>
        /// Fournit la gestion de classe pour les cas où une touche d'accès rapide explicite pour cet élément est appelée.
        /// </summary>
        /// <param name="e">Données de l'événement de touche d'accès rapide.Les données d'événement signalent la touche qui a été appelée et indiquent si l'objet <see cref="T:System.Windows.Input.AccessKeyManager" /> qui contrôle que l'envoi de ces événements a également envoyé cet appel de touche d'accès rapide à d'autres éléments.</param>
        protected override void OnAccessKey(AccessKeyEventArgs e)
        {
            if (this.TextBox != null) this.TextBox.Focus();

            base.OnAccessKey(e);
        }

        /// <summary>
        /// En cas de substitution dans une classe dérivée, appelé chaque fois que le code de l'application ou que des processus internes appellent <see cref="M:System.Windows.FrameworkElement.ApplyTemplate" />.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (this.TextBox != null)
            {
                this.TextBox.LostFocus -= this.TextBox_LostFocus;
                this.TextBox.TextChanged -= this.TextBox_TextChanged;
                this.TextBox.GotFocus -= this.TextBox_GotFocus;
                this.TextBox.RemoveHandler(Mouse.PreviewMouseDownEvent, new MouseButtonEventHandler(this.TextBox_PreviewMouseDown));
            }

            this.TextBox = this.GetTemplateChild(PART_TextBox) as TextBox;

            if (this.TextBox != null)
            {
                this.TextBox.Text = this.Text;
                this.TextBox.LostFocus += this.TextBox_LostFocus;
                this.TextBox.TextChanged += this.TextBox_TextChanged;
                this.TextBox.GotFocus += this.TextBox_GotFocus;
                this.TextBox.AddHandler(Mouse.PreviewMouseDownEvent, new MouseButtonEventHandler(this.TextBox_PreviewMouseDown), true);
            }

            if (this.Spinner != null) this.Spinner.Spin -= this.OnSpinnerSpin;

            this.Spinner = this.GetTemplateChild(PART_Spinner) as ASpinner;

            if (this.Spinner != null) this.Spinner.Spin += this.OnSpinnerSpin;

            this.SetValidSpinDirection();
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


        /// <summary>
        /// Raises the <see cref="E:PreviewKeyDown" /> event.
        /// </summary>
        /// <param name="pEventArgs">The <see cref="KeyEventArgs"/> instance containing the event data.</param>
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

        /// <summary>
        /// Determines whether this instance [can move focus] the specified p direction.
        /// </summary>
        /// <param name="pDirection">The p direction.</param>
        /// <returns>
        ///   <c>true</c> if this instance [can move focus] the specified p direction; otherwise, <c>false</c>.
        /// </returns>
        private bool CanMoveFocus(FocusNavigationDirection pDirection)
        {
            QueryMoveFocusEventArgs lEventArgs = new QueryMoveFocusEventArgs(pDirection, false);
            this.RaiseEvent(lEventArgs);
            return lEventArgs.CanMoveFocus;
        }

        /// <summary>
        /// Moves the focus.
        /// </summary>
        /// <returns></returns>
        private bool MoveFocus()
        {
            if (this.CanMoveFocus(this.FocusNavigationDirection))
            {
                // Using the explicit navigation description.
                bool lResult = this.TextBox.MoveFocus(new TraversalRequest(this.FocusNavigationDirection));
                if (lResult == false)
                {
                    // Using the default one.
                    lResult = this.TextBox.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                }

                return lResult;
            }

            return false;
        }


        /// <summary>
        /// Called when [text changed].
        /// </summary>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        protected override void OnTextChanged(string oldValue, string newValue)
        {
            base.OnTextChanged(oldValue, newValue);

            if (this.IsInitialized)
            {
                if (this.UpdateValueOnEnterKey == false)
                {
                    this.SyncTextAndValueProperties(true, this.Text);
                }

                if (this.IsKeyboardFocusWithin == false)
                {
                    // Focus is not on the control. Text property has been set from the code behind and not using the GUI.
                    this.SyncTextAndValueProperties(true, this.Text);
                }
            }
        }

        /// <summary>
        /// Called when [culture information changed].
        /// </summary>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        protected override void OnCultureInfoChanged(CultureInfo oldValue, CultureInfo newValue)
        {
            if (this.IsInitialized)
            {
                this.SyncTextAndValueProperties(false, null);
            }
        }

        /// <summary>
        /// Called when [read only changed].
        /// </summary>
        /// <param name="oldValue">if set to <c>true</c> [old value].</param>
        /// <param name="newValue">if set to <c>true</c> [new value].</param>
        protected override void OnReadOnlyChanged(bool oldValue, bool newValue)
        {
            this.SetValidSpinDirection();
        }

        #endregion //Base Class Overrides

        #region Event Handlers

        private void TextBox_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            if (this.MouseWheelActiveTrigger == MouseWheelActiveTrigger.Focused)
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

        /// <summary>
        /// Handles the click outside of control with mouse capture.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void HandleClickOutsideOfControlWithMouseCapture(object sender, RoutedEventArgs e)
        {
            if (Mouse.Captured is ASpinner)
            {
                //Release the captured spinner when user clicks away from spinner.
                this.Spinner.ReleaseMouseCapture();
            }
        }

        /// <summary>
        /// Called when [spinner spin].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="SpinEventArgs"/> instance containing the event data.</param>
        private void OnSpinnerSpin(object sender, SpinEventArgs e)
        {
            if (this.AllowSpin && !this.IsReadOnly)
            {
                var activeTrigger = this.MouseWheelActiveTrigger;
                bool spin = !e.UsingMouseWheel;
                spin |= (activeTrigger == MouseWheelActiveTrigger.MouseOver);
                spin |= (this.TextBox.IsFocused && (activeTrigger == MouseWheelActiveTrigger.FocusedMouseOver));
                spin |= (this.TextBox.IsFocused && (activeTrigger == MouseWheelActiveTrigger.Focused) && (Mouse.Captured is ASpinner));

                if (spin)
                {
                    this.OnSpin(e);
                }
            }
        }

        #endregion //Event Handlers

        #region Events

        /// <summary>
        /// Event raised when an error occured on validation.
        /// </summary>
        public event InputValidationErrorEventHandler InputValidationError;

        #region ValueChanged Event

        //Due to a bug in Visual Studio, you cannot create event handlers for generic T args in XAML, so I have to use object instead.
        /// <summary>
        /// The value changed event
        /// </summary>
        public static readonly RoutedEvent ValueChangedEvent = EventManager.RegisterRoutedEvent("ValueChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<object>), typeof(UpDownBase<T>));
        /// <summary>
        /// Occurs when [value changed].
        /// </summary>
        public event RoutedPropertyChangedEventHandler<object> ValueChanged
        {
            add
            {
                this.AddHandler(ValueChangedEvent, value);
            }
            remove
            {
                this.RemoveHandler(ValueChangedEvent, value);
            }
        }

        #endregion

        #endregion //Events

        #region Methods

        /// <summary>
        /// Raises the <see cref="E:Spin" /> event.
        /// </summary>
        /// <param name="e">The <see cref="SpinEventArgs"/> instance containing the event data.</param>
        /// <exception cref="System.ArgumentNullException">e</exception>
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
            this.RaiseEvent(args);
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
              && (Equals(this.Value, ValueProperty.DefaultMetadata.DefaultValue));

            this.SyncTextAndValueProperties(updateValueFromText, this.Text, !updateValueFromText);
        }

        /// <summary>
        /// Performs an increment if conditions allow it.
        /// </summary>
        internal void DoDecrement()
        {
            if (this.Spinner == null || (this.Spinner.ValidSpinDirections & ValidSpinDirections.Decrease) == ValidSpinDirections.Decrease)
            {
                this.OnDecrement();
            }
        }

        /// <summary>
        /// Performs a decrement if conditions allow it.
        /// </summary>
        internal void DoIncrement()
        {
            if (this.Spinner == null || (this.Spinner.ValidSpinDirections & ValidSpinDirections.Increase) == ValidSpinDirections.Increase)
            {
                this.OnIncrement();
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

        /// <summary>
        /// Tries the update text property.
        /// </summary>
        private void TryUpdateTextProperty()
        {
            try
            {
                if (this.TextBox != null)
                {
                    this._isTextChangedFromUI = true;
                    this.Text = this.TextBox.Text;
                }
            }
            finally
            {
                this._isTextChangedFromUI = false;
            }
        }

        /// <summary>
        /// Handles the LostFocus event of the TextBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (this.UpdateValueOnEnterKey == false)
            {
                this.CommitInput();
            }
        }

        /// <summary>
        /// Appelé juste avant que l'événement <see cref="E:System.Windows.UIElement.IsKeyboardFocusWithinChanged" /> soit déclenché par cet élément.Implémentez cette méthode pour permettre la gestion de classes pour cet événement.
        /// </summary>
        /// <param name="e"><see cref="T:System.Windows.DependencyPropertyChangedEventArgs" /> qui contient les données d'événement.</param>
        protected override void OnIsKeyboardFocusWithinChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnIsKeyboardFocusWithinChanged(e);

            bool lNewValue = Convert.ToBoolean(e.NewValue);
            if (lNewValue == false)
            {
                this.CommitInput();
            }
        }

        /// <summary>
        /// Raises the input validation error.
        /// </summary>
        /// <param name="e">The e.</param>
        private void RaiseInputValidationError(Exception e)
        {
            if (this.InputValidationError != null)
            {
                InputValidationErrorEventArgs args = new InputValidationErrorEventArgs(e);
                this.InputValidationError(this, args);
                if (args.ThrowException)
                {
                    throw args.Exception;
                }
            }
        }

        /// <summary>
        /// Commits the modification and validate the input.
        /// </summary>
        /// <returns>
        /// True if the commit validation succeed, false otherwise.
        /// </returns>
        public virtual bool CommitInput()
        {
            //Nothing to commit if the text has not changed
            string lText = this.ConvertValueToText();

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

            return this.SyncTextAndValueProperties(true, this.Text);
        }

        /// <summary>
        /// Cancels the modification.
        /// </summary>
        /// <returns>
        /// True if the cancelation succeed, false otherwise.
        /// </returns>
        public virtual bool CancelInput()
        {
            return this.SyncTextAndValueProperties(false, this.Text);
        }

        /// <summary>
        /// Synchronizes the text and value properties.
        /// </summary>
        /// <param name="updateValueFromText">if set to <c>true</c> [update value from text].</param>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        protected bool SyncTextAndValueProperties(bool updateValueFromText, string text)
        {
            return this.SyncTextAndValueProperties(updateValueFromText, text, false);
        }

        /// <summary>
        /// Synchronizes the text and value properties.
        /// </summary>
        /// <param name="updateValueFromText">if set to <c>true</c> [update value from text].</param>
        /// <param name="text">The text.</param>
        /// <param name="forceTextUpdate">if set to <c>true</c> [force text update].</param>
        /// <returns></returns>
        protected bool SyncTextAndValueProperties(bool updateValueFromText, string text, bool forceTextUpdate)
        {
            if (this._isSyncingTextAndValueProperties)
                return true;

            this._isSyncingTextAndValueProperties = true;
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
                            if (!Equals(newValue, this.Value))
                            {
                                this.SetValueInternal(newValue);
                            }
                        }
                        catch (Exception e)
                        {
                            parsedTextIsValid = false;

                            // From the UI, just allow any input.
                            if (!this._isTextChangedFromUI)
                            {
                                // This call may throw an exception. 
                                // See RaiseInputValidationError() implementation.
                                this.RaiseInputValidationError(e);
                            }
                        }
                    }
                }

                // Do not touch the ongoing text input from user.
                if (!this._isTextChangedFromUI)
                {
                    // Don't replace the empty Text with the non-empty representation of DefaultValue.
                    bool shouldKeepEmpty = !forceTextUpdate && string.IsNullOrEmpty(this.Text) && Equals(this.Value, this.DefaultValue) && !this.DisplayDefaultValueOnEmptyText;
                    if (!shouldKeepEmpty)
                    {
                        string newText = this.ConvertValueToText();
                        if (!Equals(this.Text, newText))
                        {
                            this.Text = newText;
                        }
                    }

                    // Sync Text and textBox
                    if (this.TextBox != null) this.TextBox.Text = this.Text;
                }

                if (this._isTextChangedFromUI && !parsedTextIsValid)
                {
                    // Text input was made from the user and the text
                    // repesents an invalid value. Disable the spinner
                    // in this case.
                    if (this.Spinner != null)
                    {
                        this.Spinner.ValidSpinDirections = ValidSpinDirections.None;
                    }
                }
                else
                {
                    this.SetValidSpinDirection();
                }
            }
            finally
            {
                this._isSyncingTextAndValueProperties = false;
            }
            return parsedTextIsValid;
        }

        /// <summary>
        /// Converts the value to text.
        /// </summary>
        /// <returns></returns>
        protected string ConvertValueToText()
        {
            return this.ConvertValueToText(this.Value);
        }

        /// <summary>
        /// Sets the valid spin direction.
        /// </summary>
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

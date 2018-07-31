using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;

namespace XControls
{
    /// <summary>
    /// Enum describing the spinner position.
    /// </summary>
    public enum Location
    {
        Left,
        Right
    }

    /// <summary>
    /// Represents a spinner control that includes two Buttons.
    /// </summary>
    [TemplatePart(Name = PART_IncreaseButton, Type = typeof(ButtonBase))]
    [TemplatePart(Name = PART_DecreaseButton, Type = typeof(ButtonBase))]
    [TemplatePart(Name = PART_InfiniteButton, Type = typeof(ButtonBase))]
    [ContentProperty("Content")]
    public class ButtonSpinner : ASpinner
    {
        #region Dependencies

        /// <summary>
        /// Identifies the AllowSpin property.
        /// </summary>
        public static readonly DependencyProperty AllowSpinProperty = DependencyProperty.Register("AllowSpin", typeof(bool), typeof(ButtonSpinner), new UIPropertyMetadata(true, OnAllowSpinPropertyChanged));

        /// <summary>
        /// Identifies the ButtonSpinnerLocation property.
        /// </summary>
        public static readonly DependencyProperty ButtonSpinnerLocationProperty = DependencyProperty.Register("ButtonSpinnerLocation", typeof(Location), typeof(ButtonSpinner), new UIPropertyMetadata(Location.Right));

        /// <summary>
        /// Identifies the ShowInfiniteButton property.
        /// </summary>
        public static readonly DependencyProperty ShowInfiniteButtonProperty = DependencyProperty.Register("ShowInfiniteButton", typeof(bool), typeof(ButtonSpinner), new UIPropertyMetadata(false));

        /// <summary>
        /// Identifies the ShowButtonSpinner property.
        /// </summary>
        public static readonly DependencyProperty ShowButtonSpinnerProperty = DependencyProperty.Register("ShowButtonSpinner", typeof(bool), typeof(ButtonSpinner), new UIPropertyMetadata(true));
        
        /// <summary>
        /// Identifies the Content dependency property.
        /// </summary>
        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register("Content", typeof(object), typeof(ButtonSpinner), new PropertyMetadata(null, OnContentPropertyChanged));

        #endregion // Dependencies.

        #region Fields

        /// <summary>
        /// Stores the names of the control template parts.
        /// </summary>
        private const string PART_IncreaseButton = "PART_IncreaseButton";
        private const string PART_DecreaseButton = "PART_DecreaseButton";
        private const string PART_InfiniteButton = "PART_InfiniteButton";

        /// <summary>
        /// Stores the decrease button.
        /// </summary>
        private ButtonBase mDecreaseButton;

        /// <summary>
        /// Stores the increase button.
        /// </summary>
        private ButtonBase mIncreaseButton;

        /// <summary>
        /// Stores the infinite button.
        /// </summary>
        private ButtonBase mInfiniteButton;
        
        #endregion // Fields.

        #region Properties

        /// <summary>
        /// Gets or sets the flag allowing or not to spin.
        /// </summary>
        public bool AllowSpin
        {
            get
            {
                return (bool)this.GetValue(AllowSpinProperty);
            }
            set
            {
                this.SetValue(AllowSpinProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the property defining the position of the spin buttons.
        /// </summary>
        public Location ButtonSpinnerLocation
        {
            get
            {
                return (Location)this.GetValue(ButtonSpinnerLocationProperty);
            }
            set
            {
                this.SetValue(ButtonSpinnerLocationProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the flag defining if the infinite button can be displayed.
        /// </summary>
        public bool ShowInfiniteButton
        {
            get
            {
                return (bool)this.GetValue(ShowInfiniteButtonProperty);
            }
            set
            {
                this.SetValue(ShowInfiniteButtonProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the flag defining if all the buttons can be displayed.
        /// </summary>
        public bool ShowButtonSpinner
        {
            get
            {
                return (bool)this.GetValue(ShowButtonSpinnerProperty);
            }
            set
            {
                this.SetValue(ShowButtonSpinnerProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the control content.
        /// </summary>
        public object Content
        {
            get
            {
                return this.GetValue(ContentProperty) as object;
            }
            set
            {
                this.SetValue(ContentProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the DecreaseButton template part.
        /// </summary>
        private ButtonBase DecreaseButton
        {
            get
            {
                return this.mDecreaseButton;
            }
            set
            {
                if (this.mDecreaseButton != null)
                {
                    this.mDecreaseButton.Click -= this.OnButtonClick;
                }

                this.mDecreaseButton = value;

                if (this.mDecreaseButton != null)
                {
                    this.mDecreaseButton.Click += this.OnButtonClick;
                }
            }
        }

        /// <summary>
        /// Gets or sets the IncreaseButton template part.
        /// </summary>
        private ButtonBase IncreaseButton
        {
            get
            {
                return this.mIncreaseButton;
            }
            set
            {
                if (this.mIncreaseButton != null)
                {
                    this.mIncreaseButton.Click -= this.OnButtonClick;
                }

                this.mIncreaseButton = value;

                if (this.mIncreaseButton != null)
                {
                    this.mIncreaseButton.Click += this.OnButtonClick;
                }
            }
        }

        /// <summary>
        /// Gets or sets the InfiniteButton template part.
        /// </summary>
        private ButtonBase InfiniteButton
        {
            get
            {
                return this.mInfiniteButton;
            }
            set
            {
                if (this.mInfiniteButton != null)
                {
                    this.mInfiniteButton.Click -= this.OnButtonClick;
                }

                this.mInfiniteButton = value;

                if (this.mInfiniteButton != null)
                {
                    this.mInfiniteButton.Click += this.OnButtonClick;
                }
            }
        }

        #endregion // Properties.

        #region Constructors

        /// <summary>
        /// Initializes the <see cref="ButtonSpinner"/> class.
        /// </summary>
        static ButtonSpinner()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ButtonSpinner), new FrameworkPropertyMetadata(typeof(ButtonSpinner)));
        }

        #endregion //Constructors

        #region Methods

        /// <summary>
        /// Delegate called when the AllowSpin property changed.
        /// </summary>
        /// <param name="pObject">The modified control.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private static void OnAllowSpinPropertyChanged(DependencyObject pObject, DependencyPropertyChangedEventArgs pEventArgs)
        {
            ButtonSpinner lButton = pObject as ButtonSpinner;
            if (lButton != null)
            {
                lButton.OnAllowSpinChanged((bool)pEventArgs.OldValue, (bool)pEventArgs.NewValue);
            }
        }

        /// <summary>
        /// ContentProperty property changed handler.
        /// </summary>
        /// <param name="pObject">ButtonSpinner that changed its Content.</param>
        /// <param name="pEventArgs">Event arguments.</param>
        private static void OnContentPropertyChanged(DependencyObject pObject, DependencyPropertyChangedEventArgs pEventArgs)
        {
            ButtonSpinner lButton = pObject as ButtonSpinner;
            if (lButton != null)
            {
                lButton.OnContentChanged(pEventArgs.OldValue, pEventArgs.NewValue);
            }
        }

        /// <summary>
        /// Disables or enables the buttons based on the valid spin direction.
        /// </summary>
        private void SetButtonUsage()
        {
            if (this.IncreaseButton != null)
            {
                this.IncreaseButton.IsEnabled = this.AllowSpin && ((this.ValidSpinDirections & ValidSpinDirections.Increase) == ValidSpinDirections.Increase);
            }

            if (this.InfiniteButton != null)
            {
                this.InfiniteButton.IsEnabled = this.AllowSpin && ((this.ValidSpinDirections & ValidSpinDirections.Increase) == ValidSpinDirections.Increase);
            }

            if (this.DecreaseButton != null)
            {
                this.DecreaseButton.IsEnabled = this.AllowSpin && ((this.ValidSpinDirections & ValidSpinDirections.Decrease) == ValidSpinDirections.Decrease);
            }
        }

        /// <summary>
        /// Method called when the template is applied.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.IncreaseButton = this.GetTemplateChild(PART_IncreaseButton) as ButtonBase;
            this.DecreaseButton = this.GetTemplateChild(PART_DecreaseButton) as ButtonBase;
            this.InfiniteButton = this.GetTemplateChild(PART_InfiniteButton) as ButtonBase;

            if (this.IncreaseButton == null || this.DecreaseButton == null || this.InfiniteButton == null)
            {
                throw new InvalidOperationException("The ButtonSpinner control template is wrongly initialized.");
            }

            this.SetButtonUsage();
        }

        /// <summary>
        /// Delegate called when the mouse left button is up.
        /// </summary>
        /// <param name="pEventArgs">The event arguments.</param>
        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs pEventArgs)
        {
            base.OnMouseLeftButtonUp(pEventArgs);

            // Cancel LeftMouseButtonUp events originating from a button that has been changed to disabled.

            Point lMousePosition;
            if (this.IncreaseButton != null && this.IncreaseButton.IsEnabled == false)
            {
                lMousePosition = pEventArgs.GetPosition(this.IncreaseButton);
                if (lMousePosition.X > 0 && lMousePosition.X < this.IncreaseButton.ActualWidth &&
                    lMousePosition.Y > 0 && lMousePosition.Y < this.IncreaseButton.ActualHeight)
                {
                    pEventArgs.Handled = true;
                }
            }

            if (this.DecreaseButton != null && this.DecreaseButton.IsEnabled == false)
            {
                lMousePosition = pEventArgs.GetPosition(this.DecreaseButton);
                if (lMousePosition.X > 0 && lMousePosition.X < this.DecreaseButton.ActualWidth &&
                    lMousePosition.Y > 0 && lMousePosition.Y < this.DecreaseButton.ActualHeight)
                {
                    pEventArgs.Handled = true;
                }
            }
        }

        /// <summary>
        /// Deleaget called when a key is going ot be down.
        /// </summary>
        /// <param name="pEventArgs">The event arguments.</param>
        protected override void OnPreviewKeyDown(KeyEventArgs pEventArgs)
        {
            // Handling the spin with the keys.
            switch (pEventArgs.Key)
            {
                case Key.Up:
                    {
                        if (this.AllowSpin)
                        {
                            this.NotifySpin(new SpinEventArgs(SpinDirection.Increase));
                            pEventArgs.Handled = true;
                        }

                        break;
                    }
                case Key.Down:
                    {
                        if (this.AllowSpin)
                        {
                            this.NotifySpin(new SpinEventArgs(SpinDirection.Decrease));
                            pEventArgs.Handled = true;
                        }

                        break;
                    }
            }
        }
        
        /// <summary>
        /// Delegate called when the mouse wheel is used.
        /// </summary>
        /// <param name="pEventArgs">The event arguments.</param>
        protected override void OnMouseWheel(MouseWheelEventArgs pEventArgs)
        {
            base.OnMouseWheel(pEventArgs);

            // Handling the spin with the mouse wheel.
            if (this.IsKeyboardFocusWithin && pEventArgs.Handled == false && this.AllowSpin)
            {
                if (pEventArgs.Delta < 0)
                {
                    this.NotifySpin(new SpinEventArgs(SpinDirection.Decrease, true));
                }
                else if (0 < pEventArgs.Delta)
                {
                    this.NotifySpin(new SpinEventArgs(SpinDirection.Increase, true));
                }

                pEventArgs.Handled = true;
            }
        }

        /// <summary>
        /// Called when valid spin direction changed.
        /// </summary>
        /// <param name="pOldValue">The old value.</param>
        /// <param name="pNewValue">The new value.</param>
        protected override void OnValidSpinDirectionChanged(ValidSpinDirections pOldValue, ValidSpinDirections pNewValue)
        {
            this.SetButtonUsage();
        }

        /// <summary>
        /// Handle click event of buttons template parts, translating Click to appropriate Spin event.
        /// </summary>
        /// <param name="pSender">The event sender.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private void OnButtonClick(object pSender, RoutedEventArgs pEventArgs)
        {
            if (this.AllowSpin)
            {
                if (pSender == this.IncreaseButton)
                {
                    SpinDirection lDirection = SpinDirection.Increase;
                    this.NotifySpin(new SpinEventArgs(lDirection));
                }
                else if (pSender == this.DecreaseButton)
                {
                    SpinDirection lDirection = SpinDirection.Decrease;
                    this.NotifySpin(new SpinEventArgs(lDirection));
                }
                else if (pSender == this.InfiniteButton)
                {
                    this.NotifySpin(new SpinEventArgs(true));
                }
            }
        }

        /// <summary>
        /// Delegate called when the Content property value changed.
        /// </summary>
        /// <param name="pOldValue">The old value of the Content property.</param>
        /// <param name="pNewValue">The new value of the Content property.</param>
        protected virtual void OnContentChanged(object pOldValue, object pNewValue)
        {
        }

        /// <summary>
        /// Delegate called when the AllowSpin property value changed.
        /// </summary>
        /// <param name="pOldValue">The old value.</param>
        /// <param name="pNewValue">The new value.</param>
        protected virtual void OnAllowSpinChanged(bool pOldValue, bool pNewValue)
        {
            this.SetButtonUsage();
        }

        #endregion // Methods.
    }
}

using System;
using System.Windows;
using System.Windows.Controls;

namespace XControls
{
    /// <summary>
    /// Base class for controls that represents controls that can spin.
    /// </summary>
    public abstract class ASpinner : Control
    {
        #region Dependencies

        /// <summary>
        /// Identifies the ValidSpinDirection dependency property.
        /// </summary>
        public static readonly DependencyProperty ValidSpinDirectionProperty = DependencyProperty.Register("ValidSpinDirection", typeof(ValidSpinDirections), typeof(ASpinner), new PropertyMetadata(ValidSpinDirections.Increase | ValidSpinDirections.Decrease, OnValidSpinDirectionPropertyChanged));

        #endregion // Dependencies.

        #region Events

        /// <summary>
        /// Event raised when spinning is initiated by the end-user.
        /// </summary>
        public event EventHandler<SpinEventArgs> Spin;

        #endregion // Events.

        #region Construstors

        /// <summary>
        /// Initializes a new instance of the <see cref="ASpinner"/> class.
        /// </summary>
        protected ASpinner()
        {
        }

        #endregion // Constructors.

        #region Properties

        /// <summary>
        /// Gets or sets the flag indicating the valid spin directions.
        /// </summary>
        public ValidSpinDirections ValidSpinDirections
        {
            get
            {
                return (ValidSpinDirections)this.GetValue(ValidSpinDirectionProperty);
            }
            set
            {
                this.SetValue(ValidSpinDirectionProperty, value);
            }
        }

        #endregion // Properties.

        #region Methods

        /// <summary>
        /// ValidSpinDirectionProperty property changed handler.
        /// </summary>
        /// <param name="pObject">ButtonSpinner that changed its ValidSpinDirection.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private static void OnValidSpinDirectionPropertyChanged(DependencyObject pObject, DependencyPropertyChangedEventArgs pEventArgs)
        {
            ASpinner lSpinner = pObject as ASpinner;
            if (lSpinner != null)
            {
                lSpinner.OnValidSpinDirectionChanged((ValidSpinDirections)pEventArgs.OldValue, (ValidSpinDirections)pEventArgs.NewValue);
            }
        }

        /// <summary>
        /// Raises the OnSpin event when spinning is initiated by the end-user.
        /// </summary>
        /// <param name="pEventArgs">The event arguments.</param>
        protected virtual void NotifySpin(SpinEventArgs pEventArgs)
        {
            ValidSpinDirections lDirection = pEventArgs.Direction == SpinDirection.Increase ? ValidSpinDirections.Increase : ValidSpinDirections.Decrease;

            // Only raise the event if spin is allowed.
            if ((this.ValidSpinDirections & lDirection) == lDirection)
            {
                if (this.Spin != null)
                {
                    this.Spin(this, pEventArgs);
                }
            }
        }

        /// <summary>
        /// Method called when valid spin direction changed.
        /// </summary>
        /// <param name="pOldValue">The old value.</param>
        /// <param name="pNewValue">The new value.</param>
        protected virtual void OnValidSpinDirectionChanged(ValidSpinDirections pOldValue, ValidSpinDirections pNewValue)
        {
        }

        #endregion // Methods.
    }
}

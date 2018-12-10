using System;
using System.Windows;
using System.Windows.Controls;

namespace XControls.ProgressBar
{
    /// <summary>
    ///     This class describes a progress bar able to display the progression as text.
    /// </summary>
    /// <!-- DPE -->
    public partial class ProgressBar : UserControl
    {
        #region Constructor

        /// <summary>
        ///     Initializes a new instance of the ProgressBar class.
        /// </summary>
        public ProgressBar()
        {
            this.InitializeComponent();
        }

        #endregion // Constructor.

        #region Dependency properties

        /// <summary>
        ///     This field defines a dependency on the property "ProgressTextIsVisible".
        /// </summary>
        public static readonly DependencyProperty ProgressTextIsVisibleProperty = DependencyProperty.Register("ProgressTextIsVisible", typeof(bool), typeof(ProgressBar), new FrameworkPropertyMetadata(false, OnProgressTextIsVisibleChanged));

        /// <summary>
        ///     This field defines a dependency on the property "LabelIsVisible".
        /// </summary>
        public static readonly DependencyProperty LabelIsVisibleProperty = DependencyProperty.Register("LabelIsVisible", typeof(bool), typeof(ProgressBar), new FrameworkPropertyMetadata(true, OnLabelIsVisibleChanged));

        /// <summary>
        ///     This field defines a dependency on the property "Label".
        /// </summary>
        public static readonly DependencyProperty LabelProperty = DependencyProperty.Register("Label", typeof(string), typeof(ProgressBar), new FrameworkPropertyMetadata(OnLabelChanged));

        /// <summary>
        ///     This field defines a dependency on the property "Minimum".
        /// </summary>
        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register("Minimum", typeof(double), typeof(ProgressBar), new FrameworkPropertyMetadata(OnMinValueChanged));

        /// <summary>
        ///     This field defines a dependency on the property "Maximum".
        /// </summary>
        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register("Maximum", typeof(double), typeof(ProgressBar), new FrameworkPropertyMetadata(100.0, OnMaxValueChanged));

        /// <summary>
        ///     This field defines a dependency on the property "Value".
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(double), typeof(ProgressBar), new FrameworkPropertyMetadata(OnCurrentValueChanged));

        #endregion // Dependency properties

        #region Properties

        /// <summary>
        ///     Gets or sets the label.
        /// </summary>
        public string Label
        {
            get => Convert.ToString(this.GetValue(LabelProperty));
            set => this.SetValue(LabelProperty, value);
        }

        /// <summary>
        ///     Gets or sets the Minimum.
        /// </summary>
        public double Minimum
        {
            get => Convert.ToDouble(this.GetValue(MinimumProperty));
            set => this.SetValue(MinimumProperty, value);
        }

        /// <summary>
        ///     Gets or sets the Maximum.
        /// </summary>
        public double Maximum
        {
            get => Convert.ToDouble(this.GetValue(MaximumProperty));
            set => this.SetValue(MaximumProperty, value);
        }

        /// <summary>
        ///     Gets or sets the value.
        /// </summary>
        public double Value
        {
            get => Convert.ToDouble(this.GetValue(ValueProperty));
            set => this.SetValue(ValueProperty, value);
        }

        /// <summary>
        ///     Gets or sets the flag indicating if the progress text is visible.
        /// </summary>
        public bool ProgressTextIsVisible
        {
            get => Convert.ToBoolean(this.GetValue(ProgressTextIsVisibleProperty));
            set => this.SetValue(ProgressTextIsVisibleProperty, value);
        }

        /// <summary>
        ///     Gets or sets the flag indicating if the label is visible.
        /// </summary>
        public bool LabelIsVisible
        {
            get => Convert.ToBoolean(this.GetValue(LabelIsVisibleProperty));
            set => this.SetValue(LabelIsVisibleProperty, value);
        }

        #endregion // Properties.

        #region Methods

        /// <summary>
        ///     This delegate is called when the min value is changed.
        /// </summary>
        /// <param name="pSender">The event sender.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private static void OnLabelChanged(DependencyObject pSender, DependencyPropertyChangedEventArgs pEventArgs)
        {
            var lControl = pSender as ProgressBar;
            if (lControl != null)
            {
                // Updating the label.
                lControl.mLabel.Text = Convert.ToString(pEventArgs.NewValue);
            }
        }

        /// <summary>
        ///     This delegate is called when the min value is changed.
        /// </summary>
        /// <param name="pSender">The event sender.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private static void OnMinValueChanged(DependencyObject pSender, DependencyPropertyChangedEventArgs pEventArgs)
        {
            var lControl = pSender as ProgressBar;
            if (lControl != null)
            {
                // Updating the progress bar.
                lControl.mProgressBar.Minimum = Convert.ToDouble(pEventArgs.NewValue);
                lControl.mProgressBar.Value = 0.1;
                // and the text displayed.
                lControl.UpdateProgressText();
            }
        }

        /// <summary>
        ///     This delegate is called when the max value is changed.
        /// </summary>
        /// <param name="pSender">The event sender.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private static void OnMaxValueChanged(DependencyObject pSender, DependencyPropertyChangedEventArgs pEventArgs)
        {
            var lControl = pSender as ProgressBar;
            if (lControl != null)
            {
                // Never let the maximum value to 0 for display matter.
                var lNewValue = Convert.ToDouble(pEventArgs.NewValue);
                if (lNewValue == 0.0)
                {
                    lNewValue = 1.0;
                }

                // Updating the progress bar.
                lControl.mProgressBar.Maximum = lNewValue;

                // and the text displayed.
                lControl.UpdateProgressText();
            }
        }

        /// <summary>
        ///     This delegate is called when the min value is changed.
        /// </summary>
        /// <param name="pSender">The event sender.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private static void OnCurrentValueChanged(DependencyObject pSender, DependencyPropertyChangedEventArgs pEventArgs)
        {
            var lControl = pSender as ProgressBar;
            if (lControl != null)
            {
                // Updating the progress bar.
                lControl.mProgressBar.Value = Convert.ToDouble(pEventArgs.NewValue);
                // and the text displayed.
                lControl.UpdateProgressText();
            }
        }

        /// <summary>
        ///     This delegate is called when the text visibility is changed.
        /// </summary>
        /// <param name="pSender">The event sender.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private static void OnProgressTextIsVisibleChanged(DependencyObject pSender, DependencyPropertyChangedEventArgs pEventArgs)
        {
            var lControl = pSender as ProgressBar;
            if (lControl != null)
            {
                // Updating the text displayed.
                lControl.UpdateProgressText();
            }
        }

        /// <summary>
        ///     This delegate is called when the label vsibility is changed.
        /// </summary>
        /// <param name="pSender">The event sender.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private static void OnLabelIsVisibleChanged(DependencyObject pSender, DependencyPropertyChangedEventArgs pEventArgs)
        {
            var lControl = pSender as ProgressBar;
            if (lControl != null)
            {
                var lIsVisible = Convert.ToBoolean(pEventArgs.NewValue);
                if (lIsVisible)
                {
                    lControl.mLabel.Visibility = Visibility.Visible;
                    lControl.mLabel.Margin = new Thickness(3);
                }
                else
                {
                    lControl.mLabel.Visibility = Visibility.Collapsed;
                    lControl.mLabel.Margin = new Thickness(0);
                }
            }
        }

        /// <summary>
        ///     Updates the text displayed on the progress bar.
        /// </summary>
        internal void UpdateProgressText()
        {
            if (this.ProgressTextIsVisible)
            {
                // This notation is significant only if the minim is 0.
                // Otherwise, displaying the pourcentage.
                if (this.Minimum == 0)
                {
                    this.mProgressText.Text = this.Value + "/" + this.Maximum;
                }
                else
                {
                    this.mProgressText.Text = (int) (100.0 * (this.Value - this.Minimum) / (this.Maximum - this.Minimum)) + " %";
                }
            }
            else
            {
                this.mProgressText.Text = string.Empty;
            }
        }

        #endregion // Methods.
    }
}
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace XControls.BusyBar
{
    /// <summary>
    /// A circular type progress bar, that is similar to popular web based progress bars.
    /// Adapted from : http://www.codeproject.com/Articles/49455/Better-WPF-Circular-Progress-Bar.aspx?msg=3322272
    /// </summary>
    /// <!-- NBY -->
    public partial class BusyBar : INotifyPropertyChanged
    {
        #region Fields

        /// <summary>
        /// This field stores the animation timer.
        /// </summary>
        private readonly DispatcherTimer mAnimationTimer;

        /// <summary>
        /// The busy bar color.
        /// </summary>
        private Color mColor;

        #endregion // Fields.

        #region Methods

        /// <summary>
        /// Default constructor.
        /// </summary>
        public BusyBar()
        {
            this.InitializeComponent();

            this.Color = (Color)ColorConverter.ConvertFromString("#227CC5");

            this.mAnimationTimer = new DispatcherTimer(DispatcherPriority.Send);
            this.mAnimationTimer.Interval = new TimeSpan(0, 0, 0, 0, 75);
        }

        /// <summary>
        /// This methods starts the animation.
        /// </summary>
        private void Start()
        {
            this.mAnimationTimer.Tick += this.OnTimerTicked;
            this.mAnimationTimer.Start();
        }

        /// <summary>
        /// This method stops the animation.
        /// </summary>
        private void Stop()
        {
            this.mAnimationTimer.Stop();
            this.mAnimationTimer.Tick -= this.OnTimerTicked;
        }

        /// <summary>
        /// This methods updates the animation.
        /// </summary>
        /// <param name="pSender">The event sender.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private void OnTimerTicked(Object pSender, EventArgs pEventArgs)
        {
            this.SpinnerRotate.Angle = (this.SpinnerRotate.Angle + 36) % 360;
        }

        /// <summary>
        /// This methods updates the different circles when the control is loaded.
        /// </summary>
        /// <param name="pSender">The event sender.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private void OnControlLoaded(Object pSender, RoutedEventArgs pEventArgs)
        {
            const Double step = Math.PI * 2 / 10.0;
            const Double offset = Math.PI;

            this.C0.SetValue(Canvas.LeftProperty, 50.0 +
                Math.Sin(offset + 0.0 * step) * 50.0);
            this.C0.SetValue(Canvas.TopProperty, 50 +
                Math.Cos(offset + 0.0 * step) * 50.0);

            this.C1.SetValue(Canvas.LeftProperty, 50.0 +
                Math.Sin(offset + 1.0 * step) * 50.0);
            this.C1.SetValue(Canvas.TopProperty, 50 +
                Math.Cos(offset + 1.0 * step) * 50.0);

            this.C2.SetValue(Canvas.LeftProperty, 50.0 +
                Math.Sin(offset + 2.0 * step) * 50.0);
            this.C2.SetValue(Canvas.TopProperty, 50 +
                Math.Cos(offset + 2.0 * step) * 50.0);

            this.C3.SetValue(Canvas.LeftProperty, 50.0 +
                Math.Sin(offset + 3.0 * step) * 50.0);
            this.C3.SetValue(Canvas.TopProperty, 50 +
                Math.Cos(offset + 3.0 * step) * 50.0);

            this.C4.SetValue(Canvas.LeftProperty, 50.0 +
                Math.Sin(offset + 4.0 * step) * 50.0);
            this.C4.SetValue(Canvas.TopProperty, 50 +
                Math.Cos(offset + 4.0 * step) * 50.0);

            this.C5.SetValue(Canvas.LeftProperty, 50.0 +
                Math.Sin(offset + 5.0 * step) * 50.0);
            this.C5.SetValue(Canvas.TopProperty, 50 +
                Math.Cos(offset + 5.0 * step) * 50.0);

            this.C6.SetValue(Canvas.LeftProperty, 50.0 +
                Math.Sin(offset + 6.0 * step) * 50.0);
            this.C6.SetValue(Canvas.TopProperty, 50 +
                Math.Cos(offset + 6.0 * step) * 50.0);

            this.C7.SetValue(Canvas.LeftProperty, 50.0 +
                Math.Sin(offset + 7.0 * step) * 50.0);
            this.C7.SetValue(Canvas.TopProperty, 50 +
                Math.Cos(offset + 7.0 * step) * 50.0);

            this.C8.SetValue(Canvas.LeftProperty, 50.0 +
                Math.Sin(offset + 8.0 * step) * 50.0);
            this.C8.SetValue(Canvas.TopProperty, 50 +
                Math.Cos(offset + 8.0 * step) * 50.0);
        }

        /// <summary>
        /// This method stops the animation when the control is unloaded.
        /// </summary>
        /// <param name="pSender">The event sender.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private void OnControlUnloaded(Object pSender, RoutedEventArgs pEventArgs)
        {
            this.Stop();
        }

        /// <summary>
        /// This method is called when the visibility is changed.
        /// </summary>
        /// <param name="pSender">The event sender.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private void OnVisibilityChanged(Object pSender, DependencyPropertyChangedEventArgs pEventArgs)
        {
            Boolean lIsVisible = (Boolean)pEventArgs.NewValue;

            if
                (lIsVisible)
            {
                this.Start();
            }
            else
            {
                this.Stop();
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the color of the busy bar.
        /// </summary>
        public Color Color 
        {
            get
            {
                return this.mColor;
            }

            set
            {
                if
                    (this.mColor != value)
                {
                    this.mColor = value;

                    if
                        (this.PropertyChanged != null)
                    {
                        this.PropertyChanged(this, new PropertyChangedEventArgs("Color"));
                    }
                }
            }
        }

        #endregion // Properties.

        #region Events

        /// <summary>
        /// Raised when a property is modifyed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion //Events.
    }
}


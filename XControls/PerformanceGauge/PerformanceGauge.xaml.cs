using System.Windows;
using System.Windows.Controls;

namespace XControls.PerformanceGauge
{
    /// <summary>
    /// Defines a linear gradient performance gauge.
    /// </summary>
    public partial class PerformanceGauge : UserControl
    {
        /// <summary>
        /// Value dependency property
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value",
                                                                    typeof(double),
                                                                    typeof(PerformanceGauge),
                                                                    new FrameworkPropertyMetadata(50.0));

        /// <summary>
        /// Minimum dependency property
        /// </summary>
        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register("Minimum",
                                                                    typeof(double),
                                                                    typeof(PerformanceGauge),
                                                                    new FrameworkPropertyMetadata(0.0));
        /// <summary>
        /// Maximum dependecy property
        /// </summary>
        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register("Maximum",
                                                                    typeof(double),
                                                                    typeof(PerformanceGauge),
                                                                    new FrameworkPropertyMetadata(100.0));

        /// <summary>
        /// Initializes a new instance of the <see cref="PerformanceGauge"/> class.
        /// </summary>
        public PerformanceGauge()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Gets or sets the value of the performance gauge.
        /// </summary>
        /// <value>The value.</value>
        public double Value
        {
            get
            {
                return (double)this.GetValue(PerformanceGauge.ValueProperty);
            }

            set
            {
                this.SetValue(PerformanceGauge.ValueProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the minimum for the progress.
        /// </summary>
        /// <value>The minimum.</value>
        public double Minimum
        {
            get
            {
                return (double)this.GetValue(PerformanceGauge.MinimumProperty);
            }

            set
            {
                this.SetValue(PerformanceGauge.MinimumProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the maximum value for the progress.
        /// </summary>
        /// <value>The maximum.</value>
        public double Maximum
        {
            get
            {
                return (double)this.GetValue(PerformanceGauge.MaximumProperty);
            }

            set
            {
                this.SetValue(PerformanceGauge.MaximumProperty, value);
            }
        }

        /// <summary>
        /// Gets the end point for linear gradient. Bot defined in xaml because
        /// Point.X and Point.Y can't be bound
        /// </summary>
        /// <value>The end point.</value>
        public Point EndPoint
        {
            get
            {
                return new Point(0, this.Height);
            }
        }
    }
}

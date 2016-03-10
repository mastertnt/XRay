using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace XZoomAndPan.Controls
{
    /// <summary>
    /// Base class for the zoom and pan controls.
    /// </summary>
    public abstract class AZoomAndPanControl : ContentControl
    {
        #region Dependencies

        /// <summary>
        /// Identifies the ContentScale dependency property.
        /// </summary>
        public static readonly DependencyProperty ContentScaleProperty = DependencyProperty.Register("ContentScale", typeof(double), typeof(AZoomAndPanControl), new FrameworkPropertyMetadata(1.0));

        /// <summary>
        /// Identifies the ContentScaleStep dependency property.
        /// </summary>
        public static readonly DependencyProperty ContentScaleStepProperty = DependencyProperty.Register("ContentScaleStep", typeof(double), typeof(AZoomAndPanControl), new FrameworkPropertyMetadata(0.1));

        /// <summary>
        /// Identifies the MinContentScale dependency property.
        /// </summary>
        public static readonly DependencyProperty MinContentScaleProperty = DependencyProperty.Register("MinContentScale", typeof(double), typeof(AZoomAndPanControl), new FrameworkPropertyMetadata(0.01));

        /// <summary>
        /// Identifies the MaxContentScale dependency property.
        /// </summary>
        public static readonly DependencyProperty MaxContentScaleProperty = DependencyProperty.Register("MaxContentScale", typeof(double), typeof(AZoomAndPanControl), new FrameworkPropertyMetadata(10.0));

        /// <summary>
        /// Identifies the ContentOffsetX dependency property.
        /// </summary>
        public static readonly DependencyProperty ContentOffsetXProperty = DependencyProperty.Register("ContentOffsetX", typeof(double), typeof(AZoomAndPanControl), new FrameworkPropertyMetadata(0.0));

        /// <summary>
        /// Identifies the ContentOffsetY dependency property.
        /// </summary>
        public static readonly DependencyProperty ContentOffsetYProperty = DependencyProperty.Register("ContentOffsetY", typeof(double), typeof(AZoomAndPanControl), new FrameworkPropertyMetadata(0.0));

        /// <summary>
        /// Identifies the AnimationDuration dependency property.
        /// </summary>
        public static readonly DependencyProperty AnimationDurationProperty = DependencyProperty.Register("AnimationDuration", typeof(double), typeof(AZoomAndPanControl), new FrameworkPropertyMetadata(0.4));

        /// <summary>
        /// Identifies the ContentZoomFocusX dependency property.
        /// </summary>
        public static readonly DependencyProperty ContentZoomFocusXProperty = DependencyProperty.Register("ContentZoomFocusX", typeof(double), typeof(AZoomAndPanControl), new FrameworkPropertyMetadata(0.0));

        /// <summary>
        /// Identifies the ContentZoomFocusY dependency property.
        /// </summary>
        public static readonly DependencyProperty ContentZoomFocusYProperty = DependencyProperty.Register("ContentZoomFocusY", typeof(double), typeof(AZoomAndPanControl), new FrameworkPropertyMetadata(0.0));

        /// <summary>
        /// Identifies the ViewportZoomFocusX dependency property.
        /// </summary>
        public static readonly DependencyProperty ViewportZoomFocusXProperty = DependencyProperty.Register("ViewportZoomFocusX", typeof(double), typeof(AZoomAndPanControl), new FrameworkPropertyMetadata(0.0));

        /// <summary>
        /// Identifies the ViewportZoomFocusY dependency property.
        /// </summary>
        public static readonly DependencyProperty ViewportZoomFocusYProperty = DependencyProperty.Register("ViewportZoomFocusY", typeof(double), typeof(AZoomAndPanControl), new FrameworkPropertyMetadata(0.0));

        /// <summary>
        /// Identifies the ContentViewportWidth dependency property.
        /// </summary>
        public static readonly DependencyProperty ContentViewportWidthProperty = DependencyProperty.Register("ContentViewportWidth", typeof(double), typeof(AZoomAndPanControl), new FrameworkPropertyMetadata(0.0));

        /// <summary>
        /// Identifies the ContentViewportHeight dependency property.
        /// </summary>
        public static readonly DependencyProperty ContentViewportHeightProperty = DependencyProperty.Register("ContentViewportHeight", typeof(double), typeof(AZoomAndPanControl), new FrameworkPropertyMetadata(0.0));

        #endregion // Dependencies.

        #region Properties

        /// <summary>
        /// Get or set the X offset (in content coordinates) of the view on the content.
        /// </summary>
        public double ContentOffsetX
        {
            get
            {
                return (double)GetValue(ContentOffsetXProperty);
            }
            set
            {
                SetValue(ContentOffsetXProperty, value);
            }
        }

        /// <summary>
        /// Get or set the Y offset (in content coordinates) of the view on the content.
        /// </summary>
        public double ContentOffsetY
        {
            get
            {
                return (double)GetValue(ContentOffsetYProperty);
            }
            set
            {
                SetValue(ContentOffsetYProperty, value);
            }
        }

        /// <summary>
        /// Get or set the current scale (or zoom factor) of the content.
        /// </summary>
        public double ContentScale
        {
            get
            {
                return (double)GetValue(ContentScaleProperty);
            }
            set
            {
                SetValue(ContentScaleProperty, value);
            }
        }

        /// <summary>
        /// Get or set the step of the scale when zooming.
        /// </summary>
        public double ContentScaleStep
        {
            get
            {
                return (double)GetValue(ContentScaleStepProperty);
            }
            set
            {
                SetValue(ContentScaleStepProperty, value);
            }
        }

        /// <summary>
        /// Get or set the minimum value for 'ContentScale'.
        /// </summary>
        public double MinContentScale
        {
            get
            {
                return (double)GetValue(MinContentScaleProperty);
            }
            set
            {
                SetValue(MinContentScaleProperty, value);
            }
        }

        /// <summary>
        /// Get or set the maximum value for 'ContentScale'.
        /// </summary>
        public double MaxContentScale
        {
            get
            {
                return (double)GetValue(MaxContentScaleProperty);
            }
            set
            {
                SetValue(MaxContentScaleProperty, value);
            }
        }

        /// <summary>
        /// The X coordinate of the content focus, this is the point that we are focusing on when zooming.
        /// </summary>
        public double ContentZoomFocusX
        {
            get
            {
                return (double)GetValue(ContentZoomFocusXProperty);
            }
            set
            {
                SetValue(ContentZoomFocusXProperty, value);
            }
        }

        /// <summary>
        /// The Y coordinate of the content focus, this is the point that we are focusing on when zooming.
        /// </summary>
        public double ContentZoomFocusY
        {
            get
            {
                return (double)GetValue(ContentZoomFocusYProperty);
            }
            set
            {
                SetValue(ContentZoomFocusYProperty, value);
            }
        }

        /// <summary>
        /// The X coordinate of the viewport focus, this is the point in the viewport (in viewport coordinates) 
        /// that the content focus point is locked to while zooming in.
        /// </summary>
        public double ViewportZoomFocusX
        {
            get
            {
                return (double)GetValue(ViewportZoomFocusXProperty);
            }
            set
            {
                SetValue(ViewportZoomFocusXProperty, value);
            }
        }

        /// <summary>
        /// The Y coordinate of the viewport focus, this is the point in the viewport (in viewport coordinates) 
        /// that the content focus point is locked to while zooming in.
        /// </summary>
        public double ViewportZoomFocusY
        {
            get
            {
                return (double)GetValue(ViewportZoomFocusYProperty);
            }
            set
            {
                SetValue(ViewportZoomFocusYProperty, value);
            }
        }

        /// <summary>
        /// The duration of the animations (in seconds) started by calling AnimatedZoomTo and the other animation methods.
        /// </summary>
        public double AnimationDuration
        {
            get
            {
                return (double)GetValue(AnimationDurationProperty);
            }
            set
            {
                SetValue(AnimationDurationProperty, value);
            }
        }

        /// <summary>
        /// Get the viewport width, in content coordinates.
        /// </summary>
        public double ContentViewportWidth
        {
            get
            {
                return (double)GetValue(ContentViewportWidthProperty);
            }
            set
            {
                SetValue(ContentViewportWidthProperty, value);
            }
        }

        /// <summary>
        /// Get the viewport height, in content coordinates.
        /// </summary>
        public double ContentViewportHeight
        {
            get
            {
                return (double)GetValue(ContentViewportHeightProperty);
            }
            set
            {
                SetValue(ContentViewportHeightProperty, value);
            }
        }

        #endregion // Properties.
    }
}

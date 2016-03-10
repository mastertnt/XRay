using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using XZoomAndPan.Behaviors;

namespace XZoomAndPan.Controls
{
    /// <summary>
    /// Class defining a zoom and pan control providing user interactions facilities.
    /// </summary>
    [TemplatePart(Name = PART_ZOOM_AND_PAN_CONTROL, Type = typeof(ZoomAndPanControl))]
    public class TooledZoomAndPanControl : AZoomAndPanControl
    {
        #region Dependencies

        /// <summary>
        /// Identifies the ContentScale dependency property.
        /// </summary>
        public static readonly DependencyProperty ContentScaleProperty = DependencyProperty.Register("ContentScale", typeof(double), typeof(TooledZoomAndPanControl), new FrameworkPropertyMetadata(1.0));

        /// <summary>
        /// Identifies the MinContentScale dependency property.
        /// </summary>
        public static readonly DependencyProperty MinContentScaleProperty = DependencyProperty.Register("MinContentScale", typeof(double), typeof(TooledZoomAndPanControl), new FrameworkPropertyMetadata(0.01));

        /// <summary>
        /// Identifies the MaxContentScale dependency property.
        /// </summary>
        public static readonly DependencyProperty MaxContentScaleProperty = DependencyProperty.Register("MaxContentScale", typeof(double), typeof(TooledZoomAndPanControl), new FrameworkPropertyMetadata(10.0));

        /// <summary>
        /// Identifies the ContentOffsetX dependency property.
        /// </summary>
        public static readonly DependencyProperty ContentOffsetXProperty = DependencyProperty.Register("ContentOffsetX", typeof(double), typeof(TooledZoomAndPanControl), new FrameworkPropertyMetadata(0.0));

        /// <summary>
        /// Identifies the ContentOffsetY dependency property.
        /// </summary>
        public static readonly DependencyProperty ContentOffsetYProperty = DependencyProperty.Register("ContentOffsetY", typeof(double), typeof(TooledZoomAndPanControl), new FrameworkPropertyMetadata(0.0));

        /// <summary>
        /// Identifies the AnimationDuration dependency property.
        /// </summary>
        public static readonly DependencyProperty AnimationDurationProperty = DependencyProperty.Register("AnimationDuration", typeof(double), typeof(TooledZoomAndPanControl), new FrameworkPropertyMetadata(0.4));

        /// <summary>
        /// Identifies the ContentZoomFocusX dependency property.
        /// </summary>
        public static readonly DependencyProperty ContentZoomFocusXProperty = DependencyProperty.Register("ContentZoomFocusX", typeof(double), typeof(TooledZoomAndPanControl), new FrameworkPropertyMetadata(0.0));

        /// <summary>
        /// Identifies the ContentZoomFocusY dependency property.
        /// </summary>
        public static readonly DependencyProperty ContentZoomFocusYProperty = DependencyProperty.Register("ContentZoomFocusY", typeof(double), typeof(TooledZoomAndPanControl), new FrameworkPropertyMetadata(0.0));

        /// <summary>
        /// Identifies the ViewportZoomFocusX dependency property.
        /// </summary>
        public static readonly DependencyProperty ViewportZoomFocusXProperty = DependencyProperty.Register("ViewportZoomFocusX", typeof(double), typeof(TooledZoomAndPanControl), new FrameworkPropertyMetadata(0.0));

        /// <summary>
        /// Identifies the ViewportZoomFocusY dependency property.
        /// </summary>
        public static readonly DependencyProperty ViewportZoomFocusYProperty = DependencyProperty.Register("ViewportZoomFocusY", typeof(double), typeof(TooledZoomAndPanControl), new FrameworkPropertyMetadata(0.0));

        /// <summary>
        /// Identifies the ContentViewportWidth dependency property.
        /// </summary>
        public static readonly DependencyProperty ContentViewportWidthProperty = DependencyProperty.Register("ContentViewportWidth", typeof(double), typeof(TooledZoomAndPanControl), new FrameworkPropertyMetadata(0.0));

        /// <summary>
        /// Identifies the ContentViewportHeight dependency property.
        /// </summary>
        public static readonly DependencyProperty ContentViewportHeightProperty = DependencyProperty.Register("ContentViewportHeight", typeof(double), typeof(TooledZoomAndPanControl), new FrameworkPropertyMetadata(0.0));

        /// <summary>
        /// Identifies the IsMouseWheelScrollingEnabled dependency property.
        /// </summary>
        public static readonly DependencyProperty IsMouseWheelScrollingEnabledProperty = DependencyProperty.Register("IsMouseWheelScrollingEnabled", typeof(bool), typeof(TooledZoomAndPanControl), new FrameworkPropertyMetadata(false));

        /// <summary>
        /// Identifies the ContentWidth dependency property.
        /// </summary>
        public static readonly DependencyProperty ContentWidthProperty = DependencyProperty.Register("ContentWidth", typeof(double), typeof(TooledZoomAndPanControl), new FrameworkPropertyMetadata(0.0));

        /// <summary>
        /// Identifies the ContentHeight dependency property.
        /// </summary>
        public static readonly DependencyProperty ContentHeightProperty = DependencyProperty.Register("ContentHeight", typeof(double), typeof(TooledZoomAndPanControl), new FrameworkPropertyMetadata(0.0));

        /// <summary>
        /// Identifies the OverviewWidth dependency property.
        /// </summary>
        public static readonly DependencyProperty OverviewWidthProperty = DependencyProperty.Register("OverviewWidth", typeof(double), typeof(TooledZoomAndPanControl), new FrameworkPropertyMetadata(0.0));

        /// <summary>
        /// Identifies the OverviewHeight dependency property.
        /// </summary>
        public static readonly DependencyProperty OverviewHeightProperty = DependencyProperty.Register("OverviewHeight", typeof(double), typeof(TooledZoomAndPanControl), new FrameworkPropertyMetadata(0.0));

        /// <summary>
        /// Identifies the OverviewContent attached dependency property.
        /// </summary>
        public static readonly DependencyProperty OverviewContentProperty = DependencyProperty.RegisterAttached("OverviewContent", typeof(object), typeof(TooledZoomAndPanControl), new FrameworkPropertyMetadata(null));

        /// <summary>
        /// Identifies the OverviewDefaultOpacity attached dependency property.
        /// </summary>
        public static readonly DependencyProperty OverviewDefaultOpacityProperty = DependencyProperty.RegisterAttached("OverviewDefaultOpacity", typeof(double), typeof(TooledZoomAndPanControl), new FrameworkPropertyMetadata(1.0));

        /// <summary>
        /// Identifies the OverviewVisibility attached dependency property.
        /// </summary>
        public static readonly DependencyProperty OverviewVisibilityProperty = DependencyProperty.RegisterAttached("OverviewVisibility", typeof(Visibility), typeof(TooledZoomAndPanControl), new FrameworkPropertyMetadata(Visibility.Visible));

        #endregion // Dependencies.

        #region Fields

        /// <summary>
        /// Name of the parts that have to be in the control template.
        /// </summary>
        private const string PART_ZOOM_AND_PAN_CONTROL = "PART_ZoomAndPanControl";

        /// <summary>
        /// Stores the main zoom and pan control of the overview.
        /// </summary>
        private ZoomAndPanControl mZoomAndPanControl;

        /// <summary>
        /// Stores the behavior responsible for handling the view manipulation.
        /// </summary>
        private ManipulationBehavior mManipulationBehavior;

        #endregion // Fields.

        #region Constructors

        /// <summary>
        /// Initializes the <see cref="TooledZoomAndPanControl"/> class.
        /// </summary>
        static TooledZoomAndPanControl()
        {
            TooledZoomAndPanControl.DefaultStyleKeyProperty.OverrideMetadata(typeof(TooledZoomAndPanControl), new FrameworkPropertyMetadata(typeof(TooledZoomAndPanControl)));
        }

        #endregion // Constructors.

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

        /// <summary>
        /// Set to 'true' to enable the mouse wheel to scroll the zoom and pan control.
        /// This is set to 'false' by default.
        /// </summary>
        public bool IsMouseWheelScrollingEnabled
        {
            get
            {
                return (bool)GetValue(IsMouseWheelScrollingEnabledProperty);
            }
            set
            {
                SetValue(IsMouseWheelScrollingEnabledProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the width of the content.
        /// </summary>
        public double ContentWidth
        {
            get
            {
                return (double)GetValue(ContentWidthProperty);
            }
            set
            {
                SetValue(ContentWidthProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the height of the content.
        /// </summary>
        public double ContentHeight
        {
            get
            {
                return (double)GetValue(ContentHeightProperty);
            }
            set
            {
                SetValue(ContentHeightProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the width of the overview.
        /// </summary>
        public double OverviewWidth
        {
            get
            {
                return (double)GetValue(OverviewWidthProperty);
            }
            set
            {
                SetValue(OverviewWidthProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the height of the overview.
        /// </summary>
        public double OverviewHeight
        {
            get
            {
                return (double)GetValue(OverviewHeightProperty);
            }
            set
            {
                SetValue(OverviewHeightProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the opacity of the overview when it is displayed and the mouse is not over.
        /// </summary>
        public double OverviewDefaultOpacity
        {
            get
            {
                return (double)GetValue(OverviewDefaultOpacityProperty);
            }
            set
            {
                SetValue(OverviewDefaultOpacityProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the overview visibility.
        /// </summary>
        public Visibility OverviewVisibility
        {
            get
            {
                return (Visibility)GetValue(OverviewVisibilityProperty);
            }
            set
            {
                SetValue(OverviewVisibilityProperty, value);
            }
        }

        #endregion // Properties.

        #region Methods

        /// <summary>
        /// Called when a template has been applied to the control.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.mZoomAndPanControl = this.GetTemplateChild(PART_ZOOM_AND_PAN_CONTROL) as ZoomAndPanControl;

            if (this.mZoomAndPanControl == null)
            {
                throw new Exception("TooledZoomAndPanControl control template not correctly defined.");
            }

            this.mManipulationBehavior = new ManipulationBehavior(this.mZoomAndPanControl);
        }

        /// <summary>
        /// Sets the value of the overview content.
        /// </summary>
        /// <param name="pElement">The modified control.</param>
        /// <param name="pValue">The overview content.</param>
        public static void SetOverviewContent(UIElement pElement, object pValue)
        {
            pElement.SetValue(OverviewContentProperty, pValue);
        }

        /// <summary>
        /// Gets the value of the overview content.
        /// </summary>
        /// <param name="pElement">The control.</param>
        /// <returns>The overview content.</returns>
        public static object GetOverviewContent(UIElement pElement)
        {
            return pElement.GetValue(OverviewContentProperty);
        }

        #endregion // Methods.
    }
}

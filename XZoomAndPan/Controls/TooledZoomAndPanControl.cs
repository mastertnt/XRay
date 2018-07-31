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
    [TemplatePart(Name = PART_TOOLBAR, Type = typeof(IZoomAndPanToolbar))]
    public class TooledZoomAndPanControl : AZoomAndPanControl
    {
        #region Dependencies

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

        /// <summary>
        /// Identifies the ToolbarVisibility attached dependency property.
        /// </summary>
        public static readonly DependencyProperty ToolbarVisibilityProperty = DependencyProperty.RegisterAttached("ToolbarVisibility", typeof(Visibility), typeof(TooledZoomAndPanControl), new FrameworkPropertyMetadata(Visibility.Visible));

        #endregion // Dependencies.

        #region Fields

        /// <summary>
        /// Name of the parts that have to be in the control template.
        /// </summary>
        private const string PART_ZOOM_AND_PAN_CONTROL = "PART_ZoomAndPanControl";
        private const string PART_TOOLBAR = "PART_Toolbar";

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
            TooledZoomAndPanControl.ContentOffsetXProperty.OverrideMetadata(typeof(TooledZoomAndPanControl), new FrameworkPropertyMetadata(0.0, null, OnCoerceContentOffset));
            TooledZoomAndPanControl.ContentOffsetYProperty.OverrideMetadata(typeof(TooledZoomAndPanControl), new FrameworkPropertyMetadata(0.0, null, OnCoerceContentOffset));
        }

        #endregion // Constructors.

        #region Properties

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

        /// <summary>
        /// Gets or sets the toolbar visibility.
        /// </summary>
        public Visibility ToolbarVisibility
        {
            get
            {
                return (Visibility)GetValue(ToolbarVisibilityProperty);
            }
            set
            {
                SetValue(ToolbarVisibilityProperty, value);
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

            // Toolbar is not mandatory.
            IZoomAndPanToolbar lZoomAndPanToolbar = this.GetTemplateChild(PART_TOOLBAR) as IZoomAndPanToolbar;
            if (lZoomAndPanToolbar != null)
            {
                lZoomAndPanToolbar.BindToControl(this.mZoomAndPanControl);
            }
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

        /// <summary>
        /// Do an animated zoom to view a specific scale in the given rectangle (in content coordinates).
        /// The scale center is the rectangle center.
        /// </summary>
        /// <param name="pNewScale">The new scale.</param>
        /// <param name="pContentRect">The focused rectangle.</param>
        public override void AnimatedZoomTo(double pNewScale, Rect pContentRect)
        {
            if (this.mZoomAndPanControl != null)
            {
                this.mZoomAndPanControl.AnimatedZoomTo(pNewScale, pContentRect);
            }
        }

        /// <summary>
        /// Do an animated zoom to the specified rectangle (in content coordinates).
        /// </summary>
        /// <param name="pContentRect">The focused rectangle.</param>
        public override void AnimatedZoomTo(Rect pContentRect)
        {
            if (this.mZoomAndPanControl != null)
            {
                this.mZoomAndPanControl.AnimatedZoomTo(pContentRect);
            }
        }

        /// <summary>
        /// Instantly zoom to the specified rectangle (in content coordinates).
        /// </summary>
        /// <param name="pContentRect">The focused rectangle.</param>
        public override void ZoomTo(Rect pContentRect)
        {
            if (this.mZoomAndPanControl != null)
            {
                this.mZoomAndPanControl.ZoomTo(pContentRect);
            }
        }

        /// <summary>
        /// Instantly center the view on the specified point (in content coordinates).
        /// </summary>
        /// <param name="pContentOffset">The new content offset.</param>
        public override void SnapContentOffsetTo(Point pContentOffset)
        {
            if (this.mZoomAndPanControl != null)
            {
                this.mZoomAndPanControl.SnapContentOffsetTo(pContentOffset);
            }
        }

        /// <summary>
        /// Instantly center the view on the specified point (in content coordinates).
        /// </summary>
        /// <param name="pContentPoint">The center point.</param>
        public override void SnapTo(Point pContentPoint)
        {
            if (this.mZoomAndPanControl != null)
            {
                this.mZoomAndPanControl.SnapTo(pContentPoint);
            }
        }

        /// <summary>
        /// Use animation to center the view on the specified point (in content coordinates).
        /// </summary>
        /// <param name="pContentPoint">The center point.</param>
        public override void AnimatedSnapTo(Point pContentPoint)
        {
            if (this.mZoomAndPanControl != null)
            {
                this.mZoomAndPanControl.AnimatedSnapTo(pContentPoint);
            }
        }

        /// <summary>
        /// Zoom in/out centered on the specified point (in content coordinates).
        /// The focus point is kept locked to it's on screen position (ala google maps).
        /// </summary>
        /// <param name="pNewScale">The new scale.</param>
        /// <param name="pContentZoomFocus">The center point.</param>
        public override void AnimatedZoomAboutPoint(double pNewScale, Point pContentZoomFocus)
        {
            if (this.mZoomAndPanControl != null)
            {
                this.mZoomAndPanControl.AnimatedZoomAboutPoint(pNewScale, pContentZoomFocus);
            }
        }

        /// <summary>
        /// Zoom in/out centered on the specified point (in content coordinates).
        /// The focus point is kept locked to it's on screen position (ala google maps).
        /// </summary>
        /// <param name="pNewScale">The new scale.</param>
        /// <param name="pContentZoomFocus">The center point.</param>
        public override void ZoomAboutPoint(double pNewScale, Point pContentZoomFocus)
        {
            if (this.mZoomAndPanControl != null)
            {
                this.mZoomAndPanControl.ZoomAboutPoint(pNewScale, pContentZoomFocus);
            }
        }

        /// <summary>
        /// Zoom in/out centered on the viewport center.
        /// </summary>
        /// <param name="pNewScale">The new scale.</param>
        public override void AnimatedZoomTo(double pNewScale)
        {
            if (this.mZoomAndPanControl != null)
            {
                this.mZoomAndPanControl.AnimatedZoomTo(pNewScale);
            }
        }

        /// <summary>
        /// Zoom in/out centered on the viewport center.
        /// </summary>
        /// <param name="pNewScale">The new scale.</param>
        public override void ZoomTo(double pNewScale)
        {
            if (this.mZoomAndPanControl != null)
            {
                this.mZoomAndPanControl.ZoomTo(pNewScale);
            }
        }

        /// <summary>
        /// Do animation that scales the content so that it fits completely in the control.
        /// </summary>
        public override void AnimatedScaleToFit()
        {
            if (this.mZoomAndPanControl != null)
            {
                this.mZoomAndPanControl.AnimatedScaleToFit();
            }
        }

        /// <summary>
        /// Instantly scale the content so that it fits completely in the control.
        /// </summary>
        public override void ScaleToFit()
        {
            if (this.mZoomAndPanControl != null)
            {
                this.mZoomAndPanControl.ScaleToFit();
            }
        }

        /// <summary>
        /// Zoom the viewport out, centering on the specified point (in content coordinates).
        /// </summary>
        /// <param name="pContentZoomCenter">The center of the zoom.</param>
        public override void ZoomOut(Point pContentZoomCenter)
        {
            if (this.mZoomAndPanControl != null)
            {
                this.mZoomAndPanControl.ZoomOut(pContentZoomCenter);
            }
        }

        /// <summary>
        /// Zoom the viewport in, centering on the specified point (in content coordinates).
        /// </summary>
        /// <param name="pContentZoomCenter">The center of the zoom.</param>
        public override void ZoomIn(Point pContentZoomCenter)
        {
            if (this.mZoomAndPanControl != null)
            {
                this.mZoomAndPanControl.ZoomIn(pContentZoomCenter);
            }
        }

        /// <summary>
        /// Maps the screen position to the corresponding position in the content of this control.
        /// </summary>
        /// <param name="pScreenPos">The position in screen coordinates.</param>
        /// <param name="pContentPos">The position in content coordinates, taking in account the zoom.</param>
        /// <returns>True if the position is in the content, false otherwise. The returned pContentPos is then (-1, -1).</returns>
        public override bool MapToContent(Point pScreenPos, out Point pContentPos)
        {
            if (this.mZoomAndPanControl != null)
            {
                return this.mZoomAndPanControl.MapToContent(pScreenPos, out pContentPos);
            }

            pContentPos = new Point(-1.0, -1.0);
            return false;
        }

        /// <summary>
        /// Method called to clamp the ContentOffset X or Y value to its valid range.
        /// </summary>
        /// <param name="pObject">The modified control.</param>
        /// <param name="pBaseValue">The value to coerce.</param>
        private static object OnCoerceContentOffset(DependencyObject pObject, object pBaseValue)
        {
            TooledZoomAndPanControl lControl = pObject as TooledZoomAndPanControl;
            if (lControl != null)
            {
                double lValue = System.Convert.ToDouble(pBaseValue);
                double lMinOffsetX = 0.0;
                lValue = Math.Max(lValue, lMinOffsetX);
                return lValue;
            }

            return pBaseValue;
        }

        #endregion // Methods.
    }
}

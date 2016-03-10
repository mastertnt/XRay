using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace XZoomAndPan.Controls
{
    /// <summary>
    /// Class defining the control showing the overview.
    /// </summary>
    [TemplatePart(Name = PART_ZOOM_AND_PAN_CONTROL, Type = typeof(ZoomAndPanControl))]
    [TemplatePart(Name = PART_VIEWPORT_OVERVIEW, Type = typeof(ViewportOverview))]
    public class OverviewControl : ContentControl
    {
        #region Dependencies

        /// <summary>
        /// Identifies the ContentWidth dependency property.
        /// </summary>
        public static readonly DependencyProperty ContentWidthProperty = DependencyProperty.Register("ContentWidth", typeof(double), typeof(OverviewControl), new FrameworkPropertyMetadata(0.0));

        /// <summary>
        /// Identifies the ContentHeight dependency property.
        /// </summary>
        public static readonly DependencyProperty ContentHeightProperty = DependencyProperty.Register("ContentHeight", typeof(double), typeof(OverviewControl), new FrameworkPropertyMetadata(0.0));
        
        /// <summary>
        /// Identifies the ContentOffsetX dependency property.
        /// </summary>
        public static readonly DependencyProperty ContentOffsetXProperty = DependencyProperty.Register("ContentOffsetX", typeof(double), typeof(OverviewControl), new FrameworkPropertyMetadata(0.0));

        /// <summary>
        /// Identifies the ContentOffsetY dependency property.
        /// </summary>
        public static readonly DependencyProperty ContentOffsetYProperty = DependencyProperty.Register("ContentOffsetY", typeof(double), typeof(OverviewControl), new FrameworkPropertyMetadata(0.0));

        /// <summary>
        /// Identifies the ContentViewportWidth dependency property.
        /// </summary>
        public static readonly DependencyProperty ContentViewportWidthProperty = DependencyProperty.Register("ContentViewportWidth", typeof(double), typeof(OverviewControl), new FrameworkPropertyMetadata(0.0, null, OnCoerceContentViewportWidthCallback));

        /// <summary>
        /// Identifies the ContentViewportHeight dependency property.
        /// </summary>
        public static readonly DependencyProperty ContentViewportHeightProperty = DependencyProperty.Register("ContentViewportHeight", typeof(double), typeof(OverviewControl), new FrameworkPropertyMetadata(0.0, null, OnCoerceContentViewportHeightCallback));

        /// <summary>
        /// Identifies the DefaultOpacity attached dependency property.
        /// </summary>
        public static readonly DependencyProperty DefaultOpacityProperty = DependencyProperty.RegisterAttached("DefaultOpacity", typeof(double), typeof(OverviewControl), new FrameworkPropertyMetadata(1.0));

        #endregion // Dependencies.

        #region Fields

        /// <summary>
        /// Name of the parts that have to be in the control template.
        /// </summary>
        private const string PART_ZOOM_AND_PAN_CONTROL = "PART_ZoomAndPanControl";
        private const string PART_VIEWPORT_OVERVIEW = "PART_ViewportOverview";

        /// <summary>
        /// Stores the main zoom and pan control of the overview.
        /// </summary>
        private ZoomAndPanControl mZoomAndPanControl;

        /// <summary>
        /// Stores the viewport overview.
        /// </summary>
        private ViewportOverview mViewportOverview;

        #endregion // Fields.

        #region Constructors

        /// <summary>
        /// Initializes the <see cref="OverviewControl"/> class.
        /// </summary>
        static OverviewControl()
        {
            OverviewControl.DefaultStyleKeyProperty.OverrideMetadata(typeof(OverviewControl), new FrameworkPropertyMetadata(typeof(OverviewControl)));
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
        /// Gets or sets the opacity of the overview when it is displayed and the mouse is not over.
        /// </summary>
        public double DefaultOpacity
        {
            get
            {
                return (double)GetValue(DefaultOpacityProperty);
            }
            set
            {
                SetValue(DefaultOpacityProperty, value);
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
            this.mViewportOverview = this.GetTemplateChild(PART_VIEWPORT_OVERVIEW) as ViewportOverview;

            if (this.mZoomAndPanControl == null || this.mViewportOverview == null)
            {
                throw new Exception("OverviewControl control template not correctly defined.");
            }

            this.mZoomAndPanControl.SizeChanged += this.OnZoomAndPanControlSizeChanged;
            this.mViewportOverview.DragDelta += this.OnViewportOverviewDragDelta;
        }

        /// <summary>
        /// This delegate is called when the item is dragged.
        /// </summary>
        /// <param name="pEventSender">The event sender.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private void OnViewportOverviewDragDelta(Object pEventSender, DragDeltaEventArgs pEventArgs)
        {
            // Update the position of the overview rect as the user drags it around.
            double lNewContentOffsetX = Math.Min(Math.Max(0.0, Canvas.GetLeft(this.mViewportOverview) + pEventArgs.HorizontalChange), this.ContentWidth - this.ContentViewportWidth);
            double lNewContentOffsetY = Math.Min(Math.Max(0.0, Canvas.GetTop(this.mViewportOverview) + pEventArgs.VerticalChange), this.ContentHeight - this.ContentViewportHeight);
            Canvas.SetLeft(this.mViewportOverview, lNewContentOffsetX);
            Canvas.SetTop(this.mViewportOverview, lNewContentOffsetY);
        }

        /// <summary>
        /// Event raised when the size of the ZoomAndPanControl changes.
        /// </summary>
        /// <param name="pSender">The modified control.</param>
        /// <param name="pEventArgs">the event arguments.</param>
        private void OnZoomAndPanControlSizeChanged(object pSender, SizeChangedEventArgs pEventArgs)
        {
            // Update the scale so that the entire content fits in the window.
            this.mZoomAndPanControl.ScaleToFit();
        }

        /// <summary>
        /// Delegate called to coerce the ContentViewportWidth property.
        /// </summary>
        /// <param name="pObject">The modified control.</param>
        /// <param name="pBaseValue">The value to coerce.</param>
        /// <returns>The coerced value.</returns>
        public static object OnCoerceContentViewportWidthCallback(DependencyObject pObject, object pBaseValue)
        {
            OverviewControl lControl = pObject as OverviewControl;
            if (lControl != null)
            {
                double lContentViewportWidth = System.Convert.ToDouble(pBaseValue);
                if (lContentViewportWidth > lControl.ContentWidth)
                {
                    return lControl.ContentWidth;
                }
            }

            return pBaseValue;
        }

        /// <summary>
        /// Delegate called to coerce the ContentViewportHeight property.
        /// </summary>
        /// <param name="pObject">The modified control.</param>
        /// <param name="pBaseValue">The value to coerce.</param>
        /// <returns>The coerced value.</returns>
        public static object OnCoerceContentViewportHeightCallback(DependencyObject pObject, object pBaseValue)
        {
            OverviewControl lControl = pObject as OverviewControl;
            if (lControl != null)
            {
                double lContentViewportHeight = System.Convert.ToDouble(pBaseValue);
                if (lContentViewportHeight > lControl.ContentHeight)
                {
                    return lControl.ContentHeight;
                }
            }

            return pBaseValue;
        }

        /// <summary>
        /// Delegate called when the mouse enters in the control zone.
        /// </summary>
        /// <param name="pEventArgs">The event arguments.</param>
        protected override void OnMouseEnter(MouseEventArgs pEventArgs)
        {
            base.OnMouseEnter(pEventArgs);
            this.UpdateVisualState();
        }

        /// <summary>
        /// Delegate called when the mouse leaves in the control zone.
        /// </summary>
        /// <param name="pEventArgs">The event arguments.</param>
        protected override void OnMouseLeave(MouseEventArgs pEventArgs)
        {
            base.OnMouseLeave(pEventArgs);
            this.UpdateVisualState();
        }

        /// <summary>
        /// Update the visual state of the control.
        /// </summary>
        private void UpdateVisualState()
        {
            if (this.IsMouseOver)
            {
                VisualStateManager.GoToState(this, "MouseOver", true);
            }
            else
            {
                VisualStateManager.GoToState(this, "Normal", true);
            }
        }

        #endregion // Methods.
    }
}

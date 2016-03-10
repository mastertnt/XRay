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

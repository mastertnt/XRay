using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using XZoomAndPan.Controls;

namespace XZoomAndPan.Behaviors
{
    /// <summary>
    /// Class defining the behavior handling the zoom and pan control manipulation.
    /// </summary>
    public class ManipulationBehavior
    {
        #region Fields

        /// <summary>
        /// Stores the main zoom and pan control of the overview.
        /// </summary>
        private ZoomAndPanControl mZoomAndPanControl;

        /// <summary>
        /// Specifies the current state of the mouse handling logic.
        /// </summary>
        private MouseHandlingMode mMouseHandlingMode;

        /// <summary>
        /// The point that was clicked relative to the ZoomAndPanControl.
        /// </summary>
        private Point mOrigZoomAndPanControlMouseDownPoint;

        /// <summary>
        /// The point that was clicked relative to the content that is contained within the ZoomAndPanControl.
        /// </summary>
        private Point mOrigContentMouseDownPoint;

        #endregion // Fields.

        #region Constructors

        /// <summary>
        /// Initializes a new instance of a <see cref="ManipulationBehavior"/> class.
        /// </summary>
        /// <param name="pZoomAndPanControl">The manipulated control.</param>
        public ManipulationBehavior(ZoomAndPanControl pZoomAndPanControl)
        {
            this.mZoomAndPanControl = pZoomAndPanControl;
            this.mZoomAndPanControl.MouseDown += this.OnZoomAndPanControlMouseDown;
            this.mZoomAndPanControl.MouseMove += this.OnZoomAndPanControlMouseMove;
            this.mZoomAndPanControl.MouseUp += this.OnZoomAndPanControlMouseUp;
            this.mZoomAndPanControl.PreviewMouseWheel += this.OnZoomAndPanControlMouseWheel;

            FrameworkElement lContent = this.mZoomAndPanControl.Content as FrameworkElement;
            if (lContent != null)
            {
                lContent.MouseWheel += this.OnZoomAndPanControlMouseWheel;
            }

            this.mMouseHandlingMode = MouseHandlingMode.None;
        }

        #endregion // Constructors.

        #region Methods

        /// <summary>
        /// Event raised on mouse down in the ZoomAndPanControl.
        /// </summary>
        /// <param name="pSender">The modified control.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private void OnZoomAndPanControlMouseDown(object pSender, MouseButtonEventArgs pEventArgs)
        {
            FrameworkElement lContent = this.mZoomAndPanControl.Content as FrameworkElement;
            if (lContent == null)
            {
                return;
            }

            lContent.Focus();
            Keyboard.Focus(lContent);
            
            this.mOrigZoomAndPanControlMouseDownPoint = pEventArgs.GetPosition(this.mZoomAndPanControl);
            this.mOrigContentMouseDownPoint = pEventArgs.GetPosition(lContent);

            if (pEventArgs.ChangedButton == MouseButton.Middle)
            {
                // Initiates panning mode.
                this.mMouseHandlingMode = MouseHandlingMode.Panning;
            }

            pEventArgs.Handled = true;
        }

        /// <summary>
        /// Event raised on mouse move in the ZoomAndPanControl.
        /// </summary>
        /// <param name="pSender">The modified control.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private void OnZoomAndPanControlMouseMove(object pSender, MouseEventArgs pEventArgs)
        {
            FrameworkElement lContent = this.mZoomAndPanControl.Content as FrameworkElement;
            if (lContent == null)
            {
                return;
            }

            if (this.mMouseHandlingMode == MouseHandlingMode.Panning)
            {
                // The user is left-dragging the mouse. Pan the viewport by the appropriate amount.
                Point lCurContentMousePoint = pEventArgs.GetPosition(lContent);
                Vector lDragOffset = lCurContentMousePoint - this.mOrigContentMouseDownPoint;

                double lNewContentOffsetX = Math.Min(Math.Max(0.0, this.mZoomAndPanControl.ContentOffsetX - lDragOffset.X), 1000 - this.mZoomAndPanControl.ContentViewportWidth);
                double lNewContentOffsetY = Math.Min(Math.Max(0.0, this.mZoomAndPanControl.ContentOffsetY - lDragOffset.Y), 800 - this.mZoomAndPanControl.ContentViewportHeight);
                this.mZoomAndPanControl.ContentOffsetX = lNewContentOffsetX;
                this.mZoomAndPanControl.ContentOffsetY = lNewContentOffsetY;

                pEventArgs.Handled = true;
            }
        }

        /// <summary>
        /// Event raised on mouse up in the ZoomAndPanControl.
        /// </summary>
        /// <param name="pSender">The modified control.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private void OnZoomAndPanControlMouseUp(object pSender, MouseButtonEventArgs pEventArgs)
        {
            if (this.mMouseHandlingMode != MouseHandlingMode.None)
            {
                this.mMouseHandlingMode = MouseHandlingMode.None;
            }
        }

        /// <summary>
        /// Event raised by rotating the mouse wheel.
        /// </summary>
        /// <param name="pSender">The modified control.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private void OnZoomAndPanControlMouseWheel(object pSender, MouseWheelEventArgs pEventArgs)
        {
            FrameworkElement lContent = this.mZoomAndPanControl.Content as FrameworkElement;
            if (lContent == null)
            {
                return;
            }

            if ((Keyboard.Modifiers & ModifierKeys.Control) != 0)
            {
                // Zooming.
                if (pEventArgs.Delta > 0)
                {
                    Point lCurContentMousePoint = pEventArgs.GetPosition(lContent);
                    this.mZoomAndPanControl.ZoomIn(lCurContentMousePoint);
                }
                else if (pEventArgs.Delta < 0)
                {
                    Point lCurContentMousePoint = pEventArgs.GetPosition(lContent);
                    this.mZoomAndPanControl.ZoomOut(lCurContentMousePoint);
                }
            }
            else if ((Keyboard.Modifiers & ModifierKeys.Shift) != 0)
            {
                // Horizontal scroll.
                double lNewContentOffsetX = Math.Min(Math.Max(0.0, this.mZoomAndPanControl.ContentOffsetX - pEventArgs.Delta), 1000 - this.mZoomAndPanControl.ContentViewportWidth);
                this.mZoomAndPanControl.ContentOffsetX = lNewContentOffsetX; 
            }
            else
            {
                // Vertical scroll.
                double lNewContentOffsetY = Math.Min(Math.Max(0.0, this.mZoomAndPanControl.ContentOffsetY - pEventArgs.Delta), 800 - this.mZoomAndPanControl.ContentViewportHeight);
                this.mZoomAndPanControl.ContentOffsetY = lNewContentOffsetY;                              
            }

            pEventArgs.Handled = true;
        }

        #endregion // Methods.
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace XZoomAndPan.Controls
{
    /// <summary>
    /// This is an extension to the ZoomAndPanControol class that implements
    /// the IScrollInfo interface properties and functions.
    /// 
    /// IScrollInfo is implemented to allow ZoomAndPanControl to be wrapped (in XAML)
    /// in a ScrollViewer.  IScrollInfo allows the ScrollViewer and ZoomAndPanControl to 
    /// communicate important information such as the horizontal and vertical scrollbar offsets.
    /// 
    /// There is a good series of articles showing how to implement IScrollInfo starting here:
    ///     http://blogs.msdn.com/bencon/archive/2006/01/05/509991.aspx
    ///     
    /// </summary>
    public partial class ZoomAndPanControl : IScrollInfo
    {
        #region Properties

        /// <summary>
        /// Gets or sets if the vertical scrollbar is enabled.
        /// </summary>
        public bool CanVerticallyScroll
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets if the horizontal scrollbar is enabled.
        /// </summary>
        public bool CanHorizontallyScroll
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the width of the content (with 'ContentScale' applied).
        /// </summary>
        public double ExtentWidth
        {
            get
            {
                return this.mUnScaledExtent.Width * this.ContentScale;
            }
        }

        /// <summary>
        /// The height of the content (with 'ContentScale' applied).
        /// </summary>
        public double ExtentHeight
        {
            get
            {
                return this.mUnScaledExtent.Height * this.ContentScale;
            }
        }

        /// <summary>
        /// Get the width of the viewport onto the content.
        /// </summary>
        public double ViewportWidth
        {
            get
            {
                return this.mViewport.Width;
            }
        }

        /// <summary>
        /// Get the height of the viewport onto the content.
        /// </summary>
        public double ViewportHeight
        {
            get
            {
                return this.mViewport.Height;
            }
        }

        /// <summary>
        /// Reference to the ScrollViewer that is wrapped (in XAML) around the ZoomAndPanControl.
        /// Or set to null if there is no ScrollViewer.
        /// </summary>
        public ScrollViewer ScrollOwner
        {
            get;
            set;
        }

        /// <summary>
        /// The offset of the horizontal scrollbar.
        /// </summary>
        public double HorizontalOffset
        {
            get
            {
                return this.ContentOffsetX * this.ContentScale;
            }
        }

        /// <summary>
        /// The offset of the vertical scrollbar.
        /// </summary>
        public double VerticalOffset
        {
            get
            {
                return this.ContentOffsetY * this.ContentScale;
            }
        }

        #endregion // Properties.

        #region Methods

        /// <summary>
        /// Called when the offset of the horizontal scrollbar has been set.
        /// </summary>
        /// <param name="pOffset">The horizontal offset.</param>
        public void SetHorizontalOffset(double pOffset)
        {
            if (this.mDisableScrollOffsetSync)
            {
                return;
            }

            try
            {
                this.mDisableScrollOffsetSync = true;
                this.ContentOffsetX = pOffset / this.ContentScale;
            }
            finally
            {
                this.mDisableScrollOffsetSync = false;
            }
        }

        /// <summary>
        /// Called when the offset of the vertical scrollbar has been set.
        /// </summary>
        /// <param name="pOffset">The vertical offset.</param>
        public void SetVerticalOffset(double pOffset)
        {
            if (this.mDisableScrollOffsetSync)
            {
                return;
            }

            try
            {
                this.mDisableScrollOffsetSync = true;
                this.ContentOffsetY = pOffset / this.ContentScale;
            }
            finally
            {
                mDisableScrollOffsetSync = false;
            }
        }

        /// <summary>
        /// Shift the content offset one line up.
        /// </summary>
        public void LineUp()
        {
            this.ContentOffsetY -= (this.ContentViewportHeight / 10);
        }

        /// <summary>
        /// Shift the content offset one line down.
        /// </summary>
        public void LineDown()
        {
            this.ContentOffsetY += (this.ContentViewportHeight / 10);
        }

        /// <summary>
        /// Shift the content offset one line left.
        /// </summary>
        public void LineLeft()
        {
            this.ContentOffsetX -= (this.ContentViewportWidth / 10);
        }

        /// <summary>
        /// Shift the content offset one line right.
        /// </summary>
        public void LineRight()
        {
            this.ContentOffsetX += (this.ContentViewportWidth / 10);
        }

        /// <summary>
        /// Shift the content offset one page up.
        /// </summary>
        public void PageUp()
        {
            this.ContentOffsetY -= this.ContentViewportHeight;
        }

        /// <summary>
        /// Shift the content offset one page down.
        /// </summary>
        public void PageDown()
        {
            this.ContentOffsetY += this.ContentViewportHeight;
        }

        /// <summary>
        /// Shift the content offset one page left.
        /// </summary>
        public void PageLeft()
        {
            this.ContentOffsetX -= this.ContentViewportWidth;
        }

        /// <summary>
        /// Shift the content offset one page right.
        /// </summary>
        public void PageRight()
        {
            this.ContentOffsetX += this.ContentViewportWidth;
        }

        /// <summary>
        /// Don't handle mouse wheel input from the ScrollViewer, the mouse wheel is
        /// used for zooming in and out, not for manipulating the scrollbars.
        /// </summary>
        public void MouseWheelDown()
        {
            this.LineDown();
        }

        /// <summary>
        /// Don't handle mouse wheel input from the ScrollViewer, the mouse wheel is
        /// used for zooming in and out, not for manipulating the scrollbars.
        /// </summary>
        public void MouseWheelLeft()
        {
            this.LineLeft();
        }

        /// <summary>
        /// Don't handle mouse wheel input from the ScrollViewer, the mouse wheel is
        /// used for zooming in and out, not for manipulating the scrollbars.
        /// </summary>
        public void MouseWheelRight()
        {
            this.LineRight();
        }

        /// <summary>
        /// Don't handle mouse wheel input from the ScrollViewer, the mouse wheel is
        /// used for zooming in and out, not for manipulating the scrollbars.
        /// </summary>
        public void MouseWheelUp()
        {
            this.LineUp();
        }

        /// <summary>
        /// Bring the specified rectangle to view.
        /// </summary>
        /// <param name="pVisual">The visual to focus.</param>
        /// <param name="pRectangle">The visual bounds.</param>
        public Rect MakeVisible(Visual pVisual, Rect pRectangle)
        {
            if (this.mContentPresenter.IsAncestorOf(pVisual))
            {
                Rect lTransformedRect = pVisual.TransformToAncestor(this.mContentPresenter).TransformBounds(pRectangle);
                Rect lViewportRect = new Rect(this.ContentOffsetX, this.ContentOffsetY, this.ContentViewportWidth, this.ContentViewportHeight);
                if (lTransformedRect.Contains(lViewportRect) == false)
                {
                    double lHorizOffset = 0;
                    double lVertOffset = 0;

                    if (lTransformedRect.Left < lViewportRect.Left)
                    {
                        // Want to move viewport left.
                        lHorizOffset = lTransformedRect.Left - lViewportRect.Left;
                    }
                    else if (lTransformedRect.Right > lViewportRect.Right)
                    {
                        // Want to move viewport right.
                        lHorizOffset = lTransformedRect.Right - lViewportRect.Right;
                    }

                    if (lTransformedRect.Top < lViewportRect.Top)
                    {
                        // Want to move viewport up.
                        lVertOffset = lTransformedRect.Top - lViewportRect.Top;
                    }
                    else if (lTransformedRect.Bottom > lViewportRect.Bottom)
                    {
                        // Want to move viewport down.
                        lVertOffset = lTransformedRect.Bottom - lViewportRect.Bottom;
                    }

                    this.SnapContentOffsetTo(new Point(this.ContentOffsetX + lHorizOffset, this.ContentOffsetY + lVertOffset));
                }
            }

            return pRectangle;
        }

        #endregion // Methods.
    }
}

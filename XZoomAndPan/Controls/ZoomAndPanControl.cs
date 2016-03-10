using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using XZoomAndPan.Helpers;

namespace XZoomAndPan.Controls
{
    /// <summary>
    /// Class wrapping up zooming and panning of it's content.
    /// </summary>
    [TemplatePart(Name = PART_CONTENT_PRESENTER, Type = typeof(FrameworkElement))]
    public partial class ZoomAndPanControl : AZoomAndPanControl
    {
        #region Fields

        /// <summary>
        /// Name of the parts that have to be in the control template.
        /// </summary>
        private const string PART_CONTENT_PRESENTER = "PART_ContentPresenter";

        /// <summary>
        /// Stores the underlying content presenter.
        /// </summary>
        private FrameworkElement mContentPresenter;

        /// <summary>
        /// Stores the transform that is applied to the content to scale it by 'ContentScale'.
        /// </summary>
        private ScaleTransform mContentScaleTransform;

        /// <summary>
        /// Stores the transform that is applied to the content to offset it by 'ContentOffsetX' and 'ContentOffsetY'.
        /// </summary>
        private TranslateTransform mContentOffsetTransform;

        /// <summary>
        /// Enable the update of the content offset as the content scale changes.
        /// This enabled for zooming about a point (google-maps style zooming) and zooming to a rect.
        /// </summary>
        private bool mEnableContentOffsetUpdateFromScale;

        /// <summary>
        /// Used to disable syncronization between IScrollInfo interface and ContentOffsetX/ContentOffsetY.
        /// </summary>
        private bool mDisableScrollOffsetSync;

        /// <summary>
        /// Normally when content offsets changes the content focus is automatically updated.
        /// This syncronization is disabled when 'disableContentFocusSync' is set to 'true'.
        /// When we are zooming in or out we 'disableContentFocusSync' is set to 'true' because 
        /// we are zooming in or out relative to the content focus we don't want to update the focus.
        /// </summary>
        private bool mDisableContentFocusSync;

        /// <summary>
        /// The width of the viewport in content coordinates, clamped to the width of the content.
        /// </summary>
        private double mConstrainedContentViewportWidth;

        /// <summary>
        /// The height of the viewport in content coordinates, clamped to the height of the content.
        /// </summary>
        private double mConstrainedContentViewportHeight;

        /// <summary>
        /// Records the unscaled extent of the content.
        /// This is calculated during the measure and arrange.
        /// </summary>
        private Size mUnScaledExtent;

        /// <summary>
        /// Records the size of the viewport (in viewport coordinates) onto the content.
        /// This is calculated during the measure and arrange.
        /// </summary>
        private Size mViewport;

        #endregion // Fields.

        #region Constructors

        /// <summary>
        /// Initializes the <see cref="ZoomAndPanControl"/> class.
        /// </summary>
        static ZoomAndPanControl()
        {
            ZoomAndPanControl.DefaultStyleKeyProperty.OverrideMetadata(typeof(ZoomAndPanControl), new FrameworkPropertyMetadata(typeof(ZoomAndPanControl)));
            ZoomAndPanControl.ContentScaleProperty.OverrideMetadata(typeof(ZoomAndPanControl), new FrameworkPropertyMetadata(1.0, OnContentScaleChanged, OnCoerceContentScale));
            ZoomAndPanControl.MinContentScaleProperty.OverrideMetadata(typeof(ZoomAndPanControl), new FrameworkPropertyMetadata(0.01, OnMinOrMaxContentScaleChanged));
            ZoomAndPanControl.MaxContentScaleProperty.OverrideMetadata(typeof(ZoomAndPanControl), new FrameworkPropertyMetadata(10.0, OnMinOrMaxContentScaleChanged));
            ZoomAndPanControl.ContentOffsetXProperty.OverrideMetadata(typeof(ZoomAndPanControl), new FrameworkPropertyMetadata(0.0, OnContentOffsetXChanged, OnCoerceContentOffsetX));
            ZoomAndPanControl.ContentOffsetYProperty.OverrideMetadata(typeof(ZoomAndPanControl), new FrameworkPropertyMetadata(0.0, OnContentOffsetYChanged, OnCoerceContentOffsetY));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ZoomAndPanControl"/> class.
        /// </summary>
        public ZoomAndPanControl()
        {
            this.mContentPresenter = null;
            this.mContentScaleTransform = null;
            this.mContentOffsetTransform = null;

            this.mEnableContentOffsetUpdateFromScale = false;
            this.mDisableScrollOffsetSync = false;
            this.mConstrainedContentViewportWidth = 0.0;
            this.mConstrainedContentViewportHeight = 0.0;

            this.mUnScaledExtent = new Size(0, 0);
            this.mViewport = new Size(0, 0);
        }

        #endregion // Constructors.

        #region Methods

        /// <summary>
        /// Called when a template has been applied to the control.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.mContentPresenter = this.GetTemplateChild(PART_CONTENT_PRESENTER) as FrameworkElement;

            if (this.mContentPresenter == null)
            {
                throw new Exception("ZoomAndPanControl control template not correctly defined.");
            }

            // Setup the transform on the content so that we can scale it by 'ContentScale'.
            this.mContentScaleTransform = new ScaleTransform(this.ContentScale, this.ContentScale);

            // Setup the transform on the content so that we can translate it by 'ContentOffsetX' and 'ContentOffsetY'.
            this.mContentOffsetTransform = new TranslateTransform();
            this.UpdateTranslationX();
            this.UpdateTranslationY();

            // Setup a transform group to contain the translation and scale transforms, and then
            // assign this to the content's 'RenderTransform'.
            TransformGroup lTransformGroup = new TransformGroup();
            lTransformGroup.Children.Add(this.mContentOffsetTransform);
            lTransformGroup.Children.Add(this.mContentScaleTransform);
            this.mContentPresenter.RenderTransform = lTransformGroup;
        }

        /// <summary>
        /// Do an animated zoom to view a specific scale in the given rectangle (in content coordinates).
        /// The scale center is the rectangle center.
        /// </summary>
        /// <param name="pNewScale">The new scale.</param>
        /// <param name="pContentRect">The focused rectangle.</param>
        public void AnimatedZoomTo(double pNewScale, Rect pContentRect)
        {
            this.AnimatedZoomPointToViewportCenter(pNewScale, new Point(pContentRect.X + (pContentRect.Width / 2), pContentRect.Y + (pContentRect.Height / 2)),
                delegate(object pSender, EventArgs pEventArgs)
                {
                    // At the end of the animation, ensure that we are snapped to the specified content offset.
                    // Due to zooming in on the content focus point and rounding errors, the content offset may
                    // be slightly off what we want at the end of the animation and this bit of code corrects it.
                    this.ContentOffsetX = pContentRect.X;
                    this.ContentOffsetY = pContentRect.Y;
                });
        }

        /// <summary>
        /// Do an animated zoom to the specified rectangle (in content coordinates).
        /// </summary>
        /// <param name="pContentRect">The focused rectangle.</param>
        public void AnimatedZoomTo(Rect pContentRect)
        {
            double lScaleX = this.ContentViewportWidth / pContentRect.Width;
            double lScaleY = this.ContentViewportHeight / pContentRect.Height;
            double lNewScale = this.ContentScale * Math.Min(lScaleX, lScaleY);

            this.AnimatedZoomPointToViewportCenter(lNewScale, new Point(pContentRect.X + (pContentRect.Width / 2), pContentRect.Y + (pContentRect.Height / 2)), null);
        }

        /// <summary>
        /// Instantly zoom to the specified rectangle (in content coordinates).
        /// </summary>
        /// <param name="pContentRect">The focused rectangle.</param>
        public void ZoomTo(Rect pContentRect)
        {
            double lScaleX = this.ContentViewportWidth / pContentRect.Width;
            double lScaleY = this.ContentViewportHeight / pContentRect.Height;
            double lNewScale = this.ContentScale * Math.Min(lScaleX, lScaleY);

            this.ZoomPointToViewportCenter(lNewScale, new Point(pContentRect.X + (pContentRect.Width / 2), pContentRect.Y + (pContentRect.Height / 2)));
        }

        /// <summary>
        /// Instantly center the view on the specified point (in content coordinates).
        /// </summary>
        /// <param name="pContentOffset">The new content offset.</param>
        public void SnapContentOffsetTo(Point pContentOffset)
        {
            AnimationHelper.CancelAnimation(this, ContentOffsetXProperty);
            AnimationHelper.CancelAnimation(this, ContentOffsetYProperty);

            this.ContentOffsetX = pContentOffset.X;
            this.ContentOffsetY = pContentOffset.Y;
        }

        /// <summary>
        /// Instantly center the view on the specified point (in content coordinates).
        /// </summary>
        /// <param name="pContentPoint">The center point.</param>
        public void SnapTo(Point pContentPoint)
        {
            AnimationHelper.CancelAnimation(this, ContentOffsetXProperty);
            AnimationHelper.CancelAnimation(this, ContentOffsetYProperty);

            this.ContentOffsetX = pContentPoint.X - (this.ContentViewportWidth / 2);
            this.ContentOffsetY = pContentPoint.Y - (this.ContentViewportHeight / 2);
        }

        /// <summary>
        /// Use animation to center the view on the specified point (in content coordinates).
        /// </summary>
        /// <param name="pContentPoint">The center point.</param>
        public void AnimatedSnapTo(Point pContentPoint)
        {
            double lNewX = pContentPoint.X - (this.ContentViewportWidth / 2);
            double lNewY = pContentPoint.Y - (this.ContentViewportHeight / 2);

            AnimationHelper.StartAnimation(this, ContentOffsetXProperty, lNewX, this.AnimationDuration);
            AnimationHelper.StartAnimation(this, ContentOffsetYProperty, lNewY, this.AnimationDuration);
        }

        /// <summary>
        /// Zoom in/out centered on the specified point (in content coordinates).
        /// The focus point is kept locked to it's on screen position (ala google maps).
        /// </summary>
        /// <param name="pNewScale">The new scale.</param>
        /// <param name="pContentZoomFocus">The center point.</param>
        public void AnimatedZoomAboutPoint(double pNewScale, Point pContentZoomFocus)
        {
            pNewScale = Math.Min(Math.Max(pNewScale, this.MinContentScale), this.MaxContentScale);

            AnimationHelper.CancelAnimation(this, ContentZoomFocusXProperty);
            AnimationHelper.CancelAnimation(this, ContentZoomFocusYProperty);
            AnimationHelper.CancelAnimation(this, ViewportZoomFocusXProperty);
            AnimationHelper.CancelAnimation(this, ViewportZoomFocusYProperty);

            this.ContentZoomFocusX = pContentZoomFocus.X;
            this.ContentZoomFocusY = pContentZoomFocus.Y;
            this.ViewportZoomFocusX = (this.ContentZoomFocusX - ContentOffsetX) * this.ContentScale;
            this.ViewportZoomFocusY = (this.ContentZoomFocusY - ContentOffsetY) * this.ContentScale;

            // When zooming about a point make updates to ContentScale also update content offset.
            this.mEnableContentOffsetUpdateFromScale = true;

            AnimationHelper.StartAnimation(this, ContentScaleProperty, pNewScale, this.AnimationDuration,
                delegate(object pSender, EventArgs pEventArgs)
                {
                    this.mEnableContentOffsetUpdateFromScale = false;
                    this.ResetViewportZoomFocus();
                });
        }

        /// <summary>
        /// Zoom in/out centered on the specified point (in content coordinates).
        /// The focus point is kept locked to it's on screen position (ala google maps).
        /// </summary>
        /// <param name="pNewScale">The new scale.</param>
        /// <param name="pContentZoomFocus">The center point.</param>
        public void ZoomAboutPoint(double pNewScale, Point pContentZoomFocus)
        {
            pNewScale = Math.Min(Math.Max(pNewScale, this.MinContentScale), this.MaxContentScale);

            double lScreenSpaceZoomOffsetX = (pContentZoomFocus.X - this.ContentOffsetX) * this.ContentScale;
            double lScreenSpaceZoomOffsetY = (pContentZoomFocus.Y - this.ContentOffsetY) * this.ContentScale;
            double lContentSpaceZoomOffsetX = lScreenSpaceZoomOffsetX / pNewScale;
            double lContentSpaceZoomOffsetY = lScreenSpaceZoomOffsetY / pNewScale;
            double lNewContentOffsetX = pContentZoomFocus.X - lContentSpaceZoomOffsetX;
            double lNewContentOffsetY = pContentZoomFocus.Y - lContentSpaceZoomOffsetY;

            AnimationHelper.CancelAnimation(this, ContentScaleProperty);
            AnimationHelper.CancelAnimation(this, ContentOffsetXProperty);
            AnimationHelper.CancelAnimation(this, ContentOffsetYProperty);

            this.ContentScale = pNewScale;
            this.ContentOffsetX = lNewContentOffsetX;
            this.ContentOffsetY = lNewContentOffsetY;
        }

        /// <summary>
        /// Zoom in/out centered on the viewport center.
        /// </summary>
        /// <param name="pNewScale">The new scale.</param>
        public void AnimatedZoomTo(double pNewScale)
        {
            Point lZoomCenter = new Point(this.ContentOffsetX + (this.ContentViewportWidth / 2), this.ContentOffsetY + (this.ContentViewportHeight / 2));
            this.AnimatedZoomAboutPoint(pNewScale, lZoomCenter);
        }

        /// <summary>
        /// Zoom in/out centered on the viewport center.
        /// </summary>
        /// <param name="pNewScale">The new scale.</param>
        public void ZoomTo(double pNewScale)
        {
            Point lZoomCenter = new Point(this.ContentOffsetX + (this.ContentViewportWidth / 2), this.ContentOffsetY + (this.ContentViewportHeight / 2));
            this.ZoomAboutPoint(pNewScale, lZoomCenter);
        }

        /// <summary>
        /// Do animation that scales the content so that it fits completely in the control.
        /// </summary>
        public void AnimatedScaleToFit()
        {
            this.AnimatedZoomTo(new Rect(0, 0, this.mContentPresenter.ActualWidth, this.mContentPresenter.ActualHeight));
        }

        /// <summary>
        /// Instantly scale the content so that it fits completely in the control.
        /// </summary>
        public void ScaleToFit()
        {
            this.ZoomTo(new Rect(0, 0, this.mContentPresenter.ActualWidth, this.mContentPresenter.ActualHeight));
        }

        /// <summary>
        /// Zoom the viewport out, centering on the specified point (in content coordinates).
        /// </summary>
        /// <param name="pContentZoomCenter">The center of the zoom.</param>
        public void ZoomOut(Point pContentZoomCenter)
        {
            this.ZoomAboutPoint(this.ContentScale - this.ContentScaleStep, pContentZoomCenter);
        }

        /// <summary>
        /// Zoom the viewport in, centering on the specified point (in content coordinates).
        /// </summary>
        /// <param name="pContentZoomCenter">The center of the zoom.</param>
        public void ZoomIn(Point pContentZoomCenter)
        {
            this.ZoomAboutPoint(this.ContentScale + this.ContentScaleStep, pContentZoomCenter);
        }

        /// <summary>
        /// Zoom to the specified scale and move the specified focus point to the center of the viewport.
        /// </summary>
        /// <param name="pNewScale">The new scale.</param>
        /// <param name="pContentZoomFocus">The center point.</param>
        /// <param name="pCallback">The callback executed by the animation.</param>
        private void AnimatedZoomPointToViewportCenter(double pNewScale, Point pContentZoomFocus, EventHandler pCallback)
        {
            pNewScale = Math.Min(Math.Max(pNewScale, this.MinContentScale), this.MaxContentScale);

            AnimationHelper.CancelAnimation(this, ContentZoomFocusXProperty);
            AnimationHelper.CancelAnimation(this, ContentZoomFocusYProperty);
            AnimationHelper.CancelAnimation(this, ViewportZoomFocusXProperty);
            AnimationHelper.CancelAnimation(this, ViewportZoomFocusYProperty);

            this.ContentZoomFocusX = pContentZoomFocus.X;
            this.ContentZoomFocusY = pContentZoomFocus.Y;
            this.ViewportZoomFocusX = (this.ContentZoomFocusX - this.ContentOffsetX) * this.ContentScale;
            this.ViewportZoomFocusY = (this.ContentZoomFocusY - this.ContentOffsetY) * this.ContentScale;

            // When zooming about a point make updates to ContentScale also update content offset.
            this.mEnableContentOffsetUpdateFromScale = true;

            AnimationHelper.StartAnimation(this, ContentScaleProperty, pNewScale, this.AnimationDuration,
                delegate(object pSender, EventArgs pEventArgs)
                {
                    this.mEnableContentOffsetUpdateFromScale = false;

                    if (pCallback != null)
                    {
                        pCallback(this, EventArgs.Empty);
                    }
                });

            AnimationHelper.StartAnimation(this, ViewportZoomFocusXProperty, this.ViewportWidth / 2, this.AnimationDuration);
            AnimationHelper.StartAnimation(this, ViewportZoomFocusYProperty, this.ViewportHeight / 2, this.AnimationDuration);
        }

        /// <summary>
        /// Zoom to the specified scale and move the specified focus point to the center of the viewport.
        /// </summary>
        /// <param name="pNewScale">The new scale.</param>
        /// <param name="pContentZoomFocus">The center point.</param>
        private void ZoomPointToViewportCenter(double pNewScale, Point pContentZoomFocus)
        {
            pNewScale = Math.Min(Math.Max(pNewScale, this.MinContentScale), this.MaxContentScale);

            AnimationHelper.CancelAnimation(this, ContentScaleProperty);
            AnimationHelper.CancelAnimation(this, ContentOffsetXProperty);
            AnimationHelper.CancelAnimation(this, ContentOffsetYProperty);

            this.ContentScale = pNewScale;
            this.ContentOffsetX = pContentZoomFocus.X - (this.ContentViewportWidth / 2);
            this.ContentOffsetY = pContentZoomFocus.Y - (this.ContentViewportHeight / 2);
        }


        /// <summary>
        /// Reset the viewport zoom focus to the center of the viewport.
        /// </summary>
        private void ResetViewportZoomFocus()
        {
            this.ViewportZoomFocusX = this.ViewportWidth / 2;
            this.ViewportZoomFocusY = this.ViewportHeight / 2;
        }

        /// <summary>
        /// Update the viewport size from the specified size.
        /// </summary>
        /// <param name="pNewSize">the new view port size.</param>
        private void UpdateViewportSize(Size pNewSize)
        {
            if (this.mViewport == pNewSize)
            {
                // The viewport is already the specified size.
                return;
            }

            this.mViewport = pNewSize;

            // Update the viewport size in content coordiates.
            this.UpdateContentViewportSize();

            // Initialise the content zoom focus point.
            this.UpdateContentZoomFocusX();
            this.UpdateContentZoomFocusY();

            // Reset the viewport zoom focus to the center of the viewport.
            this.ResetViewportZoomFocus();

            // Update content offset from itself when the size of the viewport changes.
            // This ensures that the content offset remains properly clamped to its valid range.
            this.ContentOffsetX = this.ContentOffsetX;
            this.ContentOffsetY = this.ContentOffsetY;

            if (this.ScrollOwner != null)
            {
                // Tell that owning ScrollViewer that scrollbar data has changed.
                this.ScrollOwner.InvalidateScrollInfo();
            }
        }

        /// <summary>
        /// Update the size of the viewport in content coordinates after the viewport size or 'ContentScale' has changed.
        /// </summary>
        private void UpdateContentViewportSize()
        {
            this.ContentViewportWidth = this.ViewportWidth / this.ContentScale;
            this.ContentViewportHeight = this.ViewportHeight / this.ContentScale;

            this.mConstrainedContentViewportWidth = Math.Min(this.ContentViewportWidth, this.mUnScaledExtent.Width);
            this.mConstrainedContentViewportHeight = Math.Min(this.ContentViewportHeight, this.mUnScaledExtent.Height);

            this.UpdateTranslationX();
            this.UpdateTranslationY();
        }

        /// <summary>
        /// Update the X coordinate of the translation transformation.
        /// </summary>
        private void UpdateTranslationX()
        {
            if (this.mContentOffsetTransform != null)
            {
                double lScaledContentWidth = this.mUnScaledExtent.Width * this.ContentScale;
                if (lScaledContentWidth < this.ViewportWidth)
                {
                    // When the content can fit entirely within the viewport, center it.
                    this.mContentOffsetTransform.X = (this.ContentViewportWidth - this.mUnScaledExtent.Width) / 2;
                }
                else
                {
                    this.mContentOffsetTransform.X = -this.ContentOffsetX;
                }
            }
        }

        /// <summary>
        /// Update the Y coordinate of the translation transformation.
        /// </summary>
        private void UpdateTranslationY()
        {
            if (this.mContentOffsetTransform != null)
            {
                double lScaledContentHeight = this.mUnScaledExtent.Height * this.ContentScale;
                if (lScaledContentHeight < this.ViewportHeight)
                {
                    // When the content can fit entirely within the viewport, center it.
                    this.mContentOffsetTransform.Y = (this.ContentViewportHeight - this.mUnScaledExtent.Height) / 2;
                }
                else
                {
                    this.mContentOffsetTransform.Y = -this.ContentOffsetY;
                }
            }
        }

        /// <summary>
        /// Update the X coordinate of the zoom focus point in content coordinates.
        /// </summary>
        private void UpdateContentZoomFocusX()
        {
            this.ContentZoomFocusX = this.ContentOffsetX + (this.mConstrainedContentViewportWidth / 2);
        }

        /// <summary>
        /// Update the Y coordinate of the zoom focus point in content coordinates.
        /// </summary>
        private void UpdateContentZoomFocusY()
        {
            this.ContentZoomFocusY = this.ContentOffsetY + (this.mConstrainedContentViewportHeight / 2);
        }

        /// <summary>
        /// Measure the control and it's children.
        /// </summary>
        /// <param name="pConstraint">The available size.</param>
        protected override Size MeasureOverride(Size pConstraint)
        {
            Size lInfiniteSize = new Size(double.PositiveInfinity, double.PositiveInfinity);
            Size lChildSize = base.MeasureOverride(lInfiniteSize);

            if (lChildSize != this.mUnScaledExtent)
            {
                // Use the size of the child as the un-scaled extent content.
                this.mUnScaledExtent = lChildSize;

                if (this.ScrollOwner != null)
                {
                    this.ScrollOwner.InvalidateScrollInfo();
                }
            }

            // Update the size of the viewport onto the content based on the passed in 'constraint'.
            this.UpdateViewportSize(pConstraint);

            double lWidth = pConstraint.Width;
            double lHeight = pConstraint.Height;

            if (double.IsInfinity(lWidth))
            {
                // Make sure we don't return infinity!
                lWidth = lChildSize.Width;
            }

            if (double.IsInfinity(lHeight))
            {
                // Make sure we don't return infinity!
                lHeight = lChildSize.Height;
            }

            this.UpdateTranslationX();
            this.UpdateTranslationY();

            return new Size(lWidth, lHeight);
        }

        /// <summary>
        /// Arrange the control and it's children.
        /// </summary>
        /// <param name="pConstraint">The available size.</param>
        protected override Size ArrangeOverride(Size pConstraint)
        {
            Size lSize = base.ArrangeOverride(this.DesiredSize);

            if (this.mContentPresenter.DesiredSize != this.mUnScaledExtent)
            {
                // Use the size of the child as the un-scaled extent content.
                this.mUnScaledExtent = this.mContentPresenter.DesiredSize;

                if (this.ScrollOwner != null)
                {
                    this.ScrollOwner.InvalidateScrollInfo();
                }
            }

            // Update the size of the viewport onto the content based on the passed in 'arrangeBounds'.
            this.UpdateViewportSize(pConstraint);

            return lSize;
        }

        /// <summary>
        /// Event raised when the 'ContentScale' property has changed value.
        /// </summary>
        /// <param name="pObject">The modified control.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private static void OnContentScaleChanged(DependencyObject pObject, DependencyPropertyChangedEventArgs pEventArgs)
        {
            ZoomAndPanControl lControl = pObject as ZoomAndPanControl;
            if (lControl != null)
            {
                if (lControl.mContentScaleTransform != null)
                {
                    // Update the content scale transform whenever 'ContentScale' changes.
                    lControl.mContentScaleTransform.ScaleX = lControl.ContentScale;
                    lControl.mContentScaleTransform.ScaleY = lControl.ContentScale;
                }

                // Update the size of the viewport in content coordinates.
                lControl.UpdateContentViewportSize();

                if (lControl.mEnableContentOffsetUpdateFromScale)
                {
                    try
                    {
                        // Disable content focus syncronization.  We are about to update content offset whilst zooming
                        // to ensure that the viewport is focused on our desired content focus point.  Setting this
                        // to 'true' stops the automatic update of the content focus when content offset changes.
                        lControl.mDisableContentFocusSync = true;

                        // Whilst zooming in or out keep the content offset up-to-date so that the viewport is always
                        // focused on the content focus point (and also so that the content focus is locked to the 
                        // viewport focus point - this is how the google maps style zooming works).
                        double lViewportOffsetX = lControl.ViewportZoomFocusX - (lControl.ViewportWidth / 2);
                        double lViewportOffsetY = lControl.ViewportZoomFocusY - (lControl.ViewportHeight / 2);
                        double lContentOffsetX = lViewportOffsetX / lControl.ContentScale;
                        double lContentOffsetY = lViewportOffsetY / lControl.ContentScale;
                        lControl.ContentOffsetX = (lControl.ContentZoomFocusX - (lControl.ContentViewportWidth / 2)) - lContentOffsetX;
                        lControl.ContentOffsetY = (lControl.ContentZoomFocusY - (lControl.ContentViewportHeight / 2)) - lContentOffsetY;
                    }
                    finally
                    {
                        lControl.mDisableContentFocusSync = false;
                    }
                }

                if (lControl.ScrollOwner != null)
                {
                    // Force the scroller update.
                    lControl.ScrollOwner.InvalidateScrollInfo();
                }
            }
        }

        /// <summary>
        /// Method called to clamp the 'ContentScale' value to its valid range.
        /// </summary>
        /// <param name="pObject">The modified control.</param>
        /// <param name="pBaseValue">The value to coerce.</param>
        private static object OnCoerceContentScale(DependencyObject pObject, object pBaseValue)
        {
            ZoomAndPanControl lControl = pObject as ZoomAndPanControl;
            if (lControl != null)
            {
                double lValue = System.Convert.ToDouble(pBaseValue);
                lValue = Math.Min(Math.Max(lValue, lControl.MinContentScale), lControl.MaxContentScale);
                return lValue;
            }

            return pBaseValue;
        }

        /// <summary>
        /// Event raised 'MinContentScale' or 'MaxContentScale' has changed.
        /// </summary>
        /// <param name="pObject">The modified control.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private static void OnMinOrMaxContentScaleChanged(DependencyObject pObject, DependencyPropertyChangedEventArgs pEventArgs)
        {
            ZoomAndPanControl lControl = pObject as ZoomAndPanControl;
            if (lControl != null)
            {
                lControl.ContentScale = Math.Min(Math.Max(lControl.ContentScale, lControl.MinContentScale), lControl.MaxContentScale);
            }
        }

        /// <summary>
        /// Event raised when the 'ContentOffsetX' property has changed value.
        /// </summary>
        /// <param name="pObject">The modified control.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private static void OnContentOffsetXChanged(DependencyObject pObject, DependencyPropertyChangedEventArgs pEventArgs)
        {
            ZoomAndPanControl lControl = pObject as ZoomAndPanControl;
            if (lControl != null)
            {
                lControl.UpdateTranslationX();

                if (lControl.mDisableContentFocusSync == false)
                {
                    // Normally want to automatically update content focus when content offset changes.
                    // Although this is disabled using 'disableContentFocusSync' when content offset changes due to in-progress zooming.
                    lControl.UpdateContentZoomFocusX();
                }

                if (lControl.mDisableScrollOffsetSync == false && lControl.ScrollOwner != null)
                {
                    // Notify the owning ScrollViewer that the scrollbar offsets should be updated.
                    lControl.ScrollOwner.InvalidateScrollInfo();
                }
            }
        }

        /// <summary>
        /// Method called to clamp the 'ContentOffsetX' value to its valid range.
        /// </summary>
        /// <param name="pObject">The modified control.</param>
        /// <param name="pBaseValue">The value to coerce.</param>
        private static object OnCoerceContentOffsetX(DependencyObject pObject, object pBaseValue)
        {
            ZoomAndPanControl lControl = pObject as ZoomAndPanControl;
            if (lControl != null)
            {
                double lValue = System.Convert.ToDouble(pBaseValue);
                double lMinOffsetX = 0.0;
                double lMaxOffsetX = Math.Max(0.0, lControl.mUnScaledExtent.Width - lControl.mConstrainedContentViewportWidth);
                lValue = Math.Min(Math.Max(lValue, lMinOffsetX), lMaxOffsetX);
                return lValue;
            }

            return pBaseValue;
        }

        /// <summary>
        /// Event raised when the 'ContentOffsetY' property has changed value.
        /// </summary>
        private static void OnContentOffsetYChanged(DependencyObject pObject, DependencyPropertyChangedEventArgs pEventArgs)
        {
            ZoomAndPanControl lControl = pObject as ZoomAndPanControl;
            if (lControl != null)
            {
                lControl.UpdateTranslationY();

                if (lControl.mDisableContentFocusSync == false)
                {
                    // Normally want to automatically update content focus when content offset changes.
                    // Although this is disabled using 'disableContentFocusSync' when content offset changes due to in-progress zooming.
                    lControl.UpdateContentZoomFocusY();
                }

                if (lControl.mDisableScrollOffsetSync == false && lControl.ScrollOwner != null)
                {
                    // Notify the owning ScrollViewer that the scrollbar offsets should be updated.
                    lControl.ScrollOwner.InvalidateScrollInfo();
                }
            }
        }

        /// <summary>
        /// Method called to clamp the 'ContentOffsetY' value to its valid range.
        /// </summary>
        /// <param name="pObject">The modified control.</param>
        /// <param name="pBaseValue">The value to coerce.</param>
        private static object OnCoerceContentOffsetY(DependencyObject pObject, object pBaseValue)
        {
            ZoomAndPanControl lControl = pObject as ZoomAndPanControl;
            if (lControl != null)
            {
                double lValue = System.Convert.ToDouble(pBaseValue);
                double lMinOffsetY = 0.0;
                double lMaxOffsetY = Math.Max(0.0, lControl.mUnScaledExtent.Height - lControl.mConstrainedContentViewportHeight);
                lValue = Math.Min(Math.Max(lValue, lMinOffsetY), lMaxOffsetY);
                return lValue;
            }

            return pBaseValue;
        }

        #endregion // Methods.
    }
}

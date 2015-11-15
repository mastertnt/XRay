using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using XGraph.Extensions;
using XGraph.ViewModels;

namespace XGraph.Controls
{
    /// <summary>
    /// This class represents a connection during the building (when the user drags it).
    /// </summary>
    /// <!-- Nicolas Baudrey -->
    public class ConnectingLine : Adorner
    {
        #region Fields

        /// <summary>
        /// This field stores the source connector of the adorner.
        /// </summary>
        private AConnector mSourceConnector = null;

        /// <summary>
        /// This field stores the pen of the adorner.
        /// </summary>
        private Pen mPen;

        /// <summary>
        /// This field stores the geometry to draw.
        /// </summary>
        private PathGeometry mDrawingGeometry;

        #endregion // Fields.

        #region Properties

        /// <summary>
        /// Gets or sets the hit connector.
        /// </summary>
        private AConnector HitConnector
        {
            get;
            set;
        }

        #endregion // Properties.

        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="pElement">The parent element.</param>
        /// <param name="pSourceConnector">The source connector.</param>
        public ConnectingLine(UIElement pElement, AConnector pSourceConnector)
            : base(pElement)
        {
            this.mSourceConnector = pSourceConnector;
            this.mPen = new Pen(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4A7EBB")), 1) {LineJoin = PenLineJoin.Round};
            this.Cursor = Cursors.Cross;
        }

        #endregion // Constructors.

        #region Methods

        /// <summary>
        /// This method is called to render the adorner.
        /// </summary>
        /// <param name="pDC">The current drawing context.</param>
        protected override void OnRender(DrawingContext pDC)
        {
            base.OnRender(pDC);
            pDC.DrawGeometry(null, this.mPen, this.mDrawingGeometry);

            // without a background the OnMouseMove event would not be fired
            // Alternative: implement a Canvas as a child of this adorner, like
            // the ConnectionAdorner does.
            pDC.DrawRectangle(Brushes.Transparent, null, new Rect(this.RenderSize));
        }

        /// <summary>
        /// This method is called when mouse move occured on the adorner.
        /// </summary>
        /// <param name="pEventArgs">The event arguments</param>
        protected override void OnMouseMove(MouseEventArgs pEventArgs)
        {
            if
                (pEventArgs.LeftButton == MouseButtonState.Pressed)
            {
                if (!this.IsMouseCaptured)
                {
                    this.CaptureMouse();
                }

                // Create a path according to the source and the end.
                this.UpdatePathGeometry(pEventArgs.GetPosition(this));

                // Redraw it.
                this.InvalidateVisual();
            }
            else
            {
                if (this.IsMouseCaptured)
                {
                    this.ReleaseMouseCapture();
                }
            }
        }

        /// <summary>
        /// This method is called when a mouse button up occured on the adorner.
        /// </summary>
        /// <param name="pEventArgs">The event arguments</param>
        protected override void OnMouseUp(MouseButtonEventArgs pEventArgs)
        {
            // Release the mouse capture.
            if (this.IsMouseCaptured)
            {
                this.ReleaseMouseCapture();
            }

            Canvas lParentCanvas = this.AdornedElement as Canvas;
            if (lParentCanvas != null)
            {
                AConnector lTargetConnector = lParentCanvas.HitControl<AConnector>(pEventArgs.GetPosition(this));
                if (lTargetConnector != null)
                {
                    GraphViewModel lGraphViewModel = lParentCanvas.DataContext as GraphViewModel;
                    if (lGraphViewModel != null)
                    {
                        PortViewModel lTargetViewModel = lTargetConnector.DataContext as PortViewModel;
                        PortViewModel lSourceViewModel = this.mSourceConnector.DataContext as PortViewModel;
                        if (lTargetViewModel != null && lTargetViewModel.CanBeConnectedTo(lSourceViewModel))
                        {
                            ConnectionViewModel lConnectionViewModel = new ConnectionViewModel();
                            lConnectionViewModel.Input = lSourceViewModel;
                            lConnectionViewModel.Output = lTargetViewModel;
                            lGraphViewModel.AddConnection(lConnectionViewModel);
                        }
                    }
                }
            }
            
            // Remove the adorner.
            AdornerLayer lLayer = AdornerLayer.GetAdornerLayer(this.AdornedElement);
            if (lLayer != null)
            {
                lLayer.Remove(this);
            }
        }
        
        /// <summary>
        /// This method computes the final geometry for the path.
        /// </summary>
        /// <param name="pFinalPosition">The final path position.</param>
        /// <returns>The path geometry.</returns>
        private void UpdatePathGeometry(Point pFinalPosition)
        {
            if (this.mDrawingGeometry == null)
            {
                this.mDrawingGeometry = new PathGeometry();
            }

            List<Point> lPoints = this.mSourceConnector.Position.GetShortestPath(pFinalPosition);
            this.mDrawingGeometry.Figures.Clear();
            PathFigure lFigure = new PathFigure();
            lFigure.StartPoint = this.mSourceConnector.Position;
            lPoints.RemoveAt(0);
            lFigure.Segments.Add(new PolyLineSegment(lPoints, true));
            this.mDrawingGeometry.Figures.Add(lFigure);
        }

        #endregion // Methods.
    }
}


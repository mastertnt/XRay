using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using XGraph.Extensions;

namespace XGraph.Controls
{
    /// <summary>
    /// Class defining a connection renderer.
    /// </summary>
    public class ConnectionRenderer : Shape
    {
        #region Fields

        /// <summary>
        /// This field stores the geometry to draw.
        /// </summary>
        private PathGeometry mDrawingGeometry;

        #endregion // Fields.

        #region Dependencies

        /// <summary>
        /// Identifies the From dependency property.
        /// </summary>
        public static readonly DependencyProperty FromProperty = DependencyProperty.Register("From", typeof(Point), typeof(ConnectionRenderer), new FrameworkPropertyMetadata(new Point(), OnPositionChanged));

        /// <summary>
        /// Identifies the To dependency property.
        /// </summary>
        public static readonly DependencyProperty ToProperty = DependencyProperty.Register("To", typeof(Point), typeof(ConnectionRenderer), new FrameworkPropertyMetadata(new Point(), OnPositionChanged));

        #endregion // Dependencies.

        #region Constructors

        /// <summary>
        /// Initializes the <see cref="ConnectionRenderer"/> class.
        /// </summary>
        static ConnectionRenderer()
        {
            // Default line color.
            Shape.StrokeProperty.OverrideMetadata(typeof(ConnectionRenderer), new FrameworkPropertyMetadata(Brushes.White));

            // Default line thickness.
            Shape.StrokeThicknessProperty.OverrideMetadata(typeof(ConnectionRenderer), new FrameworkPropertyMetadata(1.0));

            // Default line join process.
            Shape.StrokeLineJoinProperty.OverrideMetadata(typeof(ConnectionRenderer), new FrameworkPropertyMetadata(PenLineJoin.Round));
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ConnectionRenderer()
        {
            // Creating the geometry.
            this.mDrawingGeometry = new PathGeometry();
        }

        #endregion // Constructors.
        
        #region Properties

        /// <summary>
        /// Gets or sets the initiale position of the connection.
        /// </summary>
        public Point From
        {
            get
            {
                return (Point)this.GetValue(FromProperty);
            }
            set
            {
                this.SetValue(FromProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the final position of the connection.
        /// </summary>
        public Point To
        {
            get
            {
                return (Point)this.GetValue(ToProperty);
            }
            set
            {
                this.SetValue(ToProperty, value);
            }
        }

        /// <summary>
        /// Gets the geometry of the shape.
        /// </summary>
        protected override Geometry DefiningGeometry
        {
            get
            {
                // Computing the shortest path between the points.
                List<Point> lPoints = this.From.GetShortestPath(this.To);
                lPoints.RemoveAt(0);

                // Building the path.
                PathSegment[] lPath = { new BezierSegment(lPoints[0], lPoints[1], lPoints[2], true) };

                // Building the figure using the path.
                PathFigure[] lFigures = { new PathFigure(this.From, lPath, false) };

                // Building the final geometry.
                return new PathGeometry(lFigures);
            }
        }

        #endregion // Properties.

        #region Methods

        /// <summary>
        /// Delegate called when the initiale or the final position changed.
        /// </summary>
        /// <param name="pObject">The modified control.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private static void OnPositionChanged(DependencyObject pObject, DependencyPropertyChangedEventArgs pEventArgs)
        {
            ConnectionRenderer lRenderer = pObject as ConnectionRenderer;
            if (lRenderer != null)
            {
                lRenderer.InvalidateVisual();
            }
        }

        ///// <summary>
        ///// This method updates the final geometry for the path.
        ///// </summary>
        ///// <returns>The path geometry.</returns>
        //private void UpdatePathGeometry()
        //{
        //    if (this.mDrawingGeometry == null)
        //    {
        //        this.mDrawingGeometry = new PathGeometry();
        //    }

        //    List<Point> lPoints = this.From.GetShortestPath(this.To);
        //    this.mDrawingGeometry.Figures.Clear();
        //    PathFigure lFigure = new PathFigure { StartPoint = this.From };
        //    lPoints.RemoveAt(0);
        //    lFigure.Segments.Add(new BezierSegment(lPoints[0], lPoints[1], lPoints[2], true));
        //    this.mDrawingGeometry.Figures.Add(lFigure);
        //}

        #endregion // Methods.
    }
}

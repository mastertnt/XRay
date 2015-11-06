using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace XGraph.Controls
{
    /// <summary>
    /// Class defining the main part shape of the node view.
    /// </summary>
    public class NodeViewShape : Shape
    {
        #region Dependencies

        /// <summary>
        /// Identifies the BevelLenght dependency property.
        /// </summary>
        public static readonly DependencyProperty BevelLenghtProperty = DependencyProperty.Register("BevelLenght", typeof(double), typeof(NodeViewShape), new FrameworkPropertyMetadata(0.0));

        #endregion // Dependencies.
        
        #region Properties

        /// <summary>
        /// Gets or sets the bevel length of the shape.
        /// </summary>
        public double BevelLenght
        {
            get
            {
                return (double)this.GetValue(BevelLenghtProperty);
            }
            set
            {
                this.SetValue(BevelLenghtProperty, value);
            }
        }

        /// <summary>
        /// Gets the geometry of the shape.
        /// </summary>
        protected override Geometry DefiningGeometry
        {
            get
            {
                // Defining the points.
                Point lP1 = new Point(0.0d, this.BevelLenght);
                Point lP2 = new Point(this.BevelLenght, 0.0d);
                Point lP3 = new Point(this.ActualWidth, 0.0d);
                Point lP4 = new Point(this.ActualWidth, this.ActualHeight - this.BevelLenght);
                Point lP5 = new Point(this.ActualWidth - this.BevelLenght, this.ActualHeight);
                Point lP6 = new Point(0.0d, this.ActualHeight);

                // Building the path.
                List<PathSegment> lPath = new List<PathSegment>(3);
                lPath.Add(new LineSegment(lP1, true));
                lPath.Add(new LineSegment(lP2, true));
                lPath.Add(new LineSegment(lP3, true));
                lPath.Add(new LineSegment(lP4, true));
                lPath.Add(new LineSegment(lP5, true));
                lPath.Add(new LineSegment(lP6, true));

                // Building the figure using the path.
                PathFigure[] lFigures = new PathFigure[] { new PathFigure(lP1, lPath, true) };

                // Building the final geometry.
                return new PathGeometry(lFigures, FillRule.EvenOdd, null);
            }
        }

        #endregion // Properties.
    }
}

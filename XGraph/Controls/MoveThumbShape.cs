using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace XGraph.Controls
{
    /// <summary>
    /// Class defining the shape of the move thumb.
    /// </summary>
    /// <!-- Damien Porte -->
    public class MoveThumbShape : Shape
    {
        #region Dependencies

        /// <summary>
        /// Identifies the BevelLenght dependency property.
        /// </summary>
        public static readonly DependencyProperty BevelLenghtProperty = DependencyProperty.Register("BevelLenght", typeof(double), typeof(MoveThumbShape), new FrameworkPropertyMetadata(0.0));

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
                Point lP1 = new Point(0.0d, 0.0d);
                Point lP2 = new Point(0.0d, this.BevelLenght);
                Point lP3 = new Point(this.BevelLenght, 0.0d);
                

                // Building the path.
                List<PathSegment> lPath = new List<PathSegment>(3) {new LineSegment(lP1, true), new LineSegment(lP2, true), new LineSegment(lP3, true)};

                // Building the figure using the path.
                PathFigure[] lFigures = { new PathFigure(lP1, lPath, true) };

                // Building the final geometry.
                return new PathGeometry(lFigures, FillRule.EvenOdd, null);
            }
        }

        #endregion // Properties.
    }
}

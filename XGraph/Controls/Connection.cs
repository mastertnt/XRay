using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using PropertyChanged;
using XGraph.Extensions;
using XGraph.ViewModels;

namespace XGraph.Controls
{
    /// <summary>
    /// This class represents a connection.
    /// </summary>
    /// <!-- Nicolas Baudrey -->
    [ImplementPropertyChanged]
    public class Connection : ContentControl
    {
        #region Fields

        /// <summary>
        /// This field stores the geometry to draw.
        /// </summary>
        private PathGeometry mDrawingGeometry;

        /// <summary>
        /// This field stores the pen of the adorner.
        /// </summary>
        private readonly Pen mPen;

        #endregion // Fields.

        #region Constructors

        /// <summary>
        /// Static constructor.
        /// </summary>
        static Connection()
        {
            // set the key to reference the style for this control
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(Connection), new FrameworkPropertyMetadata(typeof(Connection)));
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Connection()
        {
            this.mPen = new Pen(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4A7EBB")), 1);
            this.mPen.LineJoin = PenLineJoin.Round;
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
            pDC.DrawRectangle(Brushes.Transparent, null, new Rect(RenderSize));
        }

        /// <summary>
        /// Method called when the control content changed.
        /// </summary>
        /// <param name="pOldContent">The previous content.</param>
        /// <param name="pNewContent">The new content.</param>
        protected override void OnContentChanged(object pOldContent, object pNewContent)
        {
            ConnectionViewModel lPreviousContent = this.Content as ConnectionViewModel;
            if (lPreviousContent != null)
            {
                lPreviousContent.Output.PropertyChanged -= this.OnPortPropertyChanged;
                lPreviousContent.Input.PropertyChanged -= this.OnPortPropertyChanged;
            }

            base.OnContentChanged(pOldContent, pNewContent);

            // The content is the view model.
            ConnectionViewModel lNewContent = pNewContent as ConnectionViewModel;
            if (lNewContent != null)
            {
                lNewContent.Output.PropertyChanged += this.OnPortPropertyChanged;
                lNewContent.Input.PropertyChanged += this.OnPortPropertyChanged;
                this.UpdatePathGeometry();
            }
        }

        /// <summary>
        /// Called when [port property changed].
        /// </summary>
        /// <param name="pSender">The sender.</param>
        /// <param name="pEventArgs">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
        private void OnPortPropertyChanged(object pSender, System.ComponentModel.PropertyChangedEventArgs pEventArgs)
        {
            this.UpdatePathGeometry();
        }

        /// <summary>
        /// This method updates the final geometry for the path.
        /// </summary>
        /// <returns>The path geometry.</returns>
        private void UpdatePathGeometry()
        {
            ConnectionViewModel lViewModel = this.Content as ConnectionViewModel;
            if
                (lViewModel != null)
            {
                if
                    (this.mDrawingGeometry == null)
                {
                    this.mDrawingGeometry = new PathGeometry();
                }
                List<Point> lPoints = lViewModel.Output.Position.GetShortestPath(lViewModel.Input.Position);
                this.mDrawingGeometry.Figures.Clear();
                PathFigure lFigure = new PathFigure {StartPoint = lViewModel.Output.Position};
                lPoints.RemoveAt(0);
                lFigure.Segments.Add(new BezierSegment(lPoints[0], lPoints[1], lPoints[2], true));
                this.mDrawingGeometry.Figures.Add(lFigure);
            }
        }

        #endregion // Methods.
    }
}

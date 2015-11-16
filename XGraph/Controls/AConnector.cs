using System;
using System.Security.Cryptography.X509Certificates;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using PropertyChanged;
using XGraph.Extensions;
using XGraph.ViewModels;

namespace XGraph.Controls
{
    /// <summary>
    /// This class represents a connector.
    /// A connector is used to anchor a connection.
    /// </summary>
    /// <!-- NBY -->
    [ImplementPropertyChanged]
    public abstract class AConnector : Control
    {
        #region Fields

        /// <summary>
        /// This field stores the drag start point, relative to the DesignerCanvas 
        /// </summary>
        private Point? mDragStartPoint;

        #endregion // Fields.

        #region Properties

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        public Point Position 
        { 
            get;
            set;
        }

        #endregion // Properties

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AConnector"/> class.
        /// </summary>
        protected AConnector()
        {
            this.LayoutUpdated += this.OnLayoutUpdated;
        }

        #endregion // Constructors.

        #region Methods

        /// <summary>
        /// This method is called each time the mouse is moved over the constrol.
        /// </summary>
        /// <param name="pEventArgs">The event arguments.</param>
        protected override void OnMouseDown(MouseButtonEventArgs pEventArgs)
        {
            base.OnMouseMove(pEventArgs);

            Canvas lParentCanvas = this.FindParentCanvas();
            if (this.mDragStartPoint.HasValue == false)
            {
                if (lParentCanvas != null)
                {
                    // position relative to DesignerCanvas
                    this.mDragStartPoint = pEventArgs.GetPosition(lParentCanvas);
                    pEventArgs.Handled = true;
                }
            }

            if (this.mDragStartPoint.HasValue)
            {
                // create connection adorner 
                if (lParentCanvas != null)
                {
                    AdornerLayer lLayer = AdornerLayer.GetAdornerLayer(lParentCanvas);
                    if (lLayer != null)
                    {
                        ConnectingLine lConnectingLine = new ConnectingLine(lParentCanvas, this);
                        lLayer.Add(lConnectingLine);
                        pEventArgs.Handled = true;
                    }
                }
            }
        }

        /// <summary>
        /// This method is called when the layout changes.
        /// </summary>
        /// <param name="pEventArgs">The event arguments.</param>
        private void OnLayoutUpdated(object sender, EventArgs pEventArgs)
        {
            Canvas lParentCanvas = this.FindParentCanvas();
            if (lParentCanvas != null)
            {
                //get centre position of this Connector relative to the DesignerCanvas
                this.Position = this.TransformToVisual(lParentCanvas).Transform(new Point(this.ActualWidth / 2, this.ActualHeight / 2));
            }
        }

        /// <summary>
        /// Returns the parent canvas.
        /// </summary>
        /// <returns>The connector parent canvas.</returns>
        private Canvas FindParentCanvas()
        {
            Canvas lParentCanvas = this.FindVisualParent<Canvas>();
            if (lParentCanvas == null)
            {
                ConnectorsAdorner lAdornerParent = this.FindVisualParent<ConnectorsAdorner>();
                if (lAdornerParent != null)
                {
                    return lAdornerParent.AdornedPortView.FindVisualParent<Canvas>();
                }
            }

            return lParentCanvas;
        }

        #endregion // Methods.
    }
}

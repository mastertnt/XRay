using System.Security.Cryptography.X509Certificates;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace XGraph.Controls
{
    /// <summary>
    /// This class represents a connector.
    /// A connector is used to anchor a connection.
    /// </summary>
    /// <!-- NBY -->
    public class Connector : ContentControl
    {
        #region Fields

        /// <summary>
        /// This field stores the drag start point, relative to the DesignerCanvas 
        /// </summary>
        private Point? mDragStartPoint = null;

        #endregion

        #region Constructors

        /// <summary>
        /// Static constructor.
        /// </summary>
        static Connector()
        {
            // set the key to reference the style for this control
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (Connector), new FrameworkPropertyMetadata(typeof (Connector)));
        }

        #endregion // Constructors.

        #region Methods

        /// <summary>
        /// Method called when the control template is applied.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

          
        }

        /// <summary>
        /// Invoked when an unhandled <see cref="E:System.Windows.Input.Mouse.MouseDown" /> attached event reaches an element in its route that is derived from this class. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.Input.MouseButtonEventArgs" /> that contains the event data. This event data reports details about the mouse button that was pressed and the handled state.</param>
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
        }

        /// <summary>
        /// This method is called each time the mouse is moved over the constrol.
        /// </summary>
        /// <param name="pEventArgs">The event arguments.</param>
        protected override void OnMouseMove(MouseEventArgs pEventArgs)
        {
            base.OnMouseMove(pEventArgs);
            this.Cursor = Cursors.Cross;

            //DesignerCanvas lParentCanvas = GetDesignerCanvas(this);
            //if
            //    (this.mDragStartPoint.HasValue == false)
            //{
            //    if
            //        (lParentCanvas != null)
            //    {
            //        // position relative to DesignerCanvas
            //        this.mDragStartPoint = new Point?(pEventArgs.GetPosition(lParentCanvas));
            //        pEventArgs.Handled = true;
            //    }
            //}

            //if
            //    (this.mDragStartPoint.HasValue)
            //{
            //    // create connection adorner 
            //    if
            //        (lParentCanvas != null)
            //    {
            //        AdornerLayer lLayer = AdornerLayer.GetAdornerLayer(lParentCanvas);
            //        if
            //            (lLayer != null)
            //        {
            //            ConnectingLine lConnectingLine = new ConnectingLine(lParentCanvas, this);
            //            if
            //                (lConnectingLine != null)
            //            {
            //                lLayer.Add(lConnectingLine);
            //                pEventArgs.Handled = true;
            //            }
            //        }
            //    }
            //}
        }

        #endregion // Methods.
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using XGraph.Extensions;

namespace XGraph.Controls
{
    /// <summary>
    /// Class defining an input connector.
    /// </summary>
    public class OutputConnector : AConnector
    {
        #region Fields

        /// <summary>
        /// This field stores the drag start point, relative to the DesignerCanvas 
        /// </summary>
        private Point? mDragStartPoint;

        #endregion // Fields.

        #region Constructors

        /// <summary>
        /// Initializes the <see cref="OutputConnector"/> class.
        /// </summary>
        static OutputConnector()
        {
            // set the key to reference the style for this control
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(OutputConnector), new FrameworkPropertyMetadata(typeof(OutputConnector)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OutputConnector"/> class.
        /// </summary>
        /// <param name="pParentPort">The connector parent port.</param>
        public OutputConnector(PortView pParentPort)
            : base(pParentPort)
        {
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

            AdornerLayeredCanvas lParentCanvas = this.FindVisualParent<AdornerLayeredCanvas>();
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
                    AdornerLayer lLayer = lParentCanvas.AdornerLayer;
                    if (lLayer != null)
                    {
                        ConnectingLine lConnectingLine = new ConnectingLine(lParentCanvas, this);
                        lLayer.Add(lConnectingLine);
                        pEventArgs.Handled = true;
                    }
                }
            }
        }

        #endregion // Methods.
    }
}

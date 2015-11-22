using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace XGraph.Controls
{
    /// <summary>
    /// Classe defining the connectors adorner.
    /// </summary>
    /// <!-- Damien Porte -->
    public class ConnectorsAdorner : Adorner
    {
        #region Fields

        /// <summary>
        /// Stores the adorner children.
        /// </summary>
        /// <remarks>
        /// Using a visual collection to keep the visual parenting aspect.
        /// </remarks>
        private VisualCollection mVisualChildren;

        #endregion // Fields.

        #region Constructors

        /// <summary>
        /// Innitializes a new instance of the <see cref="ConnectorsAdorner"/> class.
        /// </summary>
        /// <param name="pAdornedElement">The orned prot view.</param>
        public ConnectorsAdorner(PortView pAdornedElement)
            : base(pAdornedElement)
        {
            // Creating the connectors contained in the adorner.
            this.mVisualChildren = new VisualCollection(this);
            this.mVisualChildren.Add(new InputConnector(pAdornedElement));
            this.mVisualChildren.Add(new OutputConnector(pAdornedElement));

            // Ensuring the measure is well computed.
            this.AdornedPortView.SizeChanged += new SizeChangedEventHandler(this.OnPortViewSizeChanged);
        }

        #endregion // Constructors.

        #region Properties

        /// <summary>
        /// Gets the input connector.
        /// </summary>
        public InputConnector InputConnector
        {
            get
            {
                return this.GetVisualChild(0) as InputConnector;
            }
        }

        /// <summary>
        /// Gets the output connector.
        /// </summary>
        public OutputConnector OutputConnector
        {
            get
            {
                return this.GetVisualChild(1) as OutputConnector;
            }
        }

        /// <summary>
        /// Gets the adorned port view.
        /// </summary>
        public PortView AdornedPortView
        {
            get
            {
                return this.AdornedElement as PortView;
            }
        }

        /// <summary>
        /// Gets the visual children count.
        /// </summary>
        protected override Int32 VisualChildrenCount
        {
            get
            {
                return this.mVisualChildren.Count;
            }
        }

        #endregion // Properties.

        #region Methods

        /// <summary>
        /// Delegate called when the port view size changed.
        /// </summary>
        /// <param name="pSender">The modified port view.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private void OnPortViewSizeChanged(object pSender, SizeChangedEventArgs pEventArgs)
        {
            this.InvalidateMeasure();
        }

        /// <summary>
        /// Computes the size of the adorner.
        /// </summary>
        /// <param name="pAvailableSize">The initial available size.</param>
        /// <returns>The viewport desired size.</returns>
        protected override Size MeasureOverride(Size pAvailableSize)
        {
            // Getting the size of the adorned port view.
            Size lPortViewSize = new Size(this.AdornedPortView.ActualWidth, this.AdornedPortView.ActualHeight);

            // Compting the final size by adding the width of the wanted connector.
            Size lAdornerSize = lPortViewSize;
            if (this.AdornedPortView.Direction == ViewModels.PortDirection.Input)
            {
                this.InputConnector.Measure(pAvailableSize);
                lAdornerSize.Width += this.InputConnector.DesiredSize.Width;
            }
            else
            {
                this.OutputConnector.Measure(pAvailableSize);
                lAdornerSize.Width += this.OutputConnector.DesiredSize.Width;
            }

            // Returning the final adorner size.
            return lAdornerSize;
        }

        /// <summary>
        /// Arranges the connectors by taking in account the computed size of the panel viewport.
        /// </summary>
        /// <param name="pFinalSize">The available size.</param>
        /// <returns>The size used (here equals to the available size).</returns>
        protected override Size ArrangeOverride(Size pFinalSize)
        {
            // Getting the size of the adorned port view.
            double lPortViewWidth = this.AdornedPortView.ActualWidth;
            double lPortViewHeight = this.AdornedPortView.ActualHeight;

            // Displaying only the wanted connector by calling the arrange method depending on the port direction.
            if (this.AdornedPortView.Direction == ViewModels.PortDirection.Input)
            {
                double lConnectorWidth = this.InputConnector.DesiredSize.Width;
                double lConnectorHeight = this.InputConnector.DesiredSize.Height;
                this.InputConnector.Arrange(new Rect(-lConnectorWidth, lPortViewHeight / 2.0 - lConnectorHeight / 2.0, lConnectorWidth, lConnectorHeight));
            }
            else
            {
                double lConnectorWidth = this.OutputConnector.DesiredSize.Width;
                double lConnectorHeight = this.OutputConnector.DesiredSize.Height;
                this.OutputConnector.Arrange(new Rect(lPortViewWidth, lPortViewHeight / 2.0 - lConnectorHeight / 2.0, lConnectorWidth, lConnectorHeight));
            }

            // Return the final size.
            return pFinalSize;
        }


        /// <summary>
        /// Returns the indexed visual child.
        /// </summary>
        /// <param name="pIndex">The index of the visual child.</param>
        /// <returns>The found visual child.</returns>
        protected override Visual GetVisualChild(Int32 pIndex)
        {
            return this.mVisualChildren[pIndex];
        }

        #endregion // Methods.
    }
}

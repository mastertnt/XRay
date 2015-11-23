using System;
using System.Security.Cryptography.X509Certificates;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using PropertyChanged;
using XGraph.Extensions;
using XGraph.ViewModels;
using System.Windows.Data;

namespace XGraph.Controls
{
    /// <summary>
    /// This class represents a connector.
    /// A connector is used to anchor a connection.
    /// </summary>
    /// <!-- Nicolas Baudrey -->
    [ImplementPropertyChanged]
    public abstract class AConnector : UserControl
    {
        #region Dependencies

        /// <summary>
        /// Identifies the Position dependency property.
        /// </summary>
        public static readonly DependencyProperty PositionProperty = DependencyProperty.Register("Position", typeof(Point), typeof(AConnector), new FrameworkPropertyMetadata(new Point()));

        #endregion // Dependencies.

        #region Properties

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        public Point Position 
        {
            get
            {
                return (Point)this.GetValue(PositionProperty);
            }
            set
            {
                this.SetValue(PositionProperty, value);
            }
        }

        /// <summary>
        /// Gets the connector parent port.
        /// </summary>
        public PortView ParentPort
        {
            get;
            private set;
        }

        #endregion // Properties

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AConnector"/> class.
        /// </summary>
        /// <param name="pParentPort">The connector parent port.</param>
        protected AConnector(PortView pParentPort)
        {
            this.ParentPort = pParentPort;
            this.LayoutUpdated += this.OnLayoutUpdated;
        }

        #endregion // Constructors.

        #region Methods

        /// <summary>
        /// This method is called when the layout changes.
        /// </summary>
        /// <param name="pEventArgs">The event arguments.</param>
        private void OnLayoutUpdated(object sender, EventArgs pEventArgs)
        {
            AdornerLayeredCanvas lParentCanvas = this.FindVisualParent<AdornerLayeredCanvas>();
            if (lParentCanvas != null)
            {
                // Get centre position of this Connector relative to the DesignerCanvas.
                this.Position = this.TransformToVisual(lParentCanvas.AdornerLayer).Transform(new Point(this.ActualWidth / 2, this.ActualHeight / 2));
            }
        }

        #endregion // Methods.
    }
}

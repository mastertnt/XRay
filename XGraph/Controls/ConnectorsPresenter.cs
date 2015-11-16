using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace XGraph.Controls
{
    /// <summary>
    /// Class used as proxy to add connectors to the <see cref="PortView"/> using XAML.
    /// </summary>
    /// <!-- Damien Porte -->
    public class ConnectorsPresenter : Control
    {
        #region Methods

        /// <summary>
        /// Delegate called when the control is initialized.
        /// </summary>
        /// <param name="pEventArgs">The event arguments.</param>
        protected override void OnInitialized(EventArgs pEventArgs)
        {
            // The adorned port view is given to the control by the DataContext property.
            PortView lPortView = this.DataContext as PortView;
            if (lPortView != null)
            {
                // Creating the adorner layer.
                AdornerLayer lLayer = AdornerLayer.GetAdornerLayer(lPortView);
                
                // Creating the adorner and propagating this control background.
                ConnectorsAdorner lAdorner = new ConnectorsAdorner(lPortView);
                lAdorner.InputConnector.Background = this.Background;
                lAdorner.OutputConnector.Background = this.Background;

                // Adding the adorner to the layer.
                lLayer.Add(lAdorner);
            }
        }

        #endregion // Methods.
    }
}

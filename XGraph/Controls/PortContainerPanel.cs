using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using XGraph.ViewModels;

namespace XGraph.Controls
{
    /// <summary>
    /// Panel used to organize the <see cref="PortView"/> elements in the <see cref="PortContainer"/> in two columns.
    /// </summary>
    /// <remarks>Left column containns the input ports, right column contains the output ports.</remarks>
    /// <!-- Damien Porte -->
    public class PortContainerPanel : Panel
    {
        #region Methods

        /// <summary>
        /// Computes the size of the panel viewport.
        /// </summary>
        /// <param name="pAvailableSize">The initial available size.</param>
        /// <returns>The viewport desired size.</returns>
        protected override Size MeasureOverride(Size pAvailableSize)
        {
            Size lInputPortsSize = new Size();
            Size lOutputPortsSize = new Size();

            // Iterating threw the ports to have the highest column.
            for (int i = 0, count = this.Children.Count; i < count; ++i)
            {
                PortView lPort = this.Children[i] as PortView;
                if (lPort != null)
                {
                    lPort.Measure(pAvailableSize);

                    if (lPort.Direction == PortDirection.Input)
                    {
                        lInputPortsSize.Height += lPort.DesiredSize.Height;
                        lInputPortsSize.Width = Math.Max(lInputPortsSize.Width, lPort.DesiredSize.Width);
                    }
                    else
                    {
                        lOutputPortsSize.Height += lPort.DesiredSize.Height;
                        lOutputPortsSize.Width = Math.Max(lOutputPortsSize.Width, lPort.DesiredSize.Width);
                    }
                }
            }

            // Computing the size of the panel.
            Size lAvailableSize = new Size
            {
                Height = Math.Max(lInputPortsSize.Height, lOutputPortsSize.Height),
                Width = lInputPortsSize.Width + lOutputPortsSize.Width
            };

            return lAvailableSize;
        }

        /// <summary>
        /// Arranges the port views by taking in account the computed size of the panel viewport.
        /// </summary>
        /// <param name="pFinalSize">The available size.</param>
        /// <returns>The size used (here equals to the available size).</returns>
        protected override Size ArrangeOverride(Size pFinalSize)
        {
            int lInputPortsCount = 0;
            int lOutputPortsCount = 0;

            // Both ports column can have columns of different sizes depending on the data template.
            double lInputPortsWidth = this.Children.Cast<PortView>().Where(pPort => pPort.Direction == PortDirection.Input).Max(pPort => pPort.DesiredSize.Width);
            lInputPortsWidth = Math.Round(lInputPortsWidth);
            double lOutputPortsWidth = this.Children.Cast<PortView>().Where(pPort => pPort.Direction == PortDirection.Output).Max(pPort => pPort.DesiredSize.Width);
            lOutputPortsWidth = Math.Round(lOutputPortsWidth);

            for (int i = 0, lCount = this.Children.Count; i < lCount; ++i)
            {
                PortView lPort = this.Children[i] as PortView;
                if (lPort != null)
                {
                    if (lPort.Direction == PortDirection.Input)
                    {
                        // Input ports are in the first column.
                        lPort.Arrange(new Rect(0.0, lInputPortsCount * lPort.DesiredSize.Height, lInputPortsWidth, lPort.DesiredSize.Height));
                        lInputPortsCount++;
                    }
                    else
                    {
                        // Output ports are in the second column.
                        lPort.Arrange(new Rect(lInputPortsWidth, lOutputPortsCount * lPort.DesiredSize.Height, lOutputPortsWidth, lPort.DesiredSize.Height));
                        lOutputPortsCount++;
                    }
                }
            }

            return pFinalSize;
        }

        #endregion // Methods.
    }
}

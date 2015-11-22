using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace XGraph.Controls
{
    /// <summary>
    /// Class defining an input connector.
    /// </summary>
    public class InputConnector : AConnector
    {
        #region Constructors

        /// <summary>
        /// Initializes the <see cref="InputConnector"/> class.
        /// </summary>
        static InputConnector()
        {
            // set the key to reference the style for this control
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(InputConnector), new FrameworkPropertyMetadata(typeof(InputConnector)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InputConnector"/> class.
        /// </summary>
        /// <param name="pParentPort">The connector parent port.</param>
        public InputConnector(PortView pParentPort)
            : base(pParentPort)
        {
        }

        #endregion // Constructors.
    }
}

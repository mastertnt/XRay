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
    public class OutputConnector : AConnector
    {
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
        public OutputConnector()
        {
        }

        #endregion // Constructors.
    }
}

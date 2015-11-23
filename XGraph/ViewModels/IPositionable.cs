using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XGraph.ViewModels
{
    /// <summary>
    /// Interface defining a positionable element.
    /// </summary>
    public interface IPositionable
    {
        #region Properties

        /// <summary>
        /// Gets or sets the x position of the node.
        /// </summary>
        double X
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the y position of the node.
        /// </summary>
        double Y
        {
            get;
            set;
        }

        #endregion // Properties.
    }
}

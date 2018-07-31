using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandTest
{
    /// <summary>
    /// Interface defining an object having a tooltip.
    /// </summary>
    public interface ITooltipable
    {
        #region Properties

        /// <summary>
        /// Gets or sets the tooltip description.
        /// </summary>
        string Tooltip
        {
            get;
            set;
        }

        #endregion // Properties.
    }
}

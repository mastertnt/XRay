using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XControls
{
    /// <summary>
    /// Class defining a control that has to be updated when the popup is displayed.
    /// </summary>
    public interface IUpdateable
    {
        /// <summary>
        /// Update the control.
        /// </summary>
        void Update();
    }
}

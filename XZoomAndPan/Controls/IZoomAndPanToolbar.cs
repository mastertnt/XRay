using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XZoomAndPan.Controls
{
    /// <summary>
    /// Base interface for a zoom and pan control toolbar.
    /// </summary>
    public interface IZoomAndPanToolbar
    {
        /// <summary>
        /// Binds the tool bar to the given zoom and pan control.
        /// </summary>
        /// <param name="pControl">The control to bind.</param>
        void BindToControl(AZoomAndPanControl pControl);
    }
}

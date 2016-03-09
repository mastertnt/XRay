using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XZoomAndPan.Behaviors
{
    /// <summary>
    /// Defines the current state of the mouse handling logic.
    /// </summary>
    public enum MouseHandlingMode
    {
        /// <summary>
        /// Not in any special mode.
        /// </summary>
        None,

        /// <summary>
        /// The user is panning the viewport.
        /// </summary>
        Panning,

        /// <summary>
        /// The user is zooming in or out in the view.
        /// </summary>
        Zooming,

        /// <summary>
        /// The user is doing vertical scroll.
        /// </summary>
        VerticalScrolling,

        /// <summary>
        /// The user is doing horizontal scroll.
        /// </summary>
        HorizontalScrolling
    }
}

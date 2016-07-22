﻿using System;
using System.Collections.Generic;
using System.Text;

namespace XControls
{
    /// <summary>
    /// Enum defining the selection behavior of a control.
    /// </summary>
    public enum AutoSelectBehavior
    {
        /// <summary>
        /// Do not select the text.
        /// </summary>
        Never,

        /// <summary>
        /// Select all the text on get focus.
        /// </summary>
        OnFocus
    }
}

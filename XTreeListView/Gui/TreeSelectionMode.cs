using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XTreeListView.Gui
{
    /// <summary>
    /// Enumeration which defines the selection behavior.
    /// </summary>
    public enum TreeSelectionMode
    {
        /// <summary>
        /// No selection is available.
        /// </summary>
        NoSelection = 0,

        /// <summary>
        /// Selection mode corresponding to single selection (only one item).
        /// </summary>
        SingleSelection = 1,

        /// <summary>
        /// Selection mode corresponding to multi selection.
        /// </summary>
        MultiSelection = 2
    };
}

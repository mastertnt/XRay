using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XControls.Core
{
    /// <summary>
    /// Class containing constants fields.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// Constant representing the infinity symbol.
        /// </summary>
        public const char INFINITY_SYMBOL = (char)0x221E;

        /// <summary>
        /// Constant representing the approximation symbol.
        /// </summary>
        public const string APPROXIMATION_SYMBOL = "~";

        /// <summary>
        /// Stores the watermark string format when the symbol must be displayed as well.
        /// </summary>
        public const string WATERMARK_WITH_SYMBOL_STRING_FORMAT = "{0} ({1})";
    }
}

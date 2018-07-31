using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Markup;
using XControls.Core;

namespace XControls.Core.Markup
{
    /// <summary>
    /// Class allowing the user to display an infinite symbol into a WPF control.
    /// </summary>
    [MarkupExtensionReturnType(typeof(string))]
    public class InfiniteStringMarkupExtension : MarkupExtension
    {
        #region Methods

        /// <summary>
        /// Returns the infinite symbol.
        /// </summary>
        /// <param name="pServiceProvider">The execution context.</param>
        /// <returns>The infinite symbol.</returns>
        public override object ProvideValue(IServiceProvider pServiceProvider)
        {
            return Constants.INFINITY_SYMBOL.ToString();
        }

        #endregion // Methods.
    }
}

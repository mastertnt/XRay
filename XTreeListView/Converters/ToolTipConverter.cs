using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace XTreeListView.Converters
{
    /// <summary>
    /// Class defining a converter used to hide the tooltip if it is not valid.
    /// </summary>
    public class ToolTipConverter : IValueConverter
    {
        #region Methods

        /// <summary>
        /// Convert from tooltip property to GUI tooltip.
        /// </summary>
        /// <param name="pValue">The value to convert.</param>
        /// <param name="pTargetType">The target type.</param>
        /// <param name="pExtraParameter">The extra parameter to use (not used by the lConverter).</param>
        /// <param name="pCulture">The culture to use (not used by the lConverter).</param>
        /// <returns>The value converted.</returns>
        public object Convert(object pValue, Type pTargetType, object pExtraParameter, CultureInfo pCulture)
        {
            string lStringToolTip = pValue as string;
            if (lStringToolTip is string && string.IsNullOrEmpty(lStringToolTip))
            {
                // Do not display the tooltip is the string to display is empty.
                return null;
            }

            return pValue;
        }

        /// <summary>
        /// Does nothing.
        /// </summary>
        /// <param name="pValue">The value to convert.</param>
        /// <param name="pTargetType">The target type.</param>
        /// <param name="pExtraParameter">The extra parameter to use (not used by the lConverter).</param>
        /// <param name="pCulture">The culture to use (not used by the lConverter).</param>
        /// <returns>The value converted.</returns>
        public object ConvertBack(object pValue, Type pTargetType, object pExtraParameter, CultureInfo pCulture)
        {
            return Binding.DoNothing;
        }

        #endregion // Methods.
    }
}

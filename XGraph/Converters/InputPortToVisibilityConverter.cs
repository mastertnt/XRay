using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using XGraph.ViewModels;

namespace XGraph.Converters
{
    /// <summary>
    /// Converts the input port direction to a visibility. 
    /// </summary>
    public class InputPortToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Convert from B to A.
        /// </summary>
        /// <param name="pValue">The value to convert.</param>
        /// <param name="pTargetType">The target type.</param>
        /// <param name="pExtraParameter">The extra parameter to use (not used by the converter).</param>
        /// <param name="pCulture">The culture to use (not used by the converter).</param>
        public object ConvertBack(object pValue, Type pTargetType, object pExtraParameter, CultureInfo pCulture)
        {
            return pValue;
        }

        /// <summary>
        /// Convert from A to B.
        /// </summary>
        /// <param name="pValue">The value to convert.</param>
        /// <param name="pTargetType">The target type.</param>
        /// <param name="pExtraParameter">The extra parameter to use (not used by the converter).</param>
        /// <param name="pCulture">The culture to use (not used by the converter).</param>
        /// <returns>The value converted.</returns>
        public Object Convert(object pValue, Type pTargetType, object pExtraParameter, CultureInfo pCulture)
        {
            PortDirection lDirection = (PortDirection)pValue;
            return lDirection == PortDirection.Input ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}

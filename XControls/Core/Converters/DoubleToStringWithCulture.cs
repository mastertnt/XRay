using System;
using System.Globalization;
using System.Windows.Data;

namespace XControls.Core.Converters
{
    /// <summary>
    ///     This converter is only used to convert a string to double with the current culture.
    /// </summary>
    /// <!-- NBY -->
    [ValueConversion(typeof(double), typeof(string))]
    public class DoubleToStringWithCulture : IValueConverter
    {
        /// <summary>
        ///     Convert from B to A.
        /// </summary>
        /// <param name="pValue">The value to convert.</param>
        /// <param name="pTargetType">The target type.</param>
        /// <param name="pExtraParameter">The extra parameter to use (not used by the converter).</param>
        /// <param name="pCulture">The culture to use (not used by the converter).</param>
        /// <returns>he converted value.</returns>
        public object ConvertBack(object pValue, Type pTargetType, object pExtraParameter, CultureInfo pCulture)
        {
            if (pValue is string)
            {
                double lValue;
                var lIsNum = double.TryParse(pValue as string, out lValue);
                if (lIsNum)
                {
                    return lValue;
                }
            }

            return Binding.DoNothing;
        }

        /// <summary>
        ///     Convert from A to B.
        /// </summary>
        /// <param name="pValue">The value to convert.</param>
        /// <param name="pTargetType">The target type.</param>
        /// <param name="pExtraParameter">The extra parameter to use (not used by the converter).</param>
        /// <param name="pCulture">The culture to use (not used by the converter).</param>
        /// <returns>The converted value.</returns>
        public object Convert(object pValue, Type pTargetType, object pExtraParameter, CultureInfo pCulture)
        {
            if (pValue is double)
            {
                return System.Convert.ToString(pValue);
            }

            return Binding.DoNothing;
        }
    }
}
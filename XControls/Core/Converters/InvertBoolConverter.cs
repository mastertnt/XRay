using System;
using System.Globalization;
using System.Windows.Data;

namespace XControls.Core.Converters
{
    /// <summary>
    ///     This lConverter allows to invert a boolean value.
    /// </summary>
    /// <!-- DPE -->
    [ValueConversion(typeof(bool), typeof(bool))]
    public class InvertBoolConverter : IValueConverter
    {
        #region Methods

        /// <summary>
        ///     Convert from Boolean to Boolean.
        /// </summary>
        /// <param name="pValue">The value to convert.</param>
        /// <param name="pTargetType">The target type.</param>
        /// <param name="pExtraParameter">The extra parameter to use (not used by the lConverter).</param>
        /// <param name="pCulture">The culture to use (not used by the lConverter).</param>
        /// <returns>The value converted.</returns>
        public object ConvertBack(object pValue, Type pTargetType, object pExtraParameter, CultureInfo pCulture)
        {
            if (pValue != null && pValue is bool)
            {
                return !(bool) pValue;
            }

            return Binding.DoNothing;
        }

        /// <summary>
        ///     Convert from Boolean to Boolean.
        /// </summary>
        /// <param name="pValue">The value to convert.</param>
        /// <param name="pTargetType">The target type.</param>
        /// <param name="pExtraParameter">The extra parameter to use (not used by the lConverter).</param>
        /// <param name="pCulture">The culture to use (not used by the lConverter).</param>
        /// <returns>The value converted.</returns>
        public object Convert(object pValue, Type pTargetType, object pExtraParameter, CultureInfo pCulture)
        {
            if (pValue != null && pValue is bool)
            {
                return !(bool) pValue;
            }

            return Binding.DoNothing;
        }

        #endregion // Methods.
    }
}
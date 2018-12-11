using System;
using System.Globalization;
using System.Windows.Data;

namespace XControls.Core.Converters
{
    /// <summary>
    ///     Converter inverting a boolean.
    /// </summary>
    public class InverseBoolConverter : IValueConverter
    {
        #region Methods

        /// <summary>
        ///     Converts.
        /// </summary>
        /// <param name="pValue">The value to convert.</param>
        /// <param name="pTargetType">The target type.</param>
        /// <param name="pExtraParameter">The extra parameter to use (not used by the lConverter).</param>
        /// <param name="pCulture">The culture to use (not used by the lConverter).</param>
        /// <returns>The value converted.</returns>
        public object Convert(object pValue, Type pTargetType, object pExtraParameter, CultureInfo pCulture)
        {
            return !(bool) pValue;
        }

        /// <summary>
        ///     Converts back.
        /// </summary>
        /// <param name="pValue">The value to convert.</param>
        /// <param name="pTargetType">The target type.</param>
        /// <param name="pExtraParameter">The extra parameter to use (not used by the lConverter).</param>
        /// <param name="pCulture">The culture to use (not used by the lConverter).</param>
        /// <returns>The value converted.</returns>
        public object ConvertBack(object pValue, Type pTargetType, object pExtraParameter, CultureInfo pCulture)
        {
            throw new NotImplementedException();
        }

        #endregion // Methods.
    }
}
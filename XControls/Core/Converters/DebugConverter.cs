using System;
using System.Globalization;
using System.Windows.Data;

namespace XControls.Core.Converters
{
    /// <summary>
    ///     Class defining a debug converter.
    /// </summary>
    public class DebugConverter : IValueConverter
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
        public object Convert(object pValue, Type pTargetType, object pParameter, CultureInfo pCulture)
        {
            if (pValue == null)
            {
                return "Null";
            }

            return pValue;
        }

        /// <summary>
        ///     Converts back.
        /// </summary>
        /// <param name="pValue">The value to convert.</param>
        /// <param name="pTargetType">The target type.</param>
        /// <param name="pExtraParameter">The extra parameter to use (not used by the lConverter).</param>
        /// <param name="pCulture">The culture to use (not used by the lConverter).</param>
        /// <returns>The value converted.</returns>
        public object ConvertBack(object pValue, Type pTargetType, object pParameter, CultureInfo pCulture)
        {
            return pValue;
        }

        #endregion // Methods.
    }
}
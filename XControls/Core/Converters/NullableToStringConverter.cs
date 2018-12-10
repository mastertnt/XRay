using System;
using System.Globalization;
using System.Windows.Data;

namespace XControls.Core.Converters
{
    /// <summary>
    ///     This lConverter allows to convert from nullable object to String type.
    /// </summary>
    /// <!-- DPE -->
    [ValueConversion(typeof(string), typeof(object))]
    public class NullableToStringConverter : IValueConverter
    {
        #region Methods

        /// <summary>
        ///     Convert from String to Nullable.
        /// </summary>
        /// <param name="pValue">The value to convert.</param>
        /// <param name="pTargetType">The target type.</param>
        /// <param name="pExtraParameter">The extra parameter to use (not used by the lConverter).</param>
        /// <param name="pCulture">The culture to use (not used by the lConverter).</param>
        /// <returns>The value converted.</returns>
        public object ConvertBack(object pValue, Type pTargetType, object pExtraParameter, CultureInfo pCulture)
        {
            if (!(pValue is string))
            {
                return null;
            }

            if ((pValue as string).Equals(string.Empty))
            {
                return null;
            }

            if (pTargetType.IsGenericType && pTargetType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                var lArguments = pTargetType.GetGenericArguments();
                return System.Convert.ChangeType(pValue, lArguments[0]);
            }

            return System.Convert.ChangeType(pValue, pTargetType);
        }

        /// <summary>
        ///     Convert from Nullable to String.
        /// </summary>
        /// <param name="pValue">The value to convert.</param>
        /// <param name="pTargetType">The target type.</param>
        /// <param name="pExtraParameter">The extra parameter to use (not used by the lConverter).</param>
        /// <param name="pCulture">The culture to use (not used by the lConverter).</param>
        /// <returns>The value converted.</returns>
        public object Convert(object pValue, Type pTargetType, object pExtraParameter, CultureInfo pCulture)
        {
            if (pValue == null)
            {
                return string.Empty;
            }

            return System.Convert.ToString(pValue);
        }

        #endregion
    }
}
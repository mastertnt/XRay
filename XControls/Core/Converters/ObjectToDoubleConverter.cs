using System;
using System.Globalization;
using System.Windows.Data;

namespace XControls.Core.Converters
{
    /// <summary>
    ///     This lConverter allows to convert from Object to Double.
    /// </summary>
    /// <!-- NBY -->
    [ValueConversion(typeof(object), typeof(double))]
    public class ObjectToDoubleConverter : IValueConverter
    {
        #region Methods

        /// <summary>
        ///     Convert from Double to Object.
        /// </summary>
        /// <param name="pValue">The value to convert.</param>
        /// <param name="pTargetType">The target type.</param>
        /// <param name="pExtraParameter">The extra parameter to use (not used by the lConverter).</param>
        /// <param name="pCulture">The culture to use (not used by the lConverter).</param>
        public object ConvertBack(object pValue, Type pTargetType, object pExtraParameter, CultureInfo pCulture)
        {
            return pValue;
        }

        /// <summary>
        ///     Convert from Object to Double.
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
                return null;
            }

            // Explicit unboxing
            var lType = pValue.GetType();
            if (lType == typeof(int))
            {
                return pValue;
            }

            if (lType == typeof(long))
            {
                return pValue;
            }

            if (lType == typeof(byte))
            {
                return pValue;
            }

            if (lType == typeof(double))
            {
                return pValue;
            }

            if (lType == typeof(float))
            {
                return pValue;
            }

            return Binding.DoNothing;
        }

        #endregion // Methods.
    }
}
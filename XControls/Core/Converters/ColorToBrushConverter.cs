using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace XControls.Core.Converters
{
    /// <summary>
    ///     This lConverter allows to convert from Brush to Color type.
    /// </summary>
    /// <!-- NBY -->
    [ValueConversion(typeof(Color), typeof(Brush))]
    public class ColorToBrushConverter : IValueConverter
    {
        #region Fields

        /// <summary>
        ///     This fiels stores the invert lConverter.
        /// </summary>
        private readonly BrushToColorConverter mInvertConverter = new BrushToColorConverter();

        #endregion // Fields.

        #region Methods

        /// <summary>
        ///     Convert from Brush to Color.
        /// </summary>
        /// <param name="pValue">The value to convert.</param>
        /// <param name="pTargetType">The target type.</param>
        /// <param name="pExtraParameter">The extra parameter to use (not used by the lConverter).</param>
        /// <param name="pCulture">The culture to use (not used by the lConverter).</param>
        /// <returns>The value converted.</returns>
        public object ConvertBack(object pValue, Type pTargetType, object pExtraParameter, CultureInfo pCulture)
        {
            return this.mInvertConverter.Convert(pValue, pTargetType, pExtraParameter, pCulture);
        }

        /// <summary>
        ///     Convert from Color to Brush.
        /// </summary>
        /// <param name="pValue">The value to convert.</param>
        /// <param name="pTargetType">The target type.</param>
        /// <param name="pExtraParameter">The extra parameter to use (not used by the lConverter).</param>
        /// <param name="pCulture">The culture to use (not used by the lConverter).</param>
        /// <returns>The value converted.</returns>
        public object Convert(object pValue, Type pTargetType, object pExtraParameter, CultureInfo pCulture)
        {
            return this.mInvertConverter.ConvertBack(pValue, pTargetType, pExtraParameter, pCulture);
        }

        #endregion // Methods.
    }
}
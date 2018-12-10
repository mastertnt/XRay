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
    [ValueConversion(typeof(Brush), typeof(Color))]
    public class BrushToColorConverter : IValueConverter
    {
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
            if (pValue == null)
            {
                return Binding.DoNothing;
            }

            if (pValue is System.Drawing.Color)
            {
                var lFormColor = (System.Drawing.Color) pValue;
                var lMediaColor = new Color();
                lMediaColor.R = lFormColor.R;
                lMediaColor.G = lFormColor.G;
                lMediaColor.B = lFormColor.B;
                lMediaColor.A = lFormColor.A;
                return new SolidColorBrush(lMediaColor);
            }

            if (pValue is Color)
            {
                return new SolidColorBrush((Color) pValue);
            }

            return Binding.DoNothing;
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
            var lSolidBrush = pValue as SolidColorBrush;

            if (lSolidBrush != null)
            {
                return lSolidBrush.Color;
            }

            return Binding.DoNothing;
        }

        #endregion // Methods.
    }
}
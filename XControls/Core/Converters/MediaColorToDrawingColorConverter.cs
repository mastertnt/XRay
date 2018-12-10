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
    [ValueConversion(typeof(Color), typeof(System.Drawing.Color))]
    public class MediaColorToDrawingColorConverter : IValueConverter
    {
        #region Fields

        /// <summary>
        ///     Used as singleton.
        /// </summary>
        public static MediaColorToDrawingColorConverter Instance = new MediaColorToDrawingColorConverter();

        #endregion

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
            var lInColor = (System.Drawing.Color) pValue;
            var lOutColor = new Color();
            lOutColor.R = lInColor.R;
            lOutColor.G = lInColor.G;
            lOutColor.B = lInColor.B;
            lOutColor.A = lInColor.A;
            return lOutColor;
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
            var lInColor = (Color) pValue;
            var lOutColor = System.Drawing.Color.FromArgb(lInColor.A, lInColor.R, lInColor.G, lInColor.B);
            return lOutColor;
        }

        #endregion // Methods.
    }
}
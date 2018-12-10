using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace XControls.Core.Converters
{
    /// <summary>
    ///     This class converts two booleans (IsValid and HasBeenEdited) to a color code.
    ///     Green for HasBeenEdited = true and IsValid = true
    ///     Orange for HasBeenEdited = false and IsValid = true
    ///     Red for IsValid = false
    /// </summary>
    /// <!-- NBY -->
    public class ValidationToBrushConverter : IMultiValueConverter
    {
        #region Methods

        /// <summary>
        ///     Convert from Int32 to Boolean.
        /// </summary>
        /// <param name="pValue">The value to convert.</param>
        /// <param name="pTargetTypes">The target types.</param>
        /// <param name="pExtraParameter">The extra parameter to use (not used by the lConverter).</param>
        /// <param name="pCulture">The culture to use (not used by the lConverter).</param>
        /// <returns>The value converted.</returns>
        public object[] ConvertBack(object pValue, Type[] pTargetTypes, object pExtraParameter, CultureInfo pCulture)
        {
            // Do nothing.
            return null;
        }

        /// <summary>
        ///     Convert from two Booleans to Color.
        /// </summary>
        /// <param name="pValues">The values to convert.</param>
        /// <param name="pTargetType">The target type.</param>
        /// <param name="pExtraParameter">The extra parameter to use (not used by the lConverter).</param>
        /// <param name="pCulture">The culture to use (not used by the lConverter).</param>
        /// <returns>The value converted.</returns>
        public object Convert(object[] pValues, Type pTargetType, object pExtraParameter, CultureInfo pCulture)
        {
            if (pValues.Length < 2)
            {
                return new SolidColorBrush(Colors.White);
            }

            if (pValues[0] == null || pValues[1] == null)
            {
                return new SolidColorBrush(Colors.White);
            }


            var lIsValid = (bool) pValues[0];
            var lHasBeenUsedEdited = (bool) pValues[1];

            if (lIsValid == false)
            {
                return new SolidColorBrush((Color) ColorConverter.ConvertFromString("#FFA3A2"));
            }

            if (lHasBeenUsedEdited)
            {
                return new SolidColorBrush((Color) ColorConverter.ConvertFromString("#DAFDA8"));
            }


            return new SolidColorBrush((Color) ColorConverter.ConvertFromString("#FFBE87"));
        }

        #endregion // Methods.
    }
}
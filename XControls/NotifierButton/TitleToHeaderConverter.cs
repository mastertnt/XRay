using System;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace XControls.NotifierButton
{
    /// <summary>
    /// Converter used to convert the notification title to the view header.
    /// </summary>
    public class TitleToHeaderConverter : IMultiValueConverter
    {
        #region Methods

        /// <summary>
        /// Convert from the title to the header.
        /// </summary>
        /// <param name="pValues">The values to convert.</param>
        /// <param name="pTargetType">The target type.</param>
        /// <param name="pParameter">Additional parameters.</param>
        /// <param name="pCulture">The culture info.</param>
        /// <returns>The converted value.</returns>
        public object Convert(object[] pValues, Type pTargetType, object pParameter, System.Globalization.CultureInfo pCulture)
        {
            if (pValues.Count() != 2 || pValues[0] == DependencyProperty.UnsetValue || pValues[1] == DependencyProperty.UnsetValue)
            {
                return Binding.DoNothing;
            }

            int lIndex = (int)pValues[0];
            string lTitle = pValues[1] as string;

            return string.Format("[{0}] {1}", lIndex, lTitle);
        }

        /// <summary>
        /// Does nothing.
        /// </summary>
        /// <param name="pValue">The value to convert.</param>
        /// <param name="pTargetTypes">The target types.</param>
        /// <param name="pParameter">Additional parameters.</param>
        /// <param name="pCulture">The culture info.</param>
        /// <returns>The converted values.</returns>
        public object[] ConvertBack(object pValue, Type[] pTargetTypes, object pParameter, System.Globalization.CultureInfo pCulture)
        {
            return new object[] { Binding.DoNothing };
        }

        #endregion // Methods.
    }
}

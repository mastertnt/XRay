using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace XControls.Core.Converters
{
    /// <summary>
    ///     This lConverter allows to convert from Boolean to Visibility type.
    /// </summary>
    /// <!-- NBY -->
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class BooleanToVisibilityConverter : IValueConverter
    {
        #region Properties

        /// <summary>
        ///     This property is used to invert the visibility with the lConverter.
        /// </summary>
        public bool InvertVisibility
        {
            get;
            set;
        }

        /// <summary>
        ///     This property is used to set the visibility equals to "False"
        /// </summary>
        public Visibility NotVisibleValue
        {
            get;
            set;
        }

        #endregion // Properties.

        #region Methods

        /// <summary>
        ///     Default constructor.
        /// </summary>
        public BooleanToVisibilityConverter()
        {
            this.InvertVisibility = false;
            this.NotVisibleValue = Visibility.Collapsed;
        }

        /// <summary>
        ///     Convert from Visibility to Boolean.
        /// </summary>
        /// <param name="pValue">The value to convert.</param>
        /// <param name="pTargetType">The target type.</param>
        /// <param name="pExtraParameter">The extra parameter to use (not used by the lConverter).</param>
        /// <param name="pCulture">The culture to use (not used by the lConverter).</param>
        /// <returns>The value converted.</returns>
        public object ConvertBack(object pValue, Type pTargetType, object pExtraParameter, CultureInfo pCulture)
        {
            return pValue is Visibility && (Visibility) pValue == Visibility.Visible ? !this.InvertVisibility : this.InvertVisibility;
        }

        /// <summary>
        ///     Convert from Boolean to Visibility.
        /// </summary>
        /// <param name="pValue">The value to convert.</param>
        /// <param name="pTargetType">The target type.</param>
        /// <param name="pExtraParameter">The extra parameter to use (not used by the lConverter).</param>
        /// <param name="pCulture">The culture to use (not used by the lConverter).</param>
        /// <returns>The value converted.</returns>
        public object Convert(object pValue, Type pTargetType, object pExtraParameter, CultureInfo pCulture)
        {
            // Checks if the value is valid.
            if (pValue == null)
            {
                return Visibility.Visible;
            }

            var lIsVisible = true;

            // Convert according to the type.
            if (pValue is bool)
            {
                lIsVisible = (bool) pValue;
            }
            else if (pValue is bool?)
            {
                var lNullable = (bool?) pValue;
                lIsVisible = lNullable.HasValue ? lNullable.Value : false;
            }

            // Checks if the value must be invert.
            if (this.InvertVisibility)
            {
                lIsVisible = !lIsVisible;
            }

            return lIsVisible ? Visibility.Visible : this.NotVisibleValue;
        }

        #endregion // Methods.
    }
}
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
    [ValueConversion(typeof(int), typeof(Visibility))]
    public class Int32ToVisibilityConverter : IValueConverter
    {
        #region Properties

        /// <summary>
        ///     Gets or sets the visibility equals to 0.
        /// </summary>
        public Visibility ZeroValue
        {
            get;
            set;
        }


        /// <summary>
        ///     Gets or sets the visibility not equals to 0.
        /// </summary>
        public Visibility NotZeroValue
        {
            get;
            set;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Default constructor.
        /// </summary>
        public Int32ToVisibilityConverter()
        {
            this.ZeroValue = Visibility.Collapsed;
            this.NotZeroValue = Visibility.Visible;
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
            // Checks if the value is valid.
            if (pValue == null)
            {
                return 0;
            }

            var lVisibility = (Visibility) pValue;
            if (lVisibility == this.NotZeroValue)
            {
                return 1;
            }

            return 0;
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
            if (pValue == null || !(pValue is int))
            {
                return Visibility.Visible;
            }

            var lValue = (int) pValue;
            if (lValue == 0)
            {
                return this.ZeroValue;
            }

            return this.NotZeroValue;
        }

        #endregion // Methods.
    }
}
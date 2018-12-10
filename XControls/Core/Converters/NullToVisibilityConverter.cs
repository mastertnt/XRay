using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace XControls.Core.Converters
{
    /// <summary>
    ///     This lConverter allows to convert from Null to Visibility type.
    /// </summary>
    /// <!-- NBY -->
    [ValueConversion(typeof(Visibility), typeof(object))]
    public class NullToVisibilityConverter : IValueConverter
    {
        #region Properties

        /// <summary>
        ///     Gets or set the visibility corresponding to "null".
        /// </summary>
        public Visibility NullVisibility
        {
            get;
            set;
        }

        /// <summary>
        ///     Gets or set the visibility corresponding to not "null".
        /// </summary>
        public Visibility NotNullVisibility
        {
            get;
            set;
        }

        #endregion // Properties.

        #region Methods

        /// <summary>
        ///     Default constructor.
        /// </summary>
        public NullToVisibilityConverter()
        {
            this.NullVisibility = Visibility.Collapsed;
            this.NotNullVisibility = Visibility.Visible;
        }

        /// <summary>
        ///     Convert from Visibility to Null.
        /// </summary>
        /// <param name="pValue">The value to convert.</param>
        /// <param name="pTargetType">The target type.</param>
        /// <param name="pExtraParameter">The extra parameter to use (not used by the lConverter).</param>
        /// <param name="pCulture">The culture to use (not used by the lConverter).</param>
        /// <returns>The value converted.</returns>
        public object ConvertBack(object pValue, Type pTargetType, object pExtraParameter, CultureInfo pCulture)
        {
            return null;
        }

        /// <summary>
        ///     Convert from Null to Visibility.
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
                return this.NullVisibility;
            }

            return this.NotNullVisibility;
        }

        #endregion
    }
}
using System;
using System.Windows.Data;

namespace XTreeListView.Converters
{
    /// <summary>
    /// This lConverter allows to convert from Object nullness to Boolean type.
    /// </summary>
    /// <!-- NBY -->
    [ValueConversion(typeof(Boolean), typeof(Object))]
    public class NullToBoolConverter : IValueConverter
    {
        #region Properties

        /// <summary>
        /// Gets or set the visibility corresponding to "True".
        /// </summary>
        public Boolean NullBoolean
        { 
            get; 
            set; 
        }

        /// <summary>
        /// Gets or set the visibility corresponding to "False".
        /// </summary>
        public Boolean NotNullBoolean
        { 
            get; 
            set; 
        }

        #endregion // Properties.

        #region Methods

        /// <summary>
        /// Default constructor.
        /// </summary>
        public NullToBoolConverter()
        {
            this.NotNullBoolean = false;
            this.NullBoolean = true;
        }

        /// <summary>
        /// Convert from Visibility to Bool.
        /// </summary>
        /// <param name="pValue">The value to convert.</param>
        /// <param name="pTargetType">The target type.</param>
        /// <param name="pExtraParameter">The extra parameter to use (not used by the lConverter).</param>
        /// <param name="pCulture">The culture to use (not used by the lConverter).</param>
        /// <returns>The value converted.</returns>
        public Object ConvertBack(Object pValue, Type pTargetType, Object pExtraParameter, System.Globalization.CultureInfo pCulture)
        {
            return null;
        }

        /// <summary>
        /// Convert from Bool to Visibility.
        /// </summary>
        /// <param name="pValue">The value to convert.</param>
        /// <param name="pTargetType">The target type.</param>
        /// <param name="pExtraParameter">The extra parameter to use (not used by the lConverter).</param>
        /// <param name="pCulture">The culture to use (not used by the lConverter).</param>
        /// <returns>The value converted.</returns>
        public Object Convert(Object pValue, Type pTargetType, Object pExtraParameter, System.Globalization.CultureInfo pCulture)
        {
            if
                (pValue == null)
            {
                return this.NullBoolean;
            }

            return this.NotNullBoolean;
        }

        #endregion
    }
}
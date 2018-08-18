using System;
using System.Windows.Data;

namespace XTreeListView.Converters
{
    /// <summary>
    /// This lConverter allows to convert from Object nullness to Boolean type.
    /// </summary>
    [ValueConversion(typeof(Boolean), typeof(Object))]
    public class NullToBoolConverter : IValueConverter
    {
        #region Properties

        /// <summary>
        /// Gets or set the visibility corresponding to "True".
        /// </summary>
        public bool NullBoolean
        { 
            get; 
            set; 
        }

        /// <summary>
        /// Gets or set the visibility corresponding to "False".
        /// </summary>
        public bool NotNullBoolean
        { 
            get; 
            set; 
        }

        #endregion // Properties.

        #region Methods

        /// <summary>
        /// Initialiazes a new instance of the <see cref="NullToBoolConverter"/> class.
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
        public object ConvertBack(object pValue, Type pTargetType, object pExtraParameter, System.Globalization.CultureInfo pCulture)
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
        public object Convert(object pValue, Type pTargetType, object pExtraParameter, System.Globalization.CultureInfo pCulture)
        {
            if (pValue == null)
            {
                return this.NullBoolean;
            }

            return this.NotNullBoolean;
        }

        #endregion // Methods.
    }
}
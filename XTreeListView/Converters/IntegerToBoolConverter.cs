using System;
using System.Windows;
using System.Windows.Data;

namespace XTreeListView.Converters
{
    /// <summary>
    /// This converter allows to convert from integer to boolean type.
    /// </summary>
    [ValueConversion(typeof(int), typeof(bool))]
    public class IntegerToBoolConverter : IValueConverter
    {
        #region Properties

        /// <summary>
        /// Gets or sets the boolean equals to 0.
        /// </summary>
        public bool ZeroValue 
        { 
            get; 
            set; 
        }

        #endregion // Properties.

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="IntegerToBoolConverter"/> class.
        /// </summary>
        public IntegerToBoolConverter()
        {
            this.ZeroValue = false;
        }

        #endregion // Constructors.

        #region Methods

        /// <summary>
        /// Convert from boolean to integer.
        /// </summary>
        /// <param name="pValue">The value to convert.</param>
        /// <param name="pTargetType">The target type.</param>
        /// <param name="pExtraParameter">The extra parameter to use (not used by the lConverter).</param>
        /// <param name="pCulture">The culture to use (not used by the lConverter).</param>
        /// <returns>The value converted.</returns>
        public object ConvertBack(object pValue, Type pTargetType, object pExtraParameter, System.Globalization.CultureInfo pCulture)
        {
            return Binding.DoNothing;
        }

        /// <summary>
        /// Convert from integer to boolean.
        /// </summary>
        /// <param name="pValue">The value to convert.</param>
        /// <param name="pTargetType">The target type.</param>
        /// <param name="pExtraParameter">The extra parameter to use (not used by the lConverter).</param>
        /// <param name="pCulture">The culture to use (not used by the lConverter).</param>
        /// <returns>The value converted.</returns>
        public object Convert(object pValue, Type pTargetType, object pExtraParameter, System.Globalization.CultureInfo pCulture)
        {
            // Checks if the value is valid.
            if  (   (pValue == null)
                ||  (pValue is int == false)
                )
            {
                return false;
            }

            int lValue = (int)pValue;
            if (lValue == 0)
            {
                return this.ZeroValue;
            }

            return this.ZeroValue == false;
        }

        #endregion // Methods.
    }
}

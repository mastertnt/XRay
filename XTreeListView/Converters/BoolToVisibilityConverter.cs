using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows;
using System.Globalization;

namespace XTreeListView.Converters
{
    /// <summary>
    /// Converter used to convert a boolean to visibility.
    /// </summary>
    [ValueConversion(typeof(bool), typeof(Visibility))]
    internal class BoolToVisibilityConverter : IValueConverter
    {
        #region Properties

        /// <summary>
        /// Gets or sets the visibility value if the boolean is true.
        /// </summary>
        public Visibility TrueValue
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the visibility value if the boolean is false.
        /// </summary>
        public Visibility FalseValue
        {
            get;
            set;
        }

        #endregion // Properties.
        
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BoolToVisibilityConverter"/> class.
        /// </summary>
        public BoolToVisibilityConverter()
        {
            this.TrueValue = Visibility.Visible;
            this.FalseValue = Visibility.Hidden;
        }

        #endregion // Constructors.

        #region Methods

        /// <summary>
        /// Convert from boolean to Visibility.
        /// </summary>
        /// <param name="pValue">The value to convert.</param>
        /// <param name="pTargetType">The target type.</param>
        /// <param name="pExtraParameter">The extra parameter to use (not used by the lConverter).</param>
        /// <param name="pCulture">The culture to use (not used by the lConverter).</param>
        /// <returns>The value converted.</returns>
        public object Convert(object pValue, Type pTargetType, object pExtraParameter, CultureInfo pCulture)
        {
            bool lValue = System.Convert.ToBoolean(pValue);
            if (lValue)
            {
                return this.TrueValue;
            }
            else
            {
                return this.FalseValue;
            }
        }

        /// <summary>
        /// Convert from Visibility to Boolean (Not supported).
        /// </summary>
        /// <param name="pValue">The value to convert.</param>
        /// <param name="pTargetType">The target type.</param>
        /// <param name="pExtraParameter">The extra parameter to use (not used by the lConverter).</param>
        /// <param name="pCulture">The culture to use (not used by the lConverter).</param>
        /// <returns>The value converted.</returns>
        public object ConvertBack(object pValue, Type pTargetType, object pExtraParameter, CultureInfo pCulture)
        {
            return Binding.DoNothing;
        }

        #endregion // Methods.
    }
}

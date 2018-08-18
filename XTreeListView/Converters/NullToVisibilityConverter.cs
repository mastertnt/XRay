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
    [ValueConversion(typeof(object), typeof(Visibility))]
    internal class NullToVisibilityConverter : IValueConverter
    {
        #region Properties

        /// <summary>
        /// Gets or sets the visibility value if the value is null.
        /// </summary>
        public Visibility NullValue
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the visibility value if the value is not null.
        /// </summary>
        public Visibility NotNullValue
        {
            get;
            set;
        }

        #endregion // Properties.
        
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BoolToVisibilityConverter"/> class.
        /// </summary>
        public NullToVisibilityConverter()
        {
            this.NullValue = Visibility.Hidden;
            this.NotNullValue = Visibility.Visible;
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
            if (pValue == null)
            {
                return this.NullValue;
            }
            else
            {
                return this.NotNullValue;
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

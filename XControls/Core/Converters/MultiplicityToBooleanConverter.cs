using System;
using System.Globalization;
using System.Windows.Data;

namespace XControls.Core.Converters
{
    /// <summary>
    ///     This class converts an integer to boolean. The result is true if an integer is greather than one.
    /// </summary>
    /// <!-- NBY -->
    [ValueConversion(typeof(int), typeof(bool))]
    public class MultiplicityToBooleanConverter : IValueConverter
    {
        #region Constructors

        /// <summary>
        ///     Default constructor.
        /// </summary>
        public MultiplicityToBooleanConverter()
        {
            this.IncludeOne = false;
            this.ZeroBoolValue = false;
        }

        #endregion // Constructors.

        #region Properties

        /// <summary>
        ///     Gets or sets the flag to know if the '1' is considered as a multiple.
        ///     The default value is false.
        /// </summary>
        public bool IncludeOne
        {
            get;
            set;
        }

        /// <summary>
        ///     Gets or sets the boolean value to return if the value is zero (or one if the flag IncludeOne is set to true).
        /// </summary>
        public bool ZeroBoolValue
        {
            get;
            set;
        }

        #endregion // Properties.

        #region Methods

        /// <summary>
        ///     Convert from Int32 to Boolean.
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

        /// <summary>
        ///     Convert from Boolean to Int32.
        /// </summary>
        /// <param name="pValue">The value to convert.</param>
        /// <param name="pTargetType">The target type.</param>
        /// <param name="pExtraParameter">The extra parameter to use (not used by the lConverter).</param>
        /// <param name="pCulture">The culture to use (not used by the lConverter).</param>
        /// <returns>The value converted.</returns>
        public object Convert(object pValue, Type pTargetType, object pExtraParameter, CultureInfo pCulture)
        {
            try
            {
                var lValue = System.Convert.ToInt32(pValue);
                if (lValue == 0)
                {
                    return this.ZeroBoolValue;
                }

                if (this.IncludeOne == false && lValue == 1)
                {
                    return this.ZeroBoolValue;
                }

                return !this.ZeroBoolValue;
            }
            catch // Managed
            {
                // Convertion did not succeed...
                return false;
            }
        }

        #endregion // Methods.
    }
}
using System;
using System.Windows.Data;
using System.Globalization;
using System.Windows;

namespace XTreeListView.Converters
{
    /// <summary>
    /// This class defines a converter used to convert a width into a margin.
    /// </summary>
    public class WidthToLeftMarginConverter : IValueConverter
    {
        #region Constructors

        /// <summary>
        /// Insitializes a new instance of the <see cref="WidthToLeftMarginConverter"/> class.
        /// </summary>
        public WidthToLeftMarginConverter()
        {
            this.InvertMargin = false;
            this.Margin = 0.0;
        }

        #endregion // Constructors.

        #region Properties

        /// <summary>
        /// Gets or sets the flag indicating if the identation must be inverted.
        /// </summary>
        public bool InvertMargin 
        { 
            get; 
            set; 
        }

        /// <summary>
        /// Gets or sets the margin to remove to the width.
        /// </summary>
        public double Margin
        {
            get;
            set;
        }

        #endregion // Properties.

        #region Methods

        /// <summary>
        /// Convert from Level to Tickness.
        /// </summary>
        /// <param name="pValue">The value to convert.</param>
        /// <param name="pTargetType">The target type.</param>
        /// <param name="pExtraParameter">The extra parameter to use (not used by the lConverter).</param>
        /// <param name="pCulture">The culture to use (not used by the lConverter).</param>
        /// <returns>The value converted.</returns>
        public object Convert(object pValue, Type pTargetType, object pExtraParameter, CultureInfo pCulture)
        {
            double lLeftThickness = System.Convert.ToDouble(pValue);
            lLeftThickness -= this.Margin;

            if (this.InvertMargin)
            {
                lLeftThickness = -lLeftThickness;
            }

            return new Thickness(lLeftThickness, 0, 0, 0);
        }

        /// <summary>
        /// Does nothing.
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

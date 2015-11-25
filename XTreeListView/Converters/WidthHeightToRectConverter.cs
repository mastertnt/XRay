using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Globalization;
using System.Windows;

namespace XTreeListView.Converters
{
    /// <summary>
    /// This class convert a width and height into a rectangle.
    /// </summary>
    public class WidthHeightToRectConverter : IMultiValueConverter
    {
        #region Constructor

        /// <summary>
        /// Insitializes a new instance of the <see cref="WidthHeightToRectConverter"/> class.
        /// </summary>
        public WidthHeightToRectConverter()
        {
            this.Margin = new Thickness();
        }

        #endregion // Constructor.

        #region Properties

        /// <summary>
        /// Gets or sets the margin to apply to the rectangle.
        /// </summary>
        public Thickness Margin { get; set; }

        #endregion // Properties.

        #region Methods

        /// <summary>
        /// Convert from width and height to Rect.
        /// </summary>
        /// <param name="pValues">The width and the height.</param>
        /// <param name="pTargetType">The target type (rect).</param>
        /// <param name="pParameter">The converters additional parameters.</param>
        /// <param name="pCulture">The converter culture to use.</param>
        /// <returns>The converted rect.</returns>
        public object Convert(object[] pValues, Type pTargetType, object pParameter, CultureInfo pCulture)
        {
            double lWidth = (double)pValues[0];
            double lHeight = (double)pValues[1];

            // Building the rectangle.
            Rect lRect = new Rect();
            lRect.Width = lWidth;
            lRect.Height = lHeight;

            // Applying the margin.
            lRect.X += this.Margin.Left;
            lRect.Width = Math.Max(0.0, lRect.Width - this.Margin.Left);
            lRect.Y += this.Margin.Top;
            lRect.Height = Math.Max(0.0, lRect.Height - this.Margin.Top);
            lRect.Width = Math.Max(0.0, lRect.Width - this.Margin.Right);
            lRect.Height = Math.Max(0.0, lRect.Height - this.Margin.Bottom);

            // To be sure the rendering wont be affected.
            lRect.X -= 1;
            lRect.Y -= 1;
            lRect.Height += 1;
            lRect.Width += 1;

            return lRect;
        }

        /// <summary>
        /// Do nothing.
        /// </summary>
        /// <param name="pValues">The value to convert.</param>
        /// <param name="pTargetTypes">The target types.</param>
        /// <param name="pParameter">The converters additional parameters.</param>
        /// <param name="pCulture">The converter culture to use.</param>
        /// <returns>Binding.DoNothing</returns>
        public object[] ConvertBack(object pValues, Type[] pTargetTypes, object pParameter, CultureInfo pCulture)
        {
            return new object[] { Binding.DoNothing };
        }

        #endregion // Methods.
    }
}

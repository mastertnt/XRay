using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Globalization;
using System.Windows.Data;
using XTreeListView.ViewModel;

namespace XTreeListView.Converters
{
    /// <summary>
    /// This class indent the given item by computing the left margin.
    /// </summary>
    [ValueConversion(typeof(IHierarchicalItemViewModel), typeof(Thickness))]
    internal class LevelToIndentConverter : IValueConverter
    {
        #region Constants

        /// <summary>
        /// Constant used to set an indentation size (in pixels).
        /// </summary>
        private const double IndentSize = 19.0;

        #endregion // Constants.

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
            int lLevel = 0;
            IHierarchicalItemViewModel lItemToIndent = pValue as IHierarchicalItemViewModel;
            if
                (lItemToIndent != null)
            {
                IHierarchicalItemViewModel lCurrentItem = lItemToIndent;
                while
                    (lCurrentItem.Parent != null)
                {
                    lCurrentItem = lCurrentItem.Parent;
                    lLevel++;
                }
            }

            return new Thickness(lLevel * LevelToIndentConverter.IndentSize, 0, 0, 0);
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

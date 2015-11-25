using System;
using System.Linq;
using System.Windows.Data;
using System.Globalization;
using System.Diagnostics;
using XTreeListView.ViewModel;

namespace XTreeListView.Converters
{
    /// <summary>
    /// This class defines a converter which defines if the tree item is a group or not.
    /// </summary>
    internal class ItemToIsGroupConverter : IMultiValueConverter
    {
        #region Methods

        /// <summary>
        /// Converts the tree configuration to the item group state.
        /// </summary>
        /// <param name="pValues">The values defining the tree configuration.</param>
        /// <param name="pTargetType">The target type.</param>
        /// <param name="pParameter">The additional parameters.</param>
        /// <param name="pCulture">The culture to use during the conversion.</param>
        /// <returns>The returned item group state.</returns>
        public object Convert(object[] pValues, Type pTargetType, object pParameter, CultureInfo pCulture)
        {
            Debug.Assert(pValues.Count() == 2);

            try
            {
#pragma warning disable 1587
                /// To be a group, an item must have the root as its parent view model, and the FirstLevelItemAsGroup property to true.
                /// First parameter is the view model, second is the FirstLevelItemAsGroup property.
#pragma warning restore 1587
                IHierarchicalItemViewModel lViewModel = pValues[0] as IHierarchicalItemViewModel;
                bool lFirstLevelItemAsGroup = System.Convert.ToBoolean(pValues[1]);
                if
                    (   (lViewModel != null)
                    &&  (lViewModel.Parent is IRootHierarchicalItemViewModel)
                    &&  (lFirstLevelItemAsGroup)
                    )
                {
                    return true;
                }

                return false;
            }
            catch
            {
                return Binding.DoNothing;
            }
        }

        /// <summary>
        /// Converts the item group state to the tree configuration.
        /// </summary>
        /// <param name="pValue">The item group state.</param>
        /// <param name="pTargetType">The target type.</param>
        /// <param name="pParameter">The additional parameters.</param>
        /// <param name="pCulture">The culture to use during the conversion.</param>
        /// <returns>Returns nothing.</returns>
        public object[] ConvertBack(object pValue, Type[] pTargetType, object pParameter, CultureInfo pCulture)
        {
            return new object[] { Binding.DoNothing };
        }

        #endregion // Methods.
    }
}

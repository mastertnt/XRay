using System;
using System.Collections.Generic;
using System.Linq;

namespace XSystem.Collections
{
    /// <summary>
    ///     This class stores extension method on IEnumerable.
    /// </summary>
// ReSharper disable InconsistentNaming
    public static class IEnumerableExtensions
// ReSharper restore InconsistentNaming
    {
        /// <summary>
        ///     This method extends IEnumerable to add ForEach functionnality.
        /// </summary>
        /// <typeparam name="T">The templated type.</typeparam>
        /// <param name="pInstance">The instance to apply.</param>
        /// <param name="pAction">The action to apply.</param>
        public static void ForEach<T>(this IEnumerable<T> pInstance, Action<T> pAction)
        {
            pInstance.ToList().ForEach(pAction);
        }

        /// <summary>
        ///     This methods allows to add a value to IEnumerable.
        /// </summary>
        /// <typeparam name="T">The type of the object to add.</typeparam>
        /// <param name="pInstance">The instance to apply.</param>
        /// <param name="pValue">The value to add.</param>
        /// <returns></returns>
        public static IEnumerable<T> Add<T>(this IEnumerable<T> pInstance, T pValue)
        {
            foreach (var lCurrent in pInstance)
            {
                yield return lCurrent;
            }
            yield return pValue;
        }

        /// <summary>
        ///     This methods allows to remove a value to IEnumerable.
        /// </summary>
        /// <typeparam name="T">The type of the object to add.</typeparam>
        /// <param name="pInstance">The instance to apply.</param>
        /// <param name="pValue">The value to remove.</param>
        /// <returns></returns>
        public static IEnumerable<T> Remove<T>(this IEnumerable<T> pInstance, T pValue)
        {
            return pInstance.Where(pCurrent => pCurrent.Equals(pValue) == false);
        }
    }
}
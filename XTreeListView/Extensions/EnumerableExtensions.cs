using System;
using System.Collections.Generic;

namespace XTreeListView.Extensions
{
    /// <summary>
    /// Class extending the <see cref="System.Collections.IEnumerable"/> class.
    /// </summary>
    public static class EnumerableExtensions
    {
        #region Methods

        /// <summary>
        /// Method applying a function to all elements in an enumerable object such as a list.
        /// </summary>
        /// <typeparam name="TElement">The type of element</typeparam>
        /// <param name="pEnumerable">the source</param>
        /// <param name="pAction">The action to apply on all those elements</param>
        public static void ForEach<TElement>(this IEnumerable<TElement> pEnumerable, Action<TElement> pAction)
        {
            if (pEnumerable == null || pAction == null)
            {
                throw new NullReferenceException();
            }

            // Process.
            foreach (TElement lElement in pEnumerable)
            {
                pAction(lElement);
            }
        }

        #endregion // Methods.
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.CompilerServices;

namespace XSerialization
{
    /// <summary>
    /// Defines a class used to compare two objects by reference.
    /// </summary>
    public class ObjectRefEqualityComparer : IEqualityComparer<object>
    {
        #region Methods

        /// <summary>
        /// Tests if the object are equals by reference.
        /// </summary>
        /// <param name="pFirst">The first object.</param>
        /// <param name="pSecond">The second object.</param>
        /// <returns>True if the objects are equals, false otherwise.</returns>
        public new bool Equals(object pFirst, object pSecond)
        {
            return object.ReferenceEquals(pFirst, pSecond);
        }

        /// <summary>
        /// Returns the hashcode of the object reference.
        /// </summary>
        /// <param name="pObj">The object to test.</param>
        /// <returns>The object reference hashcode.</returns>
        public int GetHashCode(object pObj)
        {
            return RuntimeHelpers.GetHashCode(pObj);
        }

        #endregion // Methods.
    }
}

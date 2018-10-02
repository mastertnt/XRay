using System.Reflection;

namespace XTreeListView.Extensions
{
    /// <summary>
    /// Class extending the <see cref="object"/> class.
    /// </summary>
    public static class ObjectExtensions
    {
        #region Properties

        /// <summary>
        /// Returns the value of the property of a given object using reflexion.
        /// </summary>
        /// <typeparam name="TObject">The type of the reflected object.</typeparam>
        /// <typeparam name="TValue">The type of the property value.</typeparam>
        /// <param name="pObject">The reflected object.</param>
        /// <param name="pPropertyName">The property name.</param>
        /// <returns>The property value.</returns>
        public static TValue GetPropertyValue<TObject, TValue>(this TObject pObject, string pPropertyName)
        {
            return (TValue)typeof(TObject).InvokeMember(pPropertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetProperty | BindingFlags.Instance, null, pObject, new object[] { });
        }

        /// <summary>
        /// Sets the value of the property of a given object using reflexion.
        /// </summary>
        /// <typeparam name="TObject">The type of the reflected object.</typeparam>
        /// <typeparam name="TValue">The type of the property value.</typeparam>
        /// <param name="pObject">The reflected object.</param>
        /// <param name="pPropertyName">The property name.</param>
        /// <param name="pValue">The new property value.</param>
        public static void SetPropertyValue<TObject, TValue>(this TObject pObject, string pPropertyName, TValue pValue)
        {
            typeof(TObject).InvokeMember(pPropertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.SetProperty | BindingFlags.Instance, null, pObject, new object[] { pValue });
        }

        /// <summary>
        /// Returns the value of the field of a given object using reflexion.
        /// </summary>
        /// <typeparam name="TObject">The type of the reflected object.</typeparam>
        /// <typeparam name="TValue">The type of the property value.</typeparam>
        /// <param name="pObject">The reflected object.</param>
        /// <param name="pFieldName">The field name.</param>
        /// <returns>The property value.</returns>
        public static TValue GetFieldValue<TObject, TValue>(this TObject pObject, string pPropertyName)
        {
            return (TValue)typeof(TObject).InvokeMember(pPropertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance, null, pObject, new object[] { });
        }

        /// <summary>
        /// Calls a method on a given object using reflexion.
        /// </summary>
        /// <typeparam name="TObject">The type of the reflected object.</typeparam>
        /// <typeparam name="TReturnValue">The type of the returned value.</typeparam>
        /// <param name="pObject">The reflected object.</param>
        /// <param name="pMethodName">The method name.</param>
        /// <param name="pParameters">The method parameters.</param>
        /// <returns>The method returned value.</returns>
        public static TReturnValue CallMethod<TObject, TReturnValue>(this TObject pObject, string pMethodName, params object[] pParameters)
        {
            return (TReturnValue)typeof(TObject).InvokeMember(pMethodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.InvokeMethod | BindingFlags.Instance, null, pObject, pParameters);
        }

        /// <summary>
        /// Calls a method on a given object using reflexion.
        /// </summary>
        /// <typeparam name="TObject">The type of the reflected object.</typeparam>
        /// <param name="pObject">The reflected object.</param>
        /// <param name="pMethodName">The method name.</param>
        /// <param name="pParameters">The method parameters.</param>
        public static void CallMethod<TObject>(this TObject pObject, string pMethodName, params object[] pParameters)
        {
            typeof(TObject).InvokeMember(pMethodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.InvokeMethod | BindingFlags.Instance, null, pObject, pParameters);
        }

        #endregion // Properties.
    }
}

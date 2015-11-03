using System;
using System.IO;
using System.Reflection;

namespace XSystem
{
    /// <summary>
    /// This class stores extension methods on Object.
    /// </summary>
    public static class ObjectExtensions
    {
        #region Methods

        /// <summary>
        /// This method notifies an event by refleciton.
        /// </summary>
        /// <param name="pSource">The event source.</param>
        /// <param name="pEventName">The event name.</param>
        /// <param name="pEventParameters">The event parameters.</param>
        public static void NotifyEvent(this object pSource, string pEventName, object[] pEventParameters)
        {
            FieldInfo lField = pSource.GetType().GetField(pEventName, BindingFlags.Instance | BindingFlags.NonPublic);
            if (lField != null)
            {
                MulticastDelegate lMulticastDelegate = (MulticastDelegate)lField.GetValue(pSource);
                if (lMulticastDelegate != null)
                {
                    foreach (Delegate lDelegate in lMulticastDelegate.GetInvocationList())
                    {
                        lDelegate.Method.Invoke(lDelegate.Target, pEventParameters);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the property value.
        /// </summary>
        /// <param name="pObject">The object.</param>
        /// <param name="pPropertyName">Name of the property to set.</param>
        /// <param name="pPropertyValue">Value of the property to set.</param>
        /// <returns>The returned value</returns>
        public static void SetPropertyValue(this object pObject, string pPropertyName, object pPropertyValue)
        {
            PropertyInfo lProperty = pObject.GetType().GetProperty( pPropertyName );

            if
                ( lProperty != null )
            {
                lProperty.SetValue( pObject, pPropertyValue, null );
            }
        }

        /// <summary>
        /// Gets the property value.
        /// </summary>
        /// <param name="pObject">The object.</param>
        /// <param name="pPropertyName">Name of the property to retrieve.</param>
        /// <returns>The returned value</returns>
        public static object GetPropertyValue(this object pObject, string pPropertyName)
        {
            PropertyInfo lProperty = pObject.GetType().GetProperty( pPropertyName );

            if 
                ( lProperty != null )
            {
                return lProperty.GetValue( pObject, null );
            }

            return null;
        }

        /// <summary>
        /// Gets the property value.
        /// </summary>
        /// <param name="pObject">The object.</param>
        /// <param name="pPropertyName">Name of the property to retrieve.</param>
        /// <returns>The returned value</returns>
        public static T GetPropertyValue<T>(this object pObject, string pPropertyName) where T : class 
        {
            object lValue = GetPropertyValue(pObject, pPropertyName);
            return lValue as T;
        }

        /// <summary>
        /// Gets the property value.
        /// </summary>
        /// <param name="pObject">The object.</param>
        /// <param name="pPropertyName">Name of the property to retrieve.</param>
        /// <returns>The returned value</returns>
        public static Type GetPropertyType(this object pObject, string pPropertyName)
        {
            PropertyInfo lProperty = pObject.GetType().GetProperty( pPropertyName );

            if
                ( lProperty != null )
            {
                return lProperty.PropertyType;
            }

            return null;
        }

        #endregion // Methods.
    }
}

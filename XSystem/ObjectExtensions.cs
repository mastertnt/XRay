using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
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
            PropertyInfo lProperty = pObject.GetType().GetProperty(pPropertyName);
            if (lProperty != null && lProperty.CanWrite)
            {
                lProperty.SetValue(pObject, pPropertyValue, null);
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
            PropertyInfo lProperty = pObject.GetType().GetProperty(pPropertyName);
            if (lProperty != null && lProperty.CanRead)
            {
                return lProperty.GetValue(pObject, null);
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
        /// <param name="pFieldName">Name of the field to set.</param>
        /// <param name="pFieldValue">Value of the property to set.</param>
        /// <returns>The returned value</returns>
        public static void SetFieldValue(this object pObject, string pFieldName, object pFieldValue)
        {
            FieldInfo[] lFields = pObject.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            FieldInfo lField = lFields.FirstOrDefault(pField => pField.Name == pFieldName);
            if (lField != null)
            {
                lField.SetValue(pObject, pFieldValue);
            }
        }

        /// <summary>
        /// Gets the property value.
        /// </summary>
        /// <param name="pObject">The object.</param>
        /// <param name="pFieldName">Name of the field to retrieve.</param>
        /// <param name="pFieldType">Type of the field to retrieve (null if the field is not found).</param>
        /// <returns>The returned value</returns>
        public static object GetFieldValue(this object pObject, string pFieldName, out Type pFieldType)
        {
            FieldInfo[] lFields = pObject.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            FieldInfo lField = lFields.FirstOrDefault(pField => pField.Name == pFieldName);
            if (lField != null)
            {
                pFieldType = lField.FieldType;
                return lField.GetValue(pObject);
            }
            pFieldType = null;
            return null;
        }

        /// <summary>
        /// Gets the property value.
        /// </summary>
        /// <param name="pObject">The object.</param>
        /// <param name="pFieldName">Name of the field to retrieve.</param>
        /// <returns>The returned value</returns>
        public static T GetFieldValue<T>(this object pObject, string pFieldName) where T : class
        {
            Type lType;
            object lValue = GetFieldValue(pObject, pFieldName, out lType);
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
            PropertyInfo lProperty = pObject.GetType().GetProperty(pPropertyName);
            if (lProperty != null)
            {
                return lProperty.PropertyType;
            }
            
            return null;
        }

        /// <summary>
        /// Converts the specified target type.
        /// </summary>
        /// <param name="pObject">The object.</param>
        /// <param name="pTargetType">Type of the target.</param>
        /// <returns>The converted value.</returns>
        public static object Convert(this object pObject, Type pTargetType)
        {
            Type lSourceType = pObject.GetType();
            switch (lSourceType.CanBeAssigned(pTargetType))
            {
                case AssignationType.SourceToTarget_Direct:
                    {
                        TypeConverter lConverter = TypeDescriptor.GetConverter(lSourceType);
                        var lConvertedValue = lConverter.ConvertTo(pObject, pTargetType);
                        return lConvertedValue;
                    }
                case AssignationType.TargetToSource_Back:
                    {
                        TypeConverter lConverter = TypeDescriptor.GetConverter(pTargetType);
                        var lConvertedValue = lConverter.ConvertFrom(null, null, pObject);
                        return lConvertedValue;
                    }
                case AssignationType.Assignable:
                    {
                        return pObject;
                    }
            }

            return null;
        }

        /// <summary>
        /// Converts the specified target type.
        /// </summary>
        /// <typeparam name="TTargetType">The type of the target type.</typeparam>
        /// <param name="pObject">The object.</param>
        /// <returns>
        /// The converted value.
        /// </returns>
        public static TTargetType Convert<TTargetType>(this object pObject)
        {
            Type lSourceType = pObject.GetType();
            switch (lSourceType.CanBeAssigned(typeof(TTargetType)))
            {
                case AssignationType.SourceToTarget_Direct:
                    {
                        TypeConverter lConverter = TypeDescriptor.GetConverter(lSourceType);
                        var lConvertedValue = lConverter.ConvertTo(pObject, typeof(TTargetType));
                        return (TTargetType)lConvertedValue;
                    }
                case AssignationType.TargetToSource_Back:
                    {
                        TypeConverter lConverter = TypeDescriptor.GetConverter(typeof(TTargetType));
                        var lConvertedValue = lConverter.ConvertFrom(null, null, pObject);
                        return (TTargetType)lConvertedValue;
                    }
                case AssignationType.Assignable:
                    {
                        return (TTargetType)pObject;
                    }
            }

            return default(TTargetType);
        }

        #endregion // Methods.
    }
}

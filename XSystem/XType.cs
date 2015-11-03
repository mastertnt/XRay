using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using XSystem.Collections;

namespace XSystem
{
    /// <summary>
    /// This class provides method extensions on class Type.
    /// </summary>
    public static class XType
    {
        #region Fields

        /// <summary>
        /// The field stores a distance cache for DistanceTo method.
        /// </summary>
        private static bool msAssemblyLoad;

        /// <summary>
        /// The field stores a distance cache for DistanceTo method.
        /// </summary>
        private static readonly CacheDictionary<string, int> msDistanceToCache = new CacheDictionary<string, int>(50);

        /// <summary>
        /// The field stores a distance cache for GetBaseTypes method.
        /// </summary>
        private static readonly CacheDictionary<Type, IEnumerable<Type>> msBaseTypesCache = new CacheDictionary<Type, IEnumerable<Type>>(20);

        /// <summary>
        /// The field stores a distance cache for GetInheritedTypes method.
        /// </summary>
        private static readonly CacheDictionary<string, IEnumerable<Type>> msInheritedTypesCache = new CacheDictionary<string, IEnumerable<Type>>(50);

        /// <summary>
        /// The field stores a distance cache for GetFirstAttributeOfType method.
        /// </summary>
        private static readonly CacheDictionary<string, IEnumerable<Attribute>> msAttributesOfType = new CacheDictionary<string, IEnumerable<Attribute>>(250);

        /// <summary>
        /// The field stores a distance cache for GetFirstAttributeOfType method.
        /// </summary>
        private static readonly CacheDictionary<string, Attribute> msAttributesOfTypeForProperty = new CacheDictionary<string, Attribute>(250);

        #endregion // Fields.

        #region Methods

        /// <summary>
        /// This method returns the first attribute of a given type.
        /// </summary>
        /// <typeparam name="T">The type to inspect</typeparam>
        /// <param name="pPropertyInfo">The property info to inspect.</param>
        /// <returns>The attribute retrieved, null otherwise.</returns>
        public static T GetFirstAttributeOfType<T>(this PropertyInfo pPropertyInfo) where T : Attribute
        {
            string lKey = pPropertyInfo.Name + "--" + pPropertyInfo.DeclaringType.FullName + "--" + typeof(T).FullName;
            if (msAttributesOfTypeForProperty.ContainsKey(lKey) == false)
            {
                PropertyDescriptor lPropertyDescriptor = TypeDescriptor.GetProperties(pPropertyInfo.DeclaringType).Cast<PropertyDescriptor>().FirstOrDefault(pPropertyDescriptor => pPropertyDescriptor.Name == pPropertyInfo.Name);
                if (lPropertyDescriptor != null)
                {
                    return lPropertyDescriptor.GetFirstAttributeOfType<T>();
                }
                return null;
            }

            return msAttributesOfTypeForProperty[lKey] as T;
        }

        /// <summary>
        /// This method returns the first attribute of a given type.
        /// </summary>
        /// <typeparam name="T">The type to inspect</typeparam>
        /// <param name="pDescriptor">The property descriptor.</param>
        /// <returns>The attribute retrieved, null otherwise.</returns>
        public static T GetFirstAttributeOfType<T>(this PropertyDescriptor pDescriptor) where T : Attribute
        {
            string lKey = pDescriptor.Name + "--" + pDescriptor.ComponentType.FullName + "--" + typeof(T).FullName;
            if (msAttributesOfTypeForProperty.ContainsKey(lKey) == false)
            {
                Attribute lAttribute = pDescriptor.Attributes.OfType<T>().FirstOrDefault();
                msAttributesOfTypeForProperty.Add(lKey, lAttribute);
            }

            return msAttributesOfTypeForProperty[lKey] as T;
        }

        /// <summary>
        /// Gets the first type of the attribute of.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pThis">The current instance.</param>
        /// <returns>The first attribute retrieved, null otherwise.</returns>
        public static T GetFirstAttributeOfType<T>(this Type pThis) where T : Attribute
        {
            return pThis.GetAttributesOfType<T>().FirstOrDefault();
        }

        /// <summary>
        /// Returns the attributes of type T found on the instance of type pType.
        /// </summary>
        /// <typeparam name="T">The type of the attribute.</typeparam>
        /// <param name="pType">The type of the instance.</param>
        /// <returns>The attributes retrieved, null otherwise.</returns>
        public static IEnumerable<T> GetAttributesOfType<T>(this Type pType) where T : Attribute
        {
            string lKey = pType.FullName + "--" + typeof (T).FullName;
            if (msAttributesOfType.ContainsKey(lKey) == false)
            {
                // Getting the attributes of the type hierarchy.
                List<Object> lCustomAttributes = new List<Object>();
                Type lType = pType;
                while
                    (lType != null)
                {
                    lCustomAttributes.AddRange(lType.GetCustomAttributes(false));
                    lType = lType.BaseType;
                }

                // Adding the one defined on interfaces.
                Type[] lInterfaces = pType.GetInterfaces();
                foreach (Type lInterface in lInterfaces)
                {
                    lCustomAttributes.AddRange(lInterface.GetCustomAttributes(false));
                }

                msAttributesOfType.Add(lKey, lCustomAttributes.OfType<T>());
                
            }
            return msAttributesOfType[lKey].Cast<T>();
        }

        /// <summary>
        /// This method creates all concrete classes.
        /// </summary>
        /// <typeparam name="TReturnedType">The type of the returned type.</typeparam>
        /// <param name="pThis">The current instance.</param>
        /// <param name="pConstructorParameters">The constructor parameters.</param>
        /// <returns>
        /// The list of concrete classes found in current AppDomain
        /// </returns>
        public static IEnumerable<TReturnedType> CreateAll<TReturnedType>(this Type pThis,  object[] pConstructorParameters) where TReturnedType : class
        {
            return pThis.GetInheritedTypes(false, pConstructorParameters.Count()).Select(pType => Activator.CreateInstance(pType, pConstructorParameters)).OfType<TReturnedType>();
        }

        /// <summary>
        /// This method creates all concrete classes.
        /// </summary>
        /// <typeparam name="TReturnedType">The type of the returned type.</typeparam>
        /// <param name="pThis">The current instance.</param>
        /// <param name="pConstructorParameters">The constructor parameters.</param>
        /// <param name="pSourceAssembly">The source assembly.</param>
        /// <returns>
        /// The list of concrete classes found in current AppDomain
        /// </returns>
        public static IEnumerable<TReturnedType> CreateAll<TReturnedType>(this Type pThis, object[] pConstructorParameters, Assembly pSourceAssembly) where TReturnedType : class
        {
            return pThis.GetInheritedTypes(false, pConstructorParameters.Count(), pSourceAssembly).Select(pType => Activator.CreateInstance(pType, pConstructorParameters)).OfType<TReturnedType>();
        }

        /// <summary>
        /// This method creates all concrete classes.
        /// </summary>
        /// <typeparam name="TReturnedType">The type of the returned type.</typeparam>
        /// <param name="pThis">The current instance.</param>
        /// <returns>
        /// The list of concrete classes found in current AppDomain
        /// </returns>
        public static IEnumerable<TReturnedType> CreateAll<TReturnedType>(this Type pThis) where TReturnedType : class
        {
            return pThis.GetInheritedTypes().Select(Activator.CreateInstance).OfType<TReturnedType>();
        }

        /// <summary>
        /// This method creates all concrete classes.
        /// </summary>
        /// <param name="pThis">The current instance.</param>
        /// <returns>The list of concrete classes found in current AppDomain</returns>
        public static IEnumerable<object> CreateAll(this Type pThis)
        {
            return pThis.GetInheritedTypes().Select(Activator.CreateInstance);
        }

        /// <summary>
        /// This method creates all concrete classes.
        /// </summary>
        /// <param name="pThis">The current instance.</param>
        /// <param name="pConstructorParameters">The constructor parameters.</param>
        /// <returns>
        /// The list of concrete classes found in current AppDomain
        /// </returns>
        public static IEnumerable<object> CreateAll(this Type pThis, object[] pConstructorParameters)
        {
            return pThis.GetInheritedTypes(false, pConstructorParameters.Count()).Select(pType => Activator.CreateInstance(pType, pConstructorParameters));
        }

        /// <summary>
        /// This method creates all concrete classes.
        /// </summary>
        /// <param name="pThis">The current instance.</param>
        /// <param name="pConstructorParameters">The constructor parameters.</param>
        /// <param name="pSourceAssembly">The source assembly.</param>
        /// <returns>
        /// The list of concrete classes found in current AppDomain
        /// </returns>
        public static IEnumerable<object> CreateAll(this Type pThis, object[] pConstructorParameters, Assembly pSourceAssembly)
        {
            return pThis.GetInheritedTypes(false, pConstructorParameters.Count(), pSourceAssembly).Select(pType => Activator.CreateInstance(pType, pConstructorParameters));
        }

        /// <summary>
        /// This method looks for inherited types of this type.
        /// </summary>
        /// <param name="pThis">The current instance.</param>
        /// <param name="pCanBeAbstract">Flag to know if the class can be astract.</param>
        /// <param name="pConstructorArgCount">if set to <c>0</c> [the type has a default constructor].</param>
        /// <param name="pSourceAssembly">The source assembly where the type must be located.</param>
        /// <returns>
        /// The list of retrieved types in the current domain.
        /// </returns>
        public static IEnumerable<Type> GetInheritedTypes(this Type pThis, bool pCanBeAbstract = false, int pConstructorArgCount = 0, Assembly pSourceAssembly = null)
        {
            Assembly lSourceAssembly = pThis.Assembly;
            if (pSourceAssembly != null)
            {
                lSourceAssembly = pSourceAssembly;
            }

            if (msAssemblyLoad == false)
            {
                AppDomain.CurrentDomain.AssemblyLoad += OnAssemblyLoad;
                msAssemblyLoad = true;
            }

            string lKey = pThis.FullName + " : " + pCanBeAbstract + " : " + pConstructorArgCount + ":" + lSourceAssembly.GetName().FullName;
            
            if (msInheritedTypesCache.ContainsKey(lKey) == false)
            {
                List<Assembly> lAssemblyToCheck = AppDomain.CurrentDomain.GetAssemblies().Where(pAssembly => pAssembly.GetReferencedAssemblies().FirstOrDefault(pAssemblyName => pAssemblyName.FullName == lSourceAssembly.GetName().FullName) != null).ToList();
                lAssemblyToCheck.Add(lSourceAssembly);
                IEnumerable<Type> lAllTypes = lAssemblyToCheck.SelectMany(pAssembly => pAssembly.GetTypes());
                if (pConstructorArgCount == 0)
                {
                    msInheritedTypesCache.Add(lKey, lAllTypes.Where(pType => pType.IsInterface == false && pType.IsAbstract == pCanBeAbstract && pThis.IsAssignableFrom(pType) && pType.GetConstructor(Type.EmptyTypes) != null));
                }
                else
                {
                    IEnumerable<Type> lFoundTypes = lAllTypes.Where(pType => pType.IsInterface == false && pType.IsAbstract == pCanBeAbstract && pThis.IsAssignableFrom(pType));
                    IEnumerable<Type> lFilterdTypes = lFoundTypes.Where(pType => pType.GetConstructors().Any(pConstructor => pConstructor.GetParameters().Count() == pConstructorArgCount));
                    msInheritedTypesCache.Add(lKey, lFilterdTypes);
                }
                
            }
            
            return msInheritedTypesCache[lKey];
        }

        /// <summary>
        /// Handles the AssemblyLoad event of the CurrentDomain control.
        /// </summary>
        /// <param name="pEventSender">The source of the event.</param>
        /// <param name="pArgs">The <see cref="AssemblyLoadEventArgs"/> instance containing the event data.</param>
        static void OnAssemblyLoad(object pEventSender, AssemblyLoadEventArgs pArgs)
        {
            msInheritedTypesCache.Clear();
            msBaseTypesCache.Clear();
            msInheritedTypesCache.Clear();
            msAttributesOfType.Clear();
            AppDomainExtensions.msTypeByFullName.Clear();
            AppDomainExtensions.msUnkwownTypes.Clear();
        }

        /// <summary>
        /// This method retrieves all base types (performance hit)
        /// </summary>
        /// <param name="pThis">The current instance.</param>
        /// <returns>All parent types</returns>
        public static IEnumerable<Type> GetBaseTypes(this Type pThis)
        {
            if (msBaseTypesCache.ContainsKey(pThis) == false)
            {
                List<Type> lBaseTypes = new List<Type>();
                Type lType = pThis;

                while (lType != null)
                {
                    lType = lType.BaseType;
                    if (lType != null)
                    {
                        lBaseTypes.Add(lType);
                    }
                }

                msBaseTypesCache.Add(pThis, lBaseTypes);
            }

            return msBaseTypesCache[pThis];
        }

        /// <summary>
        /// This method computes a distance between two types.
        /// </summary>
        /// <param name="pSourceType">The source type</param>
        /// <param name="pParentType">A parent type to look for.</param>
        /// <returns>An evaluated distance. </returns>
        /// <remarks>The distance between two types (A,B) if A and B are concrete class and A is the base type of B is 1.</remarks>
        /// <remarks>The distance between two types (B,A) if A is an interface and B a concrecte class and implements A, the distance is 0.1</remarks>
        /// <remarks>The distance between two types (C,A) if A is the base type of B and B if the base type of C, the distance is 2.</remarks>
        /// <remarks>The distance between two types (C,InterfaceB) If B is the base type of C and B implements InterfaceB, the distance between C and InterfaceB is 1.1.</remarks>
        public static int DistanceTo(this Type pSourceType, Type pParentType)
        {
            string lKey = pSourceType.FullName + "To" + pParentType.FullName;
            if (msDistanceToCache.ContainsKey(lKey) == false)
            {
                int lValue = PrivateDistanceTo(pSourceType, pParentType);

                // Store the value in the cache.
                msDistanceToCache.Add(lKey, lValue);
            }
            
            // The source type is not a child of target type.
            return msDistanceToCache[lKey];
        }

        /// <summary>
        /// This method computes a distance between two types (private).
        /// </summary>
        /// <param name="pSourceType">The source type</param>
        /// <param name="pParentType">A parent type to look for.</param>
        /// <returns>An evaluated distance. </returns>
        /// <remarks>The distance between two types (A,B) if A and B are concrete class and A is the base type of B is 1.</remarks>
        /// <remarks>The distance between two types (B,A) if A is an interface and B a concrecte class and implements A, the distance is 0.1</remarks>
        /// <remarks>The distance between two types (C,A) if A is the base type of B and B if the base type of C, the distance is 2.</remarks>
        /// <remarks>The distance between two types (C,InterfaceB) If B is the base type of C and B implements InterfaceB, the distance between C and InterfaceB is 1.1.</remarks>
        private static int PrivateDistanceTo(this Type pSourceType, Type pParentType)
        {
            if (!pParentType.IsAssignableFrom(pSourceType)) return -1;

            if (pParentType == pSourceType)
            {
                return 0;
            }

            if (pParentType.IsInterface)
            {
                Type[] lInterfaces = pSourceType.GetInterfaces();
                if (lInterfaces.Contains(pParentType))
                {
                    return 0;
                }
            }

            if (pSourceType.BaseType == typeof(object))
            {
                return 1;
            }

            // The source type is not a child of target type.
            return 1 + PrivateDistanceTo(pSourceType.BaseType, pParentType);
        }

        /// <summary>
        /// This method generates a default value for a given type.
        /// </summary>
        /// <param name="pType">The type.</param>
        /// <returns>The default value.</returns>
        public static object DefaultValue(this Type pType)
        {
            if (pType.IsValueType)
            {
                return Activator.CreateInstance(pType);
            }
            return pType == typeof(string) ? string.Empty : null;
        }

        /// <summary>
        /// This method checks if the type can be considered as simple.
        /// </summary>
        /// <param name="pType">The type to check.</param>
        /// <returns>True if the type is simple, false otherwise.</returns>
        public static bool IsSimple(this Type pType)
        {
            if (pType.IsPrimitive)
            {
                return true;
            }

            if (pType == typeof(string))
            {
                return true;
            }

            if (pType == typeof(DateTime))
            {
                return true;
            }

            return pType == typeof(decimal) || pType.IsEnum;
        }

        /// <summary>
        /// This method checks if the type is a struct.
        /// </summary>
        /// <param name="pType">The type to check.</param>
        /// <returns>True if the type is a structure, false otherwise.</returns>
        public static bool IsStruct(this Type pType)
        {
            return pType.IsValueType && !pType.IsEnum && !pType.IsPrimitive && pType != typeof(decimal);
        }

        /// <summary>
        /// Retrieve the method used to evaluate the evaluate the behavior.
        /// </summary>
        /// <param name="pType">The first type to test.</param>
        /// <param name="pAnotherType">The second type to test.</param>
        /// <param name="pMethodName">The method name.</param>
        /// <returns>
        /// The retrieved method or null if not found.
        /// </returns>
        public static MethodInfo FindMethod(Type pType, Type pAnotherType, string pMethodName)
        {
            if (string.IsNullOrEmpty(pMethodName) == false)
            {
                Type lCurrentType = pType;
                string lMethodName = pMethodName;
                if (pMethodName.Contains("."))
                {
                    string lFullTypeName = pMethodName.Substring(0, pMethodName.LastIndexOf(".", System.StringComparison.Ordinal));
                    Type lFoundType = AppDomain.CurrentDomain.GetTypeByFullName(lFullTypeName);
                    lMethodName = pMethodName.Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries).LastOrDefault();
                    if (lFoundType != null)
                    {
                        lCurrentType = lFoundType;
                    }
                }

                if (lMethodName == null)
                {
                    return null;
                }

                MethodInfo lMethod = lCurrentType.GetMethod(lMethodName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);
                if (lMethod != null)
                {
                    // Now, call the method to retrieve the value.
                    return lMethod;
                }

                if (pAnotherType != null)
                {
                    // If the method is not found, we are looking for the method on the instance type.
                    lMethod = pAnotherType.GetMethod(lMethodName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);
                    if (lMethod != null)
                    {
                        // Now, call the method to retrieve the value.
                        return lMethod;
                    }
                }
            }
            return null;
        }

        #endregion // Methods.
    }
}

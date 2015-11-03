using System;
using System.Reflection;
using XSystem.Collections;

namespace XSystem
{
    /// <summary>
    /// This class stores extension methods on AppDomain.
    /// </summary>
    public static class AppDomainExtensions
    {
        #region Fields
        /// <summary>
        /// Stores the map referencing the types by their full names.
        /// </summary>
        internal static readonly CacheDictionary<string, Type> msTypeByFullName = new CacheDictionary<string, Type>(50);

        /// <summary>
        /// Stores the map referencing the types by their full names.
        /// </summary>
        internal static readonly CacheDictionary<string, string> msUnkwownTypes = new CacheDictionary<string, string>(50);

        #endregion // Fields.

        #region Methods

        /// <summary>
        /// This method finds the first type with is fully qualified name matching pTypeFullName.
        /// </summary>
        /// <param name="pThis">The application domain.</param>
        /// <param name="pTypeAssemblyQualifiedName">The type name with the assembly.</param>
        /// <returns>
        /// The found type or null if none was found
        /// </returns>
        public static Type GetTypeByAssemblyQualifiedName(this AppDomain pThis, string pTypeAssemblyQualifiedName)
        {
            // Avoid looking for XRoot.
            if (msUnkwownTypes.ContainsKey(pTypeAssemblyQualifiedName))
            {
                return null;
            }

            if (msTypeByFullName.ContainsKey(pTypeAssemblyQualifiedName))
            {
                return msTypeByFullName[pTypeAssemblyQualifiedName];
            }

            Type lFoundType = Type.GetType(pTypeAssemblyQualifiedName);
            if (lFoundType != null)
            {
                msTypeByFullName[pTypeAssemblyQualifiedName] = lFoundType;
                return lFoundType;
            }

            // Not found type.
            msUnkwownTypes.Add(pTypeAssemblyQualifiedName, string.Empty);

            return null;
        }

        /// <summary>
        /// This method finds the first type with is fully qualified name matching pTypeFullName.
        /// </summary>
        /// <param name="pThis">The application domain.</param>
        /// <param name="pTypeFullName">The type name.</param>
        /// <returns>The found type or null if none was found</returns>
        public static Type GetTypeByFullName(this AppDomain pThis, string pTypeFullName)
        {
            string lTypeName = pTypeFullName + " @@ " + "UND_ASS";
            if (msUnkwownTypes.ContainsKey(lTypeName))
            {
                return null;
            }

            if (msTypeByFullName.ContainsKey(lTypeName))
            {
                return msTypeByFullName[lTypeName];
            }

            Assembly[] lAssembliesLoaded = pThis.GetAssemblies();
            foreach
                (Assembly lLoadedAssembly in lAssembliesLoaded)
            {
                Type lFoundType = lLoadedAssembly.GetType(pTypeFullName);
                if (lFoundType != null)
                {
                    msTypeByFullName[lTypeName] = lFoundType;
                    return lFoundType;
                }
            }

            // Not found type.
            msUnkwownTypes.Add(lTypeName, string.Empty);

            return null;
        }

        #endregion // Methods.
    }
}

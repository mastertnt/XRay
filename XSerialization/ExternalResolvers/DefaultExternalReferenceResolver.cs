using System;
using System.Collections.Generic;
using System.IO;

namespace XSerialization.ExternalResolvers
{
    /// <summary>
    /// This class implement an external reference resolver.
    /// </summary>
    public class DefaultExternalReferenceResolver : IXExternalReferenceResolver
    {
        #region Fields

        /// <summary>
        /// This field stores all objects.
        /// </summary>
        private readonly Dictionary<object, ExternalReference> mObjects = new Dictionary<object, ExternalReference>();

        #endregion // Fields.

        #region Properties

        /// <summary>
        /// Gets all referenced obects.
        /// </summary>
        public IEnumerable<object> ReferencedObjects
        {
            get
            {
                return this.mObjects.Keys;
            }
        }

        #endregion // Properties.

        #region Methods

        /// <summary>
        /// This method checks if an object has the external storage.
        /// </summary>
        /// <param name="pObject">The object to retrieve.</param>
        /// <returns>True if the object has external reference.</returns>
        public bool HasExternalReference(object pObject)
        {
            return this.mObjects.ContainsKey(pObject);
        }

        /// <summary>
        /// This method gets an external reference.
        /// </summary>
        /// <param name="pObject">The object to retrieve.</param>
        /// <returns>The external reference or string.empty if no reference has been found.</returns>
        public string GetExternalReference(object pObject)
        {
            if (this.mObjects.ContainsKey(pObject))
            {
                return this.mObjects[pObject].Url;
            }

            return string.Empty;
        }

        /// <summary>
        /// This method registers an external reference.
        /// If the external reference is in relative form it will be converted in absolute form with Environment.CurrentDirectory.
        /// </summary>
        /// <param name="pObject">The object to reference.</param>
        /// <param name="pUrl">The external reference in absolute form.</param>
        /// <param name="pIsRelative">True if the external reference must be considered as relative.</param>
        /// <returns>true if the reference has been added, false if the reference has been updated.</returns>
        public bool RegisterExternalReference(object pObject, string pUrl, bool pIsRelative)
        {
            string lFullExternalReference;
            bool lIsRelative = pIsRelative;
            if (Path.IsPathRooted(pUrl))
            {
                lFullExternalReference = pUrl;
            }
            else
            {
                lFullExternalReference = Environment.CurrentDirectory + Path.DirectorySeparatorChar + pUrl;
                lIsRelative = true;
            }

            if (this.mObjects.ContainsKey(pObject))
            {
                this.mObjects[pObject] = new ExternalReference { Url = lFullExternalReference, IsRelative = lIsRelative };
                return false;
            }

            this.mObjects[pObject] = new ExternalReference { Url = lFullExternalReference, IsRelative = lIsRelative };
            return true;
        }

        /// <summary>
        /// This method unregisters an external reference.
        /// </summary>
        /// <param name="pObject">The object to unreference.</param>
        /// <returns>true if the reference has been added, false if the reference has been updated.</returns>
        public bool UnregisterExternalReference(object pObject)
        {
            if (this.mObjects.ContainsKey(pObject))
            {
                this.mObjects.Remove(pObject);
                return true;
            }
            return false;
        }

        #endregion // Methods.

        /// <summary>
        /// A typedef for external reference.
        /// </summary>
        struct ExternalReference
        {
            /// <summary>
            /// Gets or sets the exernal reference.
            /// </summary>
            public string Url
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets a flag to know if the reference must be considered as relative.
            /// </summary>
            public bool IsRelative
            {
                get;
                set;
            }
        }
    }
}

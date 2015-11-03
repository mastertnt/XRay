using System.Collections.Generic;

namespace XSerialization
{
    /// <summary>
    /// This interface is used to resolved external references on objects during serialization.
    /// A default implementation is provided by DefaultExternalReferenceResolver.
    /// </summary>
    public interface IXExternalReferenceResolver
    {
        /// <summary>
        /// Gets all referenced obects.
        /// </summary>
        IEnumerable<object> ReferencedObjects
        {
            get;
        }

        /// <summary>
        /// This method checks if an object has the external storage.
        /// </summary>
        /// <param name="pObject">The object to retrieve.</param>
        /// <returns>True if the object has external reference.</returns>
        bool HasExternalReference(object pObject);
        
        /// <summary>
        /// This method gets an external reference.
        /// </summary>
        /// <param name="pObject">The object to retrieve.</param>
        /// <returns>The external reference or string.empty if no reference has been found.</returns>
        string GetExternalReference(object pObject);

        /// <summary>
        /// This method registers an external reference.
        /// If the external reference is in relative form it will be converted in absolute form with Environment.CurrentDirectory.
        /// </summary>
        /// <param name="pObject">The object to reference.</param>
        /// <param name="pUrl">The external reference in absolute form.</param>
        /// <param name="pIsRelative">True if the external reference must be considered as relative.</param>
        /// <returns>true if the reference has been added, false if the reference has been updated.</returns>
        bool RegisterExternalReference(object pObject, string pUrl, bool pIsRelative);

        /// <summary>
        /// This method unregisters an external reference.
        /// </summary>
        /// <param name="pObject">The object to unreference.</param>
        /// <returns>true if the reference has been added, false if the reference has been updated.</returns>
        bool UnregisterExternalReference(object pObject);
    }
}

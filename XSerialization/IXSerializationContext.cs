using System;
using System.IO;
using System.Reflection;
using System.Xml.Linq;

namespace XSerialization
{
    /// <summary>
    /// This interface declares a serialization context.
    /// </summary>
    public interface IXSerializationContext
    {
        #region Properties

        /// <summary>
        /// Gets a flag to know if the context is writing an object.
        /// </summary>
        bool IsWriting { get; }

        /// <summary>
        /// Gets the current URI file.
        /// </summary>
        Uri CurrentFile { get; }

        #endregion // Properties

        #region Methods

        /// <summary>
        /// This method pushes the object.
        /// </summary>
        /// <param name="pObject">The object to push as current</param>
        /// <param name="pObjectReference">The object reference. NO_OBJECT_REFERENCE means the object reference will be fixed by the context.</param>
        void PushObject(object pObject, int pObjectReference = XConstants.SYSTEM_REFERENCE);

        /// <summary>
        /// This method pushes the object.
        /// </summary>
        /// <param name="pError">The error to add.</param>
        void PushError(XSerializationError pError);

        /// <summary>
        /// This method pops the object.
        /// </summary>
        void PopObject();

        /// <summary>
        /// Gets the object reference identifier.
        /// </summary>
        /// <param name="pObjectReference">The object reference.</param>
        /// <returns>The object if retrieved, null otherwise.</returns>
        object GetObjectByReference(int pObjectReference); 

        /// <summary>
        /// Gets the object reference identifier.
        /// </summary>
        /// <param name="pObject">The object to test.</param>
        /// <returns>The object identifier or -1 if the object has no reference.</returns>
        int GetObjectReference(object pObject); 

        /// <summary>
        /// Gets the current object.
        /// </summary>
        object CurrentObject { get; }

        /// <summary>
        /// Gets the current directory.
        /// </summary>
        DirectoryInfo CurrentDirectory { get; }

        /// <summary>
        /// Gets the external reference resolver.
        /// </summary>
        IXExternalReferenceResolver ExternalReferenceResolver { get; }

        /// <summary>
        /// This method is used to select the contract according the best contract.
        /// </summary>
        /// <param name="pElement">The current element.</param>
        /// <param name="pObject">The object can be a property info, a type or a value.</param>
        /// <returns>The best contract according to constraints.</returns>
        IXSerializationContract SelectContract(XElement pElement, object pObject);

        /// <summary>
        /// This method is used to select the contract according the best contract.
        /// </summary>
        /// <param name="pElement">The current element.</param>
        /// <param name="pPropertyInfo">The current property info.</param>
        /// <param name="pType">The current type.</param>
        /// <param name="pObject">The current object.</param>
        /// <returns>The best contract according to constraints.</returns>
        IXSerializationContract SelectContract(XElement pElement, PropertyInfo pPropertyInfo, Type pType, object pObject);

        /// <summary>
        /// This method is used to revolve a type.
        /// </summary>
        /// <param name="pTypeElement">The type element.</param>
        /// <returns>The retrieved type.</returns>
        Type ResolveType(XElement pTypeElement);

        /// <summary>
        /// This method is used to store a type inside a serializer.
        /// </summary>
        /// <param name="pType">The type to store.</param>
        /// <returns>The corresponding element.</returns>
        XElement ReferenceType(Type pType);

        /// <summary>
        /// This method get a serialization parameter by name
        /// </summary>
        /// <typeparam name="TType">The serialization parameter type to return</typeparam>
        /// <param name="pParameterName">The serialization parameter name</param>
        /// <param name="pIsFound">Return whether it found the parameter or not</param>
        /// <returns>The serialization parameter</returns>
        TType GetSerializationParameter<TType>(string pParameterName, out bool pIsFound) where TType : struct;

        /// <summary>
        /// This method set a serialization parameter that will be indexed by the supplied name
        /// </summary>
        /// <typeparam name="TType">The serialization parameter type</typeparam>
        /// <param name="pParameterName">The serialization parameter name</param>
        /// <param name="pParameter">The serialization parameter</param>
        void SetSerializationParameter<TType>(string pParameterName, TType pParameter) where TType : struct;

        #endregion Methods
    }
}

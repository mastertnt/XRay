using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace XMetadata.MetadataDescriptors
{
    /// <summary>
    /// Type key structure definition.
    /// </summary>
    public struct TypeKey
    {
        #region Fields

        /// <summary>
        /// Stores the type name
        /// </summary>
        private string mName;

        #endregion // Fields.

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeKey"/> struct.
        /// </summary>
        /// <param name="pName">The name</param>
        public TypeKey(string pName)
        {
            this.mName = pName;
        }

        #endregion // Constructors.

        #region Methods

        /// <summary>
        /// Implicit cast operator from type key to string name.
        /// </summary>
        /// <param name="pKey">The type key</param>
        public static implicit operator string(TypeKey pKey)
        {
            return pKey.mName;
        }

        #endregion // Methods.
    }

    /// <summary>
    /// Target key structure definition.
    /// </summary>
    public struct TargetKey
    {
        #region Fields

        /// <summary>
        /// Stores the target name
        /// </summary>
        private string mName;

        #endregion // Fields.

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TargetKey"/> struct.
        /// </summary>
        /// <param name="pName">The name</param>
        public TargetKey(string pName)
        {
            this.mName = pName;
        }

        #endregion // Constructors.

        #region Methods

        /// <summary>
        /// Implicit cast operator from target key to string name.
        /// </summary>
        /// <param name="pKey">The target key</param>
        public static implicit operator string(TargetKey pKey)
        {
            return pKey.mName;
        }

        #endregion // Methods.
    }

    /// <summary>
    /// Definition of the <see cref="IMetadataSetType"/> interface.
    /// </summary>
    public interface IMetadataSetType
    {
        #region Properties

        /// <summary>
        /// Gets or sets the object's identifier. Persists over the time.
        /// </summary>
        string Id
        {
            get;
        }

        /// <summary>
        /// Gets or sets the tactical metadata target tactical data type name. (e.g: Area2D, Area3D, Point, Polyline, ...)
        /// </summary>
        [ReadOnly(true)]
        string TargetType
        {
            get;
        }

        /// <summary>
        /// Gets the parent type.
        /// </summary>
        IMetadataSetType ParentType
        {
            get;
        }

        /// <summary>
        /// Gets the set of nested types if any.
        /// </summary>
        IEnumerable<IMetadataSetType> NestedTypes
        {
            get;
        }

        /// <summary>
        /// Gets the nested type by its identifier.
        /// </summary>
        /// <param name="pIdentifier">The type identifier/name</param>
        /// <returns>The type if any, Null otherwise.</returns>
        IMetadataSetType this[TypeKey pIdentifier]
        {
            get;
        }

        /// <summary>
        /// Gets the nested type by its target identifier.
        /// </summary>
        /// <param name="pIdentifier">The target identifier/name</param>
        /// <returns>The type if any, Null otherwise.</returns>
        IMetadataSetType this[TargetKey pIdentifier]
        {
            get;
        }

        #endregion // Properties.

        #region Methods

        /// <summary>
        /// Creates an instance of the tactical data defined by this metadata type.
        /// </summary>
        /// <param name="pAttributeCreator">The creator used to affect attributes to the metadata properties.</param>
        /// <returns>The new tactical data instance.</returns>
        MetadataSet CreateInstance(AAttributesCreator pAttributeCreator);
        
        /// <summary>
        /// Returns the type of the metadata as a string.s
        /// </summary>
        /// <returns>The type as string.</returns>
        string GetTypeAsString();

        /// <summary>
        /// Returns the parent type path as a string.
        /// </summary>
        /// <returns>The parent type path.</returns>
        string GetParentTypeAsString();

        #endregion // Methods.
    }
}

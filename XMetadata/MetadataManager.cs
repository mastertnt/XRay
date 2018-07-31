using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Linq;
using System.Linq;
using XSystem;
using XMetadata.MetadataDescriptors.Readers;
using XMetadata.Metadata.Readers;
using XMetadata.MetadataDescriptors;

namespace XMetadata
{
    /// <summary>
    /// Definition of the <see cref="MetadataManager"/> class.
    /// </summary>
    public sealed class MetadataManager
    {
        #region Constants

        /// <summary>
        /// Stores the type as string separator.
        /// </summary>
        public const string TYPE_AS_STRING_SEP = "|";

        /// <summary>
        /// Stores the type as string separator.
        /// </summary>
        public const string PARENT_TYPE_AS_STRING_SEP = "+";

        /// <summary>
        /// Stores the constant metadata type tag.
        /// </summary>
        public const string cMetadataTypeTag = "TacticalMetadataType";

        /// <summary>
        /// Stores the constant metadata tag.
        /// </summary>
        public const string cMetadataTag = "Metadata";

        /// <summary>
        /// Stores the constant metadata id tag.
        /// </summary>
        public const string cMetadataIdTag = "id";

        /// <summary>
        /// Stores the constant metadata targetType tag.
        /// </summary>
        public const string cMetadataTargetTypeTag = "targetType";

        /// <summary>
        /// Stores the constant metadata property name tag.
        /// </summary>
        public const string cMetadataPropertyNameTag = "propertyName";

        /// <summary>
        /// Stores the constant metadata userType tag.
        /// </summary>
        public const string cMetadataUserTypeTag = "userType";

        /// <summary>
        /// Stores the constant metadata point list tag.
        /// </summary>
        public const string cMetadataPointListTag = "pointList";

        /// <summary>
        /// Stores the constant Type tag.
        /// </summary>
        public const string cTypeTag = "type";

        #endregion // Constants.

        #region Fields

        /// <summary>
        /// Stores the empty list of type names. (just for iteration purpose)
        /// </summary>
        private static readonly string[] sEmptyTypeNames = new string[0];

        /// <summary>
        /// Stores the set of tactical metadata types by tactical data Id then by target tactical data type name.
        /// (e.g: Area2D, MonArea2DId, Area2DSpecificType)
        /// </summary>
        private static readonly Dictionary<string, Dictionary<string, MetadataSetType>> sTacticalMetadataTypes;

        /// <summary>
        /// Stores the set of metadata readers.
        /// </summary>
        private static readonly Dictionary<string, AMetadataReader> sReaders;

        /// <summary>
        /// Stores the metadata type reader.
        /// </summary>
        private static MetadataTypeReader sTypeReader;

        /// <summary>
        /// Stores the tactical metadata manager unique instance.
        /// The variable is declared to be volatile to ensure that assignment to the 
        /// instance variable completes before the instance variable can be accessed.
        /// </summary>
        private static volatile MetadataManager sInstance;

        /// <summary>
        /// Stores the sync root to lock on the manager rather than locking on the
        /// type itself to avoid deadlocks.
        /// </summary>
        private static object sSyncRoot = new object();

        #endregion // Fields.

        #region Properties

        /// <summary>
        /// Gets the tactical metadata manager handle.
        /// </summary>
        public static MetadataManager Instance
        {
            get
            {
                // This double-check locking approach solves the thread concurrency problems
                if (sInstance == null)
                {
                    // Lock on
                    lock (sSyncRoot)
                    {
                        // Delay instantiation until the object is first accessed
                        if (sInstance == null)
                        {
                            sInstance = new MetadataManager();
                        }
                    }
                }

                return sInstance;
            }
        }

        /// <summary>
        /// Gets the set of tactical metadata types names given the tactical data target type.
        /// </summary>
        /// <param name="pTargetType">The tactical data target type.</param>
        /// <returns>The set of tactical metadata types names.</returns>
        public IEnumerable<string> this[TargetKey pTargetType]
        {
            get
            {
                Dictionary<string, MetadataSetType> lTypes;
                if (sTacticalMetadataTypes.TryGetValue(pTargetType, out lTypes))
                {
                    foreach (MetadataSetType lType in lTypes.Values)
                    {
                        yield return lType.Id;
                    }
                }

                yield break;
            }
        }

        #endregion // Properties.

        #region Constructors

        /// <summary>
        /// Initializes static member(s) of the <see cref="MetadataManager"/> class.
        /// </summary>
        static MetadataManager()
        {
            // Creates all readers
            sReaders = new Dictionary<string, AMetadataReader>();
            IEnumerable<Type> lReaderTypes = typeof(AMetadataReader).GetInheritedTypes();
            foreach (Type lReaderType in lReaderTypes)
            {
                ConstructorInfo lDefaultConstructor = lReaderType.GetConstructor(Type.EmptyTypes);
                if (lDefaultConstructor != null)
                {
                    AMetadataReader lReader = lDefaultConstructor.Invoke(null) as AMetadataReader;
                    sReaders[lReader.Type] = lReader;
                }
            }

            // Create the type reader.
            sTypeReader = new MetadataTypeReader();

            // TODO See how it works.
            // Prepares the cache of tactical metadata types by tactical data type name.
            //sTacticalMetadataTypes = new Dictionary<string, Dictionary<string, MetadataSetType>>();
            //IEnumerable<Type> lTacticalDataTypes = typeof(ATacticalData).GetInheritedTypes(false, 1);
            //foreach (Type lTacticalDataType in lTacticalDataTypes)
            //{
            //    sTacticalMetadataTypes[lTacticalDataType.Name] = new Dictionary<string, MetadataSetType>();
            //}
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MetadataManager"/> class.
        /// </summary>
        public MetadataManager()
        {
            sInstance = this;

            this.Load();
        }

        #endregion // Constructors.

        #region Methods

        /// <summary>
        /// Gets a reader giving its identifier key.
        /// </summary>
        /// <param name="pKey"></param>
        /// <returns></returns>
        public AMetadataReader GetReader(string pKey)
        {
            AMetadataReader lReader;
            if (sReaders.TryGetValue(pKey, out lReader))
            {
                return lReader;
            }

            return null;
        }

        /// <summary>
        /// Retrieves the matadata type for the given tactical data type (e.g: Area2D, Area3D and so on..)
        /// </summary>
        /// <param name="pParentType">The parent metadata type.</param>
        /// <param name="pTarget">The target tactical data type.</param>
        /// <param name="pType">The user type.</param>
        /// <returns>The found type, null otherwise.</returns>
        public MetadataSetType GetType(MetadataSetType pParentType, TargetKey pTarget, TypeKey pType)
        {
            if (pParentType == null)
            {
                Dictionary<string, MetadataSetType> lTypes;
                if (sTacticalMetadataTypes.TryGetValue(pTarget, out lTypes))
                {
                    MetadataSetType lType;
                    if (lTypes.TryGetValue(pType, out lType))
                    {
                        return lType;
                    }
                }
            }

            if (pParentType != null)
            {
                return pParentType.NestedTypes.FirstOrDefault(pNestedType => pNestedType.GetTypeAsString() == this.BuildTypeAsString(pTarget, pType)) as MetadataSetType;
            }

            return null;
        }

        /// <summary>
        /// Retrieves the matadata type for the given tactical data type (e.g: Area2D, Area3D and so on..)
        /// </summary>
        /// <param name="pParentTypePathAsString">The type parent type string path.</param>
        /// <param name="pTypeAsString">The type as string.</param>
        /// <returns>The found type, null otherwise.</returns>
        public MetadataSetType GetType(string pParentTypePathAsString, string pTypeAsString)
        {
            MetadataSetType lParentType = null;
            TargetKey lTargetKey;
            TypeKey lTypeKey;

            // Going threw the parents.
            List<string> lParentTypesAsString = null;
            this.ParseParentTypePath(pParentTypePathAsString, out lParentTypesAsString);
            foreach (string lParentTypeAsString in lParentTypesAsString)
            {
                if (this.ParseTypeAsString(lParentTypeAsString, out lTargetKey, out lTypeKey))
                {
                    lParentType = this.GetType(lParentType, lTargetKey, lTypeKey);
                }
                else
                {
                    return null;
                }
            }

            // Getting the final nested type.
            if (this.ParseTypeAsString(pTypeAsString, out lTargetKey, out lTypeKey))
            {
                return this.GetType(lParentType, lTargetKey, lTypeKey);
            }

            return null;
        }

        /// <summary>
        /// Builds a tactical metadata type string.
        /// </summary>
        /// <param name="pType">The user type.</param>
        /// <returns>The type as string.</returns>
        public string BuildParentTypeAsString(MetadataSetType pType)
        {
            string lPath = string.Empty;
            IMetadataSetType lParent = pType.ParentType;
            while (lParent != null)
            {
                if (string.IsNullOrEmpty(lPath))
                {
                    lPath = lParent.GetTypeAsString();
                }
                else
                {
                    lPath = lParent.GetTypeAsString() + MetadataManager.PARENT_TYPE_AS_STRING_SEP + lPath;
                }

                lParent = lParent.ParentType;
            }

            return lPath;
        }

        /// <summary>
        /// Builds a tactical metadata type string.
        /// </summary>
        /// <param name="pType">The user type.</param>
        /// <returns>The type as string.</returns>
        public string BuildTypeAsString(MetadataSetType pType)
        {
            return this.BuildTypeAsString(new TargetKey(pType.TargetType), new TypeKey(pType.Id));
        }

        /// <summary>
        /// Builds a tactical metadata type string.
        /// </summary>
        /// <param name="pTarget">The target tactical data type.</param>
        /// <param name="pType">The user type.</param>
        /// <returns>The type as string.</returns>
        public string BuildTypeAsString(TargetKey pTarget, TypeKey pType)
        {
            return string.Format("{0}" + TYPE_AS_STRING_SEP + "{1}", (string)pTarget, (string)pType);
        }

        /// <summary>
        /// Parse the parent type path as string.
        /// </summary>
        /// <param name="pParentTypePath">The parent type path.</param>
        /// <param name="pTypes">The parsed pathes.</param>
        public void ParseParentTypePath(string pParentTypePath, out List<string> pTypes)
        {
            pTypes = new List<string>(pParentTypePath.Split(new string[] { PARENT_TYPE_AS_STRING_SEP }, System.StringSplitOptions.RemoveEmptyEntries));
        }

        /// <summary>
        /// Parse the type path as string.
        /// </summary>
        /// <param name="pTypeAsString">The type as string.</param>
        /// <param name="pTarget">The parsed target.</param>
        /// <param name="pType">The parsed type.</param>
        /// <returns>True if parse succeeded.</returns>
        public bool ParseTypeAsString(string pTypeAsString, out TargetKey pTarget, out TypeKey pType)
        {
            pTarget = new TargetKey();
            pType = new TypeKey();

            string[] lTypeParts = pTypeAsString.Split(new string[] { TYPE_AS_STRING_SEP }, System.StringSplitOptions.RemoveEmptyEntries);
            if (lTypeParts.Length == 2)
            {
                pTarget = new TargetKey(lTypeParts[0]);
                pType = new TypeKey(lTypeParts[1]);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Loads the Xml containing customization of tactical data.
        /// </summary>
        public void Load()
        {
            FileInfo lFileInfo = null; // PathManager.Instance.GetFile("Resources", @"Xml\UserTacticalDatas.xml");
            if (lFileInfo.Exists)
            {
                XElement lRoot = XElement.Load(lFileInfo.FullName);
                foreach (XElement lXTacticalMetadataType in lRoot.Descendants(cMetadataTypeTag))
                {
                    XAttribute lXTargetType = lXTacticalMetadataType.Attribute(cMetadataTargetTypeTag);
                    XAttribute lXuserType = lXTacticalMetadataType.Attribute(cMetadataUserTypeTag);
                    if (lXTargetType == null ||
                         lXuserType == null)
                    {
                        continue;
                    }

                    string lTargetType = lXTargetType.Value;

                    // If the target 
                    if (sTacticalMetadataTypes.ContainsKey(lTargetType) == false)
                    {
                        // Invalid tactical data type name...
                        continue;
                    }

                    string lUserTypeName = lXuserType.Value;
                    MetadataSetType lNewType = new MetadataSetType(lUserTypeName);
                    lNewType.TargetType = lTargetType;

                    foreach (XElement lXMetadata in lXTacticalMetadataType.Elements(cMetadataTag))
                    {
                        XAttribute lXType = lXMetadata.Attribute(cTypeTag);
                        if (lXType == null)
                        {
                            continue;
                        }

                        AMetadataReader lReader;
                        if (sReaders.TryGetValue(lXType.Value, out lReader))
                        {
                            lReader.Read(ref lNewType, lXMetadata);
                        }
                    }

                    // Checks for nested types.
                    foreach (XElement lXNestedType in lXTacticalMetadataType.Elements(cMetadataTypeTag))
                    {
                        sTypeReader.Read(ref lNewType, lXNestedType);
                    }

                    this.RegisterUserType(lNewType);
                }
            }
            else
            {
                // Log an error.
            }
        }

        /// <summary>
        /// Decorates a tactical data with the chosen metadata type if any.
        /// </summary>
        /// <param name="pToDecorate"></param>
        public MetadataSetType Decorate(IMetadatable pToDecorate)
        {
            if (pToDecorate == null)
            {
                return null;
            }

            // Reset anyway.
            pToDecorate.Metadata = null;

            string lUserTypeName = pToDecorate.UserType;
            if (string.IsNullOrEmpty(lUserTypeName))
            {
                this.Decorate(pToDecorate, null);
                return null;
            }

            Type lTacticalDataType = pToDecorate.GetType();
            Dictionary<string, MetadataSetType> lTypesById;
            if (sTacticalMetadataTypes.TryGetValue(lTacticalDataType.Name, out lTypesById))
            {
                MetadataSetType lType;
                if (lTypesById.TryGetValue(lUserTypeName, out lType))
                {
                    // Recursion over nested type(s)
                    this.Decorate(pToDecorate, lType);

                    return lType;
                }
            }

            return null;
        }

        /// <summary>
        /// Recursive fonction to decorate metadatable with metadata type(s).
        /// </summary>
        /// <param name="pToDecorate"></param>
        /// <param name="pType"></param>
        private void Decorate(IMetadatable pToDecorate, MetadataSetType pType)
        {
            // Cleaning metadata.
            pToDecorate.Metadata = null;

            // Cleaning the old type.
            MetadataSetType lOldType = pToDecorate.Type as MetadataSetType;
            if (lOldType != null)
            {
                foreach (MetadataSetType lNestedType in lOldType.NestedTypes)
                {
                    object lPropertyValue = pToDecorate.GetPropertyValue(lNestedType.TargetType);
                    if (lPropertyValue == null)
                    {
                        continue;
                    }

                    if (lPropertyValue is IEnumerable) // List of metadatable??
                    {
                        IEnumerable lList = lPropertyValue as IEnumerable;
                        foreach (object lItem in lList)
                        {
                            IMetadatable lMetadatable = lItem as IMetadatable;
                            if (lMetadatable != null)
                            {
                                this.Decorate(lMetadatable, null);
                            }
                        }
                    }
                    else if (lPropertyValue is IMetadatable) // Metadatable.
                    {
                        IMetadatable lMetadatable = lPropertyValue as IMetadatable;

                        this.Decorate(lMetadatable, null);
                    }
                }
            }

            pToDecorate.Type = pType;

            // Applying the new type.
            if (pType != null)
            {
                foreach (MetadataSetType lNestedType in pType.NestedTypes)
                {
                    object lPropertyValue = pToDecorate.GetPropertyValue(lNestedType.TargetType);
                    if (lPropertyValue == null)
                    {
                        continue;
                    }

                    if (lPropertyValue is IEnumerable) // List of metadatable??
                    {
                        IEnumerable lList = lPropertyValue as IEnumerable;
                        foreach (object lItem in lList)
                        {
                            IMetadatable lMetadatable = lItem as IMetadatable;
                            if (lMetadatable != null)
                            {
                                // Recurse for sub nested.
                                this.Decorate(lMetadatable, lNestedType);
                            }
                        }
                    }
                    else if (lPropertyValue is IMetadatable) // Metadatable.
                    {
                        IMetadatable lMetadatable = lPropertyValue as IMetadatable;
                        
                        // Recurse for sub nested.
                        this.Decorate(lMetadatable, lNestedType);
                    }
                }
            }
        }

        /// <summary>
        /// Registers a new user type
        /// </summary>
        /// <param name="pType">The new user type to register.</param>
        public void RegisterUserType(MetadataSetType pType)
        {
            if (pType == null)
            {
                return;
            }

            string lTargetType = pType.TargetType;
            Dictionary<string, MetadataSetType> lTypesById;
            if (sTacticalMetadataTypes.TryGetValue(lTargetType, out lTypesById))
            {
                lTypesById[pType.Id] = pType;
            }
        }

        /// <summary>
        /// Unregisters a user type.
        /// </summary>
        /// <param name="pType">The user type to unregister</param>
        /// <returns>True if successful, false otherwise.</returns>
        public bool UnregisterUserType(MetadataSetType pType)
        {
            if (pType == null)
            {
                return false;
            }

            string lTargetType = pType.TargetType;
            Dictionary<string, MetadataSetType> lTypesById;
            if (sTacticalMetadataTypes.TryGetValue(lTargetType, out lTypesById))
            {
                return lTypesById.Remove(pType.Id);
            }

            return false;
        }

        #endregion // Methods.
    }
}

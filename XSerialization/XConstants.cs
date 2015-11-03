namespace XSerialization
{
    /// <summary>
    /// This class stores all constants of the library.
    /// </summary>
    public static class XConstants
    {
        /// <summary>
        /// Constant which represents an object to reference by the system.
        /// </summary>
        public const int SYSTEM_REFERENCE = -1;

        /// <summary>
        /// Constant which represents a no referenced object.
        /// </summary>
        public const int NO_REFERENCED_OBJECT = -2;

        /// <summary>
        /// Constant which represents an object to reference later.
        /// </summary>
        public const int NOT_YET_REFERENCED_OBJECT = -3;

        /// <summary>
        ///  Constant which represents a root tag.
        /// </summary>
        public const string ROOT_TAG = "XRoot";

        /// <summary>
        ///  Constant which represents a type container
        /// </summary>
        public const string TYPE_CONTAINER_TAG = "XTypeContainer";

        /// <summary>
        ///  Constant which represents a NULL value.
        /// </summary>
        public const string NULL_TAG = "Null";

        /// <summary>
        ///  Constant which represents a type.
        /// </summary>
        public const string TYPE_REF_ATTRIBUTE = "ref";

        /// <summary>
        ///  Constant which represents a type.
        /// </summary>
        public const string TYPE_ATTRIBUTE = "type";

        /// <summary>
        ///  Constant which represents a reference.
        /// </summary>
        public const string REFERENCE_ATTRIBUTE = "ref";

        /// <summary>
        ///  Constant which represents a reference.
        /// </summary>
        public const string EXTERNAL_REFERENCE_ATTRIBUTE = "href";
        
        /// <summary>
        ///  Constant which represents an identifier.
        /// </summary>
        public const string ID_ATTRIBUTE = "id";

        /// <summary>
        /// Constant which represents a generic definition
        /// </summary>
        public const string GENERIC_DEFINITION_TAG = "XGenericDefinition";

        /// <summary>
        ///  Constant which represents a generic type.
        /// </summary>
        public const string TYPE_TAG = "XType";

        /// <summary>
        ///  Constant which represents a generic type.
        /// </summary>
        public const string QUALIFIED_TYPE_TAG = "XQualififedType";

        /// <summary>
        ///  Constant which represents an item (list, dictionary, ...)
        /// </summary>
        public const string ITEM_TAG = "XItem";

        /// <summary>
        ///  Constant which represents a key (dictionary)
        /// </summary>
        public const string KEY_TAG = "XKey";

        /// <summary>
        ///  Constant which represents a value (dictionary)
        /// </summary>
        public const string VALUE_TAG = "XValue";

        /// <summary>
        /// Constant which represents the assembly separator.
        /// </summary>
        public const string ASSEMBLY_SEPARATOR = " @@ ";

        /// <summary>
        /// Constant which represents an undefined assembly name.
        /// </summary>
        public const string UNDEFINED_ASSEMBLY = "UND_ASS";
    }
}

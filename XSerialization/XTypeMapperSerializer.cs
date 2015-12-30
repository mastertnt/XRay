using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace XSerialization
{
    /// <summary>
    /// This class is used to map a type to another type but with matched definition.
    /// </summary>
    public class XTypeMapperSerializer : XSerializer
    {
        #region Fields

        /// <summary>
        /// Stores the type mapping.
        /// </summary>
        private readonly Dictionary<string, string> mTypeMappings = null;

        /// <summary>
        /// Stores the mapping from an old type reference pointing on an existing new reference.
        /// </summary>
        private readonly Dictionary<string, string> mMultiTypeReferences = null;

        #endregion // Fields.

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="XTypeMapperSerializer"/> class.
        /// </summary>
        /// <param name="pTypeMappings">The dictionnary of type to map (old type to new type).</param>
        /// <param name="pExternalReferenceResolver">An external reference resolver.</param>
        /// <param name="pDiscoverContracts">Set to true to discovert new contracts.</param>
        public XTypeMapperSerializer( Dictionary<string, string> pTypeMappings, IXExternalReferenceResolver pExternalReferenceResolver = null, bool pDiscoverContracts = true)
            :base(pExternalReferenceResolver, pDiscoverContracts)
        {
            this.mTypeMappings = new Dictionary<string, string>();
            this.InitializeTypeMappings(pTypeMappings);
            this.mMultiTypeReferences = new Dictionary<string, string>();
        }

        #endregion // Constructors.

        #region Methods

        /// <summary>
        /// This method is used to read all types under element "XTypeContainer"
        /// </summary>
        /// <param name="pTypeContainer">The root element.</param>
        /// <returns>The number of discovered types.</returns>
        protected override int ReadTypeContainer(XElement pTypeContainer)
        {
            foreach (XElement lTypeRefElement in pTypeContainer.Elements())
            {
                if (lTypeRefElement.Attribute(XConstants.TYPE_REF_ATTRIBUTE) != null)
                {
                    string lTypeRef = lTypeRefElement.Attribute(XConstants.TYPE_REF_ATTRIBUTE).Value;
                    string lOldTypeName = TypeExtensions.RemoveVersionFromFullName(lTypeRefElement.Value);
                    if (this.mTypeMappings.ContainsKey(lOldTypeName))
                    {
                        lTypeRefElement.Value = this.mTypeMappings[lOldTypeName];
                    }
                    
                    Type lResolvedType = lTypeRefElement.ToType();
                    if (lResolvedType != null)
                    {
                        if (this.mTypeReferencesByType.ContainsKey(lResolvedType) == false)
                        {
                            this.mTypeReferencesByRef.Add(lTypeRef, lResolvedType);
                            this.mTypeReferencesByType.Add(lResolvedType, lTypeRef);
                            this.mElementsByType.Add(lResolvedType, lTypeRefElement);
                        }
                        else
                        {
                            // Type is already referenced. Redirecting the new reference to the existing one.
                            this.mMultiTypeReferences.Add(lTypeRef, this.mTypeReferencesByType[lResolvedType]);
                        }
                    }
                    else
                    {
                        this.mErrors.Add(new XSerializationError(XErrorType.UnkwnonType, 0, 0, this.CurrentFile, lTypeRefElement.Value));
                        File.AppendAllText(@"E:\DirectCGF\Branches\DrManhattan\Build\missing_types.xml", "<TypeMapping>" + Environment.NewLine);
                        File.AppendAllText(@"E:\DirectCGF\Branches\DrManhattan\Build\missing_types.xml", "<OldType>");
                        File.AppendAllText(@"E:\DirectCGF\Branches\DrManhattan\Build\missing_types.xml", lTypeRefElement.Value);
                        File.AppendAllText(@"E:\DirectCGF\Branches\DrManhattan\Build\missing_types.xml", "</OldType>");
                        File.AppendAllText(@"E:\DirectCGF\Branches\DrManhattan\Build\missing_types.xml", "<NewType>");
                        File.AppendAllText(@"E:\DirectCGF\Branches\DrManhattan\Build\missing_types.xml", lTypeRefElement.Value);
                        File.AppendAllText(@"E:\DirectCGF\Branches\DrManhattan\Build\missing_types.xml", "</NewType>");
                        File.AppendAllText(@"E:\DirectCGF\Branches\DrManhattan\Build\missing_types.xml", "</TypeMapping>" + Environment.NewLine);
                    }
                }
            }
            return this.mTypeReferencesByRef.Count;
        }

        /// <summary>
        /// This method is used to revolve a type inside a serializer.
        /// </summary>
        /// <param name="pTypeElement">The type element.</param>
        /// <returns>The retrieved type.</returns>
        public override Type ResolveType(XElement pTypeElement)
        {
            if (pTypeElement != null)
            {
                if (pTypeElement.Attribute(XConstants.TYPE_REF_ATTRIBUTE) != null)
                {
                    string lTypeRef = pTypeElement.Attribute(XConstants.TYPE_REF_ATTRIBUTE).Value;

                    // Verifying if the reference can be redirected.
                    if (this.mMultiTypeReferences.ContainsKey(lTypeRef))
                    {
                        lTypeRef = this.mMultiTypeReferences[lTypeRef];
                    }

                    if (this.mTypeReferencesByRef.ContainsKey(lTypeRef))
                    {
                        return this.mTypeReferencesByRef[lTypeRef];
                    }
                }

                return pTypeElement.ToType();
            }

            return null;
        }

        /// <summary>
        /// Initializes the type mappings.
        /// </summary>
        /// <param name="pTypeMappings">The dictionnary of type to map (old type to new type).</param>
        private void InitializeTypeMappings(Dictionary<string, string> pTypeMappings)
        {
            foreach (KeyValuePair<string, string> lPair in pTypeMappings)
            {
                // Removing the version information of the ol type.
                string lNewKey = TypeExtensions.RemoveVersionFromFullName(lPair.Key);

                // Filling the map using this new type.
                this.mTypeMappings.Add(lNewKey, lPair.Value);
            }
        }

        #endregion // Methods.
    }
}

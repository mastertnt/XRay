using System;
using System.IO;
using System.Xml.Linq;

namespace XSerialization.Values
{
    /// <summary>
    /// This class implements a contract to read/write external references.
    /// </summary>
    public class ExternalReferenceSerializationContract : IXSerializationContract
    {
        /// <summary>
        /// The target element.
        /// </summary>
        private XElement mTargetElement;

        /// <summary>
        /// Flag to know if an external creation is necessary.
        /// </summary>
        public bool NeedCreate
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// This method determines whether this type can manage the specified object.
        /// </summary>
        /// <param name="pObjectType">The object type to test.</param>
        /// <param name="pSerializationContext">The serialization context.</param>
        /// <returns>The depth of inheritance or -1 if the contract cannot support.</returns>
        public virtual SupportPriority CanManage(Type pObjectType, IXSerializationContext pSerializationContext)
        {
            return SupportPriority.CANNOT_SUPPORT;
        }

        /// <summary>
        /// This method determines whether this instance can manage the specified object.
        /// </summary>
        /// <param name="pObject">The object to test.</param>
        /// <param name="pSerializationContext">The serialization context.</param>
        /// <returns>The depth of inheritance or -1 if the contract cannot support.</returns>
        public virtual SupportPriority CanManage(object pObject, IXSerializationContext pSerializationContext)
        {
            return SupportPriority.CANNOT_SUPPORT;
        }

        /// <summary>
        /// This method determines whether this instance can manage the specified object.
        /// </summary>
        /// <param name="pParentElement">The parent element.</param>
        /// <param name="pSerializationContext">The serialization context.</param>
        /// <returns>The depth of inheritance or -1 if the contract cannot support.</returns>
        public virtual SupportPriority CanManage(XElement pParentElement, IXSerializationContext pSerializationContext)
        {
            // Look for : <ParentElement><Null/><ParentElement>
            if (pParentElement.Attribute(XConstants.EXTERNAL_REFERENCE_ATTRIBUTE) != null)
            {
                this.mTargetElement = pParentElement;
                return new SupportPriority(SupportLevel.Element, 0);
            }
            return SupportPriority.CANNOT_SUPPORT;
        }

        /// <summary>
        /// This method creates the specified element.
        /// </summary>
        /// <param name="pParentElement">The parent element.</param>
        /// <param name="pSerializationContext">The serialization context.</param>
        /// <returns>The created object.</returns>
        public virtual object Create(XElement pParentElement, IXSerializationContext pSerializationContext)
        {
            XAttribute lAttribute = this.mTargetElement.Attribute(XConstants.EXTERNAL_REFERENCE_ATTRIBUTE);
            string lExternalReference = lAttribute.Value;
            string lFullExternalReference;
            bool lIsRelative;
            if (Path.IsPathRooted(lExternalReference))
            {
                lFullExternalReference = lExternalReference;
                lIsRelative = false;
            }
            else
            {
                lFullExternalReference = pSerializationContext.CurrentDirectory.FullName + Path.DirectorySeparatorChar + lExternalReference;
                lIsRelative = true;
                
            }
            XSerializer lDeserializer = new XSerializer(pSerializationContext.ExternalReferenceResolver);
            object lResult = lDeserializer.Deserialize(lFullExternalReference);
            pSerializationContext.ExternalReferenceResolver.RegisterExternalReference(lResult, lFullExternalReference, lIsRelative);
            return lResult;
        }

        /// <summary>
        /// This method reads the specified element.
        /// </summary>
        /// <param name="pObjectToInitialize">The object to initialize</param>
        /// <param name="pParentElement">The element.</param>
        /// <param name="pSerializationContext">The serialization context.</param>
        /// <returns>The initialized object if the input object is valid.</returns>
        public virtual object Read(object pObjectToInitialize, XElement pParentElement, IXSerializationContext pSerializationContext)
        {
            // Nothing to do.
            return pObjectToInitialize;
        }

        /// <summary>
        /// This method writes the specified object.
        /// </summary>
        /// <param name="pObject">The object to serialize.</param>
        /// <param name="pParentElement">The parent element.</param>
        /// <param name="pSerializationContext">The serialization context.</param>
        /// <returns>The modified parent element</returns>
        public virtual XElement Write(object pObject, XElement pParentElement, IXSerializationContext pSerializationContext)
        {
            string lExternalReference = pSerializationContext.ExternalReferenceResolver.GetExternalReference(pObject);
            string lRelativePath = this.MakeRelativePath(pSerializationContext.CurrentDirectory.FullName, lExternalReference);
            pParentElement.SetAttributeValue(XConstants.EXTERNAL_REFERENCE_ATTRIBUTE, lRelativePath);
            return pParentElement;
        }

        /// <summary>
        /// THis method creates a relative path from one file or folder to another.
        /// </summary>
        /// <param name="pFromPath">Contains the directory that defines the start of the relative path.</param>
        /// <param name="pToPath">Contains the path that defines the endpoint of the relative path.</param>
        /// <returns>The relative path from the start directory to the end path.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="UriFormatException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        private String MakeRelativePath(String pFromPath, String pToPath)
        {
            string lFromPath = pFromPath + Path.DirectorySeparatorChar;

            if (String.IsNullOrEmpty(lFromPath)) throw new ArgumentNullException("pFromPath");
            if (String.IsNullOrEmpty(pToPath)) throw new ArgumentNullException("pToPath");

            Uri lFromUri = new Uri(lFromPath);
            Uri lToUri = new Uri(pToPath);

            // Path can't be made relative.
            if (lFromUri.Scheme != lToUri.Scheme)
            {
                return pToPath;
            } 

            Uri lRelativeUri = lFromUri.MakeRelativeUri(lToUri);
            String lRelativePath = Uri.UnescapeDataString(lRelativeUri.ToString());

            if (lToUri.Scheme.ToUpperInvariant() == "FILE")
            {
                lRelativePath = lRelativePath.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            }

            return lRelativePath;
        }
    }
}

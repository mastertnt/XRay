using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using XSerialization.Decorators;
using XSerialization.ExternalResolvers;
using XSerialization.Values;

namespace XSerialization
{
    /// <summary>
    /// This class is used to serialize/deserialize object from XElement.
    /// </summary>
    public class XSerializer : IXSerializationContext
    {
        #region Fields

        /// <summary>
        /// This field stores all the errors.
        /// </summary>
        protected readonly List<XSerializationError> mErrors = new List<XSerializationError>();

        /// <summary>
        /// Stores all the serialization context parameters
        /// </summary>
        private readonly Hashtable mSerializationContextParameters = new Hashtable();

        /// <summary>
        /// This field stores the current objects.
        /// </summary>
        private readonly Stack<object> mCurrentObjects = new Stack<object>();

        /// <summary>
        /// This field stores the object references by reference.
        /// </summary>
        private readonly Dictionary<int, object> mObjectReferencesByRef = new Dictionary<int, object>();

        /// <summary>
        /// This field stores the object references by object.
        /// </summary>
        private readonly Dictionary<object, int> mObjectReferencesByObj = new Dictionary<object, int>(new ObjectRefEqualityComparer());

        /// <summary>
        /// This field stores the XElement by type.
        /// </summary>
        protected readonly Dictionary<Type, XElement> mElementsByType = new Dictionary<Type, XElement>();

        /// <summary>
        /// This field stores the type references by reference.
        /// </summary>
        protected readonly Dictionary<string, Type> mTypeReferencesByRef = new Dictionary<string, Type>();

        /// <summary>
        /// This field stores the type references by type.
        /// </summary>
        protected readonly Dictionary<Type, string> mTypeReferencesByType = new Dictionary<Type, string>(); 

        /// <summary>
        /// This field stores the current directory.
        /// </summary>
        private DirectoryInfo mCurrentDirectory;

        /// <summary>
        /// The reference counter.
        /// </summary>
        private int mReferenceCounter;

        /// <summary>
        /// The reference counter.
        /// </summary>
        private int mTypeReferenceCounter;

        /// <summary>
        /// The list of all contracts that can be used by the serializer.
        /// </summary>
        private readonly List<IXSerializationContract> mContracts = new List<IXSerializationContract>();

        /// <summary>
        /// This field stores the null contract.
        /// </summary>
        private readonly NullValueSerializationContract mNullContract;

        #endregion // Fields.

        #region Events

        /// <summary>
        /// Fired each time an error gets raised on this serializer.
        /// </summary>
        public event Action<XSerializationError> ErrorRaised;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the errors occured during serialization or deserialization.
        /// </summary>
        public IEnumerable<XSerializationError> Errors
        {
            get { return this.mErrors; }
        }

        /// <summary>
        /// Gets the current object.
        /// </summary>
        public object CurrentObject
        {
            get
            {
                if (this.mCurrentObjects.Any())
                {
                    return this.mCurrentObjects.Peek();
                }
                return null;
            }
        }

        /// <summary>
        /// Gets the current directory.
        /// </summary>
        public DirectoryInfo CurrentDirectory
        {
            get { return this.mCurrentDirectory; }
        }

        /// <summary>
        /// Gets the external reference resolver.
        /// </summary>
        public IXExternalReferenceResolver ExternalReferenceResolver
        {
            get; 
            private set;
        }

        /// <summary>
        /// Gets the flag to know if the serializer is writing or not.
        /// </summary>
        public bool IsWriting
        {
            get; 
            private set; 
        }

        /// <summary>
        /// Gets the current URI file.
        /// </summary>
        public Uri CurrentFile
        {
            get; 
            private set; 
        }

        /// <summary>
        /// Gets the type of the internal reference excluded.
        /// </summary>
        /// <value>
        /// The type of the internal reference excluded.
        /// </value>
        internal List<String> InternalReferenceExcludedType
        {
            get;
            private set;
        }

        #endregion // Properties.

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="XSerializer"/> class.
        /// </summary>
        /// <param name="pExternalReferenceResolver">An external reference resolver.</param>
        /// <param name="pDiscoverContracts">Set to true to discovert new contracts.</param>
        public XSerializer(IXExternalReferenceResolver pExternalReferenceResolver = null, bool pDiscoverContracts = true)
        {
            if (pExternalReferenceResolver == null)
            {
                this.ExternalReferenceResolver = new DefaultExternalReferenceResolver();
            }
            else
            {
                this.ExternalReferenceResolver = pExternalReferenceResolver;
            }

            this.InternalReferenceExcludedType = new List<string>();

            if (pDiscoverContracts)
            {
                foreach (var lContract in SerializationContractManager.Instance.Contracts)
                {
                    this.mContracts.Add(lContract);
                }
            }
            this.mNullContract = SerializationContractManager.Instance.Contracts.FirstOrDefault(pElt => pElt is NullValueSerializationContract) as NullValueSerializationContract;
        }

        #endregion // Constructors.

        #region Methods

        /// <summary>
        /// Registers a type which will be excluded from the internal refrences mechanism.
        /// </summary>
        /// <param name="pTypeFullName">Full name of the type to exclude.</param>
        public virtual void RegisterInternalReferenceExcludedType(string pTypeFullName)
        {
            if (!this.InternalReferenceExcludedType.Contains(pTypeFullName))
            {
                this.InternalReferenceExcludedType.Add(pTypeFullName);
            }
        }

        /// <summary>
        /// This method deserializes a XElement into an object.
        /// </summary>
        /// <param name="pElement">The element to deserialize.</param>
        /// <returns>The created object, null if the deserialization failed.</returns>
        public virtual object Deserialize(XElement pElement)
        {
            this.IsWriting = false;
            this.mErrors.Clear();
            XElement lTypeContainerElement = pElement.Element(XConstants.TYPE_CONTAINER_TAG);
            if (lTypeContainerElement != null)
            {
                this.ReadTypeContainer(lTypeContainerElement);
            }

            XElement lTypeElement = pElement.Element(XConstants.TYPE_TAG);
            if (lTypeElement != null)
            {
                IXSerializationContract lContract = this.SelectContract(pElement, this.ResolveType(lTypeElement));
                if (lContract != null)
                {
                    if (this.mCurrentDirectory == null)
                    {
                        this.mCurrentDirectory = new DirectoryInfo(Environment.CurrentDirectory);
                    }

                    object lObject = null;
                    SerializationContractDecorator lDecoratorContract = lContract as SerializationContractDecorator;
                    if (lDecoratorContract != null && lDecoratorContract.NeedCreate)
                    {
                        // The decorator read method will handle the object creation.
                        lObject = lContract.Read(lObject, pElement, this);
                    }
                    else
                    {
                        // Forcing the object creation.
                        lObject = lContract.Create(pElement, this);
                        lObject = lContract.Read(lObject, pElement, this);
                    }

                    return lObject;
                }
            }
            
            IXmlLineInfo lInfo = pElement;
            this.PushError(new XSerializationError(XErrorType.UnkwnonType, lInfo.LineNumber, lInfo.LinePosition, this.CurrentFile, string.Empty));
            return null;
        }


        /// <summary>
        /// This method deserializes a XElement into an object.
        /// </summary>
        /// <param name="pInputFilename">The input filename.</param>
        /// <returns>The created object, null if the deserialization failed.</returns>
        public virtual object Deserialize(string pInputFilename)
        {
            try
            {
                FileInfo lFileInfo = new FileInfo(pInputFilename);
                this.CurrentFile = new Uri(lFileInfo.FullName);
                this.mCurrentDirectory = lFileInfo.Directory;
                XElement lElement = XElement.Load(pInputFilename);
                return this.Deserialize(lElement);
            }
            catch (XmlException lException)
            {
                this.PushError(new XSerializationError(XErrorType.InvalidXml, lException.LineNumber, lException.LinePosition, this.CurrentFile, lException.SourceUri));
            }
            
            return null;
        }

        /// <summary>
        /// This method serializes an object into an XElement and dump it into a file.
        /// </summary>
        /// <param name="pObjectToSerialize">The object to serialize.</param>
        /// <param name="pOutputFilename">The output filename.</param>
        /// <returns>A valid XElement if the serialization succeed, null otherwise.</returns>
        public virtual XElement Serialize(object pObjectToSerialize, string pOutputFilename)
        {
            FileInfo lFileInfo = new FileInfo(pOutputFilename);
            this.CurrentFile = new Uri(lFileInfo.FullName);
            this.mCurrentDirectory = lFileInfo.Directory;
            XElement lElement = this.Serialize(pObjectToSerialize);
            if (lElement != null)
            {
                lElement.Save(pOutputFilename);
                return lElement;
            }
            return null;
        }

        /// <summary>
        /// This method serializes an object into an XElement and dump it to a stream.
        /// </summary>
        /// <param name="pObjectToSerialize">The object to serialize.</param>
        /// <param name="pOutputStream">The output stream.</param>
        /// <returns>A valid XElement if the serialization succeed, null otherwise.</returns>
        public virtual XElement Serialize(object pObjectToSerialize, Stream pOutputStream)
        {
            FileStream lFileStream = pOutputStream as FileStream;
            if (lFileStream != null)
            {
                this.CurrentFile = new Uri(lFileStream.Name);
            }

            XElement lElement = this.Serialize(pObjectToSerialize);
            if (lElement != null)
            {
                lElement.Save(pOutputStream);
                return lElement;
            }
            return null;
        }

        /// <summary>
        /// This method serializes an object into an XElement.
        /// </summary>
        /// <param name="pObjectToSerialize">The object to serialize.</param>
        /// <returns>A valid XElement if the serialization succeed, null otherwise.</returns>
        public virtual XElement Serialize(object pObjectToSerialize)
        {
            this.IsWriting = true;
            this.mErrors.Clear();
            XElement lParentElement = new XElement(XConstants.ROOT_TAG);
            IXSerializationContract lContract = this.SelectContract(null, null, null, pObjectToSerialize);
            if (lContract != null)
            {
                if (this.mCurrentDirectory == null)
                {
                    this.mCurrentDirectory = new DirectoryInfo(Environment.CurrentDirectory);
                }
                
                // Write the object.
                lContract.Write(pObjectToSerialize, lParentElement, this);

                XElement lTypeContainerElement = new XElement(XConstants.TYPE_CONTAINER_TAG);
                foreach (KeyValuePair<Type, XElement> lTypeInfo in this.mElementsByType)
                {
                    lTypeInfo.Value.SetAttributeValue(XConstants.TYPE_REF_ATTRIBUTE, this.mTypeReferencesByType[lTypeInfo.Key]);
                    lTypeContainerElement.Add(lTypeInfo.Value);
                }

                lParentElement.AddFirst(lTypeContainerElement);

                return lParentElement;
            }
            return null;
        }

        /// <summary>
        /// This method pushes the object.
        /// </summary>
        /// <param name="pObject">The object to push as current</param>
        /// <param name="pObjectReference">The object reference. -1 means the object reference will be fixed by the context.</param>
        public virtual void PushObject(object pObject, int pObjectReference)
        {
            this.mCurrentObjects.Push(pObject);
            
            // Auto-compute the object reference (generally during writing).
            if (this.mObjectReferencesByObj.ContainsKey(pObject) == false && pObjectReference == XConstants.SYSTEM_REFERENCE)
            {
                this.mObjectReferencesByRef.Add(mReferenceCounter, pObject);
                this.mObjectReferencesByObj.Add(pObject, mReferenceCounter);
                this.mReferenceCounter++;
            }
            else if (this.mObjectReferencesByObj.ContainsKey(pObject) == false && pObjectReference != XConstants.NO_REFERENCED_OBJECT && this.mObjectReferencesByRef.ContainsKey(pObjectReference) == false)
            {
                this.mObjectReferencesByRef.Add(pObjectReference, pObject);
                this.mObjectReferencesByObj.Add(pObject, pObjectReference);
            }
        }

        /// <summary>
        /// This method pushes the object.
        /// </summary>
        /// <param name="pError">The error to add.</param>
        public virtual void PushError(XSerializationError pError)
        {
            this.mErrors.Add(pError);

            if (this.ErrorRaised != null)
            {
                this.ErrorRaised(pError);
            }
        }

        /// <summary>
        /// This method pops the object.
        /// </summary>
        public virtual void PopObject()
        {
            this.mCurrentObjects.Pop();
        }

        /// <summary>
        /// Gets the object reference identifier.
        /// </summary>
        /// <param name="pObjectReference">The object reference.</param>
        /// <returns>The object if retrieved, null otherwise.</returns>
        public virtual object GetObjectByReference(int pObjectReference)
        {
            if (this.mObjectReferencesByRef.ContainsKey(pObjectReference))
            {
                return this.mObjectReferencesByRef[pObjectReference];
            }

            return null;
        }

        /// <summary>
        /// Gets the object reference identifier.
        /// </summary>
        /// <param name="pObject">The object to test.</param>
        /// <returns>The object identifier or -1 if the object has no reference.</returns>
        public virtual int GetObjectReference(object pObject)
        {
            if (this.mObjectReferencesByObj.ContainsKey(pObject))
            {
                return this.mObjectReferencesByObj[pObject];
            }

            return XConstants.NOT_YET_REFERENCED_OBJECT;
        }

        /// <summary>
        /// This method is used to revolve a type inside a serializer.
        /// </summary>
        /// <param name="pTypeElement">The type element.</param>
        /// <returns>The retrieved type.</returns>
        public virtual Type ResolveType(XElement pTypeElement)
        {
            if (pTypeElement != null)
            {
                if (pTypeElement.Attribute(XConstants.TYPE_REF_ATTRIBUTE) != null)
                {
                    string lTypeRef = pTypeElement.Attribute(XConstants.TYPE_REF_ATTRIBUTE).Value;
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
        /// This method is used to revolve a type inside a serializer.
        /// </summary>
        /// <param name="pType">The type to store.</param>
        /// <returns>The retrieved type.</returns>
        public virtual XElement ReferenceType(Type pType)
        {
            string lTypeRef = string.Empty;
            if (this.mElementsByType.ContainsKey(pType) == false)
            {
                lTypeRef = string.Format("type_{0}", this.mTypeReferenceCounter);
                this.mTypeReferenceCounter++;
                this.mElementsByType.Add(pType, pType.ToElement());
                this.mTypeReferencesByRef.Add(lTypeRef, pType);
                this.mTypeReferencesByType.Add(pType, lTypeRef);
            }
            else
            {
                lTypeRef = this.mTypeReferencesByType[pType];
            }
            XElement lRefTypeElement = new XElement(XConstants.TYPE_TAG);
            lRefTypeElement.SetAttributeValue(XConstants.TYPE_REF_ATTRIBUTE, lTypeRef);
            return lRefTypeElement;
        }

        /// <summary>
        /// This method is used to select the contract according the best contract.
        /// </summary>
        /// <param name="pElement">The current element.</param>
        /// <param name="pObject">The object can be a property info, a type or a value.</param>
        /// <returns>The best contract according to constraints.</returns>
        public virtual IXSerializationContract SelectContract(XElement pElement, object pObject)
        {
            Type lType = pObject as Type;
            PropertyInfo lPropertyInfo = pObject as PropertyInfo;
            Object lValue = null;
            if (lPropertyInfo == null && lType == null)
            {
                lValue = pObject;
            }

            return this.SelectContract(pElement, lPropertyInfo, lType, lValue);
        }

        /// <summary>
        /// This method is used to select the contract according contraints.
        /// </summary>
        /// <param name="pElement">The current element.</param>
        /// <param name="pPropertyInfo">The current property info.</param>
        /// <param name="pType">The current type.</param>
        /// <param name="pObject">The current object.</param>
        /// <returns>The best contract according to constraints.</returns>
        public virtual IXSerializationContract SelectContract(XElement pElement, PropertyInfo pPropertyInfo, Type pType, object pObject)
        {
            XSerializationAttribute lCurrentAttribute = null;

            List<Tuple<IXSerializationContract, SupportPriority>> lAvailableContracts = new List<Tuple<IXSerializationContract, SupportPriority>>();
            if (pElement != null)
            {
                Tuple<IXSerializationContract, SupportPriority> lElementContract = this.FindContract(pElement);
                if (lElementContract != null)
                {
                    lAvailableContracts.Add(lElementContract);
                }
            }

            if (pPropertyInfo != null)
            {
                object[] lAttributes = pPropertyInfo.GetCustomAttributes(typeof (XSerializationAttribute), true);
                if (lAttributes.Any())
                {
                    Tuple<IXSerializationContract, SupportPriority> lAttributeContract = this.FindContract(lAttributes[0]);
                    if (lAttributeContract != null)
                    {
                        lCurrentAttribute = lAttributes[0] as XSerializationAttribute;
                        lAvailableContracts.Add(lAttributeContract);
                    }
                }

                // Look for a contract on the type.
                if (lCurrentAttribute == null)
                {
                    Tuple<IXSerializationContract, SupportPriority> lObjectContract = this.FindContract(pPropertyInfo);
                    if (lObjectContract != null)
                    {
                        lAvailableContracts.Add(lObjectContract);
                    }
                }
            }

            if (pType != null)
            {
                Tuple<IXSerializationContract, SupportPriority> lTypeContract = this.FindContract(pType);
                if (lTypeContract != null)
                {
                    lAvailableContracts.Add(lTypeContract);
                }
            }
            if (pObject != null)
            {
                Tuple<IXSerializationContract, SupportPriority> lObjectContract = this.FindContract(pObject);
                if (lObjectContract != null)
                {
                    lAvailableContracts.Add(lObjectContract);
                }
            }
            

            if (lAvailableContracts.Any())
            {
                var lFirstOrDefault = lAvailableContracts.OrderBy(pElt => pElt.Item2).FirstOrDefault();
                if (lFirstOrDefault != null)
                {
                    IXSerializationContract lBestContract = lFirstOrDefault.Item1;
                    if (lBestContract is AttributeSerializationContract && lCurrentAttribute != null)
                    {
                        AttributeSerializationContract lDynamicContract = new AttributeSerializationContract();
                        lDynamicContract.SubContract = Activator.CreateInstance(lCurrentAttribute.SupportedContract, true) as IXSerializationContract;
                        return lDynamicContract;
                    }
                    return new SerializationContractDecorator(lBestContract, this);
                }
            }
            

            return this.mNullContract;
        }

        /// <summary>
        /// This method get a serialization parameter by name
        /// </summary>
        /// <typeparam name="TType">The serialization parameter type to return</typeparam>
        /// <param name="pParameterName">The serialization parameter name</param>
        /// <param name="pIsFound">Return whether it found the parameter or not</param>
        /// <returns>The serialization parameter</returns>
        public virtual TType GetSerializationParameter<TType>(string pParameterName, out bool pIsFound) where TType : struct
        {
            if 
                ( this.mSerializationContextParameters.ContainsKey( pParameterName ) )
            {
                pIsFound = true;
                return (TType)this.mSerializationContextParameters[ pParameterName ];
            }

            pIsFound = false;
            return default( TType );
        }

        /// <summary>
        /// This method set a serialization parameter that will be indexed by the supplied name
        /// </summary>
        /// <typeparam name="TType">The serialization parameter type</typeparam>
        /// <param name="pParameterName">The serialization parameter name</param>
        /// <param name="pParameter">The serialization parameter</param>
        public virtual void SetSerializationParameter<TType>(string pParameterName, TType pParameter) where TType : struct
        {
            this.mSerializationContextParameters[ pParameterName ] = pParameter;
        }

        /// <summary>
        /// This method is used to add a contact for this serializer only.
        /// </summary>
        /// <param name="pContract">The contract to add.</param>
        public virtual bool AddContract(IXSerializationContract pContract)
        {
            this.mContracts.Add(pContract);
            return true;
        }

        /// <summary>
        /// This method is used to add a contact for this serializer only.
        /// </summary>
        /// <param name="pContract">The contract to add.</param>
        public virtual bool RemoveContract(IXSerializationContract pContract)
        {
            return this.mContracts.Remove(pContract);
        }

        /// <summary>
        /// This method is used to read all types under element "XTypeContainer"
        /// </summary>
        /// <param name="pTypeContainer">The root element.</param>
        /// <returns>The number of discovered types.</returns>
        protected virtual int ReadTypeContainer(XElement pTypeContainer)
        {
            foreach (XElement lTypeRefElement in pTypeContainer.Elements())
            {
                if (lTypeRefElement.Attribute(XConstants.TYPE_REF_ATTRIBUTE) != null)
                {
                    string lTypeRef = lTypeRefElement.Attribute(XConstants.TYPE_REF_ATTRIBUTE).Value;
                    Type lResolvedType = lTypeRefElement.ToType();
                    if (lResolvedType != null)
                    {
                        this.mTypeReferencesByRef.Add(lTypeRef, lResolvedType);
                        this.mTypeReferencesByType.Add(lResolvedType, lTypeRef);
                        this.mElementsByType.Add(lResolvedType, lTypeRefElement);
                    }
                }
            }
            return this.mTypeReferencesByRef.Count;
        }

        /// <summary>
        /// This method finds a serialization contract.
        /// </summary>
        /// <param name="pObject">The object to test.</param>
        /// <returns>The most suited serialization contract, null if there is no contract which supports the object.</returns>
        private  Tuple<IXSerializationContract, SupportPriority> FindContract(object pObject)
        {
            List<Tuple<IXSerializationContract, SupportPriority>> lAvailableContracts = new List<Tuple<IXSerializationContract, SupportPriority>>();
// ReSharper disable once LoopCanBeConvertedToQuery
            foreach (IXSerializationContract lContract in this.mContracts)
            {
                SupportPriority lPriority = lContract.CanManage(pObject, this);
                if (lPriority != null && lPriority != SupportPriority.CANNOT_SUPPORT)
                {
                    lAvailableContracts.Add(new Tuple<IXSerializationContract, SupportPriority>(lContract, lPriority));
                }
            }

            if (lAvailableContracts.Any())
            {
                // ReSharper disable once PossibleNullReferenceException
                return lAvailableContracts.OrderBy(pElt => pElt.Item2).FirstOrDefault();
            }
            return null;
        }

        /// <summary>
        /// This method finds a serialization contract.
        /// </summary>
        /// <param name="pType">The target type.</param>
        /// <returns>The best matching contract.</returns>
        private Tuple<IXSerializationContract, SupportPriority> FindContract(Type pType)
        {
            List<Tuple<IXSerializationContract, SupportPriority>> lAvailableContracts = new List<Tuple<IXSerializationContract, SupportPriority>>();
// ReSharper disable once LoopCanBeConvertedToQuery
            foreach (IXSerializationContract lContract in this.mContracts)
            {
                SupportPriority lPriority = lContract.CanManage(pType, this);
                if (lPriority != null && lPriority != SupportPriority.CANNOT_SUPPORT)
                {
                    lAvailableContracts.Add(new Tuple<IXSerializationContract, SupportPriority>(lContract, lPriority));
                }
            }

            if (lAvailableContracts.Any())
            {
                // ReSharper disable once PossibleNullReferenceException
                return lAvailableContracts.OrderBy(pElt => pElt.Item2).FirstOrDefault();
            }
            return null;
        }

        /// <summary>
        /// This method finds a serialization contract.
        /// </summary>
        /// <param name="pElement">The target element.</param>
        /// <returns>The best matching contract.</returns>
        private Tuple<IXSerializationContract, SupportPriority> FindContract(XElement pElement)
        {   
            List<Tuple<IXSerializationContract, SupportPriority>> lAvailableContracts = new List<Tuple<IXSerializationContract, SupportPriority>>();
// ReSharper disable once LoopCanBeConvertedToQuery
            foreach (IXSerializationContract lContract in this.mContracts)
            {
                SupportPriority lPriority = lContract.CanManage(pElement, this);
                if (lPriority != null && lPriority != SupportPriority.CANNOT_SUPPORT)
                {
                    lAvailableContracts.Add(new Tuple<IXSerializationContract, SupportPriority>(lContract, lPriority));
                }   
            }
            
            if (lAvailableContracts.Any())
            {
// ReSharper disable once PossibleNullReferenceException
                return lAvailableContracts.OrderBy(pElt =>pElt.Item2).FirstOrDefault();
            }
            return null;
        }

        #endregion // Methods.
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using XSerialization.Defaults;

namespace XSerialization
{
    /// <summary>
    /// This class defines a serialization contract based on type.
    /// </summary>
    public abstract class AVersionTypeSerializationContract<TType> : ATypeSerializationContract<TType>, IXVersionable
    {
        #region Properties

        /// <summary>
        /// Gets the object applicable minimum version.
        /// </summary>
        public abstract int MinVersion
        {
            get;
        }

        /// <summary>
        /// Gets the object applicable maximum version.
        /// </summary>
        public virtual int MaxVersion
        {
            get
            {
                return int.MaxValue;
            }
        }

        /// <summary>
        /// Gets the flag indicating if the contract can handle the last version of the objects to serialize.
        /// </summary>
        private bool CanManageLastVersion
        {
            get
            {
                return (this.MaxVersion == int.MaxValue);
            }
        }

        #endregion // Properties.

        #region Methods

        /// <summary>
        /// This method determines whether this instance can manage the specified object.
        /// </summary>
        /// <param name="pType">The type to test.</param>
        /// <param name="pSerializationContext">The serialization context.</param>
        /// <returns>The depth of inheritance or -1 if the contract cannot support.</returns>
        public override SupportPriority CanManage(Type pType, IXSerializationContext pSerializationContext)
        {
            bool lVersionFound;
            int lXVersion = pSerializationContext.GetSerializationParameter<int>("Version", out lVersionFound);
            if  (lVersionFound == false)
            {
                // When no version is specified, only allowing the contract handling the last version.
                if (this.CanManageLastVersion == false)
                {
                    return SupportPriority.CANNOT_SUPPORT;
                }
            }
            else if (lXVersion < this.MinVersion || lXVersion > this.MaxVersion)
            {
                // Version out of range.
                return SupportPriority.CANNOT_SUPPORT;
            }

            return base.CanManage(pType, pSerializationContext);
        }

        /// <summary>
        /// This method reads the specified object to initialize.
        /// </summary>
        /// <param name="pObjectToInitialize">The object to initialize.</param>
        /// <param name="pElement">The element.</param>
        /// <param name="pSerializationContext">The serialization context.</param>
        /// <returns>The initialized object</returns>
        public override object Read(object pObjectToInitialize, XElement pElement, IXSerializationContext pSerializationContext)
        {
            DefaultObjectSerializationContract lDefaultContract = new DefaultObjectSerializationContract();
            return lDefaultContract.Read(pObjectToInitialize, pElement, pSerializationContext);
        }

        /// <summary>
        /// Writes the specified object.
        /// </summary>
        /// <param name="pObject">The object.</param>
        /// <param name="pParentElement">The parent element.</param>
        /// <param name="pSerializationContext">The serialization context.</param>
        /// <returns></returns>
        public override XElement Write(object pObject, XElement pParentElement, IXSerializationContext pSerializationContext)
        {
            DefaultObjectSerializationContract lDefaultContract = new DefaultObjectSerializationContract();
            return lDefaultContract.Write(pObject, pParentElement, pSerializationContext);
        }

        #endregion // Methods.
    }
}

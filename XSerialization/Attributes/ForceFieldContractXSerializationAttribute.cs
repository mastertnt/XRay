using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XSerialization.Values;

namespace XSerialization.Attributes
{
    /// <summary>
    /// This attribute can be used to stores a field instead of property info on an object.
    /// </summary>
    /// <seealso cref="XSerialization.XSerializationAttribute" />
    public class ForceFieldContractXSerializationAttribute : XSerializationAttribute
    {
        #region Properties

        /// <summary>
        /// Gets the name of the field.
        /// </summary>
        /// <value>
        /// The name of the field.
        /// </value>
        public string FieldName
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the name of the field synchronization method.
        /// </summary>
        /// <value>
        /// The name of the field synchronization method.
        /// </value>
        public string SyncFieldMethod
        {
            get;
            private set;
        }

        #endregion // Properties.

        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ForceFieldContractXSerializationAttribute(Type pContractType, string pFieldName, string pSyncFieldMethod = "")
            : base(pContractType)
        {
            this.FieldName = pFieldName;
            this.SyncFieldMethod = pSyncFieldMethod;
        }

        #endregion // Constructors.
    }
}

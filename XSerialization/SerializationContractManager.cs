using System;
using System.Collections.Generic;
using System.Linq;
using XSerialization.Attributes;
using XSystem;

namespace XSerialization
{
    /// <summary>
    /// This class is the manager of all physical units of the monitor.
    /// </summary>
    /// <!-- NBY -->
    public sealed class SerializationContractManager
    {
        #region Fields

        /// <summary>
        /// The unique instance of the simgleton.
        /// </summary>
        private static readonly SerializationContractManager msInstance = new SerializationContractManager();

        /// <summary>
        /// This field stores all serialization contracts.
        /// </summary>
        private readonly List<IXSerializationContract> mContracts; 

        #endregion // Fields.

        #region Properties

        /// <summary>
        /// Gets the unique instance of the manager.
        /// </summary>
        public static SerializationContractManager Instance
        {
            get
            {
                return SerializationContractManager.msInstance;
            }
        }

        /// <summary>
        /// Gets all the contracts.
        /// </summary>
        public List<IXSerializationContract> Contracts
        {
            get
            {
                return this.mContracts;
            }
        }
 

        #endregion // Properties.

        #region Constructors

        /// <summary>
        /// Explicit static constructor to tell C# compiler not to mark type as beforefieldinit.
        /// </summary>
        static SerializationContractManager()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SerializationContractManager"/> class.
        /// </summary>
        public SerializationContractManager()
        {
            this.mContracts = typeof(IXSerializationContract).GetInheritedTypes().Where(pType => System.Attribute.GetCustomAttributes(pType).FirstOrDefault(pAttribute => pAttribute is HideContractAttribute) == null).Select(Activator.CreateInstance).OfType<IXSerializationContract>().ToList();
        }

        #endregion // Constructors.
    }
}

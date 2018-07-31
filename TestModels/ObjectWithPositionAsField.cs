using System;
using TestModels.TestContracts;
using XSerialization.Attributes;
using XSerialization.Defaults;
using XSerialization.Primitives;

namespace TestModels
{
    /// <summary>
    /// This object is used to test field serialization contracts.
    /// </summary>
    public class ObjectWithPositionAsField
    {
        /// <summary>
        /// This field stores a position.
        /// </summary>
        private Position mPosition = new Position();

        /// <summary>
        /// This field stores an int.
        /// </summary>
        private int mIntValue;

        /// <summary>
        /// This field stores the same type object as field.
        /// </summary>
        private ObjectWithPositionAsField mSubObjectWithSameContract;

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        //[WriteFieldXSerialization("mPosition")] 
        public Position Position
        {
            get
            {
                return this.mPosition;
            }
            set
            {
                this.mPosition = value;
            }
        }

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        [ForceFieldContractXSerialization(typeof(FieldSerializationContract2), "mIntValue", "SyncMethod")]
        public int IntProperty
        {
            get
            {
                return this.mIntValue;
            }
            set
            {
                this.mIntValue = value;
            }
        }

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        [ForceFieldContractXSerialization(typeof(FieldSerializationContract1), "mSubObjectWithSameContract", "SyncMethod")]
        public ObjectWithPositionAsField SubObjectWithSameContract
        {
            get
            {
                return this.mSubObjectWithSameContract;
            }
            set
            {
                this.mSubObjectWithSameContract = value;
            }
        }

        /// <summary>
        /// Mies the method.
        /// </summary>
        private void SyncMethod()
        {
            Console.WriteLine("MyMethod");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectWithPositionAsField"/> class.
        /// </summary>
        public ObjectWithPositionAsField()
        {
        }
    }
}

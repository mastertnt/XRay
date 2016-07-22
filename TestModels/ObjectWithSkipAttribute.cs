using XSerialization.Attributes;

namespace TestModels
{
    /// <summary>
    /// 
    /// </summary>
    public enum PipoState1
    {
        /// <summary>
        /// On value.
        /// </summary>
        On,
        /// <summary>
        /// Off value.
        /// </summary>
        Off,
        /// <summary>
        /// Broken value.
        /// </summary>
        Broken,
    }

    /// <summary>
    /// Class with a skip attribute.
    /// </summary>
    /// <seealso cref="TestModels.ITestable" />
    public class ObjectWithSkipAttribute : ITestable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectWithSkipAttribute"/> class.
        /// </summary>
        public ObjectWithSkipAttribute()
        {
        }

        /// <summary>
        /// Gets or sets the enum value.
        /// </summary>
        /// <value>
        /// The enum value.
        /// </value>
        public PipoState EnumValue
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the double value.
        /// </summary>
        /// <value>
        /// The double value.
        /// </value>
         [SkipXSerialization]
        public double DoubleValue
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether [boolean value].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [boolean value]; otherwise, <c>false</c>.
        /// </value>
        public bool BooleanValue
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes the test0.
        /// </summary>
        /// <returns>a created and initialized object.</returns>
        public static object InitializeTest0()
        {
            ObjectWithSkipAttribute lResult = new ObjectWithSkipAttribute();
            return lResult;
        }

        /// <summary>
        /// Initializes the test1.
        /// </summary>
        /// <returns>a created and initialized object.</returns>
        public static object InitializeTest1()
        {
            ObjectWithSkipAttribute lResult = new ObjectWithSkipAttribute { EnumValue = PipoState.Broken, DoubleValue = 77.0, BooleanValue = true };
            return lResult;
        }
    }
}

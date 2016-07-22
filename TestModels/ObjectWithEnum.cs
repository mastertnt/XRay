namespace TestModels
{
    /// <summary>
    /// A class with a list of an enum.
    /// </summary>
    /// <seealso cref="TestModels.ITestable" />
    public class ObjectWithEnum : ITestable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectWithEnum"/> class.
        /// </summary>
        public ObjectWithEnum()
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
        /// Initializes the test0.
        /// </summary>
        /// <returns>a created and initialized object.</returns>
        public static object InitializeTest0()
        {
            ObjectWithEnum lResult = new ObjectWithEnum();
            lResult.EnumValue = PipoState.Broken;
            return lResult;
        }
    }
}

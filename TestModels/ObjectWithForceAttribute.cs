namespace TestModels
{
    /// <summary>
    /// A class with a force attribute.
    /// </summary>
    /// <seealso cref="TestModels.ITestable" />
    class ObjectWithForceAttribute : ITestable
    {
         /// <summary>
        /// Initializes a new instance of the <see cref="ObjectWithForceAttribute"/> class.
        /// </summary>
        public ObjectWithForceAttribute()
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

        public SimpleObject SimpleObject
        {
            get; set; 
        }

        /// <summary>
        /// Initializes the test0.
        /// </summary>
        /// <returns>a created and initialized object.</returns>
        public static object InitializeTest0()
        {
            ObjectWithForceAttribute lResult = new ObjectWithForceAttribute { EnumValue = PipoState.Broken, DoubleValue = 77.0, BooleanValue = true };
            lResult.SimpleObject = new SimpleObject();
            return lResult;
        }
    }
}

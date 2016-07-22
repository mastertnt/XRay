namespace TestModels
{
    /// <summary>
    /// Class representing a simple object.
    /// </summary>
    /// <seealso cref="TestModels.ITestable" />
    public class SimpleObject : ITestable
    {
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

        /// <summary>
        /// Initializes the test0.
        /// </summary>
        /// <returns>a created and initialized object.</returns>
        public static object InitializeTest0()
        {
            return new SimpleObject();
        }

        /// <summary>
        /// Initializes the test1.
        /// </summary>
        /// <returns>a created and initialized object.</returns>
        public static object InitializeTest1()
        {
            return new SimpleObject { DoubleValue = 73.25, BooleanValue = true };
        }
    }
}

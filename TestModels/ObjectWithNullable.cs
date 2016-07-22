using System;

namespace TestModels
{
    /// <summary>
    /// A class with a nullable.
    /// </summary>
    /// <seealso cref="TestModels.ITestable" />
    public class ObjectWithNullable : ITestable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectWithNullable"/> class.
        /// </summary>
        public ObjectWithNullable()
        {
            
        }

        /// <summary>
        /// Gets or sets the nullable double.
        /// </summary>
        /// <value>
        /// The nullable double.
        /// </value>
        public Double? NullableDouble
        {
            get; set; 
        }

        /// <summary>
        /// Initializes the test0.
        /// </summary>
        /// <returns>a created and initialized object.</returns>
        public static object InitializeTest0()
        {
            ObjectWithNullable lResult = new ObjectWithNullable();
            return lResult;
        }

        /// <summary>
        /// Initializes the test0.
        /// </summary>
        /// <returns>a created and initialized object.</returns>
        public static object InitializeTest1()
        {
            ObjectWithNullable lResult = new ObjectWithNullable();
            lResult.NullableDouble = new double();
            return lResult;
        }
    }
}

using System;

namespace TestModels
{
    /// <summary>
    /// Class with a tuple.
    /// </summary>
    /// <seealso cref="TestModels.ITestable" />
    public class ObjectWithTuple : ITestable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectWithTuple"/> class.
        /// </summary>
        public ObjectWithTuple()
        {
            
        }

        /// <summary>
        /// Gets or sets the tuple.
        /// </summary>
        /// <value>
        /// The tuple.
        /// </value>
        public Tuple<int, double, int> TupleProperty
        {
            get; set;
        }

        /// <summary>
        /// Initializes the test0.
        /// </summary>
        /// <returns>a created and initialized object.</returns>
        public static object InitializeTest0()
        {
            ObjectWithTuple lResult = new ObjectWithTuple();
            return lResult;
        }

        /// <summary>
        /// Initializes the test1.
        /// </summary>
        /// <returns>a created and initialized object.</returns>
        public static object InitializeTest1()
        {
            ObjectWithTuple lResult = new ObjectWithTuple();
            lResult.TupleProperty = new Tuple<int, double, int>(1, 72.2, 43);
            return lResult;
        }
    }
}

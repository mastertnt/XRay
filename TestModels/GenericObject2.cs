using System.Collections.Generic;

namespace TestModels
{
    /// <summary>
    /// A generic object with two parameters.
    /// </summary>
    /// <typeparam name="TFirst">The first generic type.</typeparam>
    /// <typeparam name="TSecond">The second generic type.</typeparam>
    /// <seealso cref="TestModels.ITestable" />
    public class GenericObject2<TFirst, TSecond> : ITestable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GenericObject2{TFirst, TSecond}"/> class.
        /// </summary>
        public GenericObject2()
        {
        
        }

        /// <summary>
        /// Gets or sets the t value.
        /// </summary>
        /// <value>
        /// The t value.
        /// </value>
        public TFirst FirstValue
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the u value.
        /// </summary>
        /// <value>
        /// The u value.
        /// </value>
        public TSecond SecondValue
        {
            get; set;
        }

        /// <summary>
        /// Initializes the test0.
        /// </summary>
        /// <returns>a created and initialized object.</returns>
        public static object InitializeTest0()
        {
            return new GenericObject2<int, int> {FirstValue = 42, SecondValue = 33};
        }

        /// <summary>
        /// Initializes the test1.
        /// </summary>
        /// <returns>a created and initialized object.</returns>
        public static object InitializeTest1()
        {
            return new GenericObject2<int, KeyValuePair<int, int>> { FirstValue = 42, SecondValue = new KeyValuePair<int, int>(10, 11) };
        }


        /// <summary>
        /// Initializes the test2.
        /// </summary>
        /// <returns>a created and initialized object.</returns>
        public static object InitializeTest2()
        {
            return new GenericObject2<int, SimpleObject> { FirstValue = 42, SecondValue = null };
        }

        /// <summary>
        /// Initializes the test3.
        /// </summary>
        /// <returns>a created and initialized object.</returns>
        public static object InitializeTest3()
        {
            return new GenericObject2<int, SimpleObject> { FirstValue = 42, SecondValue = new SimpleObject() { BooleanValue = true, DoubleValue = 31} };
        }
    }
}

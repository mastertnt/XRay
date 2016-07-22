using System;

namespace TestModels
{
    /// <summary>
    /// A generic object with two parameters.
    /// </summary>
    /// <typeparam name="TFirst">The first generic type.</typeparam>
    /// <typeparam name="TSecond">The second generic type.</typeparam>
    /// <typeparam name="TThird">The third generic type.</typeparam>
    /// <seealso cref="TestModels.ITestable" />
    public class GenericObject3<TFirst, TSecond, TThird> : ITestable
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericObject3{TFirst, TSecond, TThird}"/> class.
        /// </summary>
        public GenericObject3()
        {
        
        }

        /// <summary>
        /// Gets or sets the tuple.
        /// </summary>
        /// <value>
        /// The tuple.
        /// </value>
        public Tuple<TFirst, TSecond, TThird> Tuple
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
            GenericObject3<int, double, int> lResult = new GenericObject3<int, double, int>();
            lResult.Tuple = new Tuple<int, double, int>(1, 72.2, 43);
            return lResult;
        }
    }
}

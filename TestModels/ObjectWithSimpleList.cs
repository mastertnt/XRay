using System.Collections.Generic;

namespace TestModels
{
    /// <summary>
    /// Class with a simple list.
    /// </summary>
    /// <seealso cref="TestModels.ITestable" />
    public class ObjectWithSimpleList : ITestable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectWithSimpleList"/> class.
        /// </summary>
        public ObjectWithSimpleList()
        {
            
        }


        /// <summary>
        /// Gets or sets the int values.
        /// </summary>
        /// <value>
        /// The int values.
        /// </value>
        public List<int> IntValues
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
            ObjectWithSimpleList lResult = new ObjectWithSimpleList();
            lResult.IntValues = new List<int>();
           return lResult;
        }

        /// <summary>
        /// Initializes the test1.
        /// </summary>
        /// <returns>a created and initialized object.</returns>
        public static object InitializeTest1()
        {
            ObjectWithSimpleList lResult = new ObjectWithSimpleList();
            lResult.IntValues = new List<int>();
            lResult.IntValues.Add(10);
            lResult.IntValues.Add(20);
            lResult.IntValues.Add(30);
            lResult.IntValues.Add(40);
            return lResult;
        }

        /// <summary>
        /// Initializes the test2.
        /// </summary>
        /// <returns>a created and initialized object.</returns>
        public static object InitializeTest2()
        {
            ObjectWithSimpleList lResult = new ObjectWithSimpleList();
            return lResult;
        }
    }
}

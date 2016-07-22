using System.Collections.Generic;

namespace TestModels
{
    /// <summary>
    /// Class with a sorted dictionnary.
    /// </summary>
    /// <seealso cref="TestModels.ITestable" />
    public class ObjectWithSortedDictionary : ITestable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectWithSortedDictionary"/> class.
        /// </summary>
        public ObjectWithSortedDictionary()
        {
            
        }


        /// <summary>
        /// Gets or sets the int values.
        /// </summary>
        /// <value>
        /// The int values.
        /// </value>
        public SortedDictionary<int, string> IntValues
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
            ObjectWithSortedDictionary lResult = new ObjectWithSortedDictionary();
            return lResult;
        }

        /// <summary>
        /// Initializes the test1.
        /// </summary>
        /// <returns>a created and initialized object.</returns>
        public static object InitializeTest1()
        {
            ObjectWithSortedDictionary lResult = new ObjectWithSortedDictionary();
            lResult.IntValues = new SortedDictionary<int, string>();
            return lResult;
        }

        /// <summary>
        /// Initializes the test2.
        /// </summary>
        /// <returns>a created and initialized object.</returns>
        public static object InitializeTest2()
        {
            ObjectWithSortedDictionary lResult = new ObjectWithSortedDictionary();
            lResult.IntValues = new SortedDictionary<int, string>();
            lResult.IntValues.Add(0, "0");
            lResult.IntValues.Add(12, "120");
            lResult.IntValues.Add(24, "240");
            lResult.IntValues.Add(48, "480");
            return lResult;
        }
    }
}

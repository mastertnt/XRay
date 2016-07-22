using System.Collections.Generic;

namespace TestModels
{
    /// <summary>
    /// Class with a string list.
    /// </summary>
    /// <seealso cref="TestModels.ITestable" />
    public class ObjectWithStringList : ITestable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectWithStringList"/> class.
        /// </summary>
        public ObjectWithStringList()
        {
        }

        /// <summary>
        /// Gets or sets the string values.
        /// </summary>
        /// <value>
        /// The string values.
        /// </value>
        public List<string> StringValues
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
            ObjectWithStringList lResult = new ObjectWithStringList();
            return lResult;
        }

        /// <summary>
        /// Initializes the test1.
        /// </summary>
        /// <returns>a created and initialized object.</returns>
        public static object InitializeTest1()
        {
            ObjectWithStringList lResult = new ObjectWithStringList();
            lResult.StringValues = new List<string>();
            lResult.StringValues.Add("10");
            lResult.StringValues.Add("20");
            lResult.StringValues.Add("30");
            lResult.StringValues.Add("40");
            return lResult;
        }

        /// <summary>
        /// Initializes the test2.
        /// </summary>
        /// <returns>a created and initialized object.</returns>
        public static object InitializeTest2()
        {
            ObjectWithStringList lResult = new ObjectWithStringList();
            lResult.StringValues = new List<string>();
            lResult.StringValues.Add("10");
            lResult.StringValues.Add("20");
            lResult.StringValues.Add("30");
            lResult.StringValues.Add(null);
            lResult.StringValues.Add("40");
            return lResult;
        }
    }
}

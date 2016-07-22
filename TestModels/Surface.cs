using System.Collections.Generic;

namespace TestModels
{
    /// <summary>
    /// Class representing a surface.
    /// </summary>
    /// <seealso cref="TestModels.ITestable" />
    public class Surface : ITestable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Surface"/> class.
        /// </summary>
        public Surface()
        {
        }
        
        /// <summary>
        /// Gets or sets the big test.
        /// </summary>
        /// <value>
        /// The big test.
        /// </value>
        public List<KeyValuePair<double, List<KeyValuePair<double, double>>>> BigTest
        {
            get; set; 
        }

        /// <summary>
        /// Initializes the test0.
        /// </summary>
        /// <returns>a created and initialized object.</returns>
        public static object InitializeTest0()
        {
            Surface lResult = new Surface();

            return lResult;
        }

        /// <summary>
        /// Initializes the test1.
        /// </summary>
        /// <returns>a created and initialized object.</returns>
        public static object InitializeTest1()
        {
            Surface lResult = new Surface();

            lResult.BigTest = new List<KeyValuePair<double, List<KeyValuePair<double, double>>>>();

            List<KeyValuePair<double, double>> lCurve0 = new List<KeyValuePair<double, double>>();
            lCurve0.Add(new KeyValuePair<double, double>(1, 0));
            lCurve0.Add(new KeyValuePair<double, double>(2, 0));
            lCurve0.Add(new KeyValuePair<double, double>(3, 0));
            lCurve0.Add(new KeyValuePair<double, double>(4, 0));
            lResult.BigTest.Add(new KeyValuePair<double, List<KeyValuePair<double, double>>>(0, lCurve0));

            List<KeyValuePair<double, double>> lCurve1 = new List<KeyValuePair<double, double>>();
            lCurve1.Add(new KeyValuePair<double, double>(11, 1));
            lCurve1.Add(new KeyValuePair<double, double>(12, 1));
            lCurve1.Add(new KeyValuePair<double, double>(13, 1));
            lCurve1.Add(new KeyValuePair<double, double>(14, 1));
            lResult.BigTest.Add(new KeyValuePair<double, List<KeyValuePair<double, double>>>(1, lCurve1));

            return lResult;
        }
    }
}

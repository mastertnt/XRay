using System.Collections.Generic;

namespace TestModels
{
    /// <summary>
    /// Class with a simple dictionnary.
    /// </summary>
    /// <seealso cref="TestModels.ITestable" />
    public class ObjectWithSimpleDictionary : ITestable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectWithSimpleDictionary"/> class.
        /// </summary>
        public ObjectWithSimpleDictionary()
        {
            
        }
        
        /// <summary>
        /// Gets or sets the dictionary values.
        /// </summary>
        /// <value>
        /// The dictionary values.
        /// </value>
        public Dictionary<int, int> DictValues
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
            ObjectWithSimpleDictionary lResult = new ObjectWithSimpleDictionary();
            lResult.DictValues = new Dictionary<int, int>();
            lResult.DictValues.Add(0, 0);
            lResult.DictValues.Add(12, 120);
            lResult.DictValues.Add(24, 240);
            lResult.DictValues.Add(48, 480);
            return lResult;
        }
    }
}

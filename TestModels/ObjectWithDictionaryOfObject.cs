using System.Collections.Generic;

namespace TestModels
{
    /// <summary>
    /// A class with a dictionnary.
    /// </summary>
    /// <seealso cref="TestModels.ITestable" />
    public class ObjectWithDictionaryOfObject : ITestable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectWithDictionaryOfObject"/> class.
        /// </summary>
        public ObjectWithDictionaryOfObject()
        {
            
        }

        /// <summary>
        /// Gets or sets the object values.
        /// </summary>
        /// <value>
        /// The object values.
        /// </value>
        public SortedDictionary<int, SimpleObject> ObjectValues
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
            ObjectWithDictionaryOfObject lResult = new ObjectWithDictionaryOfObject();
            return lResult;
        }

        /// <summary>
        /// Initializes the test1.
        /// </summary>
        /// <returns>a created and initialized object.</returns>
        public static object InitializeTest1()
        {
            ObjectWithDictionaryOfObject lResult = new ObjectWithDictionaryOfObject();
            lResult.ObjectValues = new SortedDictionary<int, SimpleObject>();
            SimpleObject lRef = new SimpleObject {DoubleValue = 0.0};
            lResult.ObjectValues.Add(0, lRef);
            lResult.ObjectValues.Add(12, new SimpleObject { DoubleValue = 12.0 });
            lResult.ObjectValues.Add(24, lRef);
            lResult.ObjectValues.Add(48, new SimpleObject { DoubleValue = 48.0 });
            return lResult;
        }

        /// <summary>
        /// Initializes the test0.
        /// </summary>
        /// <returns>a created and initialized object.</returns>
        public static object InitializeTest2()
        {
            ObjectWithDictionaryOfObject lResult = new ObjectWithDictionaryOfObject();
            lResult.ObjectValues = new SortedDictionary<int, SimpleObject>();
            lResult.ObjectValues.Add(0, new SimpleObject { DoubleValue = 0.0 });
            lResult.ObjectValues.Add(12, new SimpleObject { DoubleValue = 12.0 });
            lResult.ObjectValues.Add(24, new SimpleObject { DoubleValue = 24.0 });
            lResult.ObjectValues.Add(48, new SimpleObject { DoubleValue = 48.0 });
            return lResult;
        }
    }
}

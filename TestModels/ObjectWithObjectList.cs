using System.Collections.Generic;

namespace TestModels
{
    /// <summary>
    /// A class with an object list.
    /// </summary>
    /// <seealso cref="TestModels.ITestable" />
    public class ObjectWithObjectList : ITestable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectWithObjectList"/> class.
        /// </summary>
        public ObjectWithObjectList()
        {
            
        }

        /// <summary>
        /// Initializes the test0.
        /// </summary>
        /// <returns>a created and initialized object.</returns>
        public static object InitializeTest0()
        {
            ObjectWithObjectList lResult = new ObjectWithObjectList();
            lResult.SimpleObjectsValues = new List<SimpleObject>();

            lResult.SimpleObjectsValues.Add(new SimpleObject { BooleanValue = false, DoubleValue = 42.1 });
            lResult.SimpleObjectsValues.Add(new InheritedObject
            {
                DoubleValue = 31.125,
                BooleanValue = true,
                Message = "LX"
            });
            return lResult;
        }

        /// <summary>
        /// Initializes the test1.
        /// </summary>
        /// <returns>a created and initialized object.</returns>
        public static object InitializeTest1()
        {
            ObjectWithObjectList lResult = new ObjectWithObjectList();
            lResult.SimpleObjectsValues = new List<SimpleObject>();

            lResult.SimpleObjectsValues.Add(new SimpleObject { BooleanValue = false, DoubleValue = 42.1 });
            lResult.SimpleObjectsValues.Add(null);
            return lResult;
        }

        /// <summary>
        /// Gets or sets the simple objects values.
        /// </summary>
        /// <value>
        /// The simple objects values.
        /// </value>
        public List<SimpleObject> SimpleObjectsValues
        {
            get;
            set;
        }
    }
}

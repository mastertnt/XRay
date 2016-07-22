using System.Collections.Generic;

namespace TestModels
{
    /// <summary>
    /// A class with a list of double.
    /// </summary>
    /// <seealso cref="TestModels.ITestable" />
    public class ObjectWithDoubleList : ITestable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectWithDoubleList"/> class.
        /// </summary>
        public ObjectWithDoubleList()
        {
            
        }

        /// <summary>
        /// Gets or sets the int2 values.
        /// </summary>
        /// <value>
        /// The int2 values.
        /// </value>
        public List<List<Position>> Int2Values
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
            ObjectWithDoubleList lResult = new ObjectWithDoubleList();
            lResult.Int2Values = new List<List<Position>>();
            List<Position> lPosition = new List<Position>();
            lPosition.Add(new Position { X = 77, Y = 03 });
            lPosition.Add(null);
            lPosition.Add(new Position { X = 77, Y = 04 });
            lResult.Int2Values.Add(lPosition);
            return lResult;
        }

        /// <summary>
        /// Initializes the test1.
        /// </summary>
        /// <returns>a created and initialized object.</returns>
        public static object InitializeTest1()
        {
            ObjectWithDoubleList lResult = new ObjectWithDoubleList();
            lResult.Int2Values = new List<List<Position>>();
            List<Position> lPosition = new List<Position>();
            lPosition.Add(new Position { X = 77, Y = 03 });
            lPosition.Add(new Position { X = 77, Y = 12 });
            lPosition.Add(new Position { X = 77, Y = 04 });
            lResult.Int2Values.Add(lPosition);
            return lResult;
        }
    }
}

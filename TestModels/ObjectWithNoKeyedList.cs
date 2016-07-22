using System.Collections.Generic;

namespace TestModels
{
    /// <summary>
    /// A class with a keyed list.
    /// </summary>
    /// <seealso cref="TestModels.ITestable" />
    public class ObjectWithNoKeyedList : ITestable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectWithNoKeyedList"/> class.
        /// </summary>
        public ObjectWithNoKeyedList()
        {
            this.Positions = new List<Position>();
        }

        /// <summary>
        /// Gets or sets the positions.
        /// </summary>
        /// <value>
        /// The positions.
        /// </value>
        public List<Position> Positions
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
            ObjectWithNoKeyedList lResult = new ObjectWithNoKeyedList();
            lResult.Positions = new List<Position>();
            lResult.Positions.Add(new Position { X = 12, Y = 44 });
            lResult.Positions.Add(new Position { X = 77, Y = 44 });
            return lResult;
        }

        /// <summary>
        /// Initializes the test1.
        /// </summary>
        /// <returns>a created and initialized object.</returns>
        public static object InitializeTest1()
        {
            ObjectWithNoKeyedList lResult = new ObjectWithNoKeyedList();
            lResult.Positions = new List<Position>();
            return lResult;
        }
    }
}

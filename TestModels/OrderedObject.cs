using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XSerialization.Attributes;

namespace TestModels
{
    /// <summary>
    /// This class exposes the orderXAttribute.
    /// </summary>
    /// <seealso cref="TestModels.ITestable" />
    public class OrderedObject : ITestable
    {
        [OrderXSerialization(4)]
        public int Test4
        {
            get;
            set;
        }

        [OrderXSerialization(2)]
        public int Test2
        {
            get;
            set;
        }

        [OrderXSerialization(1)]
        public int Test1
        {
            get;
            set;
        }
        public int Test5
        {
            get;
            set;
        }

        [OrderXSerialization(3)]
        public int Test3
        {
            get;
            set;
        }


        [OrderXSerialization(0)]
        public int Test0
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
            OrderedObject lResult = new OrderedObject
            {
                Test0 = 0,
                Test1 = 1,
                Test2 = 2,
                Test3 = 3,
                Test4 = 4,
                Test5 = 5
            };
            return lResult;
        }
    }
}

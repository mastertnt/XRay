using System;

namespace TestModels
{
    /// <summary>
    /// Class with another object aggregated.
    /// </summary>
    /// <seealso cref="TestModels.ITestable" />
    public class ObjectWithAggregate : ITestable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectWithAggregate"/> class.
        /// </summary>
        public ObjectWithAggregate()
        {
            
        }

        /// <summary>
        /// An aggregated object.
        /// </summary>
        public GenericObject3<int, double, SimpleObject> GenericObject3
        {
            get;
            set;
        }

        /// <summary>
        /// An aggregated object.
        /// </summary>
        public SimpleObject AggregatedSimple
        {
            get;
            set;
        }

        /// <summary>
        /// An aggregated object.
        /// </summary>
        public SimpleObject AggregatedSimple2
        {
            get;
            set;
        }

        /// <summary>
        /// An aggregated object.
        /// </summary>
        public ObjectWithEnum AggregatedEnum
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
            ObjectWithAggregate lResult = new ObjectWithAggregate();
            return lResult;
        }
        
        /// <summary>
        /// Initializes the test1.
        /// </summary>
        /// <returns>a created and initialized object.</returns>
        public static object InitializeTest1()
        {
            ObjectWithAggregate lResult = new ObjectWithAggregate();
            lResult.AggregatedSimple = new SimpleObject();
            lResult.AggregatedEnum = new ObjectWithEnum();
            lResult.AggregatedSimple2 = new SimpleObject();
            lResult.GenericObject3 =  new GenericObject3<int, double, SimpleObject>();
            lResult.GenericObject3.Tuple = new Tuple<int, double, SimpleObject>(12, 44.1, lResult.AggregatedSimple);
            return lResult;
        }

        /// <summary>
        /// Initializes the test2.
        /// </summary>
        /// <returns>a created and initialized object.</returns>
        public static object InitializeTest2()
        {
            ObjectWithAggregate lResult = new ObjectWithAggregate();
            lResult.AggregatedSimple = new SimpleObject();
            lResult.AggregatedEnum = new ObjectWithEnum();
            lResult.AggregatedSimple2 = null;
            return lResult;
        }

        /// <summary>
        /// Initializes the test3.
        /// </summary>
        /// <returns>a created and initialized object.</returns>
        public static object InitializeTest3()
        {
            ObjectWithAggregate lResult = new ObjectWithAggregate();
            lResult.AggregatedSimple = new SimpleObject();
            lResult.AggregatedEnum = new ObjectWithEnum();
            lResult.AggregatedSimple2 = lResult.AggregatedSimple;
            return lResult;
        }

    }
}

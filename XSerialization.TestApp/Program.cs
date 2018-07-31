using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using TestModels;

namespace XSerialization.TestApp
{
    /// <summary>
    /// The status of the test.
    /// </summary>
    enum TestStatus
    {
        Sucess,
        ComparisonFailed,
        ExceptionOccured,
    }

    /// <summary>
    /// This class stores some performance information. 
    /// </summary>
    class Performance
    {
        /// <summary>
        /// Gets or sets the minimum.
        /// </summary>
        /// <value>
        /// The minimum.
        /// </value>
        public TimeSpan Min
        {
            get
            {
                return this.AllRuns.Min();
            }
        }

        /// <summary>
        /// Gets or sets the maximum.
        /// </summary>
        /// <value>
        /// The maximum.
        /// </value>
        public TimeSpan Max
        {
            get
            {
                return this.AllRuns.Max();
            }
        }

        /// <summary>
        /// Gets or sets the mean.
        /// </summary>
        /// <value>
        /// The mean.
        /// </value>
        public TimeSpan MeanTime
        {
            get
            {
                double lTotalMilliseconds = this.AllRuns.Average(pElt => pElt.TotalMilliseconds);
                TimeSpan lAverage = new TimeSpan(0, 0, 0, (int)lTotalMilliseconds);
                return lAverage;
            }
        }

        /// <summary>
        /// Gets or sets all runs.
        /// </summary>
        /// <value>
        /// All runs.
        /// </value>
        public List<TimeSpan> AllRuns
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Performance"/> class.
        /// </summary>
        public Performance()
        {
            this.AllRuns =  new List<TimeSpan>();
        }
    }

    
    /// <summary>
    /// This structure stores the result of the test.
    /// </summary>
    class TestResult
    {
        /// <summary>
        /// Gets or sets the serialization performance infos.
        /// </summary>
        /// <value>
        /// The runs.
        /// </value>
        public Performance SerializationPerformanceInfos
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the deserialization performance infos.
        /// </summary>
        /// <value>
        /// The runs.
        /// </value>
        public Performance DeserializationPerformanceInfos
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the init performance infos.
        /// </summary>
        /// <value>
        /// The runs.
        /// </value>
        public Performance InitPerformanceInfos
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of test.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is ok.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is ok; otherwise, <c>false</c>.
        /// </value>
        public TestStatus Status
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the exception in case of failed.
        /// </summary>
        /// <value>
        /// The exception.
        /// </value>
        public Exception Exception
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the first filename.
        /// </summary>
        /// <value>
        /// The first filename.
        /// </value>
        public string FirstFilename
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the second filename.
        /// </summary>
        /// <value>
        /// The first filename.
        /// </value>
        public string SecondFilename
        {
            get;
            set;
        }
    }

    class Program
    {
        /// <summary>
        /// The output path
        /// </summary>
        private static string msOutputPath;

        /// <summary>
        /// Tests the serialization/deserialization of the given object.
        /// </summary>
        /// <param name="pTestName">Name of the test.</param>
        /// <param name="pFirstFilename">The first filename.</param>
        /// <param name="pSecondFilename">The second filename.</param>
        /// <param name="pObject">The object to test.</param>
        /// <returns></returns>
        static TestResult DoTest(string pTestName, string pFirstFilename, string pSecondFilename, object pObject)
        {
            TestResult lResult = new TestResult() { Name = pTestName, DeserializationPerformanceInfos = new Performance(), SerializationPerformanceInfos = new Performance(), InitPerformanceInfos = new Performance() };
            try
            {
                lResult.FirstFilename = pFirstFilename;
                lResult.SecondFilename = pSecondFilename;
                Stopwatch lSerializationTimer = new Stopwatch();
                Stopwatch lDeserializationTimer = new Stopwatch();
                Stopwatch lInitTimer = new Stopwatch();
                

                // First, serialize the object.
                lSerializationTimer.Restart();
                lInitTimer.Restart();
                XSerializer lFirstWriteSerializer = new XSerializer();
                lInitTimer.Stop();
                lResult.InitPerformanceInfos.AllRuns.Add(lInitTimer.Elapsed);
                XElement lComputedElement = lFirstWriteSerializer.Serialize(pObject);
                lComputedElement.Save(pFirstFilename);
                lSerializationTimer.Stop();
                lResult.SerializationPerformanceInfos.AllRuns.Add(lSerializationTimer.Elapsed);

                // Read the serialized object.
                lDeserializationTimer.Restart();
                lInitTimer.Restart();
                XSerializer lReadSerializer = new XSerializer();
                lInitTimer.Stop();
                lResult.InitPerformanceInfos.AllRuns.Add(lInitTimer.Elapsed);
                object lReadObject = lReadSerializer.Deserialize(pFirstFilename);
                lDeserializationTimer.Stop();
                lResult.DeserializationPerformanceInfos.AllRuns.Add(lDeserializationTimer.Elapsed);

                lSerializationTimer.Restart();
                lInitTimer.Restart();
                XSerializer lSecondWriteSerializer = new XSerializer();
                lInitTimer.Stop();
                lResult.InitPerformanceInfos.AllRuns.Add(lInitTimer.Elapsed);
                XElement lComputedElement0 = lSecondWriteSerializer.Serialize(lReadObject);
                lComputedElement0.Save(pSecondFilename);
                lSerializationTimer.Stop();
                lResult.SerializationPerformanceInfos.AllRuns.Add(lSerializationTimer.Elapsed);

                // Now, compare the text in each file.
                string lFirstContent = File.ReadAllText(pFirstFilename);
                string lSecondContent = File.ReadAllText(pSecondFilename);
                if (lFirstContent != lSecondContent)
                {
                    lResult.Status = TestStatus.ComparisonFailed;
                }
                else
                {
                    lResult.Status = TestStatus.Sucess;
                }
            }
            catch (Exception lEx)
            {
                lResult.Status = TestStatus.ExceptionOccured;
                lResult.Exception = lEx;
            }
            return lResult;

        }

        /// <summary>
        /// Main entry point of the application.
        /// </summary>
        /// <param name="pArguments">The arguments.</param>
        static void Main(string[] pArguments)
        {
            msOutputPath = Path.GetTempPath();
            if (pArguments.Length == 1)
            {
                if (Directory.Exists(pArguments[1]))
                {
                    // Store the arguments.
                    msOutputPath = pArguments[1];
                }
            }

            Console.WriteLine("XSerialization.TestApp [path]");
            Console.WriteLine("If the path is not specified, the program will used Path.GetTempPath()");

            //ObjectWithPositionAsField lObjectToSave = new ObjectWithPositionAsField
            //{
            //    Position =
            //    {
            //        X = 42,
            //        Y = 43
            //    },
            //    IntProperty = 663
            //};

            string lFirstPath = msOutputPath + Guid.NewGuid() + ".xml";
            string lSecondPath = msOutputPath + Guid.NewGuid() + ".xml";

            //TestResult lResult = DoTest("Test for serialization of field instead of property", lFirstPath, lSecondPath, lObjectToSave);
            //if (lResult.Status != TestStatus.Sucess)
            //{
            //    Console.WriteLine("First file : " + lResult.FirstFilename);
            //    Console.WriteLine("Second file : " + lResult.SecondFilename);
            //}
            //Console.WriteLine("The test " + lResult.Name + " has status : " + lResult.Status);


            //ObjectWithPositionAsField lObjectToSave1 = new ObjectWithPositionAsField
            //{
            //    Position =
            //    {
            //        X = 54,
            //        Y = 67
            //    },
            //    IntProperty = 484,
            //    SubObjectWithSameContract = lObjectToSave
            //};

            object lModel = OrderedObject.InitializeTest0();
            TestResult lResult1 = DoTest("OrderedObject.InitializeTest0", @"k:\first.xml", @"k:\second.xml", lModel);
            if (lResult1.Status != TestStatus.Sucess)
            {
                Console.WriteLine("First file : " + lResult1.FirstFilename);
                Console.WriteLine("Second file : " + lResult1.SecondFilename);
            }
            Console.WriteLine("The test " + lResult1.Name + " has status : " + lResult1.Status);
            Console.WriteLine("The test serialization average time is " + lResult1.SerializationPerformanceInfos.MeanTime.TotalMilliseconds + " ms");
            Console.WriteLine("The test serialization min time is " + lResult1.SerializationPerformanceInfos.Min.TotalMilliseconds + " ms");
            Console.WriteLine("The test serialization max time is " + lResult1.SerializationPerformanceInfos.Max.TotalMilliseconds + " ms");
            Console.WriteLine("The test deserialization average time is " + lResult1.DeserializationPerformanceInfos.MeanTime.TotalMilliseconds + " ms");
            Console.WriteLine("The test deserialization min time is " + lResult1.DeserializationPerformanceInfos.Min.TotalMilliseconds + " ms");
            Console.WriteLine("The test deserialization max time is " + lResult1.DeserializationPerformanceInfos.Max.TotalMilliseconds + " ms");
            Console.WriteLine("The test init average time is " + lResult1.InitPerformanceInfos.MeanTime.TotalMilliseconds + " ms");
            Console.WriteLine("The test init min time is " + lResult1.InitPerformanceInfos.Min.TotalMilliseconds + " ms");
            Console.WriteLine("The test init max time is " + lResult1.InitPerformanceInfos.Max.TotalMilliseconds + " ms");
            Console.WriteLine("The test " + lResult1.Name + " has status : " + lResult1.Status);
        }
    }
}

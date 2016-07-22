using System;
using System.Collections.Generic;

namespace TestModels
{
    /// <summary>
    /// A full test model.
    /// </summary>
    /// <seealso cref="TestModels.ITestable" />
    public class FullModel : ITestable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FullModel"/> class.
        /// </summary>
        public FullModel()
        {
            
        }

        /// <summary>
        /// Gets or sets the simple object.
        /// </summary>
        /// <value>
        /// The simple object.
        /// </value>
        public SimpleObject SimpleObject { get; set; }

        /// <summary>
        /// Gets or sets the key value pair.
        /// </summary>
        /// <value>
        /// The key value pair.
        /// </value>
        public KeyValuePair<double, double?> KeyValuePair { get; set; }

        /// <summary>
        /// Gets or sets the tuple.
        /// </summary>
        /// <value>
        /// The tuple.
        /// </value>
        public Tuple<double, double, double, double, bool> Tuple { get; set; }

        /// <summary>
        /// Gets or sets the double value.
        /// </summary>
        /// <value>
        /// The double value.
        /// </value>
        public double DoubleValue
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether [boolean value].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [boolean value]; otherwise, <c>false</c>.
        /// </value>
        public bool BooleanValue
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the date time.
        /// </summary>
        /// <value>
        /// The date time.
        /// </value>
        public DateTime DateTime
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the enum value.
        /// </summary>
        /// <value>
        /// The enum value.
        /// </value>
        public PipoState EnumValue
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the enum value.
        /// </summary>
        /// <value>
        /// The enum value.
        /// </value>
        public int ReadOnly
        {
            get; protected set; 
        }

        /// <summary>
        /// Initializes the test0.
        /// </summary>
        /// <returns>a created and initialized object.</returns>
        public static object InitializeTest0()
        {
            FullModel lResult = new FullModel();
            lResult.DoubleValue = 73.25;
            lResult.BooleanValue = true;
            lResult.KeyValuePair = new KeyValuePair<double, double?>(44, 22.21);
            lResult.Tuple = new Tuple<double, double, double, double, bool>(12.1, 12.2, 12.3, 12.4, true);
            lResult.SimpleObject = SimpleObject.InitializeTest1() as SimpleObject;
            lResult.DateTime = new DateTime(1977, 03, 15);
            lResult.EnumValue = PipoState.Broken;
            lResult.ReadOnly = 63;
            return lResult;
        }
    }
}

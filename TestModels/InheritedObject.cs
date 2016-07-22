namespace TestModels
{
    /// <summary>
    /// Inherited object.
    /// </summary>
    /// <seealso cref="TestModels.SimpleObject" />
    public class InheritedObject : SimpleObject
    {
        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public string Message
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the message2.
        /// </summary>
        /// <value>
        /// The message2.
        /// </value>
        public string Message2
        {
            get
            {
                return "uiop";
            }
        }

        /// <summary>
        /// Initializes the test0.
        /// </summary>
        /// <returns>a created and initialized object.</returns>
        public new static object InitializeTest0()
        {
            return new InheritedObject();
        }

        /// <summary>
        /// Initializes the test1.
        /// </summary>
        /// <returns>a created and initialized object.</returns>
        public static new object InitializeTest1()
        {
            return new InheritedObject { Message = "Message\"\'/\\à" };
        }
    }
}

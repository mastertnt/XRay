namespace XMetadata
{
    /// <summary>
    /// Definition of the <see cref="Enum"/> class.
    /// </summary>
    public class Enum
    {
        #region Fields

        /// <summary>
        /// Stores the enum value.
        /// </summary>
        private int mValue;

        /// <summary>
        /// Stores the enum value name.
        /// </summary>
        private string mName;

        #endregion // Fields.

        #region Properties

        /// <summary>
        /// Gets or sets the enum value.
        /// </summary>
        public int Value
        {
            get
            {
                return this.mValue;
            }
            set
            {
                this.mValue = value;
            }
        }

        /// <summary>
        /// Gets or sets the enum value name.
        /// </summary>
        public string Name
        {
            get
            {
                return this.mName;
            }
            set
            {
                this.mName = value;
            }
        }

        #endregion // Properties.

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Enum"/> class.
        /// </summary>
        public Enum()
        {
        }

        #endregion // Constructors.
    }
}

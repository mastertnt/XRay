using System.ComponentModel;
using System.Xml.Serialization;

namespace XMetadata.MetadataDescriptors
{
    /// <summary>
    /// Definition of the <see cref="ABoundableMetadata{T}"/> class.
    /// </summary>
    public abstract class ABoundableMetadata<T> : AMetadata<T>, IBoundableMetadata
    {
        #region Fields

        /// <summary>
        /// Stores the minimum metadata value.
        /// </summary>
        private T mMin;

        /// <summary>
        /// Stores the maximum metadata value.
        /// </summary>
        private T mMax;

        #endregion // Fields.

        #region Properties

        /// <summary>
        /// Gets or sets the Min metadata data.
        /// </summary>
        [XmlIgnore]
        [Browsable(false)]
        object IBoundableMetadata.Min
        {
            get
            {
                return this.Min;
            }
            set
            {
                if (value is T)
                {
                    this.Min = (T)value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the Max metadata data.
        /// </summary>
        [XmlIgnore]
        [Browsable(false)]
        object IBoundableMetadata.Max
        {
            get
            {
                return this.Max;
            }
            set
            {
                if (value is T)
                {
                    this.Max = (T)value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the Min metadata data.
        /// </summary>
        [XmlIgnore]
        [Browsable(false)]
        public T Min
        {
            get
            {
                return this.mMin;
            }
            set
            {
                this.mMin = value;
            }
        }

        /// <summary>
        /// Gets or sets the Max metadata data.
        /// </summary>
        [XmlIgnore]
        [Browsable(false)]
        public T Max
        {
            get
            {
                return this.mMax;
            }
            set
            {
                this.mMax = value;
            }
        }

        #endregion // Properties.

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ABoundableMetadata{T}"/> class.
        /// </summary>
        /// <param name="pId">The metadata identifier.</param>
        protected ABoundableMetadata(string pId)
            : base(pId)
        {

        }

        #endregion // Constructors.
    }
}

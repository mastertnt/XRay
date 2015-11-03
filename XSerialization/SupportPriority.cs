using System;

namespace XSerialization
{
    /// <summary>
    /// Enumerated type which defines the support level of a contract.
    /// The lowest value means higher priority.
    /// </summary>
    public enum SupportLevel
    {
        /// <summary>
        /// The element priority (value).
        /// </summary>
        Element = 10,
        /// <summary>
        /// The attribute priority.
        /// </summary>
        Attribute = 20,
        /// <summary>
        /// The property information priority.
        /// </summary>
        PropertyInfo = 30,
        /// <summary>
        /// The type priority.
        /// </summary>
        Type = 40,
        /// <summary>
        /// The interface priority.
        /// </summary>
        Interface = 50,
        /// <summary>
        /// The default priority.
        /// </summary>
        Default = 60,
        /// <summary>
        /// The not supported priority.
        /// </summary>
        NotSupported = Int32.MaxValue,
    }
    
    /// <summary>
    /// This class defines a support priority.
    /// </summary>
    public class SupportPriority : IComparable<SupportPriority>
    {
        #region Constants

        /// <summary>
        /// This constants must be used when a contract cannot support an object.
        /// </summary>
// ReSharper disable once InconsistentNaming
        public static readonly SupportPriority CANNOT_SUPPORT = new SupportPriority(SupportLevel.NotSupported, -1);

        #endregion // Constants

        #region Properties

        /// <summary>
        /// The level of support.
        /// </summary>
        public SupportLevel Level
        {
            get;
            private set;
        }

        /// <summary>
        /// Get or sets the sub priority.
        /// This meaning of this value depends on the level.
        /// For a type, it means the depth of inheritance.
        /// </summary>
        public int SubPriority
        {
            get;
            private set;
        }

        #endregion // Properties.

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SupportPriority"/> class.
        /// </summary>
        /// <param name="pLevel">The level.</param>
        /// <param name="pSubPriority">The sub priority.</param>
        public SupportPriority(SupportLevel pLevel, int pSubPriority)
        {
            this.Level = pLevel;
            this.SubPriority = pSubPriority;
        }

        #endregion // Constructors.

        #region Methods

        /// <summary>
        /// This method overloads the CompareTo.
        /// </summary>
        /// <param name="pSecondMember">The second member.</param>
        /// <returns>0 if two values are equal, -1 if the second member is greater than the current value, 1 if this is lower than the second member</returns>
        public int CompareTo(SupportPriority pSecondMember)
        {
            if (this.Level == pSecondMember.Level)
            {
                if (this.SubPriority < pSecondMember.SubPriority)
                {
                    return -1;
                }

                if (this.SubPriority > pSecondMember.SubPriority)
                {
                    return 1;
                }

                return 0;
            }

            if (this.Level < pSecondMember.Level)
            {
                return -1;
            }

            return 1;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("SupportPriority : Level : {0}, SubPriority : {1}", this.Level, this.SubPriority);
        }

        #endregion // Methods.
    }
}

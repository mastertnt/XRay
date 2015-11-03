namespace XSerialization.Attributes
{
    /// <summary>
    /// This attribute can be used to force a serialization on a read-only property.
    /// </summary>
    public class ForceWriteXSerializationAttribute : XInternalSerializationAttribute
    {
        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ForceWriteXSerializationAttribute()
        {
        }

        #endregion // Constructors.
    }
}

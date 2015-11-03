namespace XSerialization.Attributes
{
    /// <summary>
    /// This property can used to force a write but not read on a property (mainly used by a service or a factory).
    /// </summary>
    public class WriteOnlyXSerializationAttribute : ForceWriteXSerializationAttribute
    {
        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public WriteOnlyXSerializationAttribute()
        {
        }

        #endregion // Constructors.
    }
}

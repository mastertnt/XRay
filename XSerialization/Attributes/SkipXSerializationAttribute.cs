using XSerialization.Values;

namespace XSerialization.Attributes
{
    /// <summary>
    /// This attribute can be used to skip the serialization on a property.
    /// </summary>
    public sealed class SkipXSerializationAttribute : XSerializationAttribute
    {
        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public SkipXSerializationAttribute()
            :base(typeof(NoWriteSerializationContract))
        {

        }

        #endregion // Constructors.
    }
}

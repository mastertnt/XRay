namespace XSerialization
{
    /// <summary>
    /// This interface describes a serialization contrat on a type.
    /// </summary>
    public interface IXTypedSerializationContract<in T> : IXSerializationContract
    {
        /// <summary>
        /// This method checks if the object can be managed by the contract.
        /// </summary>
        /// <param name="pObject">The object to manage.</param>
        /// <remarks>The object can be a type, a property info, ... </remarks>
        /// <param name="pSerializationContext">The context of serialization</param>
        SupportPriority CanManage(T pObject, IXSerializationContext pSerializationContext);
    }
}

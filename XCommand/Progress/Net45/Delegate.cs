
namespace System
{
    /// <summary>
    /// Delegate defining a generic event handler.
    /// </summary>
    /// <typeparam name="TEventArgs">The event arguments tyle.</typeparam>
    /// <param name="pSender">The event sender.</param>
    /// <param name="pEventArgs">The event arguments.</param>
    public delegate void GenericEventHandler<TEventArgs>(object pSender, TEventArgs pEventArgs);
}

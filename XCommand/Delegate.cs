
namespace XCommand
{
    /// <summary>
    /// Delegate defining a user command execution event handler.
    /// </summary>
    /// <param name="pSource">The source.</param>
    /// <param name="pEventArgs">The event arguments.</param>
    public delegate void CommandExecutionEventHandler<TSource>(TSource pSource, CommandExecutionEventArgs pEventArgs);

    /// <summary>
    /// Delegate defining a context changed event handler.
    /// </summary>
    /// <param name="pSource">The source.</param>
    /// <param name="pEventArgs">The event arguments.</param>
    public delegate void ContextChangedEventHandler<TSource>(TSource pSource, ContextChangedEventArgs pEventArgs);

    /// <summary>
    /// Delegate defining a session created event handler.
    /// </summary>
    /// <param name="pSource">The service source.</param>
    /// <param name="pEventArgs">The event arguments.</param>
    public delegate void SessionCreatedEventHandler(IUserCommandManager pSource, SessionCreatedEventArgs pEventArgs);
}

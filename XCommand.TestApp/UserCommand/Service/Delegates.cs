using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandTest.UserCommand.Service
{
    /// <summary>
    /// Delegate defining a user command execution event handler.
    /// </summary>
    /// <param name="pSource">The source.</param>
    /// <param name="pEventArgs">The event arguments.</param>
    public delegate void CommandExecutionEventHandler<TSource>(TSource pSource, CommandExecutionEventArgs pEventArgs);

    /// <summary>
    /// Delegate defining a context change event handler.
    /// </summary>
    /// <param name="pSource">The source.</param>
    /// <param name="pEventArgs">The event arguments.</param>
    public delegate void ContextChangedEventHandler<TSource>(TSource pSource, ContextChangedEventArgs pEventArgs);

    /// <summary>
    /// Delegate defining a session changed event handler.
    /// </summary>
    /// <param name="pSource">The service source.</param>
    /// <param name="pEventArgs">The event arguments.</param>
    public delegate void SessionChangedEventHandler(IUserCommandManagerService pSource, SessionChangedEventArgs pEventArgs);
}

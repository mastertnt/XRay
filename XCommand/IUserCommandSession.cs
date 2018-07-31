using System.Collections.Generic;

namespace XCommand
{
    /// <summary>
    /// Interface used to define a command user session.
    /// </summary>
    public interface IUserCommandSession
    {
        #region Properties

        /// <summary>
        /// Gets the current context of the session.
        /// </summary>
        IUserCommandContext CurrentContext
        {
            get;
        }

        /// <summary>
        /// Gets the contextes of this session.
        /// </summary>
        IEnumerable<IUserCommandContext> Contextes
        {
            get;
        }

        #endregion // Properties.

        #region Events

        /// <summary>
        /// Event raised when the context changed.
        /// </summary>
        event ContextChangedEventHandler<IUserCommandSession> ContextChanged;

        #endregion // Events.

        #region Methods

        /// <summary>
        /// Returns a context by its id.
        /// </summary>
        /// <param name="pId">The context id.</param>
        /// <returns>The context if it exists, null otherwise.</returns>
        IUserCommandContext GetContextById(string pId);

        #endregion // Methods.
    }
}

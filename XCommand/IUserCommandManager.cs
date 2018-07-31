using System.Collections.Generic;

namespace XCommand
{
    /// <summary>
    /// Interface defining a user command manager.
    /// </summary>
    public interface IUserCommandManager
    {
        #region Properties

        /// <summary>
        /// Gets the sessions.
        /// </summary>
        IEnumerable<IUserCommandSession> Sessions
        {
            get;
        }

        /// <summary>
        /// Gets the flag indicating if the manager can do a command.
        /// </summary>
        bool CanDo
        {
            get;
        }

        /// <summary>
        /// Gets the flag indicating if the manager can redo a command.
        /// </summary>
        bool CanRedo
        {
            get;
        }

        /// <summary>
        /// Gets the flag indicating if the manager can undo a command.
        /// </summary>
        bool CanUndo
        {
            get;
        }

        #endregion // Properties.

        #region Events

        /// <summary>
        /// Event raised to notify a command execution progression.
        /// </summary>
        event CommandExecutionEventHandler<IUserCommandManager> CommandDoing;

        /// <summary>
        /// Event raised to notify a command execution successfully finished.
        /// </summary>
        event CommandExecutionEventHandler<IUserCommandManager> CommandDone;

        /// <summary>
        /// Event raised to notify a command execution or revertion failed.
        /// </summary>
        event CommandExecutionEventHandler<IUserCommandManager> CommandFailed;

        /// <summary>
        /// Event raised to notify a command revertion progression.
        /// </summary>
        event CommandExecutionEventHandler<IUserCommandManager> CommandUndoing;

        /// <summary>
        /// Event raised to notify a command revertion successfully finished.
        /// </summary>
        event CommandExecutionEventHandler<IUserCommandManager> CommandUndone;

        /// <summary>
        /// Event raised to notify a context modification.
        /// </summary>
        event ContextChangedEventHandler<IUserCommandManager> ContextChanged;

        #endregion // Events.

        #region Methods

        /// <summary>
        /// Registers and execute the given command.
        /// </summary>
        /// <param name="pCommand">The command to execute.</param>
        void Do(IUserCommand pCommand);

        /// <summary>
        /// Undo the current command of the context.
        /// </summary>
        void Undo();

        /// <summary>
        /// Redo the current command of the context.
        /// </summary>
        void Redo();

        /// <summary>
        /// Makes the context having the given id the current one.
        /// If it doesn't exist, it is created.
        /// </summary>
        /// <param name="pNewContextId">The new context id.</param>
        /// <returns>True if the context has been changed, false otherwise.</returns>
        bool SwitchContext(string pNewContextId);

        /// <summary>
        /// Change the mode of the command manager.
        /// </summary>
        /// <param name="pNewMode">The command manager new mode.</param>
        /// <return>True if the mode has been changed, false otherwise.</return>
        bool SwitchMode(UserCommandManagerMode pNewMode);

        #endregion // Methods.
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandTest.UserCommand.Service
{
    /// <summary>
    /// Interface defining a command session context.
    /// </summary>
    public interface IUserCommandContext
    {
        #region Properties

        /// <summary>
        /// Gets the context id.
        /// </summary>
        string Id
        {
            get;
        }

        /// <summary>
        /// Gets the current index of the context.
        /// The index references the last done command.
        /// </summary>
        int CurrentCommandIndex
        {
            get;
        }

        /// <summary>
        /// Gets the commands executed in this context.
        /// </summary>
        IEnumerable<IUserCommand> Commands
        {
            get;
        }

        /// <summary>
        /// Gets the flag indicating if the context can redo a command.
        /// </summary>
        bool CanRedo
        {
            get;
        }

        /// <summary>
        /// Gets the flag indicating if the context can undo a command.
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
        event CommandExecutionEventHandler<IUserCommandContext> CommandDoing;

        /// <summary>
        /// Event raised to notify a command execution successfully finished.
        /// </summary>
        event CommandExecutionEventHandler<IUserCommandContext> CommandDone;

        /// <summary>
        /// Event raised to notify a command execution or revertion failed.
        /// </summary>
        event CommandExecutionEventHandler<IUserCommandContext> CommandFailed;

        /// <summary>
        /// Event raised to notify a command revertion progression.
        /// </summary>
        event CommandExecutionEventHandler<IUserCommandContext> CommandUndoing;

        /// <summary>
        /// Event raised to notify a command revertion successfully finished.
        /// </summary>
        event CommandExecutionEventHandler<IUserCommandContext> CommandUndone;

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

        #endregion // Methods.
    }
}

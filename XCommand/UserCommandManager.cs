using System.Collections.Generic;
using System.Linq;

namespace XCommand
{
    /// <summary>
    /// Class defining a command manager.
    /// </summary>
    public class UserCommandManager : IUserCommandManager
    {
        #region Fields

        /// <summary>
        /// Stores the sessions of the manager.
        /// </summary>
        private UserCommandSessionList mSessions;

        /// <summary>
        /// Stores the mode of the manager.
        /// </summary>
        private UserCommandManagerMode mMode;

        /// <summary>
        /// Stores the command doing handler executed before the event raise.
        /// </summary>
        private CommandExecutionEventHandler<IUserCommandManager> mCommandDoingHandler;

        /// <summary>
        /// Stores the command done handler executed before the event raise.
        /// </summary>
        private CommandExecutionEventHandler<IUserCommandManager> mCommandDoneHandler;

        /// <summary>
        /// Stores the command failed handler executed before the event raise.
        /// </summary>
        private CommandExecutionEventHandler<IUserCommandManager> mCommandFailedHandler;

        /// <summary>
        /// Stores the command undoing handler executed before the event raise.
        /// </summary>
        private CommandExecutionEventHandler<IUserCommandManager> mCommandUndoingHandler;

        /// <summary>
        /// Stores the command undone handler executed before the event raise.
        /// </summary>
        private CommandExecutionEventHandler<IUserCommandManager> mCommandUndoneHandler;

        #endregion // Fields.

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UserCommandManager"/> class.
        /// </summary>
        public UserCommandManager()
        {
            this.mSessions = new UserCommandSessionList(this);
            this.mSessions.Add(this.CreateSession());
        }

        #endregion // Constructors.

        #region Methods

        /// <summary>
        /// Creates a new session.
        /// </summary>
        /// <returns>The created session.</returns>
        private UserCommandSession CreateSession()
        {
            switch (this.mMode)
            {
                case UserCommandManagerMode.User:
                    {
                        return new UserCommandSession(this);
                    }

                case UserCommandManagerMode.AutoTest:
                    {
                        return new AutoTestCommandSession(this);
                    }

                default:
                    {
                        // Should never happen.
                        return null;
                    }
            }
        }

        /// <summary>
        /// Notifies a session created.
        /// </summary>
        /// <param name="pEventArgs">The event arguments.</param>
        internal void NotifySessionCreated(SessionCreatedEventArgs pEventArgs)
        {
            if (this.SessionCreated != null)
            {
                this.SessionCreated(this, pEventArgs);
            }
        }

        /// <summary>
        /// Notifies a context changed.
        /// </summary>
        /// <param name="pEventArgs">The event arguments.</param>
        internal void NotifyContextChanged(ContextChangedEventArgs pEventArgs)
        {
            if (this.ContextChanged != null)
            {
                this.ContextChanged(this, pEventArgs);
            }
        }

        /// <summary>
        /// Notifies a context changed.
        /// </summary>
        /// <param name="pType">The type of notification.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        internal void NotifyCommandExecution(CommandExecutionType pType, CommandExecutionEventArgs pEventArgs)
        {
            switch (pType)
            {
                case CommandExecutionType.Doing:
                    {
                        if (this.mCommandDoingHandler != null)
                        {
                            this.mCommandDoingHandler(this, pEventArgs);
                        }

                        if (this.CommandDoing != null)
                        {
                            this.CommandDoing(this, pEventArgs);
                        }
                    }
                    break;

                case CommandExecutionType.Done:
                    {
                        if (this.mCommandDoneHandler != null)
                        {
                            this.mCommandDoneHandler(this, pEventArgs);
                        }

                        if (this.CommandDone != null)
                        {
                            this.CommandDone(this, pEventArgs);
                        }
                    }
                    break;

                case CommandExecutionType.Failed:
                    {
                        if (this.mCommandFailedHandler != null)
                        {
                            this.mCommandFailedHandler(this, pEventArgs);
                        }

                        if (this.CommandFailed != null)
                        {
                            this.CommandFailed(this, pEventArgs);
                        }
                    }
                    break;

                case CommandExecutionType.Undoing:
                    {
                        if (this.mCommandUndoingHandler != null)
                        {
                            this.mCommandUndoingHandler(this, pEventArgs);
                        }

                        if (this.CommandUndoing != null)
                        {
                            this.CommandUndoing(this, pEventArgs);
                        }
                    }
                    break;

                case CommandExecutionType.Undone:
                    {
                        if (this.mCommandUndoneHandler != null)
                        {
                            this.mCommandUndoneHandler(this, pEventArgs);
                        }

                        if (this.CommandUndone != null)
                        {
                            this.CommandUndone(this, pEventArgs);
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// Sets the command doing handler.
        /// </summary>
        /// <param name="pCommandDoingHandler">The command doing handler.</param>
        public void SetCommandDoingHandler(CommandExecutionEventHandler<IUserCommandManager> pCommandDoingHandler)
        {
            this.mCommandDoingHandler = pCommandDoingHandler;
        }

        /// <summary>
        /// Sets the command done handler.
        /// </summary>
        /// <param name="pCommandDoneHandler">The command done handler.</param>
        public void SetCommandDoneHandler(CommandExecutionEventHandler<IUserCommandManager> pCommandDoneHandler)
        {
            this.mCommandDoneHandler = pCommandDoneHandler;
        }

        /// <summary>
        /// Sets the command failed handler.
        /// </summary>
        /// <param name="pCommandFailedHandler">The command failed handler.</param>
        public void SetCommandFailedHandler(CommandExecutionEventHandler<IUserCommandManager> pCommandFailedHandler)
        {
            this.mCommandFailedHandler = pCommandFailedHandler;
        }

        /// <summary>
        /// Sets the command undone handler.
        /// </summary>
        /// <param name="pCommandUndoneHandler">The command undone handler.</param>
        public void SetCommandUndoneHandler(CommandExecutionEventHandler<IUserCommandManager> pCommandUndoneHandler)
        {
            this.mCommandUndoneHandler = pCommandUndoneHandler;
        }

        /// <summary>
        /// Sets the command undoing handler.
        /// </summary>
        /// <param name="pCommandUndoingHandler">The command undoing handler.</param>
        public void SetCommandUndoingHandler(CommandExecutionEventHandler<IUserCommandManager> pCommandUndoingHandler)
        {
            this.mCommandUndoingHandler = pCommandUndoingHandler;
        }

        #endregion // Methods.

        #region IUserCommandManager implementation

        #region Properties

        /// <summary>
        /// Gets the sessions.
        /// </summary>
        public IEnumerable<IUserCommandSession> Sessions
        {
            get
            {
                return this.mSessions;
            }
        }

        /// <summary>
        /// Gets the flag indicating if the manager can do a command.
        /// </summary>
        public bool CanDo
        {
            get
            {
                if (this.mSessions.CurrentSession == null)
                {
                    return false;
                }

                return this.mSessions.CurrentSession.CanDo;
            }
        }

        /// <summary>
        /// Gets the flag indicating if the context can redo a command.
        /// </summary>
        public bool CanRedo
        {
            get
            {
                if (this.mSessions.CurrentSession == null)
                {
                    return false;
                }

                return this.mSessions.CurrentSession.CanRedo;
            }
        }

        /// <summary>
        /// Gets the flag indicating if the context can undo a command.
        /// </summary>
        public bool CanUndo
        {
            get
            {
                if (this.mSessions.CurrentSession == null)
                {
                    return false;
                }

                return this.mSessions.CurrentSession.CanUndo;
            }
        }

        #endregion // Properties.

        #region Events

        /// <summary>
        /// Event raised to notify a command execution progression.
        /// </summary>
        public event CommandExecutionEventHandler<IUserCommandManager> CommandDoing;

        /// <summary>
        /// Event raised to notify a command execution successfully finished.
        /// </summary>
        public event CommandExecutionEventHandler<IUserCommandManager> CommandDone;

        /// <summary>
        /// Event raised to notify a command execution or revertion failed.
        /// </summary>
        public event CommandExecutionEventHandler<IUserCommandManager> CommandFailed;

        /// <summary>
        /// Event raised to notify a command revertion progression.
        /// </summary>
        public event CommandExecutionEventHandler<IUserCommandManager> CommandUndoing;

        /// <summary>
        /// Event raised to notify a command revertion successfully finished.
        /// </summary>
        public event CommandExecutionEventHandler<IUserCommandManager> CommandUndone;
        
        /// <summary>
        /// Event raised when a session created.
        /// </summary>
        public event SessionCreatedEventHandler SessionCreated;

        /// <summary>
        /// Event raised to notify a context modification.
        /// </summary>
        public event ContextChangedEventHandler<IUserCommandManager> ContextChanged;

        #endregion // Events.

        #region Methods

        /// <summary>
        /// Registers and execute the given command.
        /// </summary>
        /// <param name="pCommand">The command to execute.</param>
        public void Do(IUserCommand pCommand)
        {
            if (pCommand == null)
            {
                return;
            }

            if (this.mSessions.Any() == false || pCommand.State == UserCommandState.NewSession)
            {
                // Ensuring at least one session exists. New session is created on demande as well.
                this.mSessions.Add(this.CreateSession());
            }

            // Executing the command.
            if (this.mSessions.CurrentSession != null)
            {
                this.mSessions.CurrentSession.Do(pCommand);
            }
        }

        /// <summary>
        /// Undo the current command of the context.
        /// </summary>
        public void Undo()
        {
            // Executing the command.
            if (this.mSessions.CurrentSession != null)
            {
                this.mSessions.CurrentSession.Undo();
            }
        }

        /// <summary>
        /// Redo the current command of the context.
        /// </summary>
        public void Redo()
        {
            // Executing the command.
            if (this.mSessions.CurrentSession != null)
            {
                this.mSessions.CurrentSession.Redo();
            }
        }

        /// <summary>
        /// Makes the context having the given id the current one.
        /// If it doesn't exist, it is created.
        /// </summary>
        /// <param name="pNewContextId">The new context id.</param>
        /// <returns>True if the context has been changed, false otherwise.</returns>
        public bool SwitchContext(string pNewContextId)
        {
            if (this.mSessions.CurrentSession == null)
            {
                return false;
            }

            return (this.mSessions.CurrentSession.SwitchContext(pNewContextId) != null);
        }

        /// <summary>
        /// Change the mode of the command manager.
        /// </summary>
        /// <param name="pNewMode">The command manager new mode.</param>
        /// <return>True if the mode has been changed, false otherwise.</return>
        public bool SwitchMode(UserCommandManagerMode pNewMode)
        {
            if (pNewMode == this.mMode)
            {
                return false;
            }

            // Cleaning the sessions
            this.mSessions.Clear();

            // Updating the mode.
            this.mMode = pNewMode;

            return true;
        }

        #endregion // Methods.

        #endregion // IUserCommandManager implementation.

        #region Inner classes

        /// <summary>
        /// Utils enum defining the command execution type.
        /// </summary>
        internal enum CommandExecutionType
        {
            /// <summary>
            /// Command is doing.
            /// </summary>
            Doing,

            /// <summary>
            /// Command is done.
            /// </summary>
            Done,

            /// <summary>
            /// Command has failed
            /// </summary>
            Failed,

            /// <summary>
            /// Command is undoing.
            /// </summary>
            Undoing,

            /// <summary>
            /// Command is undone.
            /// </summary>
            Undone
        }

        #endregion // Inner classes.
    }
}

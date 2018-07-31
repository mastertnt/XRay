using CommandTest.UserCommand.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandTest.UserCommand
{
    /// <summary>
    /// Class defining a command manager service.
    /// </summary>
    public class UserCommandManagerService : IUserCommandManagerService
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

        #endregion // Fields.

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UserCommandManagerService"/> class.
        /// </summary>
        public UserCommandManagerService()
        {
            this.mSessions = new UserCommandSessionList();
        }

        #endregion // Constructors.

        #region Methods

        /// <summary>
        /// Creates a new session.
        /// </summary>
        /// <returns>The created session.</returns>
        private IUserCommandSession CreateSession()
        {
            switch (this.mMode)
            {
                case UserCommandManagerMode.User:
                    {
                        return new UserCommandSession();
                    }

                case UserCommandManagerMode.AutoTest:
                    {
                        return new AutoTestCommandSession();
                    }

                default:
                    {
                        // Should never happen.
                        return null;
                    }
            }
        }

        #endregion // Methods.

        #region IUserCommandManagerService implementation

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
        /// Gets the flag indicating if the context can redo a command.
        /// </summary>
        public bool CanRedo
        {
            get
            {
                if (this.mSessions.CurrentContext == null)
                {
                    return false;
                }

                return this.mSessions.CurrentContext.CanRedo;
            }
        }

        /// <summary>
        /// Gets the flag indicating if the context can undo a command.
        /// </summary>
        public bool CanUndo
        {
            get
            {
                if (this.mSessions.CurrentContext == null)
                {
                    return false;
                }

                return this.mSessions.CurrentContext.CanUndo;
            }
        }

        #endregion // Properties.

        #region Events

        /// <summary>
        /// Event raised to notify a command execution progression.
        /// </summary>
        public event CommandExecutionEventHandler<IUserCommandManagerService> CommandDoing;

        /// <summary>
        /// Event raised to notify a command execution successfully finished.
        /// </summary>
        public event CommandExecutionEventHandler<IUserCommandManagerService> CommandDone;

        /// <summary>
        /// Event raised to notify a command execution or revertion failed.
        /// </summary>
        public event CommandExecutionEventHandler<IUserCommandManagerService> CommandFailed;

        /// <summary>
        /// Event raised to notify a command revertion progression.
        /// </summary>
        public event CommandExecutionEventHandler<IUserCommandManagerService> CommandUndoing;

        /// <summary>
        /// Event raised to notify a command revertion successfully finished.
        /// </summary>
        public event CommandExecutionEventHandler<IUserCommandManagerService> CommandUndone;

        /// <summary>
        /// Event raised to notify a context modification.
        /// </summary>
        public event ContextChangedEventHandler<IUserCommandManagerService> ContextChanged;

        #endregion // Events.

        #region Methods

        /// <summary>
        /// Registers and execute the given command.
        /// </summary>
        /// <param name="pCommand">The command to execute.</param>
        public void Do(IUserCommand pCommand)
        {
            // Command validation.
            if (pCommand == null)
            {
                return;
            }

            if (this.mSessions.Any() == false)
            {
                // Ensuring at least one session exists.
                this.mSessions.Add(this.CreateSession());
            }
            
            // Executing the command.
        }

        /// <summary>
        /// Undo the current command of the context.
        /// </summary>
        public void Undo()
        {

        }

        /// <summary>
        /// Redo the current command of the context.
        /// </summary>
        public void Redo()
        {

        }

        /// <summary>
        /// Makes the context having the given id the current one.
        /// If it doesn't exist, it is created.
        /// </summary>
        /// <param name="pNewContextId">The new context id.</param>
        /// <returns>True if the context has been changed, false otherwise.</returns>
        public bool SwitchContext(string pNewContextId)
        {

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
            this.
        }

        #endregion // Methods.

        #endregion // IUserCommandManagerService implementation.
    }
}

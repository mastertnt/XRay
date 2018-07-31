using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace XCommand
{
    /// <summary>
    /// Class defining a user command context.
    /// </summary>
    internal class UserCommandContext : IUserCommandContext
    {
        #region Fields

        /// <summary>
        /// Stores the parent command manager.
        /// </summary>
        private UserCommandManager mParentManager;

        /// <summary>
        /// Stores the flag indicating a command is currently executing.
        /// Two commands cannot be executed together.
        /// </summary>
        private bool mIsExecutingCommand;

        #endregion // Fields.

        #region Properties

        /// <summary>
        /// Gets the last done command registered in the context.
        /// </summary>
        public IUserCommand LastExecutedCommand
        {
            get
            {
                if (this.CurrentCommandIndex < 0 || this.CurrentCommandIndex >= this.CommandsList.Count)
                {
                    return null;
                }

                return this.CommandsList[this.CurrentCommandIndex];
            }
        }

        /// <summary>
        /// Gets the commands as an editable list.
        /// </summary>
        protected ObservableCollection<IUserCommand> CommandsList
        {
            get;
            private set;
        }

        #endregion // Properties.

        #region Constructors

        /// <summary>
        /// Initializes an instance of the <see cref="UserCommandContext"/> class.
        /// </summary>
        /// <param name="pParentManager">The parent command manager.</param>
        /// <param name="pId">The user command context id.</param>
        public UserCommandContext(UserCommandManager pParentManager, string pId)
        {
            this.mParentManager = pParentManager;
            this.mIsExecutingCommand = false;

            this.Id = pId;
            this.CommandsList = new ObservableCollection<IUserCommand>();
            this.CurrentCommandIndex = -1;
        }

        #endregion // Constructors.

        #region Methods

        /// <summary>
        /// Returns the first and second index of the undone command range.
        /// </summary>
        /// <param name="pFirstIndex">The first index.</param>
        /// <param name="pLastIndex">The second index.</param>
        /// <returns>True if the indexes are valid, false otherwise.</returns>
        private bool GetUndoneCommandsRange(out int pFirstIndex, out int pLastIndex)
        {
            pFirstIndex = -1;
            pLastIndex = -1;

            if (this.CurrentCommandIndex < -1 || this.CurrentCommandIndex >= this.CommandsList.Count - 1)
            {
                return false;
            }

            // Computing the range.
            int lFirstIndex = this.CurrentCommandIndex + 1;
            int lLastIndex = this.CommandsList.Count - 1;

            if (lFirstIndex < 0 || lFirstIndex >= this.CommandsList.Count || lLastIndex < 0 || lLastIndex >= this.CommandsList.Count)
            {
                return false;
            }

            if (lLastIndex >= lFirstIndex)
            {
                pFirstIndex = lFirstIndex;
                pLastIndex = lLastIndex;

                return true;
            }

            return false;
        }

        /// <summary>
        /// Removes all the undone commands.
        /// </summary>
        private void RemoveUndoneCommand()
        {
            int lFirstIndex;
            int lLastIndex;
            if (this.GetUndoneCommandsRange(out lFirstIndex, out lLastIndex))
            {
                for (int lIndex = lFirstIndex; lIndex <= lLastIndex; lIndex ++)
                {
                    this.CommandsList.RemoveAt(lIndex);
                }
            }
        }

        /// <summary>
        /// Registers and execute the given command.
        /// </summary>
        /// <param name="pCommand">The command to execute.</param>
        public void Do(IUserCommand pCommand)
        {
            if (this.CanDo)
            {
                // Locking the execution of an other command.
                this.mIsExecutingCommand = true;

                // Command validation.
                if (pCommand == null)
                {
                    return;
                }

                // Registering on command events.
                pCommand.Doing += this.OnCommandDoing;
                pCommand.Done += this.OnCommandDone;
                pCommand.Failed += this.OnCommandFailed;

                // Do the command.
                this.CustomDo(pCommand);
            }
        }

        /// <summary>
        /// Registers and execute the given command.
        /// </summary>
        /// <param name="pCommand">The command to execute.</param>
        protected virtual void CustomDo(IUserCommand pCommand)
        {
            // Removing the undone commands.
            this.RemoveUndoneCommand();

            // Do the command.
            pCommand.Do();
        }

        /// <summary>
        /// Undo the current command of the context.
        /// </summary>
        public void Undo()
        {
            if (this.CanUndo)
            {
                // Locking the execution of an other command.
                this.mIsExecutingCommand = true;

                // Registering on command events.
                this.LastExecutedCommand.Undoing += this.OnCommandUndoing;
                this.LastExecutedCommand.Undone += this.OnCommandUndone;
                this.LastExecutedCommand.Failed += this.OnCommandFailed;

                // Do the command.
                this.CustomUndo(this.LastExecutedCommand);
            }
        }

        /// <summary>
        /// Undo the current command of the context.
        /// </summary>
        /// <param name="pCommand">The command to execute.</param>
        protected virtual void CustomUndo(IUserCommand pCommand)
        {
            // Undo the command.
            pCommand.Undo();
        }

        /// <summary>
        /// Redo the current command of the context.
        /// </summary>
        public void Redo()
        {
            if (this.CanRedo)
            {
                // Locking the execution of an other command.
                this.mIsExecutingCommand = true;

                // Getting the command to redo.
                IUserCommand lCommand = this.CommandsList[this.CurrentCommandIndex + 1];

                // Registering on command events.
                lCommand.Doing += this.OnCommandDoing;
                lCommand.Done += this.OnCommandRedone;
                lCommand.Failed += this.OnCommandFailed;

                // Do the command.
                this.CustomRedo(lCommand);
            }
        }

        /// <summary>
        /// Redo the current command of the context.
        /// </summary>
        /// <param name="pCommand">The command to execute.</param>
        protected virtual void CustomRedo(IUserCommand pCommand)
        {
            // Do the command.
            pCommand.Do();
        }

        /// <summary>
        /// Delegate called when a command is doing.
        /// </summary>
        /// <param name="pSource">The source command.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private void OnCommandDoing(IUserCommand pSource, CommandExecutionEventArgs pEventArgs)
        {
            // Custom process.
            this.CustomOnCommandDoing(pSource, pEventArgs);

            // Internal notification.
            if (this.CommandDoing != null)
            {
                this.CommandDoing(this, pEventArgs);
            }

            // Manager notification.
            this.mParentManager.NotifyCommandExecution(UserCommandManager.CommandExecutionType.Doing, pEventArgs);
        }

        /// <summary>
        /// Delegate called when a command is doing.
        /// </summary>
        /// <param name="pSource">The source command.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        protected virtual void CustomOnCommandDoing(IUserCommand pSource, CommandExecutionEventArgs pEventArgs)
        {
            // Nothing to do.
        }

        /// <summary>
        /// Delegate called when a command is undoing.
        /// </summary>
        /// <param name="pSource">The source command.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private void OnCommandUndoing(IUserCommand pSource, CommandExecutionEventArgs pEventArgs)
        {
            // Custom process.
            this.CustomOnCommandUndoing(pSource, pEventArgs);

            // Internal notification.
            if (this.CommandUndoing != null)
            {
                this.CommandUndoing(this, pEventArgs);
            }

            // Manager notification.
            this.mParentManager.NotifyCommandExecution(UserCommandManager.CommandExecutionType.Undoing, pEventArgs);
        }

        /// <summary>
        /// Delegate called when a command is undoing.
        /// </summary>
        /// <param name="pSource">The source command.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        protected virtual void CustomOnCommandUndoing(IUserCommand pSource, CommandExecutionEventArgs pEventArgs)
        {
            // Nothing to do.
        }

        /// <summary>
        /// Delegate called when a command is done.
        /// </summary>
        /// <param name="pSource">The source command.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private void OnCommandDone(IUserCommand pSource, CommandExecutionEventArgs pEventArgs)
        {
            // Unregistering form events.
            pEventArgs.Command.Doing -= this.OnCommandDoing;
            pEventArgs.Command.Done -= this.OnCommandDone;
            pEventArgs.Command.Failed -= this.OnCommandFailed;

            // Custom process.
            this.CustomOnCommandDone(pSource, pEventArgs);

            // Unlocking the command execution.
            this.mIsExecutingCommand = false;

            // Internal notification.
            if (this.CommandDone != null)
            {
                this.CommandDone(this, pEventArgs);
            }

            // Manager notification.
            this.mParentManager.NotifyCommandExecution(UserCommandManager.CommandExecutionType.Done, pEventArgs);
        }

        /// <summary>
        /// Delegate called when a command is done.
        /// </summary>
        /// <param name="pSource">The source command.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        protected virtual void CustomOnCommandDone(IUserCommand pSource, CommandExecutionEventArgs pEventArgs)
        {
            // Verifying if the context must be changed.
            if (pEventArgs.Command.State == UserCommandState.SwitchContext)
            {
                ISwitchContextCommand lCommand = pEventArgs.Command as ISwitchContextCommand;
                if (lCommand != null)
                {
                    string lNewContextId = lCommand.SwitchContext(this.Id);
                    this.mParentManager.SwitchContext(lNewContextId);
                }
            }

            // Evaluating if the new command must be added in the context list.
            bool lAddCommand = true;
            if (this.CommandsList.Any() && this.LastExecutedCommand != null && pEventArgs.Command.State == UserCommandState.Undoable)
            {
                lAddCommand = this.LastExecutedCommand.TryMerge(pEventArgs.Command) == false;
            }
            if (pEventArgs.Command.State != UserCommandState.Undoable)
            {
                lAddCommand = false;
            }

            // Adding the command in the list if wanted.
            if (lAddCommand)
            {
                this.CommandsList.Add(pEventArgs.Command);
                this.CurrentCommandIndex = this.CommandsList.Count - 1;
            }
        }

        /// <summary>
        /// Delegate called when a command is undone.
        /// </summary>
        /// <param name="pSource">The source command.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private void OnCommandUndone(IUserCommand pSource, CommandExecutionEventArgs pEventArgs)
        {
            // Unregistering form events.
            pEventArgs.Command.Undoing -= this.OnCommandUndoing;
            pEventArgs.Command.Undone -= this.OnCommandUndone;
            pEventArgs.Command.Failed -= this.OnCommandFailed;

            // Custom process.
            this.CustomOnCommandUndone(pSource, pEventArgs);

            // Unlocking the command execution.
            this.mIsExecutingCommand = false;

            // Internal notification.
            if (this.CommandUndone != null)
            {
                this.CommandUndone(this, pEventArgs);
            }

            // Manager notification.
            this.mParentManager.NotifyCommandExecution(UserCommandManager.CommandExecutionType.Undone, pEventArgs);
        }

        /// <summary>
        /// Delegate called when a command is undone.
        /// </summary>
        /// <param name="pSource">The source command.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        protected virtual void CustomOnCommandUndone(IUserCommand pSource, CommandExecutionEventArgs pEventArgs)
        {
            // Updating the current command index.
            int lCommandIndex = this.CommandsList.IndexOf(pEventArgs.Command);
            this.CurrentCommandIndex = --lCommandIndex;
        }

        /// <summary>
        /// Delegate called when a command is redone.
        /// </summary>
        /// <param name="pSource">The source command.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private void OnCommandRedone(IUserCommand pSource, CommandExecutionEventArgs pEventArgs)
        {
            // Unregistering form events.
            pEventArgs.Command.Doing -= this.OnCommandDoing;
            pEventArgs.Command.Done -= this.OnCommandRedone;
            pEventArgs.Command.Failed -= this.OnCommandFailed;

            // Custom process.
            this.CustomOnCommandRedone(pSource, pEventArgs);

            // Unlocking the command execution.
            this.mIsExecutingCommand = false;

            // Internal notification.
            if (this.CommandDone != null)
            {
                this.CommandDone(this, pEventArgs);
            }

            // Manager notification.
            this.mParentManager.NotifyCommandExecution(UserCommandManager.CommandExecutionType.Done, pEventArgs);
        }

        /// <summary>
        /// Delegate called when a command is redone.
        /// </summary>
        /// <param name="pSource">The source command.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        protected virtual void CustomOnCommandRedone(IUserCommand pSource, CommandExecutionEventArgs pEventArgs)
        {
            // Updating the current command index.
            this.CurrentCommandIndex = this.CommandsList.IndexOf(pEventArgs.Command);
        }

        /// <summary>
        /// Delegate called when a command execution failed.
        /// </summary>
        /// <param name="pSource">The source command.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private void OnCommandFailed(IUserCommand pSource, CommandExecutionEventArgs pEventArgs)
        {
            // Unregistering form events.
            pEventArgs.Command.Doing -= this.OnCommandDoing;
            pEventArgs.Command.Done -= this.OnCommandDone;
            pEventArgs.Command.Undoing -= this.OnCommandUndoing;
            pEventArgs.Command.Undone -= this.OnCommandUndone;
            pEventArgs.Command.Failed -= this.OnCommandFailed;

            // Custom process.
            this.CustomOnCommandFailed(pSource, pEventArgs);

            // Unlocking the command execution.
            this.mIsExecutingCommand = false;

            // Internal notification.
            if (this.CommandFailed != null)
            {
                this.CommandFailed(this, pEventArgs);
            }

            // Manager notification.
            this.mParentManager.NotifyCommandExecution(UserCommandManager.CommandExecutionType.Failed, pEventArgs);
        }

        /// <summary>
        /// Delegate called when a command execution failed.
        /// </summary>
        /// <param name="pSource">The source command.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        protected virtual void CustomOnCommandFailed(IUserCommand pSource, CommandExecutionEventArgs pEventArgs)
        {
            // Nothing to do.
        }

        #endregion // Methods.

        #region IUserCommandContext implementation

        #region Properties

        /// <summary>
        /// Gets the context id.
        /// </summary>
        public string Id
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the current index of the context.
        /// The index references the last done command.
        /// </summary>
        public int CurrentCommandIndex
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets the commands executed in this context.
        /// </summary>
        public IEnumerable<IUserCommand> Commands
        {
            get
            {
                return this.CommandsList;
            }
        }

        /// <summary>
        /// Gets the flag indicating if a command can be done.
        /// </summary>
        public virtual bool CanDo
        {
            get
            {
                return (this.mIsExecutingCommand == false);
            }
        }

        /// <summary>
        /// Gets the flag indicating if the context can redo a command.
        /// </summary>
        public virtual bool CanRedo
        {
            get
            {
                return (this.mIsExecutingCommand == false) && (this.CurrentCommandIndex < this.CommandsList.Count - 1);
            }
        }

        /// <summary>
        /// Gets the flag indicating if the context can undo a command.
        /// </summary>
        public virtual bool CanUndo
        {
            get
            {
                return (this.mIsExecutingCommand == false) && (this.CurrentCommandIndex >= 0);
            }
        }

        #endregion // Properties.

        #region Events

        /// <summary>
        /// Event raised to notify a command execution progression.
        /// </summary>
        public event CommandExecutionEventHandler<IUserCommandContext> CommandDoing;

        /// <summary>
        /// Event raised to notify a command execution successfully finished.
        /// </summary>
        public event CommandExecutionEventHandler<IUserCommandContext> CommandDone;

        /// <summary>
        /// Event raised to notify a command execution or revertion failed.
        /// </summary>
        public event CommandExecutionEventHandler<IUserCommandContext> CommandFailed;

        /// <summary>
        /// Event raised to notify a command revertion progression.
        /// </summary>
        public event CommandExecutionEventHandler<IUserCommandContext> CommandUndoing;

        /// <summary>
        /// Event raised to notify a command revertion successfully finished.
        /// </summary>
        public event CommandExecutionEventHandler<IUserCommandContext> CommandUndone;

        #endregion // Events.

        #endregion // IUserCommandContext implementation.
    }
}

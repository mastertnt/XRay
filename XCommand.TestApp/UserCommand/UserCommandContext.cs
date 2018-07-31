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
    /// Class defining a user command context.
    /// </summary>
    internal class UserCommandContext : IUserCommandContext
    {
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
        /// <param name="pId">The user command context id.</param>
        public UserCommandContext(string pId)
        {
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
        public bool GetUndoneCommandsRange(out int pFirstIndex, out int pLastIndex)
        {
            pFirstIndex = -1;
            pLastIndex = -1;

            if (this.CurrentCommandIndex < 0 || this.CurrentCommandIndex >= this.CommandsList.Count - 1)
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
        protected void RemoveUndoneCommand()
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
        /// Gets the flag indicating if the context can redo a command.
        /// </summary>
        public virtual bool CanRedo
        {
            get
            {
                return (this.CurrentCommandIndex < this.CommandsList.Count - 1);
            }
        }

        /// <summary>
        /// Gets the flag indicating if the context can undo a command.
        /// </summary>
        public virtual bool CanUndo
        {
            get
            {
                return (this.CurrentCommandIndex > 0);
            }
        }

        #endregion // Properties.

        #region Events

        /// <summary>
        /// Event raised to notify a command execution progression.
        /// </summary>
        public event ContextChangedEventHandler<IUserCommandContext> CommandDoing;

        /// <summary>
        /// Event raised to notify a command execution successfully finished.
        /// </summary>
        public event ContextChangedEventHandler<IUserCommandContext> CommandDone;

        /// <summary>
        /// Event raised to notify a command execution or revertion failed.
        /// </summary>
        public event ContextChangedEventHandler<IUserCommandContext> CommandFailed;

        /// <summary>
        /// Event raised to notify a command revertion progression.
        /// </summary>
        public event ContextChangedEventHandler<IUserCommandContext> CommandUndoing;

        /// <summary>
        /// Event raised to notify a command revertion successfully finished.
        /// </summary>
        public event ContextChangedEventHandler<IUserCommandContext> CommandUndone;

        #endregion // Events.

        #region Methods

        /// <summary>
        /// Registers and execute the given command.
        /// </summary>
        /// <param name="pCommand">The command to execute.</param>
        public virtual void Do(IUserCommand pCommand)
        {
            // Command validation.
            if (pCommand == null)
            {
                return;
            }

            // Removing the undone commands.
            this.RemoveUndoneCommand();

            // Do the command.
            pCommand.Do();

            // Evaluating if the new command must be added in the context list.
            bool lAddCommand = true;
            if (this.CommandsList.Any() && this.LastExecutedCommand != null && pCommand.State != UserCommandState.Internal)
            {
                lAddCommand = this.LastExecutedCommand.TryMerge(pCommand) == false;
            }
            if (pCommand.State == UserCommandState.Internal)
            {
                lAddCommand = false;
            }

            // Adding the command in the list if wanted.
            if (lAddCommand)
            {
                this.CommandsList.Add(pCommand);
                this.CurrentCommandIndex = this.CommandsList.Count - 1;
            }
        }

        /// <summary>
        /// Undo the current command of the context.
        /// </summary>
        public void Undo()
        {
            if (this.CanUndo)
            {
                // Undo the current command.
                this.LastExecutedCommand.Undo();
                this.CurrentCommandIndex--;
            }
        }

        /// <summary>
        /// Redo the current command of the context.
        /// </summary>
        public void Redo()
        {
            if (this.CanRedo)
            {
                this.CurrentCommandIndex++;
                this.LastExecutedCommand.Do();
            }
        }

        #endregion // Methods.

        #endregion // IUserCommandContext implementation.
    }
}

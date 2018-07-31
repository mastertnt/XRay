using System.Linq;

namespace XCommand
{
    /// <summary>
    /// Class defining an auto test command context.
    /// </summary>
    internal class AutoTestCommandContext : UserCommandContext
    {
        #region Properties

        /// <summary>
        /// Gets the flag indicating if the context can redo a command.
        /// </summary>
        public override bool CanRedo
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the flag indicating if the context can undo a command.
        /// </summary>
        public override bool CanUndo
        {
            get
            {
                return false;
            }
        }

        #endregion // Properties.

        #region Constructors

        /// <summary>
        /// Initializes an instance of the <see cref="AutoTestCommandContext"/> class.
        /// </summary>
        /// <param name="pParentManager">The parent command manager.</param>
        /// <param name="pId">The user command context id.</param>
        public AutoTestCommandContext(UserCommandManager pParentManager, string pId)
            : base(pParentManager, pId)
        {
        }

        #endregion // Constructors.

        #region Methods

        /// <summary>
        /// Registers and execute the given command.
        /// </summary>
        /// <param name="pCommand">The command to execute.</param>
        protected override void CustomDo(IUserCommand pCommand)
        {
            // Do the command.
            pCommand.Do();
        }

        /// <summary>
        /// Delegate called when a command is done.
        /// </summary>
        /// <param name="pSource">The source command.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        protected override void CustomOnCommandDone(IUserCommand pSource, CommandExecutionEventArgs pEventArgs)
        {
            // Evaluating if the new command must be added in the context list.
            bool lAddCommand = true;
            if (this.CommandsList.Any() && this.LastExecutedCommand != null)
            {
                lAddCommand = this.LastExecutedCommand.TryMerge(pEventArgs.Command) == false;
            }

            // Adding the command in the list if wanted.
            if (lAddCommand)
            {
                this.CommandsList.Add(pEventArgs.Command);
                this.CurrentCommandIndex = this.CommandsList.Count - 1;
            }
        }

        #endregion // Methods.
    }
}

using CommandTest.UserCommand.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandTest.UserCommand
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
        /// <param name="pId">The user command context id.</param>
        public AutoTestCommandContext(string pId)
            : base(pId)
        {
        }

        #endregion // Constructors.

        #region Methods

        /// <summary>
        /// Registers and execute the given command.
        /// </summary>
        /// <param name="pCommand">The command to execute.</param>
        public override void Do(IUserCommand pCommand)
        {
            // Command validation.
            if (pCommand == null)
            {
                return;
            }

            // Do the command.
            pCommand.Do();

            // Evaluating if the new command must be added in the context list.
            bool lAddCommand = true;
            if (this.CommandsList.Any() && this.LastExecutedCommand != null)
            {
                lAddCommand = this.LastExecutedCommand.TryMerge(pCommand) == false;
            }

            // Adding the command in the list if wanted.
            if (lAddCommand)
            {
                this.CommandsList.Add(pCommand);
                this.CurrentCommandIndex = this.CommandsList.Count - 1;
            }
        }

        #endregion // Methods.
    }
}

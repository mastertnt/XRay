using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandTest.UserCommand.Service
{
    /// <summary>
    /// Class defining the event arguments of the <see cref="CommandExecutionEventHandler"/> delegate.
    /// </summary>
    public class CommandExecutionEventArgs
    {
        #region Properties

        /// <summary>
        /// Gets the command source.
        /// </summary>
        public IUserCommand Command
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the command progress evaluator.
        /// </summary>
        public IProgressEvaluator Progress
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the command failure raison.
        /// </summary>
        public string FailureRaison
        {
            get;
            private set;
        }

        #endregion // Properties.

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandExecutionEventArgs"/> class.
        /// </summary>
        /// <param name="pCommand">The executed command.</param>
        /// <param name="pProgress">The command progress evaluator.</param>
        /// <param name="pFailureRaison">The command failure raison.</param>
        public CommandExecutionEventArgs(IUserCommand pCommand, IProgressEvaluator pProgress, string pFailureRaison)
        {
            this.Progress = pProgress;
            this.FailureRaison = pFailureRaison;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandExecutionEventArgs"/> class.
        /// </summary>
        /// <param name="pCommand">The executed command.</param>
        /// <param name="pProgress">The command progress evaluator.</param>
        public CommandExecutionEventArgs(IUserCommand pCommand, IProgressEvaluator pProgress)
            : this(pCommand, pProgress, string.Empty)
        {
        }

        #endregion // Constructors.
    }
}

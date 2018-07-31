using System;

namespace XCommand
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
        /// Gets the command progress percentage.
        /// </summary>
        public double ProgressPercent
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the command failure raison.
        /// </summary>
        public Exception FailureRaison
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
        /// <param name="pProgressPercent">The command progress percentage.</param>
        /// <param name="pFailureRaison">The command failure raison.</param>
        public CommandExecutionEventArgs(IUserCommand pCommand, double pProgressPercent, Exception pFailureRaison)
        {
            this.ProgressPercent = pProgressPercent;
            this.FailureRaison = pFailureRaison;
            this.Command = pCommand;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandExecutionEventArgs"/> class.
        /// </summary>
        /// <param name="pCommand">The executed command.</param>
        /// <param name="pProgressPercent">The command progress percentage.</param>
        /// <param name="pFailureRaison">The command failure raison.</param>
        public CommandExecutionEventArgs(IUserCommand pCommand, double pProgressPercent, string pFailureRaison)
            : this(pCommand, pProgressPercent, new Exception(pFailureRaison))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandExecutionEventArgs"/> class.
        /// </summary>
        /// <param name="pCommand">The executed command.</param>
        /// <param name="pProgressPercent">The command progress percentage.</param>
        public CommandExecutionEventArgs(IUserCommand pCommand, double pProgressPercent)
            : this(pCommand, pProgressPercent, string.Empty)
        {
        }

        #endregion // Constructors.
    }
}

using System;

namespace XCommand
{
    /// <summary>
    /// Base class for the auto test commands.
    /// </summary>
    public abstract class AAutoTestCommand : AUserCommand, IAutoTestCommand
    {
        #region Properties

        /// <summary>
        /// Gets the command specific state.
        /// </summary>
        public override sealed UserCommandState State
        {
            get
            {
                return UserCommandState.Internal;
            }
        }

        #endregion // Properties.

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AAutoTestCommand"/> class.
        /// </summary>
        /// <param name="pTimeout">The timeout of the command.</param>
        protected AAutoTestCommand(TimeSpan pTimeout)
            : base(pTimeout)
        {
        }

        #endregion // Constructors.
    }
}

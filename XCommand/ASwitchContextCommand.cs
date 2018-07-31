using System;

namespace XCommand
{
    /// <summary>
    /// Base class for the commands able to switch the context.
    /// </summary>
    public abstract class ASwitchContextCommand : AUserCommand, ISwitchContextCommand
    {
        #region Properties

        /// <summary>
        /// Gets the command specific state.
        /// </summary>
        public override sealed UserCommandState State
        {
            get
            {
                return UserCommandState.SwitchContext;
            }
        }

        #endregion // Properties.

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ASwitchContextCommand"/> class.
        /// </summary>
        /// <param name="pTimeout">The timeout of the command.</param>
        protected ASwitchContextCommand(TimeSpan pTimeout)
            : base(pTimeout)
        {
        }

        #endregion // Constructors.

        #region Methods

        /// <summary>
        /// Evaluates the new context id depending on the old context id.
        /// </summary>
        /// <param name="pOldContextId">The old context id.</param>
        /// <returns>The new context id.</returns>
        public abstract string SwitchContext(string pOldContextId);

        #endregion // Methods.
    }
}


namespace XCommand
{
    /// <summary>
    /// Class defining an auto test command session.
    /// </summary>
    internal class AutoTestCommandSession : UserCommandSession
    {
        #region Constructors

        /// <summary>
        /// Initializes an instance of the <see cref="AutoTestCommandSession"/> class.
        /// </summary>
        /// <param name="pParentManager">The parent command manager.</param>
        public AutoTestCommandSession(UserCommandManager pParentManager)
            : base(pParentManager)
        {
        }

        #endregion // Constructors.

        #region Methods

        /// <summary>
        /// Creates a context with the given id.
        /// </summary>
        /// <param name="pParentManager">The parent command manager.</param>
        /// <param name="pId">The id of the new context.</param>
        /// <returns>The new context if a context having the same id does not have the same id, null otherwise.</returns>
        protected override UserCommandContext CustomCreateContext(UserCommandManager pParentManager, string pId)
        {
            return new AutoTestCommandContext(pParentManager, pId);
        }

        #endregion // Methods.
    }
}

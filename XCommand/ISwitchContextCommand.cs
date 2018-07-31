
namespace XCommand
{
    /// <summary>
    /// Interface defining a command able to switch context.
    /// </summary>
    public interface ISwitchContextCommand : IUserCommand
    {
        #region Methods

        /// <summary>
        /// Evaluates the new context id depending on the old context id.
        /// </summary>
        /// <param name="pOldContextId">The old context id.</param>
        /// <returns>The new context id.</returns>
        string SwitchContext(string pOldContextId);

        #endregion // Methods.
    }
}

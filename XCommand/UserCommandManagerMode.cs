
namespace XCommand
{
    /// <summary>
    /// Enum defining the mode the user command manager can be.
    /// </summary>
    public enum UserCommandManagerMode
    {
        /// <summary>
        /// Mode configuring the service to be used with the classic undo/redo mode.
        /// Default mode.
        /// </summary>
        User,

        /// <summary>
        /// Mode configuring the service to be used in an auto test context.
        /// All the commands are regsitered in order to be replayed in an auto test session.
        /// </summary>
        AutoTest
    }
}

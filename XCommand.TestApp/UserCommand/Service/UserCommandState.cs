using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandTest.UserCommand.Service
{
    /// <summary>
    /// Enum defining a user command specific state.
    /// </summary>
    public enum UserCommandState
    {
        /// <summary>
        /// State meaning the command can be undone. Default state.
        /// </summary>
        Undoable,

        /// <summary>
        /// State meaning the command is not executed by a user interaction. Test purpose for instance.
        /// The command is then not undoable.
        /// </summary>
        Internal,

        /// <summary>
        /// State meaning the command creates a new undo/redo session.
        /// The command is then not undoable.
        /// </summary>
        NewSession
    }
}

using CommandTest.UserCommand.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandTest.UserCommand
{
    /// <summary>
    /// Class defining an auto test command session.
    /// </summary>
    internal class AutoTestCommandSession : UserCommandSession
    {
        #region Methods

        /// <summary>
        /// Creates a context with the given id.
        /// </summary>
        /// <param name="pId">The id of the new context.</param>
        /// <returns>The new context if a context having the same id does not have the same id, null otherwise.</returns>
        protected override IUserCommandContext CustomCreateContext(string pId)
        {
            return new AutoTestCommandContext(pId);
        }

        #endregion // Methods.
    }
}

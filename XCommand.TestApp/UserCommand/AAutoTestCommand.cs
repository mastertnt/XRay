using CommandTest.UserCommand.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandTest.UserCommand
{
    /// <summary>
    /// Base class for the auto test commands.
    /// </summary>
    public abstract class AAutoTestCommand : AUserCommand, IAutoTestCommand
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AAutoTestCommand"/> class.
        /// </summary>
        /// <param name="pTimeout">The timeout of the command.</param>
        protected AAutoTestCommand(Time pTimeout)
            : base(pTimeout)
        {
        }

        #endregion // Constructors.

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
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandTest.UserCommand.Service
{
    /// <summary>
    /// Class defining the event arguments of the <see cref="SessionChangedEventHandler"/> delegate.
    /// </summary>
    public class SessionChangedEventArgs
    {
        #region Properties

        /// <summary>
        /// Gets the new context.
        /// </summary>
        public IUserCommandSession NewSession
        {
            get;
            private set;
        }
        
        /// <summary>
        /// Gets the old context.
        /// </summary>
        public IUserCommandSession OldSession
        {
            get;
            private set;
        }

        #endregion // Properties.

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ContextChangedEventArgs"/> class.
        /// </summary>
        /// <param name="pSession">The parent session.</param>
        /// <param name="pOldSession">The source command.</param>
        /// <param name="pNewSession">The command progress evaluator.</param>
        public SessionChangedEventArgs(IUserCommandSession pOldSession, IUserCommandSession pNewSession)
        {
            this.OldSession = pOldSession;
            this.NewSession = pNewSession;
        }

        #endregion // Constructors.
    }
}

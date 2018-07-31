using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandTest.UserCommand.Service
{
    /// <summary>
    /// Class defining the event arguments of the <see cref="ContextChangedEventHandler"/> delegate.
    /// </summary>
    public class ContextChangedEventArgs
    {
        #region Properties

        /// <summary>
        /// Gets the parent session modifying the context.
        /// </summary>
        public IUserCommandSession Session
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the new context.
        /// </summary>
        public IUserCommandContext NewContext
        {
            get;
            private set;
        }
        
        /// <summary>
        /// Gets the old context.
        /// </summary>
        public IUserCommandContext OldContext
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
        /// <param name="pOldContext">The old context.</param>
        /// <param name="pNewContext">The new context.</param>
        public ContextChangedEventArgs(IUserCommandSession pSession, IUserCommandContext pOldContext, IUserCommandContext pNewContext)
        {
            this.Session = pSession;
            this.OldContext = pOldContext;
            this.NewContext = pNewContext;
        }

        #endregion // Constructors.
    }
}

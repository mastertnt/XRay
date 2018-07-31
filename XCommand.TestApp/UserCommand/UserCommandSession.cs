using CommandTest.UserCommand.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandTest.UserCommand
{
    /// <summary>
    /// Class defining a user command session.
    /// </summary>
    internal class UserCommandSession : IUserCommandSession
    {
        #region Fields

        /// <summary>
        /// Stores the contextes of this session.
        /// </summary>
        private Dictionary<string, IUserCommandContext> mContextes;

        #endregion // Fields.

        #region Constructors

        /// <summary>
        /// Initializes an instance of the <see cref="UserCommandSession"/> class.
        /// </summary>
        public UserCommandSession()
        {
            this.mContextes = new Dictionary<string, IUserCommandContext>();
            this.SwitchContext(Constants.DEFAULT_CONTEXT_ID);
        }

        #endregion // Constructors.

        #region Methods

        /// <summary>
        /// Creates a context with the given id.
        /// </summary>
        /// <param name="pId">The id of the new context.</param>
        /// <returns>The new context if a context having the same id does not have the same id, null otherwise.</returns>
        private IUserCommandContext CreateContext(string pId)
        {
            if (this.mContextes.ContainsKey(pId) == false)
            {
                IUserCommandContext lContext = this.CustomCreateContext(pId);
                if (lContext != null)
                {
                    this.mContextes[pId] = lContext;
                    return lContext;
                }
            }

            return null;
        }

        /// <summary>
        /// Creates a context with the given id.
        /// </summary>
        /// <param name="pId">The id of the new context.</param>
        /// <returns>The new context if a context having the same id does not have the same id, null otherwise.</returns>
        protected virtual IUserCommandContext CustomCreateContext(string pId)
        {
            return new UserCommandContext(pId);
        }

        #endregion // Methods.

        #region IUserCommandSession implementation

        #region Properties

        /// <summary>
        /// Gets the current context of the session.
        /// </summary>
        public IUserCommandContext CurrentContext
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets the contextes of this session.
        /// </summary>
        public IEnumerable<IUserCommandContext> Contextes
        {
            get
            {
                return this.mContextes.Values;
            }
        }

        #endregion // Properties.

        #region Events

        /// <summary>
        /// Event raised when the context changed.
        /// </summary>
        public event ContextChangedEventHandler<IUserCommandSession> ContextChanged;

        #endregion // Events.

        #region Methods

        /// <summary>
        /// Returns a context by its id.
        /// </summary>
        /// <param name="pId">The context id.</param>
        /// <returns>The context if it exists, null otherwise.</returns>
        public IUserCommandContext GetContextById(string pId)
        {
            IUserCommandContext lContext;
            if (this.mContextes.TryGetValue(pId, out lContext) == true)
            {
                return lContext;
            }

            return null;
        }

        /// <summary>
        /// Makes the context having the given id the current one.
        /// If it doesn't exist, it is created.
        /// </summary>
        /// <param name="pNewContextId">The new context id.</param>
        /// <returns>The new current context.</returns>
        public IUserCommandContext SwitchContext(string pNewContextId)
        {
            if (this.CurrentContext != null && this.CurrentContext.Id == pNewContextId)
            {
                return this.CurrentContext;
            }

            IUserCommandContext lOldContext = this.CurrentContext;
            IUserCommandContext lNewContext = this.GetContextById(pNewContextId);
            if (lNewContext == null)
            {
                lNewContext = this.CreateContext(pNewContextId);
            }

            if (lNewContext == null)
            {
                // Do not change for a null context.
                return this.CurrentContext;
            }
            else
            {
                // Notifying the modification and updating the current context.
                this.CurrentContext = lNewContext;

                if (this.ContextChanged != null)
                {
                    this.ContextChanged(this, new ContextChangedEventArgs(this, lOldContext, lNewContext));
                }

                return this.CurrentContext;
            }
        }

        #endregion // Methods.

        #endregion // IUserCommandSession implementation.
    }
}

using System.Collections.Generic;

namespace XCommand
{
    /// <summary>
    /// Class defining a user command session.
    /// </summary>
    internal class UserCommandSession : IUserCommandSession
    {
        #region Fields

        /// <summary>
        /// Stores the parent command manager.
        /// </summary>
        private UserCommandManager mParentManager;

        /// <summary>
        /// Stores the contextes of this session.
        /// </summary>
        private Dictionary<string, UserCommandContext> mContextes;

        #endregion // Fields.

        #region Properties

        /// <summary>
        /// Gets the current context of the session.
        /// </summary>
        public UserCommandContext CurrentContext
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets the flag indicating if a command can be done.
        /// </summary>
        public bool CanDo
        {
            get
            {
                if (this.CurrentContext != null)
                {
                    return this.CurrentContext.CanDo;
                }

                return false;
            }
        }

        /// <summary>
        /// Gets the flag indicating if the context can redo a command.
        /// </summary>
        public bool CanRedo
        {
            get
            {
                if (this.CurrentContext != null)
                {
                    return this.CurrentContext.CanRedo;
                }

                return false;
            }
        }

        /// <summary>
        /// Gets the flag indicating if the context can undo a command.
        /// </summary>
        public bool CanUndo
        {
            get
            {
                if (this.CurrentContext != null)
                {
                    return this.CurrentContext.CanUndo;
                }

                return false;
            }
        }

        #endregion // Properties.

        #region Constructors

        /// <summary>
        /// Initializes an instance of the <see cref="UserCommandSession"/> class.
        /// </summary>
        /// <param name="pParentManager">The parent command manager.</param>
        public UserCommandSession(UserCommandManager pParentManager)
        {
            this.mParentManager = pParentManager;
            this.mContextes = new Dictionary<string, UserCommandContext>();
            this.SwitchContext(CommandConstants.DEFAULT_CONTEXT_ID);
        }

        #endregion // Constructors.

        #region Methods

        /// <summary>
        /// Creates a context with the given id.
        /// </summary>
        /// <param name="pId">The id of the new context.</param>
        /// <returns>The new context if a context having the same id does not have the same id, null otherwise.</returns>
        private UserCommandContext CreateContext(string pId)
        {
            // Validating the id.
            pId = pId.Trim();
            if (string.IsNullOrEmpty(pId))
            {
                return null;
            }

            if (this.mContextes.ContainsKey(pId) == false)
            {
                UserCommandContext lContext = this.CustomCreateContext(this.mParentManager, pId);
                if (lContext != null)
                {
                    this.mContextes[pId] = lContext;
                    return lContext;
                }
            }

            return null;
        }

        /// <summary>
        /// Returns a context by its id.
        /// </summary>
        /// <param name="pId">The context id.</param>
        /// <returns>The context if it exists, null otherwise.</returns>
        private UserCommandContext GetContextById(string pId)
        {
            UserCommandContext lContext;
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
        public UserCommandContext SwitchContext(string pNewContextId)
        {
            if (this.CurrentContext != null && this.CurrentContext.Id == pNewContextId)
            {
                return this.CurrentContext;
            }

            UserCommandContext lOldContext = this.CurrentContext;
            UserCommandContext lNewContext = this.GetContextById(pNewContextId);
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

                // Internal notification.
                ContextChangedEventArgs lEventArgs = new ContextChangedEventArgs(this, lOldContext, lNewContext);
                if (this.ContextChanged != null)
                {
                    this.ContextChanged(this, lEventArgs);
                }

                // Manager notification.
                this.mParentManager.NotifyContextChanged(lEventArgs);

                return this.CurrentContext;
            }
        }

        /// <summary>
        /// Creates a context with the given id.
        /// </summary>
        /// <param name="pParentManager">The parent command manager.</param>
        /// <param name="pId">The id of the new context.</param>
        /// <returns>The new context if a context having the same id does not have the same id, null otherwise.</returns>
        protected virtual UserCommandContext CustomCreateContext(UserCommandManager pParentManager, string pId)
        {
            return new UserCommandContext(pParentManager, pId);
        }

        /// <summary>
        /// Registers and execute the given command.
        /// </summary>
        /// <param name="pCommand">The command to execute.</param>
        public void Do(IUserCommand pCommand)
        {
            if (this.CurrentContext != null)
            {
                this.CurrentContext.Do(pCommand);
            }
        }

        /// <summary>
        /// Undo the current command of the context.
        /// </summary>
        public void Undo()
        {
            if (this.CurrentContext != null)
            {
                this.CurrentContext.Undo();
            }
        }

        /// <summary>
        /// Redo the current command of the context.
        /// </summary>
        public void Redo()
        {
            if (this.CurrentContext != null)
            {
                this.CurrentContext.Redo();
            }
        }

        #endregion // Methods.

        #region IUserCommandSession implementation

        #region Properties

        /// <summary>
        /// Gets the current context of the session.
        /// </summary>
        IUserCommandContext IUserCommandSession.CurrentContext
        {
            get
            {
                return this.CurrentContext;
            }
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
        IUserCommandContext IUserCommandSession.GetContextById(string pId)
        {
            return this.GetContextById(pId);
        }

        #endregion // Methods.

        #endregion // IUserCommandSession implementation.
    }
}

using System.Collections.ObjectModel;

namespace XCommand
{
    /// <summary>
    /// Class defining a <see cref="IUserCommandSession"/> list.
    /// </summary>
    internal class UserCommandSessionList : ObservableCollection<UserCommandSession>
    {
        #region Fields

        /// <summary>
        /// Stores the parent command manager.
        /// </summary>
        private UserCommandManager mParentManager;

        /// <summary>
        /// Stores the current session.
        /// </summary>
        private UserCommandSession mCurrentSession;

        #endregion // Fields.

        #region Properties

        /// <summary>
        /// Gets the current session.
        /// </summary>
        public UserCommandSession CurrentSession
        {
            get
            {
                return this.mCurrentSession;
            }

            protected set
            {
                UserCommandSession lOldSession = this.mCurrentSession;
                UserCommandSession lNewSession = value;
                if (lOldSession != lNewSession)
                {
                    this.mCurrentSession = lNewSession;
                    this.mParentManager.NotifySessionCreated(new SessionCreatedEventArgs(lOldSession, lNewSession));
                }
            }
        }

        #endregion // Properties.

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UserCommandSessionList"/> class.
        /// </summary>
        /// <param name="pParentManager">The parent command manager.</param>
        public UserCommandSessionList(UserCommandManager pParentManager)
        {
            this.mParentManager = pParentManager;
        }

        #endregion // Constructors.

        #region Methods

        /// <summary>
        /// Insert an item at the given index.
        /// </summary>
        /// <param name="pIndex">The index of the insertion.</param>
        /// <param name="pItem">The items to insert.</param>
        protected override void InsertItem(int pIndex, UserCommandSession pItem)
        {
            // Add has been called.
            if (pIndex == this.Count)
            {
                this.CurrentSession = pItem;
            }

            base.InsertItem(pIndex, pItem);
        }

        /// <summary>
        /// Clears all the items.
        /// </summary>
        protected override void ClearItems()
        {
            base.ClearItems();
            this.CurrentSession = null;
        }

        #endregion // Methods.
    }
}

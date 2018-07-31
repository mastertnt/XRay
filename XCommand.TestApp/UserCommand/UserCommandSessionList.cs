using CommandTest.UserCommand.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandTest.UserCommand
{
    /// <summary>
    /// Class defining a <see cref="IUserCommandSession"/> list.
    /// </summary>
    public class UserCommandSessionList : ObservableCollection<IUserCommandSession>
    {
        #region Fields

        /// <summary>
        /// Stores the current session.
        /// </summary>
        private IUserCommandSession mCurrentSession;

        #endregion // Fields.

        #region Properties

        /// <summary>
        /// Gets the current session.
        /// </summary>
        public IUserCommandSession CurrentSession
        {
            get
            {
                return this.mCurrentSession;
            }

            protected set
            {

            }
        }

        /// <summary>
        /// Gets the current context.
        /// </summary>
        public IUserCommandContext CurrentContext
        {
            get
            {
                if (this.CurrentSession != null)
                {
                    return this.CurrentSession.CurrentContext;
                }

                return null;
            }
        }

        #endregion // Properties.

        #region Events

        /// <summary>
        /// Event raised when a session changed.
        /// </summary>
        public SessionChangedEventHandler SessionChanged;

        #endregion // Events.

        #region Methods

        /// <summary>
        /// Insert an item at the given index.
        /// </summary>
        /// <param name="pIndex">The index of the insertion.</param>
        /// <param name="pItem">The items to insert.</param>
        protected override void InsertItem(int pIndex, IUserCommandSession pItem)
        {
            base.InsertItem(pIndex, pItem);

            // Add has been called.
            if (pIndex == this.Count)
            {
                this.CurrentSession = pItem;
            }
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

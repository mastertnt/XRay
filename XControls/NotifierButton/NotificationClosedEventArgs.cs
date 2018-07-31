using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XControls
{
    /// <summary>
    /// Class defining the event arguments of the <see cref="NotificationClosedEventHandler{TSource}"/> delegate.
    /// </summary>
    public class NotificationClosedEventArgs
    {
        #region Properties

        /// <summary>
        /// Gets the identifier of the notification.
        /// </summary>
        public string NotificationId
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the notification answer.
        /// </summary>
        public Answers Answer
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the flag indicating if the notification has been closed because of a timeout.
        /// </summary>
        public bool Timeout
        {
            get;
            private set;
        }

        #endregion // Properties.

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationClosedEventArgs"/> class.
        /// </summary>
        /// <param name="pNotificationId">The id of the notification.</param>
        /// <param name="pAnswer">The returned answer.</param>
        /// <param name="pTimeout">Flag indicating if the notification has been closed because of a timeout.</param>
        public NotificationClosedEventArgs(string pNotificationId, Answers pAnswer, bool pTimeout)
        {
            this.NotificationId = pNotificationId;
            this.Answer = pAnswer;
            this.Timeout = pTimeout;
        }

        #endregion // Constructors.
    }
}

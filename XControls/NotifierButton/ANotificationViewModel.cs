using System.ComponentModel;
using System.Timers;

namespace XControls.NotifierButton
{
    /// <summary>
    /// Base class defining a notification view model.
    /// </summary>
    public abstract class ANotificationViewModel : INotifyPropertyChanged
    {
        #region Fields

        /// <summary>
        /// Stores the life timer.
        /// </summary>
        private Timer mLifeTimer;

        /// <summary>
        /// Stores the notification quick style.
        /// </summary>
        private NotificationQuickStyle mQuickStyle;

        #endregion // Fields.

        #region Properties

        /// <summary>
        /// Gets the type of the notification.
        /// </summary>
        public string Type
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the identifier of the notification.
        /// </summary>
        public string Id
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the index of this notification.
        /// </summary>
        public int Index
        {
            get
            {
                if (this.Parent != null)
                {
                    return this.Parent.Notifications.IndexOf(this) + 1;
                }

                return -1;
            }
        }

        /// <summary>
        /// Gets or sets the message title.
        /// </summary>
        public string Title
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the message to display.
        /// </summary>
        public string Message
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the default answer for this notification.
        /// </summary>
        public Answers DefaultAnswer
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the life timeout of the notification in seconds.
        /// Notification will stay alive until user answer if the timeout is 0.0.
        /// </summary>
        public double LifeTimeout
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the notification quick style.
        /// </summary>
        public NotificationQuickStyle QuickStyle
        {
            get
            {
                return this.mQuickStyle;
            }

            set
            {
                this.mQuickStyle = value;
                this.NotifyPropertyChanged("QuickStyle");
            }
        }

        /// <summary>
        /// Gets or sets the boolean indicating if the notification has to be shown when raised.
        /// </summary>
        public bool ShowOnRaised
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the parent view model.
        /// </summary>
        internal NotifierViewModel Parent
        {
            get;
            set;
        }

        #endregion // Properties.
        
        #region Events
        
        /// <summary>
        /// Event raised when a property changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion // Events.

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ANotificationViewModel"/> class.
        /// </summary>
        /// <param name="pType">The notification type.</param>
        /// <param name="pId">The notification id.</param>
        protected ANotificationViewModel(string pType, string pId)
        {
            this.Type = pType;
            this.Id = pId;
            this.LifeTimeout = 0.0;
            this.ShowOnRaised = false;

            this.Title = string.Empty;
            this.Message = string.Empty;
        }

        #endregion // Constructors.

        #region Methods

        /// <summary>
        /// Convert this object to a string description.
        /// </summary>
        /// <returns>The string description.</returns>
        public override string ToString()
        {
            return "Notification : " + this.Message;
        }

        /// <summary>
        /// Tries to start the timer to handle time out if the notification timeout is defined.
        /// </summary>
        internal void TryToStartTimeout()
        {
            if (this.LifeTimeout > 0.0)
            {
                this.mLifeTimer = new Timer();
                this.mLifeTimer.Elapsed += this.OnLifeTimerElapsed;
                this.mLifeTimer.AutoReset = false;
                this.mLifeTimer.Interval = this.LifeTimeout * 1000.0;
                this.mLifeTimer.Start();
            }
        }

        /// <summary>
        /// Tries to stop the life timer if it exists.
        /// </summary>
        internal void TryToStopLifeTimer()
        {
            if (this.mLifeTimer != null)
            {
                this.mLifeTimer.Stop();
            }
        }

        /// <summary>
        /// Delegate called when the timer ellapsed.
        /// </summary>
        /// <param name="pSender">The timer sender.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private void OnLifeTimerElapsed(object pSender, ElapsedEventArgs pEventArgs)
        {
            if (this.Parent != null)
            {
                lock (this.Parent)
                {
                    this.Parent.CloseNotification(this, this.DefaultAnswer, true);
                }
            }
        }

        /// <summary>
        /// Notifies a property mofication.
        /// </summary>
        /// <param name="pPropertyName">The name of the modified property.</param>
        protected void NotifyPropertyChanged(string pPropertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(pPropertyName));
            }
        }

        /// <summary>
        /// Cleans the view model.
        /// </summary>
        protected internal virtual void Clean()
        {
            // Nothing to do.
        }

        #endregion // Methods.
    }
}

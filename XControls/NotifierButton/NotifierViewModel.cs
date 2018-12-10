using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Timers;

namespace XControls.NotifierButton
{
    /// <summary>
    /// Class defining a notification view model.
    /// </summary>
    public class NotifierViewModel : INotifyPropertyChanged
    {
        #region Fields

        /// <summary>
        /// Stores the parent button.
        /// </summary>
        private NotifierButton mParentButton;

        /// <summary>
        /// Stores the displayed notification.
        /// </summary>
        private ANotificationViewModel mDisplayedNotification;

        /// <summary>
        /// Stores the notifications handled by a timer.
        /// </summary>
        private Dictionary<ANotificationViewModel, Timer> mNotifToTimerMap;

        /// <summary>
        /// Stores the message view height.
        /// </summary>
        private double mMessageViewHeight;

        /// <summary>
        /// Stores the message view width.
        /// </summary>
        private double mMessageViewWidth;

        #endregion // Fields.

        #region Properties

        /// <summary>
        /// Gets or sets the notifications to display.
        /// </summary>
        public ObservableCollection<ANotificationViewModel> Notifications
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the displayed notification.
        /// </summary>
        public ANotificationViewModel DisplayedNotification
        {
            get
            {
                return this.mDisplayedNotification;
            }

            set
            {
                if (this.mDisplayedNotification != value)
                {
                    this.mDisplayedNotification = value;

                    this.NotifyPropertyChanged("DisplayedNotification");
                    this.NotifyPropertyChanged("CanGoNext");
                    this.NotifyPropertyChanged("CanGoPrevious");
                }
            }
        }

        /// <summary>
        /// Gets the flag indicating if the next notification can be displayed.
        /// </summary>
        public bool CanGoNext
        {
            get
            {
                if (this.Notifications.Any())
                {
                    int lCurrentIndex = this.Notifications.IndexOf(this.DisplayedNotification);
                    return lCurrentIndex < this.Notifications.Count - 1;
                }

                return false;
            }
        }

        /// <summary>
        /// Gets the flag indicating if the previous notification can be displayed.
        /// </summary>
        public bool CanGoPrevious
        {
            get
            {
                if (this.Notifications.Any())
                {
                    int lCurrentIndex = this.Notifications.IndexOf(this.DisplayedNotification);
                    return lCurrentIndex > 0;
                }

                return false;
            }
        }

        /// <summary>
        /// Gets or sets the message view height.
        /// </summary>
        public double MessageViewHeight
        {
            get
            {
                return this.mMessageViewHeight;
            }

            set
            {
                if (this.mMessageViewHeight != value)
                {
                    this.mMessageViewHeight = value;
                    this.NotifyPropertyChanged("MessageViewHeight");
                }
            }
        }

        /// <summary>
        /// Gets or sets the message view width.
        /// </summary>
        public double MessageViewWidth
        {
            get
            {
                return this.mMessageViewWidth;
            }

            set
            {
                if (this.mMessageViewWidth != value)
                {
                    this.mMessageViewWidth = value;
                    this.NotifyPropertyChanged("MessageViewWidth");
                }
            }
        }

        #endregion // Properties.

        #region Events

        /// <summary>
        /// Event raised when a property changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Event raised when a notification is closed.
        /// </summary>
        public event NotificationClosedEventHandler<NotifierViewModel> NotificationClosed;

        #endregion // Events.

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NotifierViewModel"/> class.
        /// </summary>
        /// <param name="pParentControl">The parent control.</param>
        public NotifierViewModel(NotifierButton pParentControl)
        {
            this.mParentButton = pParentControl;
            this.mNotifToTimerMap = new Dictionary<ANotificationViewModel, Timer>();
            this.Notifications = new ObservableCollection<ANotificationViewModel>();
            this.DisplayedNotification = null;

            this.MessageViewWidth = 200;
            this.MessageViewHeight = 125;
        }

        #endregion // Constructors.

        #region Methods

        /// <summary>
        /// Adds a notification in the list.
        /// </summary>
        /// <param name="pNotification">The notification to add.</param>
        public void AddNotification(ANotificationViewModel pNotification)
        {
            if (pNotification == null)
            {
                return;
            }

            pNotification.Parent = this;
            this.Notifications.Add(pNotification);

            if (this.DisplayedNotification == null)
            {
                this.DisplayedNotification = pNotification;
            }

            if (pNotification.ShowOnRaised)
            {
                this.DisplayedNotification = pNotification;
                this.mParentButton.Dispatcher.Invoke((Action)delegate
                {
                    this.mParentButton.IsOpen = true;
                });
            }

            pNotification.TryToStartTimeout();

            this.NotifyPropertyChanged("CanGoNext");
            this.NotifyPropertyChanged("CanGoPrevious");
        }

        /// <summary>
        /// Closes a notification.
        /// </summary>
        /// <param name="pNotification">The notification to close.</param>
        /// <param name="pAnswer">The notification answer.</param>
        /// <param name="pTimeout">Flag indicating if the notification has been closed because of a timeout.</param>
        internal void CloseNotification(ANotificationViewModel pNotification, Answers pAnswer, bool pTimeout)
        {
            if (pNotification != null)
            {
                int lCurrentIndex = this.Notifications.IndexOf(pNotification);
                if (this.Notifications.Remove(pNotification))
                {
                    pNotification.Parent = null;
                    pNotification.TryToStopLifeTimer();

                    if (this.Notifications.Any() == false)
                    {
                        this.DisplayedNotification = null;
                    }
                    else if (lCurrentIndex < this.Notifications.Count)
                    {
                        // Next notification is the one added after the closed one.
                        this.DisplayedNotification = this.Notifications[lCurrentIndex];
                    }
                    else if (lCurrentIndex > this.Notifications.Count - 1)
                    {
                        // Or the last if any notification is after the closed one.
                        this.DisplayedNotification = this.Notifications[this.Notifications.Count - 1];
                    }

                    // Cleaning the notification.
                    pNotification.Clean();

                    this.NotifyNotificationClosed(pNotification.Id, pAnswer, pTimeout);
                }
            }
        }

        /// <summary>
        /// Notifies a notification gets closed.
        /// </summary>
        /// <param name="pNotificationId">The notification to close.</param>
        /// <param name="pAnswer">The notification answer.</param>
        /// <param name="pTimeout">Flag indicating if the notification has been closed because of a timeout.</param>
        private void NotifyNotificationClosed(string pNotificationId, Answers pAnswer, bool pTimeout)
        {
            if (this.NotificationClosed != null)
            {
                this.NotificationClosed(this, new NotificationClosedEventArgs(pNotificationId, pAnswer, pTimeout));
            }
        }

        /// <summary>
        /// Makes the next notification as the displayed one.
        /// </summary>
        internal void DisplayNextNotification()
        {
            if (this.CanGoNext)
            {
                int lCurrentIndex = this.Notifications.IndexOf(this.DisplayedNotification);
                this.DisplayedNotification = this.Notifications[lCurrentIndex + 1];
            }
        }

        /// <summary>
        /// Makes the previous notification as the displayed one.
        /// </summary>
        internal void DisplayPreviousNotification()
        {
            if (this.CanGoPrevious)
            {
                int lCurrentIndex = this.Notifications.IndexOf(this.DisplayedNotification);
                this.DisplayedNotification = this.Notifications[lCurrentIndex - 1];
            }
        }

        /// <summary>
        /// Notifies a property mofication.
        /// </summary>
        /// <param name="pPropertyName">The name of the modified property.</param>
        private void NotifyPropertyChanged(string pPropertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(pPropertyName));
            }
        }

        #endregion // Methods.
    }
}

using System;
using System.Windows;
using System.Windows.Data;

namespace XControls.NotifierButton
{
    /// <summary>
    /// Class defining a notifier button displaying notifications in a popup.
    /// </summary>
    [TemplatePart(Name = PART_NOTIFICATION_COUNT_RENDERER, Type = typeof(NotificationCountRenderer))]
    public class NotifierButton : DropDownButton.DropDownButton
    {
        #region Dependencies

        /// <summary>
        /// Identifies the NotificationCount dependency property.
        /// </summary>
        internal static readonly DependencyProperty NotificationCountProperty = DependencyProperty.Register("NotificationCount", typeof(int), typeof(NotifierButton), new UIPropertyMetadata(0, OnNotificationCountChanged));

        #endregion // Dependencies.

        #region Fields

        /// <summary>
        /// Name of the parts that have to be in the control template.
        /// </summary>
        private const string PART_NOTIFICATION_COUNT_RENDERER = "PART_NotificationCountRenderer";

        /// <summary>
        /// Stores the notification count renderer.
        /// </summary>
        private NotificationCountRenderer mNotificationCountRenderer;

        #endregion // Fields.

        #region Properties

        /// <summary>
        /// Gets the notifier view model.
        /// </summary>
        public NotifierViewModel ViewModel
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the notification count.
        /// </summary>
        internal int NotificationCount
        {
            get
            {
                return (int) this.GetValue(NotificationCountProperty);
            }
            set
            {
                this.SetValue(NotificationCountProperty, value);
            }
        }

        #endregion // Properties.

        #region Events

        /// <summary>
        /// Event raised when a notification is closed.
        /// </summary>
        public event NotificationClosedEventHandler<NotifierButton> NotificationClosed;

        #endregion // Events.

        #region Constructors

        /// <summary>
        /// Initializes the <see cref="NotifierButton"/> class.
        /// </summary>
        static NotifierButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NotifierButton), new FrameworkPropertyMetadata(typeof(NotifierButton)));
            IsOpenProperty.OverrideMetadata(typeof(NotifierButton), new FrameworkPropertyMetadata(false, OnIsOpenChanged));
        }

        /// <summary>
        /// Initializes an instance of the <see cref="NotifierButton"/> class.
        /// </summary>
        public NotifierButton()
        {
            this.ViewModel = new NotifierViewModel(this);
            this.ViewModel.NotificationClosed += this.OnViewModelNotificationClosed;

            // Scroller is the main control of the drop down window.
            NotificationScroller lScroller = new NotificationScroller();
            lScroller.DataContext = this.ViewModel;

            Binding lNotificationCountBinding = new Binding("Notifications.Count");
            lNotificationCountBinding.Source = this.ViewModel;
            this.SetBinding(NotificationCountProperty, lNotificationCountBinding);

            this.DropDownContent = lScroller;

            this.UpdateState(this.NotificationCount, this.NotificationCount);
        }

        #endregion // Constructors.

        #region Methods

        /// <summary>
        /// Method called when the template of the control is applied.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.mNotificationCountRenderer = this.GetTemplateChild(PART_NOTIFICATION_COUNT_RENDERER) as NotificationCountRenderer;

            if (this.mNotificationCountRenderer == null)
            {
                throw new ArgumentException("The NotifierButton template is not valid.");
            }

            this.mNotificationCountRenderer.DataContext = this.ViewModel;

            this.UpdateState(this.NotificationCount, this.NotificationCount);
        }

        /// <summary>
        /// Delegate called when the notification count property changed.
        /// </summary>
        /// <param name="pObject">The modified object.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private static void OnNotificationCountChanged(DependencyObject pObject, DependencyPropertyChangedEventArgs pEventArgs)
        {
            NotifierButton lControl = pObject as NotifierButton;
            if (lControl != null)
            {
                lControl.UpdateState((int)pEventArgs.OldValue, (int)pEventArgs.NewValue);
            }
        }

        /// <summary>
        /// Updates the button state.
        /// </summary>
        /// <param name="pOldNotificationCount">The old notification count.</param>
        /// <param name="pNewNotificationCount">The new notification count.</param>
        private void UpdateState(int pOldNotificationCount, int pNewNotificationCount)
        {
            if (this.mNotificationCountRenderer != null && this.IsOpen == false && pNewNotificationCount == pOldNotificationCount + 1)
            {
                this.mNotificationCountRenderer.Blink = true;
            }

            if (this.NotificationCount <= 0)
            {
                this.IsEnabled = false;

                // Closing the popup window.
                this.IsOpen = false;
            }
            else
            {
                this.IsEnabled = true;
            }
        }

        /// <summary>
        /// Delegate called when a notification is closed.
        /// </summary>
        /// <param name="pSource">The source view model.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private void OnViewModelNotificationClosed(NotifierViewModel pSource, NotificationClosedEventArgs pEventArgs)
        {
            this.Dispatcher.Invoke((Action)delegate
            {
                if (this.NotificationClosed != null)
                {
                    this.NotificationClosed(this, pEventArgs);
                } 
            });
        }

        /// <summary>
        /// Delegate called when the is open property changed.
        /// </summary>
        /// <param name="pObject">The modified object.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private static void OnIsOpenChanged(DependencyObject pObject, DependencyPropertyChangedEventArgs pEventArgs)
        {
            NotifierButton lControl = pObject as NotifierButton;
            if (lControl != null && (bool)pEventArgs.NewValue)
            {
                lControl.mNotificationCountRenderer.Blink = false;
            }
        }

        #endregion // Methods.
    }
}

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace XControls.NotifierButton
{
    /// <summary>
    /// Class defining the notification scroller displayed in the popup.
    /// </summary>
    [TemplatePart(Name = PART_PREVIOUS_NOTIFICATION_BUTTON, Type = typeof(Button))]
    [TemplatePart(Name = PART_NEXT_NOTIFICATION_BUTTON, Type = typeof(Button))]
    public class NotificationScroller : ContentControl
    {
        #region Dependencies

        /// <summary>
        /// Identifies the MessageViewHeight dependency property.
        /// </summary>
        public static readonly DependencyProperty MessageViewHeightProperty = DependencyProperty.Register("MessageViewHeight", typeof(double), typeof(NotificationScroller), new UIPropertyMetadata(double.NaN));

        /// <summary>
        /// Identifies the MessageViewWidth dependency property.
        /// </summary>
        public static readonly DependencyProperty MessageViewWidthProperty = DependencyProperty.Register("MessageViewWidth", typeof(double), typeof(NotificationScroller), new UIPropertyMetadata(double.NaN));

        #endregion // Dependencies.

        #region Fields

        /// <summary>
        /// Name of the parts that have to be in the control template.
        /// </summary>
        private const string PART_PREVIOUS_NOTIFICATION_BUTTON = "PART_PreviousNotificationButton";
        private const string PART_NEXT_NOTIFICATION_BUTTON = "PART_NextNotificationButton";

        /// <summary>
        /// Stores the button used to go to the previous notification.
        /// </summary>
        private Button mPreviousNotificationButton;

        /// <summary>
        /// Stores the button used to go to the next notification.
        /// </summary>
        private Button mNextNotificationButton;

        #endregion // Fields.

        #region Properties

        /// <summary>
        /// Gets the notifier view model.
        /// </summary>
        public NotifierViewModel ViewModel
        {
            get
            {
                return this.DataContext as NotifierViewModel;
            }
        }

        /// <summary>
        /// Gets or sets the message view height.
        /// </summary>
        public double MessageViewHeight
        {
            get
            {
                return (int) this.GetValue(MessageViewHeightProperty);
            }
            set
            {
                this.SetValue(MessageViewHeightProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the message view width.
        /// </summary>
        public double MessageViewWidth
        {
            get
            {
                return (int) this.GetValue(MessageViewWidthProperty);
            }
            set
            {
                this.SetValue(MessageViewWidthProperty, value);
            }
        }

        #endregion // Properties.

        #region Constructors

        /// <summary>
        /// Initializes the <see cref="NotificationScroller"/> class.
        /// </summary>
        static NotificationScroller()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NotificationScroller), new FrameworkPropertyMetadata(typeof(NotificationScroller)));
            DataContextProperty.OverrideMetadata(typeof(NotificationScroller), new FrameworkPropertyMetadata(null, OnDataContextChanged));
        }

        /// <summary>
        /// Initializes an instance of the <see cref="NotificationScroller"/> class.
        /// </summary>
        public NotificationScroller()
        {
        }

        #endregion // Constructors.

        #region Methods

        /// <summary>
        /// Delegate called when the data context property changed.
        /// </summary>
        /// <param name="pObject">The modified object.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private static void OnDataContextChanged(DependencyObject pObject, DependencyPropertyChangedEventArgs pEventArgs)
        {
            NotificationScroller lControl = pObject as NotificationScroller;
            if (lControl != null)
            {
                Binding lContentBinding = new Binding("DisplayedNotification");
                lContentBinding.Source = pEventArgs.NewValue;
                lControl.SetBinding(ContentProperty, lContentBinding);

                Binding lMessageViewHeightBinding = new Binding("MessageViewHeight");
                lMessageViewHeightBinding.Source = pEventArgs.NewValue;
                lControl.SetBinding(MessageViewHeightProperty, lMessageViewHeightBinding);

                Binding lMessageViewWidthBinding = new Binding("MessageViewWidth");
                lMessageViewWidthBinding.Source = pEventArgs.NewValue;
                lControl.SetBinding(MessageViewWidthProperty, lMessageViewWidthBinding);
            }
        }

        /// <summary>
        /// Method called when the template of the control is applied.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.mPreviousNotificationButton = this.GetTemplateChild(PART_PREVIOUS_NOTIFICATION_BUTTON) as Button;
            this.mNextNotificationButton = this.GetTemplateChild(PART_NEXT_NOTIFICATION_BUTTON) as Button;

            if  (   (this.mPreviousNotificationButton == null)
                ||  (this.mNextNotificationButton == null)
                )
            {
                throw new ArgumentException("The NotificationScroller template is not valid.");
            }

            this.mPreviousNotificationButton.Click += this.OnPreviousNotificationButtonClicked;

            Binding lCanGoPreviousBinding = new Binding("CanGoPrevious");
            lCanGoPreviousBinding.Source = this.ViewModel;
            lCanGoPreviousBinding.Mode = BindingMode.OneWay;
            this.mPreviousNotificationButton.SetBinding(IsEnabledProperty, lCanGoPreviousBinding);
            
            this.mNextNotificationButton.Click += this.OnNextNotificationButtonClicked;

            Binding lCanGoNextBinding = new Binding("CanGoNext");
            lCanGoNextBinding.Source = this.ViewModel;
            lCanGoNextBinding.Mode = BindingMode.OneWay;
            this.mNextNotificationButton.SetBinding(IsEnabledProperty, lCanGoNextBinding);
        }

        /// <summary>
        /// Delegate called when the previous notification button is clicked.
        /// </summary>
        /// <param name="pSender">The button sender.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private void OnPreviousNotificationButtonClicked(object pSender, RoutedEventArgs pEventArgs)
        {
            if (this.ViewModel != null)
            {
                this.ViewModel.DisplayPreviousNotification();
            }
        }

        /// <summary>
        /// Delegate called when the previous notification button is clicked.
        /// </summary>
        /// <param name="pSender">The button sender.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private void OnNextNotificationButtonClicked(object pSender, RoutedEventArgs pEventArgs)
        {
            if (this.ViewModel != null)
            {
                this.ViewModel.DisplayNextNotification();
            }
        }

        #endregion // Methods.
    }
}

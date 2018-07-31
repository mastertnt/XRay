using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace XControls
{
    /// <summary>
    /// Class defining the notification scroller displayed in the popup.
    /// </summary>
    [TemplatePart(Name = PART_PreviousNotificationButton, Type = typeof(Button))]
    [TemplatePart(Name = PART_NextNotificationButton, Type = typeof(Button))]
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
        private const string PART_PreviousNotificationButton = "PART_PreviousNotificationButton";
        private const string PART_NextNotificationButton = "PART_NextNotificationButton";

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
                return (int)GetValue(MessageViewHeightProperty);
            }
            set
            {
                SetValue(MessageViewHeightProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the message view width.
        /// </summary>
        public double MessageViewWidth
        {
            get
            {
                return (int)GetValue(MessageViewWidthProperty);
            }
            set
            {
                SetValue(MessageViewWidthProperty, value);
            }
        }

        #endregion // Properties.

        #region Constructors

        /// <summary>
        /// Initializes the <see cref="NotificationScroller"/> class.
        /// </summary>
        static NotificationScroller()
        {
            NotificationScroller.DefaultStyleKeyProperty.OverrideMetadata(typeof(NotificationScroller), new FrameworkPropertyMetadata(typeof(NotificationScroller)));
            NotificationScroller.DataContextProperty.OverrideMetadata(typeof(NotificationScroller), new FrameworkPropertyMetadata(null, OnDataContextChanged));
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
                lControl.SetBinding(NotificationScroller.ContentProperty, lContentBinding);

                Binding lMessageViewHeightBinding = new Binding("MessageViewHeight");
                lMessageViewHeightBinding.Source = pEventArgs.NewValue;
                lControl.SetBinding(NotificationScroller.MessageViewHeightProperty, lMessageViewHeightBinding);

                Binding lMessageViewWidthBinding = new Binding("MessageViewWidth");
                lMessageViewWidthBinding.Source = pEventArgs.NewValue;
                lControl.SetBinding(NotificationScroller.MessageViewWidthProperty, lMessageViewWidthBinding);
            }
        }

        /// <summary>
        /// Method called when the template of the control is applied.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.mPreviousNotificationButton = this.GetTemplateChild(PART_PreviousNotificationButton) as Button;
            this.mNextNotificationButton = this.GetTemplateChild(PART_NextNotificationButton) as Button;

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
            this.mPreviousNotificationButton.SetBinding(Button.IsEnabledProperty, lCanGoPreviousBinding);
            
            this.mNextNotificationButton.Click += this.OnNextNotificationButtonClicked;

            Binding lCanGoNextBinding = new Binding("CanGoNext");
            lCanGoNextBinding.Source = this.ViewModel;
            lCanGoNextBinding.Mode = BindingMode.OneWay;
            this.mNextNotificationButton.SetBinding(Button.IsEnabledProperty, lCanGoNextBinding);
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

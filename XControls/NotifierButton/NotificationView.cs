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
    /// Class defining a notification view.
    /// </summary>
    [TemplatePart(Name = PART_OkButton, Type = typeof(Button))]
    [TemplatePart(Name = PART_CancelButton, Type = typeof(Button))]
    [TemplatePart(Name = PART_YesButton, Type = typeof(Button))]
    [TemplatePart(Name = PART_NoButton, Type = typeof(Button))]
    [TemplatePart(Name = PART_CancelButtonFistColumn, Type = typeof(ColumnDefinition))]
    [TemplatePart(Name = PART_CancelButtonSecondColumn, Type = typeof(ColumnDefinition))]
    [TemplatePart(Name = PART_MessageScrollViewer, Type = typeof(ScrollViewer))]
    public class NotificationView : HeaderedContentControl
    {
        #region Dependencies

        /// <summary>
        /// Identifies the QuickStyle dependency property.
        /// </summary>
        internal static readonly DependencyProperty QuickStyleProperty = DependencyProperty.Register("QuickStyle", typeof(NotificationQuickStyle), typeof(NotificationView), new UIPropertyMetadata(null, OnQuickStyleChanged));

        /// <summary>
        /// Identifies the YesButtonStyle dependency property.
        /// </summary>
        internal static readonly DependencyProperty YesButtonStyleProperty = DependencyProperty.Register("YesButtonStyle", typeof(Style), typeof(NotificationView), new UIPropertyMetadata(null, OnYesButtonStyleChanged));

        /// <summary>
        /// Identifies the NoButtonStyle dependency property.
        /// </summary>
        internal static readonly DependencyProperty NoButtonStyleProperty = DependencyProperty.Register("NoButtonStyle", typeof(Style), typeof(NotificationView), new UIPropertyMetadata(null, OnNoButtonStyleChanged));

        /// <summary>
        /// Identifies the CancelButtonStyle dependency property.
        /// </summary>
        internal static readonly DependencyProperty CancelButtonStyleProperty = DependencyProperty.Register("CancelButtonStyle", typeof(Style), typeof(NotificationView), new UIPropertyMetadata(null, OnCancelButtonStyleChanged));

        /// <summary>
        /// Identifies the NoButtonStyle dependency property.
        /// </summary>
        internal static readonly DependencyProperty OkButtonStyleProperty = DependencyProperty.Register("OkButtonStyle", typeof(Style), typeof(NotificationView), new UIPropertyMetadata(null, OnOkButtonStyleChanged));

        #endregion // Dependencies.

        #region Fields

        /// <summary>
        /// Name of the parts that have to be in the control template.
        /// </summary>
        private const string PART_OkButton = "PART_OkButton";
        private const string PART_CancelButton = "PART_CancelButton";
        private const string PART_YesButton = "PART_YesButton";
        private const string PART_NoButton = "PART_NoButton";
        private const string PART_CancelButtonFistColumn = "PART_CancelButtonFistColumn";
        private const string PART_CancelButtonSecondColumn = "PART_CancelButtonSecondColumn";
        private const string PART_MessageScrollViewer = "PART_MessageScrollViewer";

        /// <summary>
        /// Stores the ok button.
        /// </summary>
        private Button mOkButton;

        /// <summary>
        /// Stores the cancel button.
        /// </summary>
        private Button mCancelButton;

        /// <summary>
        /// Stores the yes button.
        /// </summary>
        private Button mYesButton;

        /// <summary>
        /// Stores the no button.
        /// </summary>
        private Button mNoButton;

        /// <summary>
        /// Stores the first column handling the cancel button display.
        /// </summary>
        private ColumnDefinition mCancelButtonFistColumn;

        /// <summary>
        /// Stores the second column handling the cancel button display.
        /// </summary>
        private ColumnDefinition mCancelButtonSecondColumn;

        /// <summary>
        /// Stores the scroll viewer handling the message display.
        /// </summary>
        private ScrollViewer mMessageScrollViewer;

        #endregion // Fields.

        #region Properties

        /// <summary>
        /// Gets the notification view model.
        /// </summary>
        public ANotificationViewModel ViewModel
        {
            get
            {
                return this.DataContext as ANotificationViewModel;
            }
        }

        /// <summary>
        /// Gets or sets the quick style.
        /// </summary>
        internal NotificationQuickStyle QuickStyle
        {
            get
            {
                return (NotificationQuickStyle)GetValue(QuickStyleProperty);
            }
            set
            {
                SetValue(QuickStyleProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the yes button style.
        /// </summary>
        internal Style YesButtonStyle
        {
            get
            {
                return (Style)GetValue(YesButtonStyleProperty);
            }
            set
            {
                SetValue(YesButtonStyleProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the no button style.
        /// </summary>
        internal Style NoButtonStyle
        {
            get
            {
                return (Style)GetValue(NoButtonStyleProperty);
            }
            set
            {
                SetValue(NoButtonStyleProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the cancel button style.
        /// </summary>
        internal Style CancelButtonStyle
        {
            get
            {
                return (Style)GetValue(CancelButtonStyleProperty);
            }
            set
            {
                SetValue(CancelButtonStyleProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the ok button style.
        /// </summary>
        internal Style OkButtonStyle
        {
            get
            {
                return (Style)GetValue(OkButtonStyleProperty);
            }
            set
            {
                SetValue(OkButtonStyleProperty, value);
            }
        }

        #endregion // Properties.

        #region Constructors

        /// <summary>
        /// Initializes the <see cref="NotificationView"/> class.
        /// </summary>
        static NotificationView()
        {
            NotificationView.DefaultStyleKeyProperty.OverrideMetadata(typeof(NotificationView), new FrameworkPropertyMetadata(typeof(NotificationView)));
            NotificationView.DataContextProperty.OverrideMetadata(typeof(NotificationView), new FrameworkPropertyMetadata(null, OnDataContextChanged));
        }

        /// <summary>
        /// Initializes an instance of the <see cref="NotificationView"/> class.
        /// </summary>
        public NotificationView()
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
            NotificationView lControl = pObject as NotificationView;
            if (lControl != null)
            {
                BindingOperations.ClearAllBindings(lControl);

                ANotificationViewModel lNotificationViewModel = pEventArgs.NewValue as ANotificationViewModel;
                if (lNotificationViewModel != null)
                {
                    Binding lHeaderBinding = new Binding();
                    lHeaderBinding.Source = lControl.ViewModel;
                    lControl.SetBinding(NotificationView.HeaderProperty, lHeaderBinding);

                    Binding lQuickStyleBinding = new Binding("QuickStyle");
                    lQuickStyleBinding.Source = lControl.ViewModel;
                    lControl.SetBinding(NotificationView.QuickStyleProperty, lQuickStyleBinding);

                    QuestionViewModel lQuestionViewModel = lNotificationViewModel as QuestionViewModel;
                    if (lQuestionViewModel != null)
                    {
                        Binding lYesButtonStyleBinding = new Binding("YesButtonStyle");
                        lYesButtonStyleBinding.Source = lControl.ViewModel;
                        lControl.SetBinding(NotificationView.YesButtonStyleProperty, lYesButtonStyleBinding);

                        Binding lNoButtonStyleBinding = new Binding("NoButtonStyle");
                        lNoButtonStyleBinding.Source = lControl.ViewModel;
                        lControl.SetBinding(NotificationView.NoButtonStyleProperty, lNoButtonStyleBinding);

                        Binding lCancelButtonStyleBinding = new Binding("CancelButtonStyle");
                        lCancelButtonStyleBinding.Source = lControl.ViewModel;
                        lControl.SetBinding(NotificationView.CancelButtonStyleProperty, lCancelButtonStyleBinding);
                    }

                    InformationViewModel lInformationViewModel = lNotificationViewModel as InformationViewModel;
                    if (lInformationViewModel != null)
                    {
                        Binding lOkButtonStyleBinding = new Binding("OkButtonStyle");
                        lOkButtonStyleBinding.Source = lControl.ViewModel;
                        lControl.SetBinding(NotificationView.OkButtonStyleProperty, lOkButtonStyleBinding);
                    }
                }

                lControl.UpdateState();
            }
        }

        /// <summary>
        /// Method called when the template of the control is applied.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.mOkButton = this.GetTemplateChild(PART_OkButton) as Button;
            this.mCancelButton = this.GetTemplateChild(PART_CancelButton) as Button;
            this.mYesButton = this.GetTemplateChild(PART_YesButton) as Button;
            this.mNoButton = this.GetTemplateChild(PART_NoButton) as Button;
            this.mCancelButtonFistColumn = this.GetTemplateChild(PART_CancelButtonFistColumn) as ColumnDefinition;
            this.mCancelButtonSecondColumn = this.GetTemplateChild(PART_CancelButtonSecondColumn) as ColumnDefinition;
            this.mMessageScrollViewer = this.GetTemplateChild(PART_MessageScrollViewer) as ScrollViewer;

            if  (   (this.mOkButton == null)
                ||  (this.mCancelButton == null)
                ||  (this.mYesButton == null)
                ||  (this.mNoButton == null)
                ||  (this.mCancelButtonFistColumn == null)
                ||  (this.mCancelButtonSecondColumn == null)
                ||  (this.mMessageScrollViewer == null)
                )
            {
                throw new ArgumentException("The NotificationView template is not valid.");
            }

            this.UpdateState();
        }

        /// <summary>
        /// Updates the graphic state of the control.
        /// </summary>
        private void UpdateState()
        {
            if (this.IsInitialized)
            {
                this.mMessageScrollViewer.ScrollToVerticalOffset(0.0);

                InformationViewModel lInformationViewModel = this.ViewModel as InformationViewModel;
                if (lInformationViewModel != null)
                {
                    this.mOkButton.Click -= this.OnOkButtonClicked;
                    this.mOkButton.Click += this.OnOkButtonClicked;
                    if (string.IsNullOrEmpty(lInformationViewModel.OkButtonContent) == false)
                    {
                        this.mOkButton.Content = lInformationViewModel.OkButtonContent;
                        this.mOkButton.ToolTip = lInformationViewModel.OkButtonContent;
                    }
                    if (lInformationViewModel.OkButtonStyle != null)
                    {
                        this.mOkButton.Style = lInformationViewModel.OkButtonStyle;
                    }
                    else if (lInformationViewModel.QuickStyle != null)
                    {
                        if (lInformationViewModel.QuickStyle.OkButtonBackground != null)
                        {
                            this.mOkButton.Background = lInformationViewModel.QuickStyle.OkButtonBackground;
                        }

                        if (lInformationViewModel.QuickStyle.OkButtonForeground != null)
                        {
                            this.mOkButton.Foreground = lInformationViewModel.QuickStyle.OkButtonForeground;
                        }
                    }

                    this.mOkButton.Visibility = System.Windows.Visibility.Visible;
                    this.mCancelButton.Visibility = System.Windows.Visibility.Visible;
                    this.mYesButton.Visibility = System.Windows.Visibility.Collapsed;
                    this.mNoButton.Visibility = System.Windows.Visibility.Collapsed;

                    this.mCancelButtonFistColumn.Width = new GridLength(0);
                    this.mCancelButtonSecondColumn.Width = new GridLength(0);
                }

                QuestionViewModel lQuestionViewModel = this.ViewModel as QuestionViewModel;
                if (lQuestionViewModel != null)
                {
                    this.mYesButton.Click -= this.OnYesButtonClicked;
                    this.mYesButton.Click += this.OnYesButtonClicked;
                    if (string.IsNullOrEmpty(lQuestionViewModel.YesButtonContent) == false)
                    {
                        this.mYesButton.Content = lQuestionViewModel.YesButtonContent;
                        this.mYesButton.ToolTip = lQuestionViewModel.YesButtonContent;
                    }
                    if (lQuestionViewModel.YesButtonStyle != null)
                    {
                        this.mYesButton.Style = lQuestionViewModel.YesButtonStyle;
                    }
                    else if (lQuestionViewModel.QuickStyle != null)
                    {
                        if (lQuestionViewModel.QuickStyle.YesButtonBackground != null)
                        {
                            this.mYesButton.Background = lQuestionViewModel.QuickStyle.YesButtonBackground;
                        }

                        if (lQuestionViewModel.QuickStyle.YesButtonForeground != null)
                        {
                            this.mYesButton.Foreground = lQuestionViewModel.QuickStyle.YesButtonForeground;
                        }
                    }

                    this.mNoButton.Click -= this.OnNoButtonClicked;
                    this.mNoButton.Click += this.OnNoButtonClicked;
                    if (string.IsNullOrEmpty(lQuestionViewModel.NoButtonContent) == false)
                    {
                        this.mNoButton.Content = lQuestionViewModel.NoButtonContent;
                        this.mNoButton.ToolTip = lQuestionViewModel.NoButtonContent;
                    }
                    if (lQuestionViewModel.NoButtonStyle != null)
                    {
                        this.mNoButton.Style = lQuestionViewModel.NoButtonStyle;
                    }
                    else if (lQuestionViewModel.QuickStyle != null)
                    {
                        if (lQuestionViewModel.QuickStyle.NoButtonBackground != null)
                        {
                            this.mNoButton.Background = lQuestionViewModel.QuickStyle.NoButtonBackground;
                        }

                        if (lQuestionViewModel.QuickStyle.NoButtonForeground != null)
                        {
                            this.mNoButton.Foreground = lQuestionViewModel.QuickStyle.NoButtonForeground;
                        }
                    }

                    this.mCancelButton.Click -= this.OnCancelButtonClicked;
                    this.mCancelButton.Click += this.OnCancelButtonClicked;
                    if (string.IsNullOrEmpty(lQuestionViewModel.CancelButtonContent) == false)
                    {
                        this.mCancelButton.Content = lQuestionViewModel.CancelButtonContent;
                        this.mCancelButton.ToolTip = lQuestionViewModel.CancelButtonContent;
                    }
                    if (lQuestionViewModel.CancelButtonStyle != null)
                    {
                        this.mCancelButton.Style = lQuestionViewModel.CancelButtonStyle;
                    }
                    else if (lQuestionViewModel.QuickStyle != null)
                    {
                        if (lQuestionViewModel.QuickStyle.CancelButtonBackground != null)
                        {
                            this.mCancelButton.Background = lQuestionViewModel.QuickStyle.CancelButtonBackground;
                        }

                        if (lQuestionViewModel.QuickStyle.CancelButtonForeground != null)
                        {
                            this.mCancelButton.Foreground = lQuestionViewModel.QuickStyle.CancelButtonForeground;
                        }
                    }

                    this.mOkButton.Visibility = System.Windows.Visibility.Collapsed;
                    this.mYesButton.Visibility = System.Windows.Visibility.Visible;
                    this.mNoButton.Visibility = System.Windows.Visibility.Visible;
                    if (lQuestionViewModel.CanCancel)
                    {
                        this.mCancelButton.Visibility = System.Windows.Visibility.Visible;
                        this.mCancelButtonFistColumn.Width = new GridLength(3, GridUnitType.Star);
                        this.mCancelButtonSecondColumn.Width = new GridLength(1, GridUnitType.Star);
                    }
                    else
                    {
                        this.mCancelButton.Visibility = System.Windows.Visibility.Collapsed;
                        this.mCancelButtonFistColumn.Width = new GridLength(0);
                        this.mCancelButtonSecondColumn.Width = new GridLength(0);
                    }
                }
            }
        }

        /// <summary>
        /// Delegate called when the yes button is clicked.
        /// </summary>
        /// <param name="pSender">The button sender.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private void OnYesButtonClicked(object pSender, RoutedEventArgs pEventArgs)
        {
            if (this.ViewModel != null && this.ViewModel.Parent != null)
            {
                this.ViewModel.Parent.CloseNotification(this.ViewModel, Answers.Yes, false);
            }
        }

        /// <summary>
        /// Delegate called when the no button is clicked.
        /// </summary>
        /// <param name="pSender">The button sender.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private void OnNoButtonClicked(object pSender, RoutedEventArgs pEventArgs)
        {
            if (this.ViewModel != null && this.ViewModel.Parent != null)
            {
                this.ViewModel.Parent.CloseNotification(this.ViewModel, Answers.No, false);
            }
        }

        /// <summary>
        /// Delegate called when the ok button is clicked.
        /// </summary>
        /// <param name="pSender">The button sender.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private void OnOkButtonClicked(object pSender, RoutedEventArgs pEventArgs)
        {
            if (this.ViewModel != null && this.ViewModel.Parent != null)
            {
                this.ViewModel.Parent.CloseNotification(this.ViewModel, Answers.Ok, false);
            }
        }

        /// <summary>
        /// Delegate called when the cancel button is clicked.
        /// </summary>
        /// <param name="pSender">The button sender.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private void OnCancelButtonClicked(object pSender, RoutedEventArgs pEventArgs)
        {
            if (this.ViewModel != null && this.ViewModel.Parent != null)
            {
                this.ViewModel.Parent.CloseNotification(this.ViewModel, Answers.Cancel, false);
            }
        }

        /// <summary>
        /// Delegate called when the quick style property changed.
        /// </summary>
        /// <param name="pObject">The modified object.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private static void OnQuickStyleChanged(DependencyObject pObject, DependencyPropertyChangedEventArgs pEventArgs)
        {
            NotificationView lControl = pObject as NotificationView;
            if (lControl != null)
            {
                lControl.UpdateState();
            }
        }

        /// <summary>
        /// Delegate called when the yes button style property changed.
        /// </summary>
        /// <param name="pObject">The modified object.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private static void OnYesButtonStyleChanged(DependencyObject pObject, DependencyPropertyChangedEventArgs pEventArgs)
        {
            NotificationView lControl = pObject as NotificationView;
            if (lControl != null)
            {
                lControl.UpdateState();
            }
        }

        /// <summary>
        /// Delegate called when the no button style property changed.
        /// </summary>
        /// <param name="pObject">The modified object.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private static void OnNoButtonStyleChanged(DependencyObject pObject, DependencyPropertyChangedEventArgs pEventArgs)
        {
            NotificationView lControl = pObject as NotificationView;
            if (lControl != null)
            {
                lControl.UpdateState();
            }
        }

        /// <summary>
        /// Delegate called when the cancel button style property changed.
        /// </summary>
        /// <param name="pObject">The modified object.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private static void OnCancelButtonStyleChanged(DependencyObject pObject, DependencyPropertyChangedEventArgs pEventArgs)
        {
            NotificationView lControl = pObject as NotificationView;
            if (lControl != null)
            {
                lControl.UpdateState();
            }
        }

        /// <summary>
        /// Delegate called when the ok button style property changed.
        /// </summary>
        /// <param name="pObject">The modified object.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private static void OnOkButtonStyleChanged(DependencyObject pObject, DependencyPropertyChangedEventArgs pEventArgs)
        {
            NotificationView lControl = pObject as NotificationView;
            if (lControl != null)
            {
                lControl.UpdateState();
            }
        }

        #endregion // Methods.
    }
}

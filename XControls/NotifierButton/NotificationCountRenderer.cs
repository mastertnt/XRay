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
    /// Class defining the gui part displaying the notification count.
    /// </summary>
    public class NotificationCountRenderer : ContentControl
    {
        #region Dependencies

        /// <summary>
        /// Identifies the NotificationCount dependency property.
        /// </summary>
        public static readonly DependencyProperty NotificationCountProperty = DependencyProperty.Register("NotificationCount", typeof(int), typeof(NotificationCountRenderer), new UIPropertyMetadata(0, OnNotificationCountChanged));

        /// <summary>
        /// Identifies the Blink dependency property.
        /// </summary>
        public static readonly DependencyProperty BlinkProperty = DependencyProperty.Register("Blink", typeof(bool), typeof(NotificationCountRenderer), new UIPropertyMetadata(false));

        #endregion // Dependencies.

        #region Fields

        /// <summary>
        /// Stores the max displayed notification count.
        /// </summary>
        private const int MAX_DISPLAYED_COUNT = 9;

        #endregion // Fields.

        #region Properties

        /// <summary>
        /// Gets or sets the notification count.
        /// </summary>
        public int NotificationCount
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

        /// <summary>
        /// Gets or sets the blink state.
        /// </summary>
        public bool Blink
        {
            get
            {
                return (bool) this.GetValue(BlinkProperty);
            }
            set
            {
                this.SetValue(BlinkProperty, value);
            }
        }

        #endregion // Properties.

        #region Constructors

        /// <summary>
        /// Initializes the <see cref="NotificationCountRenderer"/> class.
        /// </summary>
        static NotificationCountRenderer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NotificationCountRenderer), new FrameworkPropertyMetadata(typeof(NotificationCountRenderer)));
            DataContextProperty.OverrideMetadata(typeof(NotificationCountRenderer), new FrameworkPropertyMetadata(null, OnDataContextChanged));
        }

        /// <summary>
        /// Initializes an instance of the <see cref="NotificationCountRenderer"/> class.
        /// </summary>
        public NotificationCountRenderer()
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
            NotificationCountRenderer lControl = pObject as NotificationCountRenderer;
            NotifierViewModel lViewModel = pEventArgs.NewValue as NotifierViewModel;
            if (lControl != null && lViewModel != null)
            {
                Binding lNotificationCountBinding = new Binding("Notifications.Count");
                lNotificationCountBinding.Source = lViewModel;
                lControl.SetBinding(NotificationCountProperty, lNotificationCountBinding);

                lControl.UpdateRendering(lViewModel.Notifications.Count);
            }
        }

        /// <summary>
        /// Delegate called when the notification count property changed.
        /// </summary>
        /// <param name="pObject">The modified object.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private static void OnNotificationCountChanged(DependencyObject pObject, DependencyPropertyChangedEventArgs pEventArgs)
        {
            NotificationCountRenderer lControl = pObject as NotificationCountRenderer;
            if (lControl != null)
            {
                // Updating the content.
                int lCount = (int)pEventArgs.NewValue;
                lControl.UpdateRendering(lCount);
            }
        }

        /// <summary>
        /// Updates the rendering knowing the notification count.
        /// </summary>
        /// <param name="pCount">The notification count.</param>
        private void UpdateRendering(int pCount)
        {
            if (pCount <= 0)
            {
                this.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.Visibility = Visibility.Visible;
                if (pCount <= MAX_DISPLAYED_COUNT)
                {
                    this.Content = pCount.ToString();
                }
                else // lCount > MAX_DISPLAYED_COUNT.
                {
                    this.Content = MAX_DISPLAYED_COUNT.ToString() + "+";
                }
            }
        }

        #endregion // Methods.
    }
}

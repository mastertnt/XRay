using System.Windows;
using System.Windows.Input;

namespace XControls.AutoSelectTextBox
{
    /// <summary>
    /// Delegate used to query a move focus.
    /// </summary>
    /// <param name="pSender">The source control.</param>
    /// <param name="pEventArgs">The event arguments.</param>
    public delegate void QueryMoveFocusEventHandler(object pSender, QueryMoveFocusEventArgs pEventArgs);

    /// <summary>
    /// Class defining a query to move the focus.
    /// </summary>
    public class QueryMoveFocusEventArgs : RoutedEventArgs
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryMoveFocusEventArgs"/> class.
        /// </summary>
        private QueryMoveFocusEventArgs()
        {
            // Default ctor private to prevent its usage.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryMoveFocusEventArgs"/> class.
        /// </summary>
        /// <param name="pDirection">The focus direction.</param>
        /// <param name="pReachedMaxLength">Flag indicating if the text maximum length has been reached.</param>
        internal QueryMoveFocusEventArgs(FocusNavigationDirection pDirection, bool pReachedMaxLength)
            : base( AutoSelectTextBox.QueryMoveFocusEvent )
        {
            // Internal to prevent anybody from building this type of event.
            this.FocusNavigationDirection = pDirection;
            this.ReachedMaxLength = pReachedMaxLength;

            // Defaults to true. If nobody does nothing, then its capable of moving focus.
            this.CanMoveFocus = true;
        }

        #endregion // Constructors.

        #region Properties

        /// <summary>
        /// Gets the focus navigation direction.
        /// </summary>
        public FocusNavigationDirection FocusNavigationDirection
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the flag indicating if the text maximum length has been reached.
        /// </summary>
        public bool ReachedMaxLength
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the flag indicating if the focus move can be done.
        /// </summary>
        public bool CanMoveFocus
        {
            get;
            set;
        }

        #endregion // Properties.
    }
}

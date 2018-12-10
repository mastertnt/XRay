using System.Windows;

namespace XControls.ButtonSpinner
{
    /// <summary>
    /// Provides data for the Spinner.Spin event.
    /// </summary>
    public class SpinEventArgs : RoutedEventArgs
    {
        #region Properties

        /// <summary>
        /// Gets the SpinDirection for the spin that has been initiated by the end-user.
        /// </summary>
        public SpinDirection Direction
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets wheter the spin event originated from a mouse wheel event.
        /// </summary>
        public bool UsingMouseWheel
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the flag indicating if the value must be set to the infinite.
        /// </summary>
        public bool GoToInfinite
        {
            get;
            private set;
        }

        #endregion // Properties.

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SpinEventArgs"/> class.
        /// </summary>
        /// <param name="pDirection">The spin direction.</param>
        public SpinEventArgs(SpinDirection pDirection)
            : this(pDirection, false, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpinEventArgs"/> class.
        /// </summary>
        /// <param name="pDirection">The spin direction.</param>
        /// <param name="pUsingMouseWheel">Flag to know if the mouse wheel is used.</param>
        public SpinEventArgs(SpinDirection pDirection, bool pUsingMouseWheel)
            : this(pDirection, pUsingMouseWheel, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpinEventArgs"/> class.
        /// </summary>
        /// <param name="pGoToInfinite">Flag to know if a "go to the infinite" is requested.</param>
        public SpinEventArgs(bool pGoToInfinite)
            : this(SpinDirection.Increase, false, pGoToInfinite)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpinEventArgs"/> class.
        /// </summary>
        /// <param name="pDirection">The spin direction.</param>
        /// <param name="pUsingMouseWheel">Flag to know if the mouse wheel is used.</param>
        /// <param name="pGoToInfinite">Flag to know if a "go to the infinite" is requested.</param>
        public SpinEventArgs(SpinDirection pDirection, bool pUsingMouseWheel, bool pGoToInfinite)
            : base()
        {
            this.Direction = pDirection;
            this.UsingMouseWheel = pUsingMouseWheel;
            this.GoToInfinite = pGoToInfinite;
        }

        #endregion // Constructors.
    }
}

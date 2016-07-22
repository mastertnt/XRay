using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XControls.Core.Input
{
    /// <summary>
    /// Delegate defining an input validation error.
    /// </summary>
    /// <param name="pSender">The event sender.</param>
    /// <param name="pEventArgs">The event arguments.</param>
    public delegate void InputValidationErrorEventHandler(object pSender, InputValidationErrorEventArgs pEventArgs);

    /// <summary>
    /// Class defining the input validation error event arguments.
    /// </summary>
    public class InputValidationErrorEventArgs : EventArgs
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="InputValidationErrorEventArgs"/> class.
        /// </summary>
        /// <param name="pException">The raised exception.</param>
        public InputValidationErrorEventArgs(Exception pException)
        {
            this.Exception = pException;
        }

        #endregion // Constructors.

        #region Properties

        /// <summary>
        /// Gets or sets the raised exception.
        /// </summary>
        public Exception Exception
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the flag indicating if the exception can be thrown.
        /// </summary>
        public bool ThrowException
        {
            get;
            set;
        }

        #endregion // Properties.
    }
}

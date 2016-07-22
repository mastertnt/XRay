using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XControls.Core.Input
{
    /// <summary>
    /// Interface defining an input that can be validating.
    /// </summary>
    public interface IValidateInput
    {
        #region Events

        /// <summary>
        /// Event raised when an error occured on validation.
        /// </summary>
        event InputValidationErrorEventHandler InputValidationError;

        #endregion // Events.

        #region Methods

        /// <summary>
        /// Commits the modification and validate the input.
        /// </summary>
        /// <returns>True if the commit validation succeed, false otherwise.</returns>
        bool CommitInput();

        /// <summary>
        /// Cancels the modification.
        /// </summary>
        /// <returns>True if the cancelation succeed, false otherwise.</returns>
        bool CancelInput();

        #endregion // Methods.
    }
}

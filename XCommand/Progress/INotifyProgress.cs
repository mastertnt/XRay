using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XCommand.Progress
{
    /// <summary>
    /// Interface defining a progress notifier.
    /// </summary>
    /// <typeparam name="TValue">The type of the value to report to notify the progression.</typeparam>
    public interface INotifyProgress : IProgress<object>
    {
        #region Properties

        /// <summary>
        /// Gets or sets the evaluator converting the reported value in percentage value.
        /// </summary>
        IProgressEvaluator Evaluator
        {
            get;
            set;
        }

        #endregion // Properties.

        #region Events

        /// <summary>
        /// Event raised to notify a progression.
        /// </summary>
        event ProgressChangedEventHandler ProgressChanged;

        #endregion // Events.
    }
}

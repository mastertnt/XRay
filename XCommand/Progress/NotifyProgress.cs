using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XCommand.Progress
{
    /// <summary>
    /// Class defining an object notifying a progress.
    /// </summary>
    public class NotifyProgress : INotifyProgress
    {
        #region Fields

        /// <summary>
        /// Stores the inner 
        /// </summary>
        private IProgress<object> mInnerProgress;

        #endregion // Fields.

        #region Properties

        /// <summary>
        /// Gets or sets the evaluator of the progress notifier.
        /// </summary>
        public IProgressEvaluator Evaluator
        {
            get;
            set;
        }

        #endregion // Properties.

        #region Events

        /// <summary>
        /// Event raised when a progression needs to be raised.
        /// </summary>
        public event ProgressChangedEventHandler ProgressChanged;

        #endregion // Events.

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NotifyProgress"/> class.
        /// </summary>
        public NotifyProgress()
        {
            this.mInnerProgress = new Progress<object>(this.NotifyHandler);
        }

        #endregion // Constructors.

        #region Methods

        /// <summary>
        /// Reports a progression update.
        /// </summary>
        /// <param name="pReportedValue">The value to report.</param>
        public void Report(object pReportedValue)
        {
            this.mInnerProgress.Report(pReportedValue);
        }

        /// <summary>
        /// Returns the progression in percentage in the [0;1] range.
        /// </summary>
        /// <param name="pReportedValue">The progression reported value.</param>
        /// <returns>The progression in percentage.</returns>
        private double EvaluatePercent(object pReportedValue)
        {
            if (this.Evaluator == null)
            {
                return CommandConstants.PERCENT_MIN_VALUE;
            }

            return this.Evaluator.Evaluate(pReportedValue);
        }

        /// <summary>
        /// Coerce the given value to the percent range [0;1].
        /// </summary>
        /// <param name="pPercentValue">The percent value.</param>
        /// <returns>The coerced percent value.</returns>
        private double CoercePercent(double pPercentValue)
        {
            if (pPercentValue < CommandConstants.PERCENT_MIN_VALUE)
            {
                return CommandConstants.PERCENT_MIN_VALUE;
            }

            if (pPercentValue > CommandConstants.PERCENT_MAX_VALUE)
            {
                return CommandConstants.PERCENT_MAX_VALUE;
            }

            return pPercentValue;
        }

        /// <summary>
        /// Handler called when a progression needs to be notified.
        /// </summary>
        /// <param name="pReportedValue">The reported value.</param>
        private void NotifyHandler(object pReportedValue)
        {
            // Converting the value in percentage.
            double lPercentValue = this.EvaluatePercent(pReportedValue);

            // Coercing the percentage.
            lPercentValue = this.CoercePercent(lPercentValue);

            // Notifying the progression.
            if (this.ProgressChanged != null)
            {
                this.ProgressChanged(this, new NotifyProgressEventArgs(pReportedValue, lPercentValue));
            }
        }

        #endregion // Methods.
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XCommand.Progress
{
    /// <summary>
    /// Class defining a the event arguments of the <see cref="ProgressChangedEventHandler"/> event handler.
    /// </summary>
    public class NotifyProgressEventArgs
    {
        #region Properties

        /// <summary>
        /// Gets the initial reported value.
        /// </summary>
        public object ReportedValue
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the progress in percentage.
        /// Value range is [0;1].
        /// </summary>
        public double PercentProgress
        {
            get;
            private set;
        }

        #endregion // Properties.

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NotifyProgressEventArgs{TValue}"/> class.
        /// </summary>
        /// <param name="pReportedValue">The initial reported value.</param>
        /// <param name="pPercentProgress">The progress in percentage.</param>
        public NotifyProgressEventArgs(object pReportedValue, double pPercentProgress)
        {
            this.ReportedValue = pReportedValue;
            this.PercentProgress = pPercentProgress;
        }

        #endregion // Constructors.
    }
}

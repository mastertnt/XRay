using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XCommand.Progress
{
    /// <summary>
    /// Interface defining an evaluator computing the progression in percent.
    /// </summary>
    public interface IProgressEvaluator
    {
        #region Methods

        /// <summary>
        /// Returns the progression in percentage in the [0;1] range.
        /// </summary>
        /// <param name="pReportedValue">The progression reported value.</param>
        /// <returns>The progression in percentage.</returns>
        double Evaluate(object pReportedValue);

        #endregion // Methods.
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XCommand.Progress.Evaluators
{
    /// <summary>
    /// Class defining an identity evaluator returning the reported value.
    /// </summary>
    public class IdentityProgressEvaluator : IProgressEvaluator
    {
        #region Methods

        /// <summary>
        /// Returns the progression in percentage in the [0;1] range.
        /// </summary>
        /// <param name="pReportedValue">The progression reported value.</param>
        /// <returns>The progression in percentage.</returns>
        public double Evaluate(object pReportedValue)
        {
            TypeConverter lDoubleConverter = TypeDescriptor.GetConverter(typeof(double));
            if (lDoubleConverter == null)
            {
                return CommandConstants.PERCENT_MIN_VALUE;
            }

            return (double)lDoubleConverter.ConvertFrom(pReportedValue);
        }

        #endregion // Methods.
    }
}

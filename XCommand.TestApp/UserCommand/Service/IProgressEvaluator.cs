using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandTest.UserCommand.Service
{
    /// <summary>
    /// Interface defining the command progress evaluator.
    /// </summary>
    public interface IProgressEvaluator
    {
        #region Properties

        /// <summary>
        /// Gets the completion percentage of the command.
        /// </summary>
        Percentage Completion
        {
            get;
        }

        #endregion // Properties.
    }
}

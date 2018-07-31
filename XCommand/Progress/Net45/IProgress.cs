using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    /// <summary>
    /// Class defining a progression reporter.
    /// </summary>
    /// <typeparam name="TValue">The type of the reported value.</typeparam>
    public interface IProgress<in TValue>
    {
        #region Methods

        /// <summary>
        /// Notifies a progression update.
        /// </summary>
        /// <param name="pValue">The reported value.</param>
        void Report(TValue pValue);

        #endregion // Methods.
    }
}

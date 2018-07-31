using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XCommand.Progress
{
    /// <summary>
    /// Delegate defining a progress changed event handler.
    /// </summary>
    /// <param name="pSource">The source.</param>
    /// <param name="pEventArgs">The event arguments.</param>
    public delegate void ProgressChangedEventHandler(INotifyProgress pSource, NotifyProgressEventArgs pEventArgs);
}


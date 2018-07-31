using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommandTest
{
    /// <summary>
    /// User-friendly capabilities. The properties of this interface shall never throw any exception.
    /// A non implementation shall return an empty string (not a null).
    /// (taken from SA.Support)
    /// </summary>
    public interface IDisplayable
    {
        /// <summary>
        /// Human readable name
        /// </summary>
        string DisplayName { get; }
        /// <summary>
        /// Description string
        /// </summary>
        string Description { get; }
        /// <summary>
        /// Help string
        /// </summary>
        string Help { get; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XSerialization
{
    /// <summary>
    /// Interface defining an X serialization class or interface to use in a given range of versions.
    /// </summary>
    public interface IXVersionable
    {
        /// <summary>
        /// Gets the object applicable minimum version.
        /// </summary>
        Version MinVersion
        {
            get;
        }

        /// <summary>
        /// Gets the object applicable maximum version.
        /// For the current version, the value must return Version(
        /// </summary>
        Version MaxVersion
        {
            get;
        }
    }
}

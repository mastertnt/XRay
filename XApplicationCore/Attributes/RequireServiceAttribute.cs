using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XApplicationCore.Attributes
{
    /// <summary>
    /// This attribute can be use to define a required service on a plugin.
    /// </summary>
    public class RequireServiceAttribute : PluginServiceAttribute
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RequireServiceAttribute"/> class.
        /// </summary>
        /// <param name="pServiceType">Type of the service.</param>
        public RequireServiceAttribute(Type pServiceType)
            :base(pServiceType)
        {
            
        }

        #endregion // Constructors.
    }
}

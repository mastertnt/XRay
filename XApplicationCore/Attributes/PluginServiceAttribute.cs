using System;

namespace XApplicationCore.Attributes
{
    /// <summary>
    /// This attribute can be use to indicate the presence of the service in the plugin.
    /// </summary>
    public class PluginServiceAttribute
    {
        #region Properties

        /// <summary>
        /// Gets the service.
        /// </summary>
        /// <value>
        /// The service.
        /// </value>
        public Type ServiceType
        {
            get;
            private set;
        }

        #endregion // Properties.

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginServiceAttribute"/> class.
        /// </summary>
        /// <param name="pServiceType">Type of the service.</param>
        protected PluginServiceAttribute(Type pServiceType)
        {
            this.ServiceType = pServiceType;
        }

        #endregion // Constructors.
    }


}

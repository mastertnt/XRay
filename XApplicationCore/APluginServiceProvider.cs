namespace XApplicationCore
{
    /// <summary>
    /// This abstract class can be usd to expose a plugin to the host.
    /// </summary>
    /// <typeparam name="TService">The exposed service.</typeparam>
    public abstract class APluginServiceProvider<TService> : IPlugin where TService : IService
    {
        #region Properties

        /// <summary>
        /// Gets the host.
        /// </summary>
        /// <value>
        /// The host.
        /// </value>
        public AApplication Host
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the display string.
        /// </summary>
        public virtual string DisplayString
        {
            get
            {
                return this.Host.FindService<ILocalizationService>().GetLocalizedDisplayString("Plugin", "DisplayString");
            }
        }

        /// <summary>
        /// Gets or sets the hosted service.
        /// </summary>
        /// <value>
        /// The hosted service.
        /// </value>
        public TService HostedService
        {
            get;
            protected set;
        }

        #endregion // Properties.

        #region Properties

        /// <summary>
        /// This method is called to attach the plugin to the specified host.
        /// </summary>
        /// <param name="pHost">The host.</param>
        public void Attach(AApplication pHost)
        {
            this.Host = pHost;
        }

        /// <summary>
        /// This method is called to detach the plugin from the specified host.
        /// </summary>
        /// <param name="pHost">The host.</param>
        public void Detach(AApplication pHost)
        {
            this.Host = null;
        }

        #endregion // Methods.
    }
}

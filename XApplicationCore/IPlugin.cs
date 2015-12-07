namespace XApplicationCore
{
    /// <summary>
    /// This interface describes a plugin.
    /// </summary>
    interface IPlugin : IDisplayable
    {
        /// <summary>
        /// Gets the host.
        /// </summary>
        /// <value>
        /// The host.
        /// </value>
        AApplication Host
        {
            get;
        }

        /// <summary>
        /// This method is called to attach the plugin to the specified host.
        /// </summary>
        /// <param name="pHost">The host.</param>
        void Attach(AApplication pHost);

        /// <summary>
        /// This method is called to detach the plugin from the specified host.
        /// </summary>
        /// <param name="pHost">The host.</param>
        void Detach(AApplication pHost);
    }
}

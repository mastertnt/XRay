using System;
using System.Runtime.Remoting.Services;

namespace XApplicationCore
{
    /// <summary>
    /// This class defines an application whichs loads all the services and plugins of the application.
    /// </summary>
    public abstract class AApplication
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AApplication"/> class.
        /// </summary>
        protected AApplication()
        {
        }

        #endregion // Constructors.

        #region Methods

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public void Initialize()
        {
            this.PreInitialize();
            this.CustomInitialize();
            this.PostInitialize();
        }

        /// <summary>
        /// This method is called before the initialization.
        /// </summary>
        protected virtual void PreInitialize()
        {
            
        }

        /// <summary>
        /// This method is called during the initialization.
        /// </summary>
        protected virtual void CustomInitialize()
        {
            
        }

        /// <summary>
        /// This method is called after the initialization.
        /// </summary>
        protected virtual void PostInitialize()
        {

        }

        /// <summary>
        /// Runs this instance.
        /// </summary>
        public abstract void Run();

        /// <summary>
        /// Finds a service.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <returns>Null if the service is not found, the given service.</returns>
        public TService FindService<TService>() where TService : IService
        {
            TService lResult = default(TService);
            return lResult;
        }

        #endregion // Methods.
    }
}

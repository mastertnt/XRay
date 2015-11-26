namespace XApplicationCore
{
    /// <summary>
    /// This class defines an application whichs loads all the services and plugins of the application.
    /// </summary>
    public class XApplication
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="XApplication"/> class.
        /// </summary>
        public XApplication()
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

        #endregion // Methods.
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace System
{
    /// <summary>
    /// Provides an <see cref="Progress{TValue}" /> that invokes callbacks for each reported progress value.
    /// </summary>
    /// <typeparam name="TValue">Specifies the type of the progress report value.</typeparam>
    public class Progress<TValue> : IProgress<TValue>
    {
        #region Fields

        /// <summary>
        /// Stores the synchronisation context.
        /// </summary>
        private readonly SynchronizationContext mSynchronizationContext;
        
        /// <summary>
        /// Stores the specific reporting handler.
        /// </summary>
        private readonly Action<TValue> mHandler;

        /// <summary>
        /// Stores the callback invoking the handlers.
        /// </summary>
        private readonly SendOrPostCallback mInvokeHandlers;

        #endregion // Fields.

        #region Events

        /// <summary>
        /// Raised for each reported progress value.
        /// </summary>
        public event GenericEventHandler<TValue> ProgressChanged;

        #endregion // Events.

        #region Constructors

        /// <summary>
        /// Initializes a nex instance of the <see cref="Progress{TValue}"/> class.
        /// </summary>
        public Progress()
        {
            this.mSynchronizationContext = SynchronizationContext.Current;
            this.mInvokeHandlers = new SendOrPostCallback(this.InvokeHandlers);
        }

        /// <summary>
        /// Initializes a nex instance of the <see cref="Progress{TValue}"/> class.
        /// </summary>
        /// <param name="pHandler">The specific report handler.</param>
        public Progress(Action<TValue> pHandler)
            : this()
        {
            if (pHandler == null)
            {
                throw new ArgumentNullException("pHandler");
            }

            this.mHandler = pHandler;
        }

        #endregion // Constructors.

        #region Methods

        /// <summary>
        /// Reports a progress change.
        /// </summary>
        /// <param name="pValue">The value of the updated progress.</param>
        protected virtual void OnReport(TValue pValue)
        {
            if (this.mHandler == null && this.ProgressChanged == null)
            {
                return;
            }

            this.mSynchronizationContext.Post(this.mInvokeHandlers, (object)pValue);
        }

        /// <summary>
        /// Reports a progress change.
        /// </summary>
        /// <param name="pValue">The value of the updated progress.</param>
        void IProgress<TValue>.Report(TValue pValue)
        {
            this.OnReport(pValue);
        }

        /// <summary>
        /// Callback invoking the handlers.
        /// </summary>
        /// <param name="pState">The state.</param>
        private void InvokeHandlers(object pState)
        {
            TValue lState = (TValue)pState;
            Action<TValue> lHandler = this.mHandler;
            GenericEventHandler<TValue> lProgressChanged = this.ProgressChanged;
            if (lHandler != null)
            {
                lHandler(lState);
            }
            if (lProgressChanged == null)
            {
                return;
            }
            
            lProgressChanged((object)this, lState);
        }

        #endregion // Methods.
    }
}

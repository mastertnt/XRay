using System;
using System.Reflection;

namespace XTreeListView.Core
{
    /// <summary>
    /// Defines an event handler that will not prevent object from GC just 
    /// because we have added and event handler to them.
    /// </summary>
    /// <!-- DPE -->
    public class WeakEventHandler<TEventHandler, TEventArgs>
    {
        #region Fields

        /// <summary>
        /// Weak reference on the execution delegate target.
        /// </summary>
        private readonly WeakReference mWeakRefToTarget;

        /// <summary>
        /// Method executed by the delegate.
        /// </summary>
        private readonly MethodInfo mMethod;

        #endregion // Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WeakEventHandler&lt;TEventHandler, TEventArgs&gt;"/> class.
        /// </summary>
        /// <param name="pOriginalDelegate">The original strong event handler.</param>
        public WeakEventHandler(OpenEventHandler pOriginalDelegate)
        {
            this.mWeakRefToTarget = new WeakReference(pOriginalDelegate.Target);
            this.mMethod = pOriginalDelegate.Method;
        }

        #endregion // Constructors

        #region Delegates

        /// <summary>
        /// This handler defines the delegate which is normally registered on an event.
        /// </summary>
        /// <param name="pSender">The event sender.</param>
        /// <param name="pEvent">The event arguments.</param>
        public delegate void OpenEventHandler(Object pSender, TEventArgs pEvent);

        #endregion // Delegates

        #region Methods

        /// <summary>
        /// Defines an implicit cast from a WeakEventHandler to an EventHandler.
        /// </summary>
        /// <param name="pWeakHandler">The weak event handler to cast.</param>
        /// <returns>An event handler.</returns>
        public static implicit operator TEventHandler(WeakEventHandler<TEventHandler, TEventArgs> pWeakHandler)
        {
            Object lHandler = Delegate.CreateDelegate(typeof(TEventHandler), pWeakHandler, "DoInvoke");
            return (TEventHandler)lHandler;
        }

        /// <summary>
        /// Executes the original delegate.
        /// </summary>
        /// <param name="pSender">The event sender.</param>
        /// <param name="pArgs">The event arguments.</param>
// ReSharper disable UnusedMember.Local
        private void DoInvoke(Object pSender, TEventArgs pArgs)
// ReSharper restore UnusedMember.Local
        {
            // Verifying if the target has not been collected.
            if 
                (this.mWeakRefToTarget.IsAlive)
            {
                Object lOriginalTarget = this.mWeakRefToTarget.Target;
                if 
                    (lOriginalTarget != null)
                {
                    // Creating the event handler...
                    OpenEventHandler lDelegate = (OpenEventHandler)Delegate.CreateDelegate(
                                typeof(OpenEventHandler), lOriginalTarget, this.mMethod.Name);

                    // ...and executing it.
                    lDelegate(pSender, pArgs);
                }
            }
        }

        #endregion // Methods
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using XTreeListView.ViewModel.Generic;

namespace XTreeListView.ViewModel
{
    /// <summary>
    /// This class implements a base view model.
    /// </summary>
    public abstract class AViewModel : IViewModel
    {
        #region Fields

        /// <summary>
        /// Stores the owned object.
        /// </summary>
        private object mUntypedOwnedObject;

        /// <summary>
        /// Stores the flag indicating if the view model is disposed.
        /// </summary>
        private bool mDisposed;

        /// <summary>
        /// Stores the visibility of the item.
        /// </summary>
        private bool mIsVisible;

        /// <summary>
        /// Stores the flag to know if the item is checked.
        /// </summary>
        private bool mIsChecked;

        /// <summary>
        /// Stores the checking enabled state of the view model.
        /// </summary>
        private bool mIsCheckingEnabled;

        /// <summary>
        /// Stores the tool tip of the item.
        /// </summary>
        private object mToolTip;

        /// <summary>
        /// Stores the bindings of property changed names between the ViewModel and an object
        /// that implements INotifyPropertyChanged (in most case the Model).
        /// For each INotifyPropertyChanged it gives a dictionary that associate a property name in this object
        /// to a list of properties in the AViewModel inherited object.
        /// e.g. with an entry ["Name", "DisplayString"] when the ViewModel (this) gets notified of the 
        /// Model's "Name" property change, it fires a propertyChangedEvent whose name is "DisplayString".
        /// </summary> 
        private readonly Dictionary<string, HashSet<string>> mPropertiesBinding;

        /// <summary>
        /// Stores the flag indicating if the notify property changed event of the view model can be raised or not.
        /// </summary>
        private bool mIsNotifyPropertyChangedEnable;

        /// <summary>
        /// Stores the flag indicating if the view model is busy.
        /// </summary>
        private bool mIsBusy;

        #endregion // Fields.

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AViewModel"/> class.
        /// </summary>
        /// <param name="pOwnedObject">The owned object.</param>
        protected AViewModel(object pOwnedObject)
        {
            this.mPropertiesBinding = new Dictionary<string, HashSet<string>>();

            this.UntypedOwnedObject = pOwnedObject;

            this.mIsNotifyPropertyChangedEnable = true;
            this.mDisposed = false;

            this.mIsVisible = true;

            this.mIsCheckingEnabled = true;
            this.mIsChecked = false;
        }

        /// <summary>
        /// Destroys this instance.
        /// This destructor will run only if the Dispose method does not get called.
        /// It gives the base class the opportunity to finalize.
        /// Do not provide destructors in types derived from this class.
        /// </summary>
        ~AViewModel()
        {
            this.Dispose(false);
        }

        #endregion // Constructors.

        #region Events

        /// <summary>
        /// Event raised when a property is modified.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Event raised when the dispose method has been called explicitely, that is not in the finalizer.
        /// </summary>
        public event Action Disposed;

        #endregion // Events.

        #region Properties

        /// <summary>
        /// Gets or sets the string displayed by default in the item in the information part.
        /// </summary>
        public virtual string DisplayString
        {
            get
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets or sets the owned object.
        /// </summary>
        public object UntypedOwnedObject
        {
            get
            {
                return this.mUntypedOwnedObject;
            }

            set
            {
                object lOldObject = this.mUntypedOwnedObject;
                object lNewObject = value;

                // Specific pre traitment.
                this.PreviewOwnedObjectChanged(lOldObject, lNewObject);

                // Unregistering the property changed event.
                INotifyPropertyChanged lOldOwnedObject = this.mUntypedOwnedObject as INotifyPropertyChanged;
                if (lOldOwnedObject != null)
                {
                    lOldOwnedObject.PropertyChanged -= this.OnOwnedObjectPropertyChanged;
                }

                this.mUntypedOwnedObject = value;

                // Registering on the property changed event.
                INotifyPropertyChanged lNewOwnedObject = this.mUntypedOwnedObject as INotifyPropertyChanged;
                if (lNewOwnedObject != null)
                {
                    lNewOwnedObject.PropertyChanged += this.OnOwnedObjectPropertyChanged;
                }

                // Specific post traitment.
                this.OwnedObjectChanged(lOldObject, lNewObject);
            }
        }

        /// <summary>
        /// Gets or sets the flag indicating the visibility of the view model.
        /// </summary>
        public virtual bool IsVisible
        {
            get
            {
                return this.mIsVisible;
            }

            set
            {
                if (this.mIsVisible != value)
                {
                    this.mIsVisible = value;
                    this.OnVisibilityChanged(value);
                    this.NotifyPropertyChanged("IsVisible");
                }
            }
        }

        /// <summary>
        /// Gets or sets the flag indicating if the item is checked or not.
        /// </summary>
        public bool IsChecked
        {
            get
            {
                return this.mIsChecked;
            }
            set
            {
                if (this.mIsChecked != value && this.IsCheckable && this.IsCheckingEnabled)
                {
                    this.mIsChecked = value;
                    this.NotifyPropertyChanged("IsChecked");
                }
            }
        }

        /// <summary>
        /// Gets or sets the flags indicating if the view model checked state can be modifyed.
        /// </summary>
        public bool IsCheckingEnabled
        {
            get
            {
                return this.mIsCheckingEnabled;
            }

            set
            {
                if (value != this.mIsCheckingEnabled)
                {
                    this.mIsCheckingEnabled = value;
                    this.NotifyPropertyChanged("IsCheckingEnabled");
                }
            }
        }

        /// <summary>
        /// Gets the flag indicating if the item is checkable.
        /// </summary>
        public virtual bool IsCheckable
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the ToolTip.
        /// </summary>
        public object ToolTip
        {
            get
            {
                return this.mToolTip;
            }

            set
            {
                if (this.mToolTip != value)
                {
                    this.mToolTip = value;
                    this.NotifyPropertyChanged("ToolTip");
                }
            }
        }

        /// <summary>
        /// Gets the icon to display in the item.
        /// </summary>
        public abstract ImageSource IconSource
        {
            get;
        }

        /// <summary>
        /// Gets the flag indicating if the view model is busy.
        /// </summary>
        public bool IsBusy
        {
            get
            {
                return this.mIsBusy;
            }

            private set
            {
                if (this.mIsBusy != value)
                {
                    this.mIsBusy = value;
                    this.NotifyPropertyChanged("IsBusy");
                }
            }
        }

        /// <summary>
        /// Gets the flag indicating if the view model is disposed.
        /// </summary>
        public bool IsDisposed
        {
            get
            {
                return this.mDisposed;
            }
        }

        #endregion // Properties.

        #region Methods

        /// <summary>
        /// Method called when the owned object is going to be modified.
        /// </summary>
        /// <param name="pPreviousOwnedObject">The previous owned object.</param>
        /// <param name="pNewOwnedObject">The new owned object.</param>
        protected virtual void PreviewOwnedObjectChanged(object pPreviousOwnedObject, object pNewOwnedObject)
        {
            // Nothing to do.
        }

        /// <summary>
        /// Method called when the owned object has been modified.
        /// </summary>
        /// <param name="pPreviousOwnedObject">The previous owned object.</param>
        /// <param name="pNewOwnedObject">The new owned object.</param>
        protected virtual void OwnedObjectChanged(object pPreviousOwnedObject, object pNewOwnedObject)
        {
            // Nothing to do.
        }

        /// <summary>
        /// Delegate called when the visibility is changed.
        /// </summary>
        /// <param name="pNewValue">The new visibility.</param>
        protected virtual void OnVisibilityChanged(bool pNewValue)
        {
            // Nothing to do.
        }

        /// <summary>
        /// Bind an expression of a property from the owned object to an expression of a property in the current view model.
        /// </summary>
        /// <param name="pModelProperty">The property name of the origin.</param>
        /// <param name="pViewModelProperty">The property name of the destination</param>
        protected void BindProperty(string pModelProperty, string pViewModelProperty)
        {
            Debug.Assert(this.UntypedOwnedObject is INotifyPropertyChanged, "The model must implement INotifyPropertyChanged.");

            // Registering the binding.
            if (this.mPropertiesBinding.ContainsKey(pModelProperty) == false)
            {
                this.mPropertiesBinding[pModelProperty] = new HashSet<string>();
            }

            this.mPropertiesBinding[pModelProperty].Add(pViewModelProperty);
        }

        /// <summary>
        /// Unbind an expression of a property from the owned object to an expression of a property in the current view model.
        /// </summary>
        /// <param name="pModelProperty">The property name of the origin.</param>
        /// <param name="pViewModelProperty">The property name of the destination</param>
        protected void UnbindProperty(string pModelProperty, string pViewModelProperty)
        {
            if (this.mPropertiesBinding.ContainsKey(pModelProperty))
            {
                this.mPropertiesBinding[pModelProperty].Remove(pViewModelProperty);
            }
        }

        /// <summary>
        /// Called each time a binded property of the owned object gets changed. 
        /// This method can only be called if OwnedObject implements INotifyPropertyChanged.
        /// </summary>
        /// <param name="pSender">The modified owned object.</param>
        /// <param name="pEvent">The event arguments.</param>
        private void OnOwnedObjectPropertyChanged(object pSender, PropertyChangedEventArgs pEvent)
        {
            // Calling internal handler.
            this.OnOwnedObjectPropertyChangedInternal(pEvent);

            // Forward if in binding list.
            if (this.mPropertiesBinding.ContainsKey(pEvent.PropertyName))
            {
                foreach (string lBoundPropertyName in this.mPropertiesBinding[pEvent.PropertyName])
                {
                    this.NotifyPropertyChanged(lBoundPropertyName);
                }
            }
        }

        /// <summary>
        /// Delegate called when a property of the owned object gets changed.
        /// </summary>
        /// <param name="pEvent">The event arguments.</param>
        protected virtual void OnOwnedObjectPropertyChangedInternal(PropertyChangedEventArgs pEvent)
        {
            // Nothing to do by default.
        }

        /// <summary>
        /// Method called when a property is modified to notify the listner.
        /// </summary>
        /// <param name="pPropertyName">The property name.</param>
        public void NotifyPropertyChanged(string pPropertyName)
        {
            if (this.PropertyChanged != null && this.mIsNotifyPropertyChangedEnable)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(pPropertyName));
            }
        }

        /// <summary>
        /// Executed a background work for the current view model.
        /// </summary>
        /// <param name="pStart">The entry point of the work to do.</param>
        /// <param name="pParams">The background work parameters.</param>
        public void DoBackgroundWork(ParameterizedBackgroundWorkStart pStart, object pParams)
        {
            // Indicating the tree view is busy.
            System.Threading.Tasks.Task lBusyTask = new System.Threading.Tasks.Task(() => this.IsBusy = true);

            // Excuting the background task.
            System.Threading.Tasks.Task lBackgroundTask = lBusyTask.ContinueWith((antecedent) => pStart(this, pParams));

            // The tree view is not busy anymore.
            lBackgroundTask.ContinueWith((pAntecedent) => this.IsBusy = false);

            // Launching the task.
            lBusyTask.Start();
        }

        /// <summary>
        /// Convert the item to the generic version.
        /// </summary>
        /// <typeparam name="TModel">The type of the owned object.</typeparam>
        /// <returns>The generic version of the item.</returns>
        public Generic.AViewModel<TModel> ToGeneric<TModel>()
        {
            return (this as Generic.AViewModel<TModel>);
        }

        /// <summary>
        /// Convert the item to the generic version.
        /// </summary>
        /// <typeparam name="TModel">The type of the owned object.</typeparam>
        /// <returns>
        /// The generic version of the item.
        /// </returns>
        IViewModel<TModel> IViewModel.ToGeneric<TModel>()
        {
            return (this as IViewModel<TModel>);
        }

        /// <summary>
        /// Dispose this view model.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);

#pragma warning disable 1587
            /// This object will be cleaned up by the Dispose method.
            /// Therefore, GC.SupressFinalize should be called to take this object off the finalization queue 
            /// and prevent finalization code for this object from executing a second time.
#pragma warning restore 1587

            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Unregisters from the underlying model.
        /// </summary>
        protected virtual void UnregisterFromModel()
        {
            // Invalidating owned object.
            this.mPropertiesBinding.Clear();
            this.UntypedOwnedObject = null;
            this.ToolTip = null;
        }

        /// <summary>
        /// Dispose this view model.
        /// </summary>
        /// <param name="pDisposing">Flag indicating if the owned objects have to be cleaned as well.</param>
        private void Dispose(bool pDisposing)
        {
            this.DisableNotifyPropertyChangedEvent();

            if (this.mUntypedOwnedObject != null && this.mUntypedOwnedObject is INotifyPropertyChanged)
            {
                (this.mUntypedOwnedObject as INotifyPropertyChanged).PropertyChanged -= this.OnOwnedObjectPropertyChanged;
            }

            if (this.mDisposed == false)
            {
                // Free other state (managed objects) section.
                if (pDisposing)
                {
                    this.NotifyDispose();
                    this.UnregisterFromModel();
                }

                // Free your own state (unmanaged objects) section.

                this.mDisposed = true;
            }


            this.EnableNotifyPropertyChangedEvent();
        }

        /// <summary>
        /// Notifies a dispose has been made.
        /// </summary>
        private void NotifyDispose()
        {
            if (this.Disposed != null)
            {
                this.Disposed();
            }
        }

        /// <summary>
        /// Locks the notify property changed event raising.
        /// </summary>
        protected void DisableNotifyPropertyChangedEvent()
        {
            this.mIsNotifyPropertyChangedEnable = false;
        }

        /// <summary>
        /// Enables the notify property changed event raising.
        /// </summary>
        protected void EnableNotifyPropertyChangedEvent()
        {
            this.mIsNotifyPropertyChangedEnable = true;
        }

        #endregion // Methods.
    }
}


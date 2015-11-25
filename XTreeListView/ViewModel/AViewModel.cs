using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using XTreeListView.Core;
using XTreeListView.ViewModel.Generic;

namespace XTreeListView.ViewModel
{
    /// <summary>
    /// This class implements a base view model.
    /// </summary>
    /// <!-- DPE -->
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
        private Visibility mVisibility;

        /// <summary>
        /// This field stores the flag to know if the item is checked.
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
        /// Stores the content of the displayed tooltip.
        /// </summary>
        private string mToolTipContent;

        /// <summary>
        /// Stores the string displayed by default in the item in the information part.
        /// </summary>
        private string mDisplayString;

        /// <summary>
        /// Stores the name of the icon to display.
        /// </summary>
        private string mIconName;

        /// <summary>
        /// Stores the bindings of property changed names between the ViewModel and an object
        /// that implements INotifyPropertyChanged (in most case the Model).
        /// For each INotifyPropertyChanged it gives a dictionary that associate a property name in this object
        /// to a list of properties in the AViewModel inherited object.
        /// e.g. with an entry ["Name", "DisplayString"] when the ViewModel (this) gets notified of the 
        /// Model's "Name" property change, it fires a propertyChangedEvent whose name is "DisplayString".
        /// </summary> 
        private readonly Dictionary<String, List<String>> mPropertiesBinding;

        /// <summary>
        /// Event handler used to notify the property modification.
        /// </summary>
        private readonly WeakEventHandler<PropertyChangedEventHandler, PropertyChangedEventArgs> mPropertyChangedHandler;

        #endregion // Fields.

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the AViewModel class.
        /// </summary>
        /// <param name="pOwnedObject">The owned object.</param>
        protected AViewModel(Object pOwnedObject)
        {
            this.mPropertiesBinding = new Dictionary<String, List<String>>();
            this.mPropertyChangedHandler = new WeakEventHandler<PropertyChangedEventHandler, PropertyChangedEventArgs>(this.OnOwnedObjectPropertyChanged);

            this.UntypedOwnedObject = pOwnedObject;

            this.mDisposed = false;
            this.mVisibility = Visibility.Visible;
            this.mIsChecked = false;
            this.mIsCheckingEnabled = true;
            this.mIconName = this.GetType().Name.ToString();
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
        /// Gets the selection relative to this view model.
        /// </summary>
        public virtual object Selection
        {
            get
            {
                return this.UntypedOwnedObject;
            }
        }

        /// <summary>
        /// Gets or sets the string displayed by default in the item in the information part.
        /// </summary>
        public virtual string DisplayString
        {
            get
            {
                return this.mDisplayString;
            }

            set
            {
                this.mDisplayString = value;
            }
        }

        /// <summary>
        /// Gets or sets the owned object.
        /// </summary>
        public virtual object UntypedOwnedObject
        {
            get
            {
                return this.mUntypedOwnedObject;
            }

            protected set
            {
                // Unregistering the property changed event.
                INotifyPropertyChanged lOldOwnedObject = this.mUntypedOwnedObject as INotifyPropertyChanged;
                if
                    (lOldOwnedObject != null)
                {
                    lOldOwnedObject.PropertyChanged -= this.mPropertyChangedHandler;
                }

                this.mUntypedOwnedObject = value;

                // Registering on the property changed event.
                INotifyPropertyChanged lNewOwnedObject = this.mUntypedOwnedObject as INotifyPropertyChanged;
                if
                    (lNewOwnedObject != null)
                {
                    lNewOwnedObject.PropertyChanged += this.mPropertyChangedHandler;
                }
            }
        }

        /// <summary>
        /// Gets the background color brush of the view model.
        /// </summary>
        public virtual Brush Background
        {
            get
            {
                return new SolidColorBrush(Colors.Transparent);
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Gets or sets the visibility state of the item.
        /// </summary>
        public virtual Visibility Visibility
        {
            get
            {
                return this.mVisibility;
            }

            set
            {
                if
                    (this.mVisibility != value)
                {
                    this.mVisibility = value;
                    this.OnVisibilityChanged(value);
                    this.NotifyPropertyChanged("Visibility");
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
                if
                    (this.mIsChecked != value && this.IsCheckable && this.IsCheckingEnabled)
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
                if
                    (value != this.mIsCheckingEnabled)
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
                if
                    (this.mToolTip != value)
                {
                    this.mToolTip = value;
                    this.NotifyPropertyChanged("ToolTip");
                }
            }
        }

        /// <summary>
        /// Sets the tooltip content.
        /// </summary>
        public string ToolTipContent
        {
            get 
            {
                return this.mToolTipContent; 
            }
            set
            {
                if (string.IsNullOrEmpty(value) == false)
                {
                    this.mToolTipContent = value;
                    this.NotifyPropertyChanged("ToolTipContent");
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
        /// Gets the icon visibility.
        /// </summary>
        public virtual Visibility IconVisibility
        {
            get
            {
                if
                    (this.IconSource == null)
                {
                    return Visibility.Collapsed;
                }

                return Visibility.Visible;
            }
        }

        /// <summary>
        /// Gets or sets the flag indicating if the view model is busy.
        /// </summary>
        public bool IsBusy
        {
            get;
            set;
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
        /// Delegate called when the visibility is changed.
        /// </summary>
        /// <param name="pNewValue">The new visibility.</param>
        protected virtual void OnVisibilityChanged(Visibility pNewValue)
        {
            // Nothing to do.
        }

        /// <summary>
        /// Bind an expression of a property from the owned object to an expression of a property in the current view model.
        /// </summary>
        /// <param name="pModelProperty">The property name of the origin.</param>
        /// <param name="pViewModelProperty">The property name of the destination</param>
        protected void BindProperty(String pModelProperty, String pViewModelProperty)
        {
            Debug.Assert(this.UntypedOwnedObject is INotifyPropertyChanged, "The model must implement INotifyPropertyChanged.");

            // Registering the binding.
            if
                (this.mPropertiesBinding.ContainsKey(pModelProperty) == false)
            {
                this.mPropertiesBinding[pModelProperty] = new List<String>();
            }

            this.mPropertiesBinding[pModelProperty].Add(pViewModelProperty);
        }

        /// <summary>
        /// unbind an expression of a property from the owned object to an expression of a property in the current view model.
        /// </summary>
        /// <param name="pModelProperty">The property name of the origin.</param>
        /// <param name="pViewModelProperty">The property name of the destination</param>
        protected void UnbindProperty(String pModelProperty, String pViewModelProperty)
        {
            if
                (this.mPropertiesBinding.ContainsKey(pModelProperty))
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
        private void OnOwnedObjectPropertyChanged(Object pSender, PropertyChangedEventArgs pEvent)
        {
            // Calling internal handler.
            this.OnOwnedObjectPropertyChangedInternal(pEvent);

            // Forward if in binding list.
            if
                (this.mPropertiesBinding.ContainsKey(pEvent.PropertyName))
            {
                foreach
                    (String lBoundPropertyName in this.mPropertiesBinding[pEvent.PropertyName])
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
        public void NotifyPropertyChanged(String pPropertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(pPropertyName));
            }
        }

        /// <summary>
        /// Executed a background work for the current view model.
        /// </summary>
        /// <param name="pStart">The entry point of the work to do.</param>
        /// <param name="pParams">The background work parameters.</param>
        public void DoBackgroundWork(ParameterizedBackgroundWorkStart pStart, Object pParams)
        {
            // Indicating the tree view is busy.
            System.Threading.Tasks.Task lBusyTask = new System.Threading.Tasks.Task(() => (this as IViewModel).IsBusy = true);

            // Excuting the background task.
            System.Threading.Tasks.Task lBackgroundTask = lBusyTask.ContinueWith((antecedent) => pStart(this, pParams));

            // The tree view is not busy anymore.
            lBackgroundTask.ContinueWith((pAntecedent) => (this as IViewModel).IsBusy = false);

            // Launching the task.
            lBusyTask.Start();
        }

        /// <summary>
        /// Convert the item to the generic version.
        /// </summary>
        /// <typeparam name="T">The type of the owned object.</typeparam>
        /// <returns>The generic version of the item.</returns>
        public Generic.AViewModel<T> ToGeneric<T>()
        {
            return (this as Generic.AViewModel<T>);
        }

        /// <summary>
        /// Convert the item to the generic version.
        /// </summary>
        /// <typeparam name="T">The type of the owned object.</typeparam>
        /// <returns>
        /// The generic version of the item.
        /// </returns>
        IViewModel<T> IViewModel.ToGeneric<T>()
        {
            return (this as IViewModel<T>);
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
            if
                (!this.mDisposed)
            {
                // Free other state (managed objects) section.
                if
                    (pDisposing)
                {
                    this.UnregisterFromModel();

                    //if (this.Disposed != null)
                    //{
                    //    this.Disposed();
                    //}
                }

                // Free your own state (unmanaged objects) section.

                this.mDisposed = true;
            }
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

        #endregion // Methods.
    }
}


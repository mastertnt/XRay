﻿using System;
using System.ComponentModel;
using System.Windows.Media;

namespace XTreeListView.ViewModel
{
    /// <summary>
    /// This interface defines the base view model interface.
    /// It expose the most commonly used properties when creating a view model.
    /// </summary>
    public interface IViewModel : INotifyPropertyChanged, IDisposable
    {
        #region Events

        /// <summary>
        /// Event raised when the dispose method has been called explicitely, that is not in the finalizer.
        /// </summary>
        event Action Disposed;

        #endregion // Events.

        #region Properties

        /// <summary>
        /// Gets the owned object if any as an object.
        /// </summary>
        object UntypedOwnedObject
        {
            get;
        }

        /// <summary>
        /// Gets the string to be displayed in the GUI.
        /// </summary>
        string DisplayString
        {
            get;
        }

        /// <summary>
        /// Gets the icon source of the view model.
        /// </summary>
        ImageSource IconSource
        {
            get;
        }

        /// <summary>
        /// Gets or sets the flag indicating the visibility of the view model.
        /// </summary>
        bool IsVisible 
        { 
            get; 
            set;
        }

        /// <summary>
        /// Gets the flag indicating the if the view model is checkable or not.
        /// </summary>
        bool IsCheckable
        {
            get;
        }

        /// <summary>
        /// Gets or sets the flag indicating if the view model is checked or not.
        /// </summary>
        bool IsChecked
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the flags indicating if the view model checked state can be modifyed.
        /// </summary>
        bool IsCheckingEnabled
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the ToolTip.
        /// </summary>
        object ToolTip
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the flag indicating if the view model is busy.
        /// </summary>
        bool IsBusy
        {
            get;
        }

        /// <summary>
        /// Gets the flag indicating if the view model is disposed.
        /// </summary>
        bool IsDisposed
        {
            get;
        }

        #endregion // Properties.

        #region Methods

        /// <summary>
        /// Convert the item to the generic version.
        /// </summary>
        /// <typeparam name="TModel">The type of the owned object.</typeparam>
        /// <returns>The generic version of the item.</returns>
        Generic.IViewModel<TModel> ToGeneric<TModel>();

        /// <summary>
        /// Executed a background work for the current view model.
        /// </summary>
        /// <param name="pStart">The entry point of the work to do.</param>
        /// <param name="pParams">The background work parameters.</param>
        void DoBackgroundWork(ParameterizedBackgroundWorkStart pStart, object pParams);

        #endregion // Methods.
    }
}

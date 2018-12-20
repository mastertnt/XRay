using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Text;

namespace XSystem.Collections
{
    /// <summary>
    ///     An observable collection synchronizer (Synchronize items with between the two collections).
    /// </summary>
    /// <typeparam name="TSourceType">The type of the source type.</typeparam>
    /// <typeparam name="TTargetType">The type of the target type.</typeparam>
    public class ObservableCollectionSynchronizer<TSourceType, TTargetType>
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="ObservableCollectionSynchronizer{TSourceType, TTargetType}" /> class.
        /// </summary>
        /// <param name="pSource">The source observable collection type.</param>
        /// <param name="pTarget">The target observable collection type.</param>
        public ObservableCollectionSynchronizer(ObservableCollection<TSourceType> pSource, ObservableCollection<TTargetType> pTarget)
        {
            this.mSource = pSource;
            this.mTarget = pTarget;
            this.mSource.CollectionChanged += this.OnSourceCollectionChanged;
            this.mTarget.CollectionChanged += this.OnTargetCollectionChanged;
        }

        #endregion // Constructors.

        #region Fields

        /// <summary>
        ///     This field stores the source collection.
        /// </summary>
        protected readonly ObservableCollection<TSourceType> mSource;

        /// <summary>
        ///     The field stores the target collection.
        /// </summary>
        protected readonly ObservableCollection<TTargetType> mTarget;

        /// <summary>
        ///     Stores the flag indicating if the behaviour is disposed.
        /// </summary>
        private bool mDisposed;

        #endregion // Fields.

        #region Methods

        /// <summary>
        ///     Dispose the object resources
        ///     NOTE: Is overridable, but if so and base not called, OnDispose will not be called anymore.
        /// </summary>
        public void Dispose()
        {
            if (this.mDisposed == false)
            {
                this.OnDispose();

#pragma warning disable 1587
                /// This object will be cleaned up by the Dispose method.
                /// Therefore, GC.SupressFinalize should be called to take this object off the finalization queue 
                /// and prevent finalization code for this object from executing a second time.
#pragma warning restore 1587

                GC.SuppressFinalize(this);

                this.mDisposed = true;
            }
        }

        /// <summary>
        ///     Method called on object disposing.
        /// </summary>
        protected virtual void OnDispose()
        {
            this.mSource.CollectionChanged -= this.OnSourceCollectionChanged;
            this.mTarget.CollectionChanged -= this.OnTargetCollectionChanged;
        }

        /// <summary>
        ///     Handles the CollectionChanged event of the IncludePlatforms control.
        /// </summary>
        /// <param name="pSender">The source of the event.</param>
        /// <param name="pEventArgs">
        ///     The <see cref="System.Collections.Specialized.NotifyCollectionChangedEventArgs" /> instance
        ///     containing the event data.
        /// </param>
        private void OnTargetCollectionChanged(object pSender, NotifyCollectionChangedEventArgs pEventArgs)
        {
            this.mSource.CollectionChanged -= this.OnSourceCollectionChanged;
            switch (pEventArgs.Action)
            {
                case NotifyCollectionChangedAction.Add:
                {
                    // Only add items.
                    if (pEventArgs.NewStartingIndex == -1)
                    {
                        foreach (TTargetType lTargetObject in pEventArgs.NewItems)
                        {
                            var lSourceObject = lTargetObject.Convert<TSourceType>();
                            this.mSource.Add(lSourceObject);
                            this.OnSourceAddedBySynchronization(lTargetObject, lSourceObject, pEventArgs.Action);
                        }
                    }
                    // Insert items at the correct index.
                    else
                    {
                        foreach (TTargetType lTargetObject in pEventArgs.NewItems)
                        {
                            var lSourceObject = lTargetObject.Convert<TSourceType>();
                            this.mSource.Insert(pEventArgs.NewStartingIndex, lSourceObject);
                            this.OnSourceAddedBySynchronization(lTargetObject, lSourceObject, pEventArgs.Action);
                        }
                    }
                }
                    break;

                case NotifyCollectionChangedAction.Remove:
                {
                    foreach (TTargetType lTargetObject in pEventArgs.OldItems)
                    {
                        var lSourceObject = lTargetObject.Convert<TSourceType>();
                        this.mSource.Remove(lSourceObject);
                        this.OnSourceRemovedBySynchronization(lTargetObject, lSourceObject, pEventArgs.Action);
                    }
                }
                    break;

                case NotifyCollectionChangedAction.Move:
                {
                    for (var lIndex = 0; lIndex < pEventArgs.NewItems.Count; lIndex++)
                    {
                        this.mSource.Move(pEventArgs.OldStartingIndex, pEventArgs.NewStartingIndex);
                    }
                }
                    break;

                case NotifyCollectionChangedAction.Replace:
                {
                    foreach (TTargetType lTargetObject in pEventArgs.NewItems)
                    {
                        var lSourceObject = lTargetObject.Convert<TSourceType>();
                        this.mSource[pEventArgs.NewStartingIndex] = lSourceObject;
                        this.OnSourceAddedBySynchronization(lTargetObject, lSourceObject, pEventArgs.Action);
                    }

                    // Notify there is some target objects removed.
                    foreach (TTargetType lTargetObject in pEventArgs.OldItems)
                    {
                        var lSourceObject = lTargetObject.Convert<TSourceType>();
                        this.OnSourceRemovedBySynchronization(lTargetObject, lSourceObject, pEventArgs.Action);
                    }
                }
                    break;

                // Rebuild the collection.
                case NotifyCollectionChangedAction.Reset:
                {
                    this.SynchronizeTargetToSource(true);
                }
                    break;
            }

            this.mSource.CollectionChanged += this.OnSourceCollectionChanged;
        }

        /// <summary>
        ///     Called when the source collection changed.
        /// </summary>
        /// <param name="pSender">The event sender.</param>
        /// <param name="pEventArgs">The <see cref="NotifyCollectionChangedEventArgs" /> instance containing the event data.</param>
        private void OnSourceCollectionChanged(object pSender, NotifyCollectionChangedEventArgs pEventArgs)
        {
            this.mTarget.CollectionChanged -= this.OnTargetCollectionChanged;
            switch (pEventArgs.Action)
            {
                case NotifyCollectionChangedAction.Add:
                {
                    // Only add items.
                    if (pEventArgs.NewStartingIndex == -1)
                    {
                        foreach (TSourceType lSourceObject in pEventArgs.NewItems)
                        {
                            var lTargetObject = lSourceObject.Convert<TTargetType>();
                            this.mTarget.Add(lTargetObject);
                            this.OnTargetAddedBySynchronization(lSourceObject, lTargetObject, pEventArgs.Action);
                        }
                    }
                    // Insert items at the correct index.
                    else
                    {
                        foreach (TSourceType lSourceObject in pEventArgs.NewItems)
                        {
                            var lTargetObject = lSourceObject.Convert<TTargetType>();
                            this.mTarget.Insert(pEventArgs.NewStartingIndex, lTargetObject);
                            this.OnTargetAddedBySynchronization(lSourceObject, lTargetObject, pEventArgs.Action);
                        }
                    }
                }
                    break;

                case NotifyCollectionChangedAction.Remove:
                {
                    foreach (TSourceType lSourceObject in pEventArgs.OldItems)
                    {
                        var lTargetObject = lSourceObject.Convert<TTargetType>();
                        this.mTarget.Remove(lTargetObject);
                        this.OnTargetRemovedBySynchronization(lSourceObject, lTargetObject, pEventArgs.Action);
                    }
                }
                    break;

                case NotifyCollectionChangedAction.Move:
                {
                    for (var lIdx = 0; lIdx < pEventArgs.NewItems.Count; lIdx++)
                    {
                        this.mTarget.Move(pEventArgs.OldStartingIndex, pEventArgs.NewStartingIndex);
                    }
                }
                    break;

                case NotifyCollectionChangedAction.Replace:
                {
                    foreach (TSourceType lSourceObject in pEventArgs.NewItems)
                    {
                        var lTargetObject = lSourceObject.Convert<TTargetType>();
                        this.mTarget[pEventArgs.NewStartingIndex] = lTargetObject;
                        this.OnTargetAddedBySynchronization(lSourceObject, lTargetObject, pEventArgs.Action);
                    }

                    // Notify there is some source objects removed.
                    foreach (TSourceType lSourceObject in pEventArgs.OldItems)
                    {
                        var lTargetObject = lSourceObject.Convert<TTargetType>();
                        this.OnTargetRemovedBySynchronization(lSourceObject, lTargetObject, pEventArgs.Action);
                    }
                }
                    break;

                // Rebuild the collection.
                case NotifyCollectionChangedAction.Reset:
                {
                    this.OnSynchronizationSourceToTarget(false);
                }
                    break;
            }

            this.mTarget.CollectionChanged += this.OnTargetCollectionChanged;
        }

        /// <summary>
        ///     Synchronize the source and the target (from the source to the target).
        /// </summary>
        public virtual void SynchronizeSourceToTarget()
        {
            this.OnSynchronizationSourceToTarget(true);
        }

        /// <summary>
        ///     Synchronize the source and the target (from the source to the target).
        /// </summary>
        /// <param name="pManageEvents">if set to <c>true</c> [p manage events].</param>
        private void OnSynchronizationSourceToTarget(bool pManageEvents)
        {
            if (pManageEvents)
            {
                this.mTarget.CollectionChanged -= this.OnTargetCollectionChanged;
            }

            this.mTarget.Clear();
            foreach (var lSourceObject in this.mSource)
            {
                var lTargetObject = lSourceObject.Convert<TTargetType>();
                this.mTarget.Add(lTargetObject);
                this.OnTargetAddedBySynchronization(lSourceObject, lTargetObject, NotifyCollectionChangedAction.Reset);
            }

            if (pManageEvents)
            {
                this.mTarget.CollectionChanged += this.OnTargetCollectionChanged;
            }
        }

        /// <summary>
        ///     Synchronize the source and the target (from the target to the source).
        /// </summary>
        public virtual void SynchronizeTargetToSource()
        {
            this.SynchronizeTargetToSource(true);
        }

        /// <summary>
        ///     Synchronize the source and the target (from the target to the source).
        /// </summary>
        /// <param name="pManageEvents">if set to <c>true</c> [p manage events].</param>
        private void SynchronizeTargetToSource(bool pManageEvents)
        {
            if (pManageEvents)
            {
                this.mSource.CollectionChanged -= this.OnSourceCollectionChanged;
            }

            this.mSource.Clear();
            foreach (var lTargetObject in this.mTarget)
            {
                var lSourceObject = lTargetObject.Convert<TSourceType>();
                this.mSource.Add(lSourceObject);
                this.OnTargetAddedBySynchronization(lSourceObject, lTargetObject, NotifyCollectionChangedAction.Reset);
            }

            if (pManageEvents)
            {
                this.mSource.CollectionChanged += this.OnSourceCollectionChanged;
            }
        }

        /// <summary>
        ///     Called when a target has been added by the synchronization process.
        ///     This method can be used to manage events on objects.
        /// </summary>
        /// <param name="pSourceObject">The object which generates the event.</param>
        /// <param name="pTargetObject">The added object.</param>
        /// <param name="pAction">The collection action.</param>
        protected virtual void OnTargetAddedBySynchronization(TSourceType pSourceObject, TTargetType pTargetObject, NotifyCollectionChangedAction pAction)
        {
            Console.WriteLine("OnTargetAddedBySynchronization from " + pSourceObject + " to " + pTargetObject + " involved action " + pAction);
        }

        /// <summary>
        ///     Called when a target has been removed by the synchronization process.
        ///     This method can be used to manage events on objects.
        /// </summary>
        /// <param name="pSourceObject">The object which generates the event.</param>
        /// <param name="pTargetObject">The removed object.</param>
        /// <param name="pAction">The collection action.</param>
        protected virtual void OnTargetRemovedBySynchronization(TSourceType pSourceObject, TTargetType pTargetObject, NotifyCollectionChangedAction pAction)
        {
            Console.WriteLine("OnTargetRemovedBySynchronization from " + pSourceObject + " to " + pTargetObject + " involved action " + pAction);
        }

        /// <summary>
        ///     Called when a soruce has been removed by the synchronization process.
        ///     This method can be used to manage events on objects.
        /// </summary>
        /// <param name="pTargetObject">The object which generates the event.</param>
        /// <param name="pSourceObject">The added object.</param>
        /// <param name="pAction">The collection action.</param>
        protected virtual void OnSourceAddedBySynchronization(TTargetType pTargetObject, TSourceType pSourceObject, NotifyCollectionChangedAction pAction)
        {
            Console.WriteLine("OnSourceAddedBySynchronization from " + pSourceObject + " to " + pTargetObject + " involved action " + pAction);
        }

        /// <summary>
        ///     Called when a source has been removed by the synchronization process.
        ///     This method can be used to manage events on objects.
        /// </summary>
        /// <param name="pTargetObject">The object which generates the event.</param>
        /// <param name="pSourceObject">The removed object.</param>
        /// <param name="pAction">The collection action.</param>
        protected virtual void OnSourceRemovedBySynchronization(TTargetType pTargetObject, TSourceType pSourceObject, NotifyCollectionChangedAction pAction)
        {
            Console.WriteLine("OnSourceRemovedBySynchronization from " + pSourceObject + " to " + pTargetObject + " involved action " + pAction);
        }

        /// <summary>
        ///     Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        ///     A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            var lResult = new StringBuilder();
            lResult.AppendLine("SOURCE :");
            var lIndex = 0;
            foreach (var lItem in this.mSource)
            {
                lResult.AppendLine(lIndex + " = " + lItem);
                lIndex++;
            }

            lResult.AppendLine("TARGET :");
            lIndex = 0;
            foreach (var lItem in this.mTarget)
            {
                lResult.AppendLine(lIndex + " = " + lItem);
                lIndex++;
            }

            return lResult.ToString();
        }

        #endregion // Methods
    }
}
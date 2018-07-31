using System;
using System.ComponentModel;
using XCommand.Progress;
using XCommand.Progress.Evaluators;

namespace XCommand
{
    /// <summary>
    /// Base class for the user commands.
    /// </summary>
    public abstract class AUserCommand : IUserCommand
    {
        #region Fields

        /// <summary>
        /// Stores the progress notifier when the command is doing.
        /// </summary>
        private INotifyProgress mDoingProgressNotifier;

        /// <summary>
        /// Stores the progress notifier when the command is undoing.
        /// </summary>
        private INotifyProgress mUndoingProgressNotifier;
        
        #endregion // Fields.

        #region Properties

        /// <summary>
        /// Gets the localized short description of the command.
        /// </summary>
        public abstract string ShortDescription
        {
            get;
        }

        #endregion // Properties.

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AUserCommand"/> class.
        /// </summary>
        /// <param name="pTimeout">The timeout of the command.</param>
        protected AUserCommand(TimeSpan? pTimeout)
            : this(pTimeout, new IdentityProgressEvaluator())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AUserCommand"/> class.
        /// </summary>
        /// <param name="pTimeout">The timeout of the command.</param>
        /// <param name="pProgressEvaluator">The progress evaluator.</param>
        protected AUserCommand(TimeSpan? pTimeout, IProgressEvaluator pProgressEvaluator)
        {
            this.Timeout = pTimeout;
            
            this.mDoingProgressNotifier = new NotifyProgress();
            this.mDoingProgressNotifier.Evaluator = pProgressEvaluator;
            this.mDoingProgressNotifier.ProgressChanged += this.OnDoingProgressChanged;

            this.mUndoingProgressNotifier = new NotifyProgress();
            this.mUndoingProgressNotifier.Evaluator = pProgressEvaluator;
            this.mUndoingProgressNotifier.ProgressChanged += this.OnUndoingProgressChanged;
        }

        #endregion // Constructors.

        #region Methods

        /// <summary>
        /// Notifies a property modification.
        /// </summary>
        /// <param name="pPropertyName">The name of the modified property.</param>
        protected void NotifyPropertyChanged(string pPropertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(pPropertyName));
            }
        }

        /// <summary>
        /// Notifies the command is doing.
        /// </summary>
        /// <param name="pReportedValue">The command completion reported value.</param>
        protected void NotifyDoing(object pReportedValue)
        {
            this.mDoingProgressNotifier.Report(pReportedValue);
        }

        /// <summary>
        /// Event raised when the doing progress is notified.
        /// </summary>
        /// <param name="pSource">The source.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private void OnDoingProgressChanged(INotifyProgress pSource, NotifyProgressEventArgs pEventArgs)
        {
            if (this.Doing != null)
            {
                this.Doing(this, new CommandExecutionEventArgs(this, pEventArgs.PercentProgress));
            }
        }

        /// <summary>
        /// Notifies the command is done.
        /// </summary>
        protected void NotifyDone()
        {
            if (this.Done != null)
            {
                this.Done(this, new CommandExecutionEventArgs(this, CommandConstants.PERCENT_MAX_VALUE));
            }
        }

        /// <summary>
        /// Notifies the command execution has failed.
        /// </summary>
        /// <param name="pReason">The reason why the command failed.</param>
        protected void NotifyFailed(string pReason)
        {
            if (this.Failed != null)
            {
                this.Failed(this, new CommandExecutionEventArgs(this, CommandConstants.PERCENT_MAX_VALUE, pReason));
            }
        }

        /// <summary>
        /// Notifies the command execution has failed.
        /// </summary>
        /// <param name="pReason">The reason why the command failed.</param>
        protected void NotifyFailed(Exception pReason)
        {
            if (this.Failed != null)
            {
                this.Failed(this, new CommandExecutionEventArgs(this, CommandConstants.PERCENT_MAX_VALUE, pReason));
            }
        }

        /// <summary>
        /// Notifies the command is undoing.
        /// </summary>
        /// <param name="pReportedValue">The command completion reported value.</param>
        protected void NotifyUndoing(object pReportedValue)
        {
            this.mUndoingProgressNotifier.Report(pReportedValue);
        }

        /// <summary>
        /// Event raised when the undoing progress is notified..
        /// </summary>
        /// <param name="pSource">The source.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private void OnUndoingProgressChanged(INotifyProgress pSource, NotifyProgressEventArgs pEventArgs)
        {
            if (this.Undoing != null)
            {
                this.Undoing(this, new CommandExecutionEventArgs(this, pEventArgs.PercentProgress));
            }
        }

        /// <summary>
        /// Notifies the command is done.
        /// </summary>
        protected void NotifyUndone()
        {
            if (this.Undone != null)
            {
                this.Undone(this, new CommandExecutionEventArgs(this, CommandConstants.PERCENT_MAX_VALUE));
            }
        }

        #endregion // Methods.

        #region IUserCommand implementation

        #region Properties

        /// <summary>
        /// Gets the real time the command has been executed.
        /// </summary>
        public TimeSpan RealTime
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the simulation time the command has been executed.
        /// </summary>
        public TimeSpan SimulationTime
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the command execution timeout.
        /// </summary>
        public TimeSpan? Timeout
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the command specific state.
        /// </summary>
        public abstract UserCommandState State
        {
            get;
        }

        #endregion // Properties.

        #region Events

        /// <summary>
        /// Event raised to notify a command execution progression.
        /// </summary>
        public event CommandExecutionEventHandler<IUserCommand> Doing;

        /// <summary>
        /// Event raised to notify a command execution successfully finished.
        /// </summary>
        public event CommandExecutionEventHandler<IUserCommand> Done;

        /// <summary>
        /// Event raised to notify a command execution or revertion failed.
        /// </summary>
        public event CommandExecutionEventHandler<IUserCommand> Failed;

        /// <summary>
        /// Event raised to notify a command revertion progression.
        /// </summary>
        public event CommandExecutionEventHandler<IUserCommand> Undoing;

        /// <summary>
        /// Event raised to notify a command revertion successfully finished.
        /// </summary>
        public event CommandExecutionEventHandler<IUserCommand> Undone;

        #endregion // Events.

        #region Methods

        /// <summary>
        /// Do the command.
        /// </summary>
        public abstract void Do();

        /// <summary>
        /// Do the commande and time stamp it.
        /// </summary>
        /// <param name="pRealTime">The relative real execution time.</param>
        /// <param name="pSimulationTime">The simulation time the command has been executed.</param>
        public void TimeStampedDo(TimeSpan pRealTime, TimeSpan pSimulationTime)
        {
            this.RealTime = pRealTime;
            this.SimulationTime = pSimulationTime;

            this.Do();
        }

        /// <summary>
        /// Undo the command.
        /// </summary>
        public abstract void Undo();

        /// <summary>
        /// Tries to merge the last executed command with this command.
        /// </summary>
        /// <param name="pLastExecutedCommand">The last executed command.</param>
        /// <returns>True if the command has been merge with this command.</returns>
        public virtual bool TryMerge(IUserCommand pLastExecutedCommand)
        {
            return false;
        }

        #endregion // Methods.

        #endregion // IUserCommand implementation.

        #region INotifyPropertyChanged implementation

        /// <summary>
        /// Event raised when a property changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion // INotifyPropertyChanged implementation.

        #region ITooltipable implementation

        #region Properties

        /// <summary>
        /// Gets or sets the tooltip description.
        /// </summary>
        public virtual string Tooltip
        {
            get;
            set;
        }

        #endregion // Properties.

        #endregion // ITooltipable implementation.
    }
}

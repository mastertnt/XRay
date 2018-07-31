using CommandTest.UserCommand.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandTest.UserCommand
{
    /// <summary>
    /// Base class for the user commands.
    /// </summary>
    public abstract class AUserCommand : IUserCommand, IProgressEvaluator
    {
        #region Fields

        /// <summary>
        /// Stores the completion.
        /// </summary>
        private Percentage mCompletion;

        #endregion // Fields.

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AUserCommand"/> class.
        /// </summary>
        /// <param name="pTimeout">The timeout of the command.</param>
        protected AUserCommand(Time pTimeout)
        {
            this.Timeout = pTimeout;
            this.mCompletion = new Percentage(0.0);
        }

        #endregion // Constructors.

        #region Methods

        /// <summary>
        /// Sets the completion of the commande.
        /// </summary>
        /// <param name="pCompletion">The command completion.</param>
        protected void SetCompletion(Percentage pCompletion)
        {
            this.mCompletion = pCompletion;
        }

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
        /// <param name="pCompletion">The command completion.</param>
        protected void NotifyDoing(Percentage pCompletion)
        {
            this.SetCompletion(pCompletion);

            if (this.Doing != null)
            {
                this.Doing(this, new CommandExecutionEventArgs(this, this));
            }
        }

        /// <summary>
        /// Notifies the command is done.
        /// </summary>
        protected void NotifyDone()
        {
            this.SetCompletion(new Percentage(100.0));

            if (this.Done != null)
            {
                this.Done(this, new CommandExecutionEventArgs(this, this));
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
                this.Failed(this, new CommandExecutionEventArgs(this, this, pReason));
            }
        }

        /// <summary>
        /// Notifies the command is undoing.
        /// </summary>
        /// <param name="pCompletion">The command completion.</param>
        protected void NotifyUndoing(Percentage pCompletion)
        {
            this.SetCompletion(pCompletion);

            if (this.Undoing != null)
            {
                this.Undoing(this, new CommandExecutionEventArgs(this, this));
            }
        }

        /// <summary>
        /// Notifies the command is done.
        /// </summary>
        protected void NotifyUndone()
        {
            this.SetCompletion(new Percentage(100.0));

            if (this.Undone != null)
            {
                this.Undone(this, new CommandExecutionEventArgs(this, this));
            }
        }

        #endregion // Methods.

        #region IUserCommand implementation

        #region Properties

        /// <summary>
        /// Gets the real time the command has been executed.
        /// </summary>
        public Time RealTime
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the simulation time the command has been executed.
        /// </summary>
        public Time SimulationTime
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the command execution timeout.
        /// </summary>
        public Time Timeout
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
        /// <param name="pSimulationTime"></param>
        public void TimeStampedDo(Time pRealTime, Time pSimulationTime)
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

        #region IDisplayable implementation

        #region Properties

        /// <summary>
        /// Gets the localized name of the command.
        /// </summary>
        public abstract string DisplayName 
        { 
            get; 
        }

        /// <summary>
        /// Gets the localized description of the command.
        /// </summary>
        public abstract string Description
        {
            get;
        }

        /// <summary>
        /// Gets the localized help of the command.
        /// </summary>
        public abstract string Help
        {
            get;
        }

        #endregion // Properties.

        #endregion // IDisplayable implementation.

        #region ITooltipable implementation

        #region Properties

        /// <summary>
        /// Gets or sets the tooltip description.
        /// </summary>
        public string Tooltip
        {
            get;
            set;
        }

        #endregion // Properties.

        #endregion // ITooltipable implementation.

        #region IProgressEvaluator implementation

        #region Properties

        /// <summary>
        /// Gets the completion percentage of the command.
        /// </summary>
        Percentage IProgressEvaluator.Completion
        {
            get
            {
                return this.mCompletion;
            }
        }

        #endregion // Properties.

        #endregion // IProgressEvaluator implementation.
    }
}

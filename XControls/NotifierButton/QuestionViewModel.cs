using System.Windows;

namespace XControls.NotifierButton
{
    /// <summary>
    /// Class defining a question notification view model.
    /// </summary>
    public class QuestionViewModel : ANotificationViewModel
    {
        #region Fields

        /// <summary>
        /// Stores the ok button style.
        /// </summary>
        private Style mYesButtonStyle;

        /// <summary>
        /// Stores the ok button style.
        /// </summary>
        private Style mNoButtonStyle;

        /// <summary>
        /// Stores the ok button style.
        /// </summary>
        private Style mCancelButtonStyle;

        #endregion // Fields.

        #region Properties

        /// <summary>
        /// Gets or sets the flag indicating if the question can be canceled.
        /// </summary>
        public bool CanCancel
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the yes button content.
        /// </summary>
        public string YesButtonContent
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the yes button specific style.
        /// If null, the default one is used.
        /// </summary>
        public Style YesButtonStyle
        {
            get
            {
                return this.mYesButtonStyle;
            }

            set
            {
                this.mYesButtonStyle = value;
                this.NotifyPropertyChanged("YesButtonStyle");
            }
        }

        /// <summary>
        /// Gets or sets the no button content.
        /// </summary>
        public string NoButtonContent
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the no button specific style.
        /// If null, the default one is used.
        /// </summary>
        public Style NoButtonStyle
        {
            get
            {
                return this.mNoButtonStyle;
            }

            set
            {
                this.mNoButtonStyle = value;
                this.NotifyPropertyChanged("NoButtonStyle");
            }
        }

        /// <summary>
        /// Gets or sets the cancel button content.
        /// </summary>
        public string CancelButtonContent
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Cancel button specific style.
        /// If null, the default one is used.
        /// </summary>
        public Style CancelButtonStyle
        {
            get
            {
                return this.mCancelButtonStyle;
            }

            set
            {
                this.mCancelButtonStyle = value;
                this.NotifyPropertyChanged("CancelButtonStyle");
            }
        }

        #endregion // Properties.

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="QuestionViewModel"/> class.
        /// </summary>
        /// <param name="pType">The notification type.</param>
        /// <param name="pId">The notification id.</param>
        public QuestionViewModel(string pType, string pId)
            : base(pType, pId)
        {
            this.DefaultAnswer = Answers.No;
            this.CanCancel = false;

            this.YesButtonContent = "Yes";
            this.YesButtonStyle = null;

            this.NoButtonContent = "No";
            this.NoButtonStyle = null;

            this.CancelButtonContent = "Cancel";
            this.CancelButtonStyle = null;
        }

        #endregion // Constructors.
    }
}

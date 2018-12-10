using System.Windows;

namespace XControls.NotifierButton
{
    /// <summary>
    /// Class defining an information notification view model.
    /// </summary>
    public class InformationViewModel : ANotificationViewModel
    {
        #region Fields

        /// <summary>
        /// Stores the ok button style.
        /// </summary>
        private Style mOkButtonStyle;

        #endregion // Fields.

        #region Properties

        /// <summary>
        /// Gets or sets the ok button content.
        /// </summary>
        public string OkButtonContent
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the ok button specific style.
        /// If null, the default one is used.
        /// </summary>
        public Style OkButtonStyle
        {
            get
            {
                return this.mOkButtonStyle;
            }

            set
            {
                this.mOkButtonStyle = value;
                this.NotifyPropertyChanged("OkButtonStyle");
            }
        }

        #endregion // Properties.

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="InformationViewModel"/> class.
        /// </summary>
        /// <param name="pType">The notification type.</param>
        /// <param name="pId">The notification id.</param>
        public InformationViewModel(string pType, string pId)
            : base(pType, pId)
        {
            this.DefaultAnswer = Answers.Ok;

            this.OkButtonContent = "Ok";
            this.OkButtonStyle = null;
        }

        #endregion // Constructors.
    }
}

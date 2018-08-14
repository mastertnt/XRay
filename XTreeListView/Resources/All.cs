using System;
using System.Windows;

namespace XTreeListView.Resources
{
    /// <summary>
    /// Class defining the global resources.
    /// </summary>
    public partial class All : ResourceDictionary
    {
        #region Fields

        /// <summary>
        /// Stores the unique instance of the singleton.
        /// </summary>
        private static All msInstance;

        #endregion // Fields.

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="All"/> class.
        /// </summary>
        private All()
        {
            this.Source = new System.Uri(@"/XTreeListView;component/Resources/All.xaml", UriKind.Relative);
        }

        #endregion // Constructors.

        #region Properties

        /// <summary>
        /// Gets the unique instance of the singleton.
        /// </summary>
        public static All Instance
        {
            get
            {
                if (msInstance == null)
                {
                    msInstance = new All();
                }

                return msInstance;
            }
        }

        #endregion // Properties.
    }
}

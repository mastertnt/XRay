using System;
using System.Windows;

namespace XGraph.Themes
{
    /// <summary>
    /// Class defining the default theme.
    /// </summary>
    public class ExpressionDark : ResourceDictionary
    {
        #region Fields

        /// <summary>
        /// Stores the unique instance of the singleton.
        /// </summary>
        private static ExpressionDark msInstance;

        #endregion // Fields.

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionDark"/> class.
        /// </summary>
        private ExpressionDark()
        {
            // Loading the xaml part by Uri because the partial class method throw an error due to the Generic.xaml loading process.
            this.Source = new System.Uri(@"/XGraph;component/Themes/ExpressionDark.xaml", UriKind.Relative);
        }

        #endregion // Constructors.

        #region Properties

        /// <summary>
        /// Gets the unique instance of the singleton.
        /// </summary>
        public static ExpressionDark Instance
        {
            get
            {
                if (msInstance == null)
                {
                    msInstance = new ExpressionDark();
                }

                return msInstance;
            }
        }

        #endregion // Properties.
    }
}

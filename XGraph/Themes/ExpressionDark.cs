using System;
using System.Windows;

namespace XGraph.Themes
{
    /// <summary>
    /// Class defining the default theme.
    /// </summary>
    public class ExpressionDark : ResourceDictionary
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionDark"/> class.
        /// </summary>
        public ExpressionDark()
        {
            // Loading the xaml part by Uri because the partial class method throw an error due to the Generic.xaml loading process.
            this.Source = new System.Uri(@"/XGraph;component/Themes/ExpressionDark.xaml", UriKind.Relative);
        }

        #endregion // Constructors.
    }
}

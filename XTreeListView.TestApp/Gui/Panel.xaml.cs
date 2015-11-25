using System.Windows.Controls;
using XTreeListView.Gui;

namespace XTreeListView.TestApp.Gui
{
    /// <summary>
    /// This class defines the panel of this plugin.
    /// </summary>
    /// <!-- DPE -->
    public partial class Panel : UserControl
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the Panel class.
        /// </summary>
        public Panel()
        {
            this.InitializeComponent();
        }

        #endregion // Constructors.

        #region Properties

        /// <summary>
        /// Gets the person tree list view.
        /// </summary>
        public TreeListView PersonTreeListView
        {
            get
            {
                return this.mPersonContainer;
            }
        }

        /// <summary>
        /// Gets the multi column tree list view.
        /// </summary>
        public TreeListView MultiTreeListView
        {
            get
            {
                return this.mMultiContainer;
            }
        }

        #endregion // Properties.
    }
}

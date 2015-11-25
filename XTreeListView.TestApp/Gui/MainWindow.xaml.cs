using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using XTreeListView.Gui;
using XTreeListView.TestApp.Model;
using XTreeListView.TestApp.ViewModel;

namespace XTreeListView.TestApp.Gui
{
    /// <summary>
    /// Class defining the main window.
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Constructors

        /// <summary>
        /// Initializes an instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();

            // Initializing the mono column tree.
            PersonRootViewModel lRootViewModel = new PersonRootViewModel();
            lRootViewModel.SetIsLoadOnDemand(true);
            lRootViewModel.Model = Person.CreateFullTestModel();
            this.mPanel.PersonTreeListView.ViewModel = lRootViewModel;

            // Initializing the multi column tree list view.
            this.mPanel.MultiTreeListView.ViewModel = new RegistryRootViewModel();
            this.mPanel.MultiTreeListView.SetFirstColumn(new TreeListViewColumn() { Header = "Name", DataMemberBindingPath = "Name" });
            this.mPanel.MultiTreeListView.AddColumn(new TreeListViewColumn() { Header = "Kind", DataMemberBindingPath = "Kind" });
            this.mPanel.MultiTreeListView.AddColumn(new TreeListViewColumn() { Header = "Data", DataMemberBindingPath = "Data" });
        }

        #endregion // Constructors.
    }
}

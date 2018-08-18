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
using XTreeListView.ViewModel;

namespace XTreeListView.TestApp.Gui
{
    /// <summary>
    /// Class defining the panel used to tes the tree list view.
    /// </summary>
    public partial class TestPanel : UserControl
    {
        #region Fields

        /// <summary>
        /// Gets or sets the tree list view to test.
        /// </summary>
        private TreeListView mTreeToTest;

        #endregion // Fields.

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TestPanel"/> class.
        /// </summary>
        public TestPanel()
        {
            this.InitializeComponent();

            this.AddColumnButton.Click += this.OnAddColumnButtonClick;
            this.RemoveColumnButton.Click += this.OnRemoveColumnButtonClick;
            this.UnselectItemsButton.Click += OnUnselectItemsButtonClick;
        }

        #endregion // Constructors.

        #region Properties

        /// <summary>
        /// Gets or sets the tree list view to test.
        /// </summary>
        public TreeListView TreeToTest
        {
            get
            {
                return this.mTreeToTest;
            }

            set
            {
                this.mTreeToTest = value;
                this.mTreeToTest.Loaded += this.OnTreeToTestLoaded;
            }
        }

        #endregion // Properties.

        #region Methods

        /// <summary>
        /// Method called when the tree to test is loaded.
        /// </summary>
        /// <param name="pSender">The tree sender.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private void OnTreeToTestLoaded(object pSender, RoutedEventArgs pEventArgs)
        {
            this.ColumnsListBox.ItemsSource = this.TreeToTest.Columns;
            this.SelectedItemsListBox.ItemsSource = this.TreeToTest.SelectedViewModels;
        }

        /// <summary>
        /// Method called when the add column button is clicked.
        /// </summary>
        /// <param name="pSender">The button sender.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private void OnAddColumnButtonClick(object pSender, RoutedEventArgs pEventArgs)
        {
            if (string.IsNullOrEmpty(this.NewColumnPropertyName.Text) == false)
            {
                this.TreeToTest.Columns.Add(new TreeListViewColumn() { Header = this.NewColumnPropertyName.Text, DataMemberBindingPath = this.NewColumnPropertyName.Text });
            }
        }

        /// <summary>
        /// Method called when the remove column button is clicked.
        /// </summary>
        /// <param name="pSender">The button sender.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private void OnRemoveColumnButtonClick(object pSender, RoutedEventArgs pEventArgs)
        {
            TreeListViewColumn lColumn = this.ColumnsListBox.SelectedValue as TreeListViewColumn;
            if (lColumn != null)
            {
                this.TreeToTest.Columns.Remove(lColumn);
            }
        }

        /// <summary>
        /// Method called when the unselect items button is clicked.
        /// </summary>
        /// <param name="pSender">The button sender.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private void OnUnselectItemsButtonClick(object pSender, RoutedEventArgs pEventArgs)
        {
            foreach (IHierarchicalItemViewModel lItem in this.SelectedItemsListBox.SelectedItems.OfType<IHierarchicalItemViewModel>().ToList())
            {
                this.TreeToTest.Unselect(lItem);
            }
        }

        #endregion // Methods.
    }
}

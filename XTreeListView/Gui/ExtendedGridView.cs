using XTreeListView.Converters;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace XTreeListView.Gui
{
    /// <summary>
    /// Class extending the <see cref="GridView"/> control.
    /// </summary>
    public class ExtendedGridView : GridView
    {
        #region Fields

        /// <summary>
        /// Stores the flag indicating if the column header is visible.
        /// </summary>
        private bool mShowColumnHeaders;

        /// <summary>
        /// Stores the resources.
        /// </summary>
        private Resources mResources;

        /// <summary>
        /// Stores the copie of the initial grid view columns properties.
        /// </summary>
        private GridViewColumnCollection mGridViewColumnBackup;

        #endregion // Fields.

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtendedGridView"/> class.
        /// </summary>
        /// <param name="pResources"></param>
        public ExtendedGridView(Resources pResources)
        {
            this.mResources = new Resources();
            this.ShowColumnHeaders = false;
            this.mGridViewColumnBackup = new GridViewColumnCollection();

            this.Columns.CollectionChanged += this.OnGridViewColumnsCollectionChanged;
        }

        #endregion // Constructors.

        #region Properties

        /// <summary>
        /// Gets or sets the flag indicating if the column headers are visible or not.
        /// </summary>
        public bool ShowColumnHeaders
        {
            get
            {
                return this.mShowColumnHeaders;
            }
            set
            {
                this.mShowColumnHeaders = value;

                if (value == true)
                {
                    this.ColumnHeaderContainerStyle = null;
                }
                else
                {
                    this.ColumnHeaderContainerStyle = this.mResources["CollapsedGridViewColumnHeaderStyle"] as Style;
                }
            }
        }

        #endregion // Properties.

        #region Methods

        /// <summary>
        /// Sets the first column of the tree list view.
        /// </summary>
        /// <param name="pColumn">The first column properties.</param>
        public void SetFirstColumn(TreeListViewColumn pColumn)
        {
            this.Columns.CollectionChanged -= this.OnGridViewColumnsCollectionChanged;

            ExtendedGridViewColumn lColumn = ExtendedGridViewColumn.CreateFrom(pColumn, 0);

            if (this.Columns.Any())
            {
                this.Columns.RemoveAt(0);
            }

            if (this.Columns.Any())
            {
                this.Columns.Insert(0, lColumn);
            }
            else
            {
                this.Columns.Add(lColumn);
            }

            if (this.mGridViewColumnBackup.Any())
            {
                this.mGridViewColumnBackup.RemoveAt(0);
            }

            if (this.mGridViewColumnBackup.Any())
            {
                this.mGridViewColumnBackup.Insert(0, lColumn.Clone());
            }
            else
            {
                this.mGridViewColumnBackup.Add(lColumn.Clone());
            }

            this.UpdateColumns();

            this.Columns.CollectionChanged += this.OnGridViewColumnsCollectionChanged;
        }

        /// <summary>
        /// Adds a new column to the tree list view.
        /// </summary>
        /// <param name="pColumn">The column to add.</param>
        public void AddColumn(TreeListViewColumn pColumn)
        {
            this.Columns.CollectionChanged -= this.OnGridViewColumnsCollectionChanged;

            ExtendedGridViewColumn lColumn = ExtendedGridViewColumn.CreateFrom(pColumn, this.Columns.Count);
            
            this.Columns.Add(lColumn);
            this.mGridViewColumnBackup.Add(lColumn.Clone());

            this.UpdateColumnHeaderVisibility();

            this.Columns.CollectionChanged += this.OnGridViewColumnsCollectionChanged;
        }

        /// <summary>
        /// Updates the grid view columns.
        /// </summary>
        private void UpdateColumns()
        {
            for (int lIter = 0; lIter < this.Columns.Count; lIter++)
            {
                // Indenting the data template used in the first column.
                if
                    (lIter == 0)
                {
                    // Creating the indentation of the data template.
                    Binding lTemplateMarginBinding = new Binding("DecoratorWidth");
                    lTemplateMarginBinding.Converter = new WidthToLeftMarginConverter();

                    // Creating the new cell data template.
                    System.Windows.DataTemplate lCellDataTemplate = new System.Windows.DataTemplate();

                    // Defining the visual tree.
                    FrameworkElementFactory lCellFactory = new FrameworkElementFactory(typeof(ContentControl));
                    lCellFactory.SetBinding(ContentControl.MarginProperty, lTemplateMarginBinding);
                    lCellFactory.SetBinding(ContentControl.ContentProperty, new Binding() { BindsDirectlyToSource = true });

                    // Trying to get the data template already defined in the first column.
                    if
                        (this.Columns[0].CellTemplate != null)
                    {
                        lCellFactory.SetValue(ContentControl.ContentTemplateProperty, this.Columns[0].CellTemplate);
                    }

                    // Trying to get the data template selector if any.
                    if
                        (this.Columns[0].CellTemplateSelector != null)
                    {
                        lCellFactory.SetValue(ContentControl.ContentTemplateSelectorProperty, this.Columns[0].CellTemplateSelector);

                        // Removing the template selector from the cell as it is now applied in the ContentControl.
                        this.Columns[0].CellTemplateSelector = null;
                    }

                    // Trying to get the display path.
                    if
                        (this.Columns[0].DisplayMemberBinding != null)
                    {
                        lCellFactory.SetValue(ContentControl.ContentProperty, this.Columns[0].DisplayMemberBinding);

                        // Removing the template selector from the cell as it is now applied in the ContentControl.
                        this.Columns[0].DisplayMemberBinding = null;
                    }

                    // Updating the cell template.
                    lCellDataTemplate.VisualTree = lCellFactory;
                    this.Columns[0].CellTemplate = lCellDataTemplate;
                }
                else
                {
                    // Other columns have classic configuration.
                    this.Columns[lIter].CellTemplate = this.mGridViewColumnBackup[lIter].CellTemplate;
                    this.Columns[lIter].CellTemplateSelector = this.mGridViewColumnBackup[lIter].CellTemplateSelector;
                    this.Columns[lIter].DisplayMemberBinding = this.mGridViewColumnBackup[lIter].DisplayMemberBinding;
                }
            }
        }

        /// <summary>
        /// Delegate called when the columns collection of the grid view is modified.
        /// </summary>
        /// <param name="pSender">The modified grid view.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private void OnGridViewColumnsCollectionChanged(object pSender, NotifyCollectionChangedEventArgs pEventArgs)
        {
            switch
                (pEventArgs.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    throw new NotImplementedException();

                case NotifyCollectionChangedAction.Remove:
                    throw new NotImplementedException();

                case NotifyCollectionChangedAction.Replace:
                    throw new NotImplementedException();

                case NotifyCollectionChangedAction.Move:
                    {
                        // Updating the backup so that columns are in the same order.
                        this.mGridViewColumnBackup.Move(pEventArgs.OldStartingIndex, pEventArgs.NewStartingIndex);

                        // Updating the grid view if the first column has been moved.
                        if (pEventArgs.OldStartingIndex == 0 || pEventArgs.NewStartingIndex == 0)
                        {
                            this.UpdateColumns();
                        }
                    }

                    break;

                case NotifyCollectionChangedAction.Reset:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Updates the column header visibility depending on the user column filter flag.
        /// </summary>
        private void UpdateColumnHeaderVisibility()
        {
            this.ShowColumnHeaders = this.Columns.Count > 1;
        }

        #endregion // Methods.
    }
}

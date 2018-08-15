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

        #endregion // Fields.

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtendedGridView"/> class.
        /// </summary>
        public ExtendedGridView()
        {
            this.ShowColumnHeaders = true;
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
                    this.ColumnHeaderContainerStyle = XTreeListView.Resources.All.Instance["CollapsedGridViewColumnHeaderStyle"] as Style;
                }
            }
        }

        /// <summary>
        /// Gets the item container default style key.
        /// </summary>
        protected override object ItemContainerDefaultStyleKey 
        { 
            get
            {
                return TreeListViewItem.MultiColumnDefaultStyleKey;
            }
        }

        #endregion // Properties.

        #region Methods

        /// <summary>
        /// Updates the grid view columns with the given collection.
        /// </summary>
        /// <param name="pCollection">The collection to synchronize.</param>
        internal void SynchronizeColumns(TreeListViewColumnCollection pCollection)
        {
            // Clearing columns.
            this.Columns.Clear();

            for (int lIter = 0; lIter < pCollection.Count; lIter++)
            {
                // Creating a column for each backuped column.
                ExtendedGridViewColumn lColumn = ExtendedGridViewColumn.CreateFrom(pCollection[lIter], lIter);

                // Indenting the data template used in the first column.
                if (lIter == 0)
                {
                    // Extra margin between the end of the decorators part and the beginning of the item data template to remove.
                    double lExtraMargin = 4;

                    // Creating the indentation of the data template.
                    Binding lTemplateMarginBinding = new Binding("DecoratorWidth");
                    lTemplateMarginBinding.Converter = new WidthToLeftMarginConverter() { Margin = lExtraMargin };

                    // Creating the new cell data template.
                    System.Windows.DataTemplate lCellDataTemplate = new System.Windows.DataTemplate();

                    // Defining the visual tree.
                    FrameworkElementFactory lCellFactory = new FrameworkElementFactory(typeof(ContentControl));
                    lCellFactory.SetBinding(ContentControl.MarginProperty, lTemplateMarginBinding);
                    lCellFactory.SetBinding(ContentControl.ContentProperty, new Binding() { BindsDirectlyToSource = true });

                    // Trying to get the data template already defined in the first column.
                    if (lColumn.CellTemplate != null)
                    {
                        lCellFactory.SetValue(ContentControl.ContentTemplateProperty, lColumn.CellTemplate);
                    }

                    // Trying to get the data template selector if any.
                    if (lColumn.CellTemplateSelector != null)
                    {
                        lCellFactory.SetValue(ContentControl.ContentTemplateSelectorProperty, lColumn.CellTemplateSelector);

                        // Removing the template selector from the cell as it is now applied in the ContentControl.
                        lColumn.CellTemplateSelector = null;
                    }

                    // Trying to get the display path.
                    if (lColumn.DisplayMemberBinding != null)
                    {
                        lCellFactory.SetValue(ContentControl.ContentProperty, lColumn.DisplayMemberBinding);

                        // Removing the template selector from the cell as it is now applied in the ContentControl.
                        lColumn.DisplayMemberBinding = null;
                    }

                    // Updating the cell template.
                    lCellDataTemplate.VisualTree = lCellFactory;
                    lColumn.CellTemplate = lCellDataTemplate;
                }

                // Adding the column.
                this.Columns.Add(lColumn);
            }
        }

        #endregion // Methods.
    }
}

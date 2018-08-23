using System;
using System.Windows.Controls;

namespace XTreeListView.Gui
{
    /// <summary>
    /// This class defines an tree list view column.
    /// </summary>
    public class TreeListViewColumn
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeListViewColumn"/> class.
        /// </summary>
        public TreeListViewColumn()
        {
            this.Header = string.Empty;
            this.DisplayMemberPath = string.Empty;
            this.Width = 1;
            this.Stretch = true;
        }

        #endregion // Constructors.

        #region Properties

        /// <summary>
        /// Gets or sets the header of the column.
        /// </summary>
        public string Header 
        { 
            get; 
            set; 
        }

        /// <summary>
        /// Gets or sets the path from the view model to the value to display in the column.
        /// </summary>
        public string DisplayMemberPath 
        { 
            get; 
            set; 
        }

        /// <summary>
        /// Gets or sets the column default width (px).
        /// </summary>
        public int Width 
        { 
            get; 
            set; 
        }

        /// <summary>
        /// Gets or sets the flag indicating if the column is stretched or not.
        /// </summary>
        /// <remarks>
        /// If the column is stretched, the Width property defines the stretching ratio and is no more in pixel.
        /// </remarks>
        public bool Stretch 
        { 
            get; 
            set; 
        }

        /// <summary>
        /// Gets the template selector.
        /// </summary>
        public DataTemplateSelector TemplateSelector 
        { 
            get; 
            set; 
        }

        #endregion // Properties.
    }
}

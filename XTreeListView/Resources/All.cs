using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Data;

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

        /// <summary>
        /// Stores the key of the data template binding to source in a cell.
        /// </summary>
        public static string BindToSourceCellDataTemplateKey = "BindToSourceCellDataTemplateKey";

        /// <summary>
        /// Stores the map of the cell data template by display member path.
        /// </summary>
        private Dictionary<string, System.Windows.DataTemplate> mCellDataTemplates;

        #endregion // Fields.

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="All"/> class.
        /// </summary>
        private All()
        {
            this.Source = new System.Uri(@"/XTreeListView;component/Resources/All.xaml", UriKind.Relative);
            
            this.mCellDataTemplates = new Dictionary<string, System.Windows.DataTemplate>();
            this.mCellDataTemplates.Add(BindToSourceCellDataTemplateKey, this[BindToSourceCellDataTemplateKey] as System.Windows.DataTemplate);
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

        #region Methods
        
        /// <summary>
        /// Returns the cell data template displaying the member path in a text block.
        /// </summary>
        /// <param name="pDisplayMemberPath">The display member path.</param>
        /// <returns>The built data template.</returns>
        public System.Windows.DataTemplate GetCellTemplate(string pDisplayMemberPath)
        {
            // Trying to get it from the cache.
            System.Windows.DataTemplate lDataTemplate;
            if (this.mCellDataTemplates.TryGetValue(pDisplayMemberPath, out lDataTemplate))
            {
                return lDataTemplate;
            }

            if (string.IsNullOrEmpty(pDisplayMemberPath) == false)
            {
                // Building dynamically the data template.
                FrameworkElementFactory lTextBlockFactory = new FrameworkElementFactory(typeof(System.Windows.Controls.TextBlock));
                Binding lDisplayMemberBinding = new Binding(pDisplayMemberPath);
                lTextBlockFactory.SetBinding(System.Windows.Controls.TextBlock.TextProperty, lDisplayMemberBinding);
                lDataTemplate = new System.Windows.DataTemplate();
                lDataTemplate.VisualTree = lTextBlockFactory;
                lDataTemplate.Seal();

                // Caching the data template.
                this.mCellDataTemplates.Add(pDisplayMemberPath, lDataTemplate);

                return lDataTemplate;
            }

            // Getting the bind to source data template.
            return this.mCellDataTemplates[BindToSourceCellDataTemplateKey];
        }

        #endregion // Methods.
    }
}

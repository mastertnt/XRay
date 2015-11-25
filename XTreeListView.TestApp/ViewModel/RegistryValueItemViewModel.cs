using System;
using System.Windows.Media.Imaging;
using XTreeListView.ViewModel.Generic;
using XTreeListView.TestApp.Model;
using System.Windows.Media;

namespace XTreeListView.TestApp.ViewModel
{
    /// <summary>
    /// This class defines a view model for the registry value model.
    /// </summary>
    /// <!-- DPE -->
    internal class RegistryValueItemViewModel : AHierarchicalItemViewModel<RegistryValue>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistryValueItemViewModel"/> class.
        /// </summary>
        /// <param name="pOwnedObject">The owned object.</param>
        public RegistryValueItemViewModel(RegistryValue pOwnedObject)
            : base(pOwnedObject)
        {
            this.ToolTip = this.Name;
        }

        #endregion // Constructors.

        #region Properties

        /// <summary>
        /// Gets the registry element name.
        /// </summary>
        public string Name
        {
            get
            {
                return this.OwnedObject.Name;
            }
        }

        /// <summary>
        /// Gets the registry element kind.
        /// </summary>
        public object Kind
        {
            get
            {
                return this.OwnedObject.Kind;
            }
        }

        /// <summary>
        /// Gets the registry element data.
        /// </summary>
        public object Data
        {
            get
            {
                return this.OwnedObject.Data;
            }
        }

        /// <summary>
        /// Gets the icon to display in the item.
        /// </summary>
        public override ImageSource IconSource
        {
            get
            {
                if
                    (this.OwnedObject.Kind == Microsoft.Win32.RegistryValueKind.String)
                {
                    return new BitmapImage(new Uri(@"/XTreeListView.TestApp;component/Resources/DataString.png", UriKind.Relative));
                }
                else
                {
                    return new BitmapImage(new Uri(@"/XTreeListView.TestApp;component/Resources/Data.png", UriKind.Relative));
                }
            }
        }

        #endregion // Properties.
    }
}

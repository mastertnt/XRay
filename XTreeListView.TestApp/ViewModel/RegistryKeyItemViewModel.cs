using System;
using System.Linq;
using Microsoft.Win32;
using System.Windows.Media.Imaging;
using XTreeListView.ViewModel.Generic;
using XTreeListView.TestApp.Model;
using System.Windows.Media;

namespace XTreeListView.TestApp.ViewModel
{
    /// <summary>
    /// This class defines a view model for the registry key model.
    /// </summary>
    /// <!-- DPE -->
    internal class RegistryKeyItemViewModel : AHierarchicalItemViewModel<RegistryKey>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistryKeyItemViewModel"/> class.
        /// </summary>
        public RegistryKeyItemViewModel(RegistryKey pOwnedObject)
            : base(pOwnedObject)
        {
            this.DisplayString = "Key";
            this.ToolTipContent = this.Name;
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
        public string Kind
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the registry element data.
        /// </summary>
        public object Data
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the icon to display in the item.
        /// </summary>
        public override ImageSource IconSource
        {
            get
            {
                return new BitmapImage(new Uri(@"/XTreeListView.TestApp;component/Resources/Folder.png", UriKind.Relative));
            }
        }

        /// <summary>
        /// Gets the flag indicating if the item has children.
        /// </summary>
        public override bool HasChildrenLoadedOnDemand
        {
            get
            {
                return true;
            }
        }

        #endregion // Properties.

        #region Methods

        /// <summary>
        /// Registers the children of this item on demand.
        /// </summary>
        protected override void InternalRegisterChildren()
        {
            if
                (this.Children.Count() == 0)
            {
                foreach
                    (String lName in this.OwnedObject.GetSubKeyNames())
                {
                    RegistryKey lSubKey = null;
                    try
                    {
                        lSubKey = this.OwnedObject.OpenSubKey(lName);
                    }
                    catch
                    {
                    }
                    if
                        (lSubKey != null)
                    {
                        this.AddChild(new RegistryKeyItemViewModel(lSubKey));
                    }
                }

                foreach
                    (String lName in this.OwnedObject.GetValueNames())
                {
                    RegistryValue lRegValue = new RegistryValue()
                    {
                        Name = lName,
                        Data = this.OwnedObject.GetValue(lName),
                        Kind = this.OwnedObject.GetValueKind(lName)
                    };

                    this.AddChild(new RegistryValueItemViewModel(lRegValue));
                }
            }
        }

        #endregion // Methods.
    }
}

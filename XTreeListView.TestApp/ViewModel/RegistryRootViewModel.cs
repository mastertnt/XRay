using System;
using Microsoft.Win32;
using XTreeListView.ViewModel.Generic;

namespace XTreeListView.TestApp.ViewModel
{
    /// <summary>
    /// This class defines the view model of the multi column tree view.
    /// </summary>
    /// <!-- DPE -->
    internal class RegistryRootViewModel : ARootHierarchicalItemViewModel<object>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistryRootViewModel"/> class.
        /// </summary>
        public RegistryRootViewModel()
        {
            this.AddChild(new RegistryKeyItemViewModel(Registry.ClassesRoot));
            this.AddChild(new RegistryKeyItemViewModel(Registry.CurrentUser));
            this.AddChild(new RegistryKeyItemViewModel(Registry.LocalMachine));
            this.AddChild(new RegistryKeyItemViewModel(Registry.Users));
            this.AddChild(new RegistryKeyItemViewModel(Registry.CurrentConfig));
        }

        #endregion // Constructors.

        #region Properties

        /// <summary>
        /// Gets the flag indicating if the items are loaded on demand.
        /// </summary>
        protected override bool LoadItemsOnDemand
        {
            get
            {
                return true;
            }
        }

        #endregion // Properties.
    }
}

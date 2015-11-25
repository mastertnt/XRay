using System;
using Microsoft.Win32;

namespace XTreeListView.TestApp.Model
{
    /// <summary>
    /// This class defines the model of a registry value.
    /// </summary>
    /// <!-- DPE -->
    internal class RegistryValue
    {
        #region Properties

        /// <summary>
        /// Gets or sets the name of the registry value.
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// Gets or sets the data of the registry value.
        /// </summary>
        public Object Data { get; set; }

        /// <summary>
        /// Gets or sets the type of the registry value.
        /// </summary>
        public RegistryValueKind Kind { get; set; }

        #endregion // Properties.
    }
}

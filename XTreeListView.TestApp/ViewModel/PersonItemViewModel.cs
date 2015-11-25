using System;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using XTreeListView.TestApp.Model;
using XTreeListView.ViewModel.Generic;

namespace XTreeListView.TestApp.ViewModel
{
    /// <summary>
    /// This class defines a view model for the Person model.
    /// </summary>
    /// <!-- DPE -->
    internal class PersonItemViewModel : AHierarchicalItemViewModel<Person>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PersonItemViewModel"/> class.
        /// </summary>
        /// <param name="pOwnedObject">The owned object.</param>
        public PersonItemViewModel(Person pOwnedObject)
            :base(pOwnedObject)
        {
            this.ToolTipContent = "Custom tooltip test";
            this.BindChildren("Children", typeof(PersonItemViewModel));
        }

        #endregion // Constructors.

        #region Properties

        /// <summary>
        /// Gets the person name.
        /// </summary>
        public override string DisplayString
        {
            get
            {
                return this.OwnedObject.Name;
            }
        }

        /// <summary>
        /// Gets the person id.
        /// </summary>
        public string Id
        {
            get
            {
                return this.OwnedObject.Id.ToString();
            }
        }

        /// <summary>
        /// Gets the flag indicating if the item has children.
        /// </summary>
        public override bool HasChildrenLoadedOnDemand
        {
            get
            {
                return this.OwnedObject.Children.Any();
            }
        }

        /// <summary>
        /// Gets the icon to display in the item.
        /// </summary>
        public override ImageSource IconSource
        {
            get
            {
                return new BitmapImage(new Uri(@"/XTreeListView.TestApp;component/Resources/Person.png", UriKind.Relative));
            }
        }

        /// <summary>
        /// Gets the falg indicating if the item is checkable.
        /// </summary>
        public override bool IsCheckable
        {
            get
            {
                return true;
            }
        }

        #endregion // Properties.

        #region Methods

        /// <summary>
        /// Returns the current object as string.
        /// </summary>
        /// <returns>The string description of the object.</returns>
        public override String ToString()
        {
            return this.OwnedObject.ToString();
        }

        #endregion // Methods.
    }
}

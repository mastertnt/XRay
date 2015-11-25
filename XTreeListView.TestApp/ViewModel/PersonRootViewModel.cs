using XTreeListView.TestApp.Model;
using XTreeListView.ViewModel.Generic;

namespace XTreeListView.TestApp.ViewModel
{
    /// <summary>
    /// This class defines the view model of the tree view without any column.
    /// </summary>
    /// <!-- DPE -->
    internal class PersonRootViewModel : ARootHierarchicalItemViewModel<Person>
    {
        #region Fields

        /// <summary>
        /// Stores the flag to know if the view model is loaded on demand.
        /// </summary>
        private bool mLoadOnDemand;

        #endregion // Fields.

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PersonRootViewModel"/> class.
        /// </summary>
        public PersonRootViewModel()
        {
        }

        #endregion // Constructors.

        #region Properties

        /// <summary>
        /// Gets or sets the model associated to this view model.
        /// </summary>
        public override Person Model
        {
            get
            {
                return base.Model;
            }

            set
            {
                base.Model = value;
                this.BindChildren("Children", typeof(PersonItemViewModel));
            }
        }

        /// <summary>
        /// Gets the flag indicating if the items are loaded on demand.
        /// </summary>
        protected override bool LoadItemsOnDemand
        {
            get
            {
                return this.mLoadOnDemand;
            }
        }

        #endregion // Properties.

        #region Methods

        /// <summary>
        /// Sets the load on demand flag.
        /// </summary>
        /// <param name="pFlag">The flag to know if the view model is loaded on demand.</param>
        public void SetIsLoadOnDemand(bool pFlag)
        {
            this.mLoadOnDemand = pFlag;
        }

        #endregion // Methods.
    }
}

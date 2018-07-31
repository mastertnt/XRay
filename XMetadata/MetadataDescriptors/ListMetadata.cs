using System.Collections.Generic;

namespace XMetadata.MetadataDescriptors
{
    /// <summary>
    /// Definition of the <see cref="ListMetadata"/> class.
    /// </summary>
    public class ListMetadata : AMetadata<List<object>>
    {
        #region Fields

        /// <summary>
        /// Stores the list of sub meta data.
        /// </summary>
        private List<IMetadata> mList;

        #endregion // Fields.

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ListMetadata"/> class.
        /// </summary>
        /// <param name="pId">The metadata identifier.</param>
        public ListMetadata(string pId)
            : base(pId)
        {
            this.mList = new List<IMetadata>();
        }

        #endregion // Constructors.

        #region Methods

        /// <summary>
        /// Adds a new child meta data.
        /// </summary>
        /// <param name="pChild">The child meta data to add.</param>
        public void AddMetadata(IMetadata pChild)
        {
            this.mList.Add(pChild);
        }

        /// <summary>
        /// Gets the default value.
        /// </summary>
        /// <returns>The default value.</returns>
        public override object GetDefautValue()
        {
            List<object> lDefault = new List<object>();
            foreach (IMetadata lChild in this.mList)
            {
                lDefault.Add(lChild.GetDefautValue());
            }

            return lDefault;
        }

        #endregion // Methods.
    }
}

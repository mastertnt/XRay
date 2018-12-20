using System.Collections.ObjectModel;

namespace XSystem.Collections
{
    /// <summary>
    ///     Provides a HashSet preserving the insertion order.
    /// </summary>
    /// <typeparam name="TItemType">The type of the item type.</typeparam>
    /// <seealso cref="System.Collections.ObjectModel.KeyedCollection{TItemType,TItemType}" />
    public class LinkedHashSet<TItemType> : KeyedCollection<TItemType, TItemType>
    {
        #region Methods

        /// <summary>
        ///     Gets the key for item.
        /// </summary>
        /// <param name="pItem">The item.</param>
        /// <returns></returns>
        protected override TItemType GetKeyForItem(TItemType pItem)
        {
            return pItem;
        }

        #endregion // Methods.

        #region Fields

        #endregion // Fields.

        #region Properties

        #endregion // Properties.

        #region Events

        #endregion // Events.

        #region Constructor

        #endregion // Constructor.
    }
}
using System;
using System.ComponentModel;

namespace XSystem.Dynamic
{
    /// <summary>
    ///     This class defines a type description provider for DynamicTypedObject.
    /// </summary>
    public class DynamicTypedObjectDescriptionProvider : TypeDescriptionProvider
    {
        #region Fields

        /// <summary>
        ///     This field stores the default provider.
        /// </summary>
        private static readonly TypeDescriptionProvider msDefaultProvider = TypeDescriptor.GetProvider(typeof(DynamicTypedObject));

        #endregion // Fields.

        #region Constructors

        /// <summary>
        ///     Default constructor.
        /// </summary>
        public DynamicTypedObjectDescriptionProvider() : base(msDefaultProvider)
        {
        }

        #endregion // Constructors.

        #region Methods

        /// <summary>
        ///     Gets the type descriptor.
        /// </summary>
        /// <param name="pObjectType">Type of the object.</param>
        /// <param name="pInstance">The instance.</param>
        /// <returns>The type descriptor if exists, null otherwise.</returns>
        public override ICustomTypeDescriptor GetTypeDescriptor(Type pObjectType, object pInstance)
        {
            if (pInstance is DynamicTypedObject)
            {
                return new DynamicTypedObjectTypeDescriptor(pInstance as DynamicTypedObject);
            }

            return base.GetTypeDescriptor(pObjectType, pInstance);
        }

        #endregion // Methods.
    }
}
using System;
using System.ComponentModel;

namespace XSystem.Dynamic
{
    /// <summary>
    /// This class is used to build a new property descriptor attached to a dynamic typed object <see cref="DynamicTypedObject" />
    /// </summary>
    public class DynamicTypedObjectPropertyDescriptor : PropertyDescriptor
    {
        #region Fields

        /// <summary>
        /// This field stores the property type.
        /// </summary>
        private readonly Type mPropertyType;

        #endregion // Fields.

        #region Properties

        /// <summary>
        /// Returns the type of the component (owner).
        /// </summary>
        public override Type ComponentType
        {
            get 
            { 
                return typeof(DynamicTypedObject);
            }
        }

        /// <summary>
        /// Returns the type of the property.
        /// </summary>
        public override Type PropertyType
        {
            get 
            {
                return this.mPropertyType; 
            }
        }
        
        /// <summary>
        /// Checks if the property is read-only.
        /// </summary>
        /// <returns>True if the property is read-only, false otherwise.</returns>
        public override bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        #endregion // Properties.

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicTypedObjectPropertyDescriptor"/> class.
        /// </summary>
        /// <param name="pDescriptor">The descriptor.</param>
        public DynamicTypedObjectPropertyDescriptor(PropertyDescriptor pDescriptor)
            : base(pDescriptor)
        {
            this.mPropertyType = pDescriptor.PropertyType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicTypedObjectPropertyDescriptor"/> class.
        /// </summary>
        /// <param name="pPropertyName">Name of the property.</param>
        /// <param name="pPropertyType">Type of the property.</param>
        /// <param name="pAttributes">The attributes</param>
        public DynamicTypedObjectPropertyDescriptor(string pPropertyName, Type pPropertyType,  Attribute[] pAttributes = null )
            : base(pPropertyName, pAttributes)
        {
            this.mPropertyType = pPropertyType;
        }

        #endregion // Constructors.

        #region Methods

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0} : {1}", this.Name, this.PropertyType.Name);
        }

        /// <summary>
        /// This method checks if the value can be reset.
        /// </summary>
        /// <param name="pComponent">The component</param>
        /// <returns>True if the property is read-only, false otherwise.</returns>
        public override bool CanResetValue(object pComponent)
        {
            return false;
        }

        /// <summary>
        /// This method gets the value.
        /// </summary>
        /// <param name="pComponent">The component.</param>
        /// <returns>the returned value.</returns>
        public override object GetValue(object pComponent)
        {
            DynamicTypedObject lObject = pComponent as DynamicTypedObject;
            if (lObject != null)
            {
                object lResult = null;
                lObject.TryGetMember(this.Name, out lResult);
                return lResult;
            }

            return null;
        }

        /// <summary>
        /// This method resets the value.
        /// </summary>
        /// <param name="pComponent">The component.</param>
        public override void ResetValue(object pComponent)
        {
            // Nothing to do.
        }

        /// <summary>
        /// This method sets the value.
        /// </summary>
        /// <param name="pComponent">The component.</param>
        /// <param name="pValue">The value to set.</param>
        public override void SetValue(object pComponent, object pValue)
        {
            DynamicTypedObject lObject = pComponent as DynamicTypedObject;
            if (lObject != null)
            {
                lObject.TrySetMember(this.Name, pValue);
            }
        }
        
        /// <summary>
        /// This method checks if the value should be serialized.
        /// </summary>
        /// <param name="pComponent">The component.</param>
        /// <returns>True if the property should be serialized, false otherwise.</returns>
        public override bool ShouldSerializeValue(object pComponent)
        {
            return false;
        }

        #endregion // Methods.
    }
}

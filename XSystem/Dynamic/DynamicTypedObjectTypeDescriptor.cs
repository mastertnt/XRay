using System;
using System.ComponentModel;

namespace XSystem.Dynamic
{
    /// <summary>
    ///     This class defines a descriptor for DynamicTypedObject
    /// </summary>
    public class DynamicTypedObjectTypeDescriptor : ICustomTypeDescriptor
    {
        #region Fields

        /// <summary>
        ///     This field stores the instance of dynamic.
        /// </summary>
        private readonly DynamicTypedObject mInstance;

        #endregion // Fields.

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DynamicTypedObjectTypeDescriptor" /> class.
        /// </summary>
        /// <param name="pInstance">The component instance.</param>
        public DynamicTypedObjectTypeDescriptor(DynamicTypedObject pInstance)
        {
            this.mInstance = pInstance;
        }

        #endregion // Constructors.

        #region Methods

        /// <summary>
        ///     This method returns the name of the instance of the component.
        /// </summary>
        /// <returns>
        ///     Name of the instance or null if the instance has no name.
        /// </returns>
        public string GetComponentName()
        {
            return TypeDescriptor.GetComponentName(this, true);
        }

        /// <summary>
        ///     This method returns the default event for the instance of a component.
        /// </summary>
        /// <returns>
        ///     <see cref="T:System.ComponentModel.EventDescriptor" /> which reprensent the default event or null if the instance
        ///     has no event.
        /// </returns>
        public EventDescriptor GetDefaultEvent()
        {
            return TypeDescriptor.GetDefaultEvent(this, true);
        }

        /// <summary>
        ///     This method returns the class name for the instance of a component.
        /// </summary>
        /// <returns>
        ///     The class name or null if the class has no name.
        /// </returns>
        public string GetClassName()
        {
            return TypeDescriptor.GetClassName(this, true);
        }

        /// <summary>
        ///     This method returns the events for the instance of a component.
        /// </summary>
        /// <param name="pAttributes">Array of types <see cref="T:System.Attribute" /> used as filter.</param>
        /// <returns>
        ///     <see cref="T:System.ComponentModel.EventDescriptorCollection" /> which represents the filtered events for the
        ///     instance of this component.
        /// </returns>
        public EventDescriptorCollection GetEvents(Attribute[] pAttributes)
        {
            return TypeDescriptor.GetEvents(this, pAttributes, true);
        }

        /// <summary>
        ///     This method returns the events for the instance of a component.
        /// </summary>
        /// <returns>
        ///     <see cref="T:System.ComponentModel.EventDescriptorCollection" /> which represents the events for the instance of
        ///     this component.
        /// </returns>
        EventDescriptorCollection ICustomTypeDescriptor.GetEvents()
        {
            return TypeDescriptor.GetEvents(this, true);
        }

        /// <summary>
        ///     This method returns a converter for the instance of a component.
        /// </summary>
        /// <returns>
        ///     <see cref="T:System.ComponentModel.TypeConverter" /> which is the converter of this instance or null if there is no
        ///     converter.
        /// </returns>
        public TypeConverter GetConverter()
        {
            return TypeDescriptor.GetConverter(this, true);
        }

        /// <summary>
        ///     This method returns the owner of the property descriptor.
        /// </summary>
        /// <param name="pPropertyDescriptor">
        ///     <see cref="T:System.ComponentModel.PropertyDescriptor" /> which represents the
        ///     property
        /// </param>
        /// <returns>
        ///     <see cref="T:System.Object" /> which represents the owner of the property.
        /// </returns>
        public object GetPropertyOwner(PropertyDescriptor pPropertyDescriptor)
        {
            return this.mInstance;
        }

        /// <summary>
        ///     This method retrieves the attribute collection for the instance of the component.
        /// </summary>
        /// <returns>
        ///     <see cref="T:System.ComponentModel.AttributeCollection" /> which contains the attribute collection for the instance
        ///     of the component.
        /// </returns>
        public AttributeCollection GetAttributes()
        {
            return TypeDescriptor.GetAttributes(this, true);
        }

        /// <summary>
        ///     This method retrieves the defaut editor of the instance of the component.
        /// </summary>
        /// <param name="pEditorBaseType"><see cref="T:System.Type" /> which represents the editor for this object.</param>
        /// <returns>
        ///     The editor of the specified type or null if the object has no editor.
        /// </returns>
        public object GetEditor(Type pEditorBaseType)
        {
            return TypeDescriptor.GetEditor(this, pEditorBaseType, true);
        }

        /// <summary>
        ///     This method retrieves the default property of the instance of the component.
        /// </summary>
        /// <returns>
        ///     <see cref="T:System.ComponentModel.PropertyDescriptor" /> which represents the default property of the instance or
        ///     NULL if the instance has no property.
        /// </returns>
        public PropertyDescriptor GetDefaultProperty()
        {
            return null;
        }

        /// <summary>
        ///     This method retrieves the property descritpors of the instance of the component.
        /// </summary>
        /// <returns>
        ///     <see cref="T:System.ComponentModel.PropertyDescriptorCollection" /> which represents the properties of the instance
        ///     of the component.
        /// </returns>
        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
        {
            return ((ICustomTypeDescriptor) this).GetProperties(new Attribute[0]);
        }

        /// <summary>
        ///     This method retrieves the property descritpors of the instance of the component.
        /// </summary>
        /// <param name="pAttributes">Array of types <see cref="T:System.Attribute" /> used as filter.</param>
        /// <returns>
        ///     <see cref="T:System.ComponentModel.PropertyDescriptorCollection" /> which represents the properties of the instance
        ///     of the component.
        /// </returns>
        public PropertyDescriptorCollection GetProperties(Attribute[] pAttributes)
        {
            return this.mInstance.PropertyDescriptors;
        }

        #endregion // Methods.
    }
}
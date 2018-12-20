using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;

namespace XSystem.Dynamic
{
    /// <summary>
    ///     This class is used to define an object properties at runtime.
    /// </summary>
    public class DynamicTypedObject : DynamicObject, INotifyPropertyChanged
    {
        #region Fields

        /// <summary>
        ///     This field stores the members of the object.
        /// </summary>
        protected readonly Dictionary<string, object> mMembers = new Dictionary<string, object>();

        #endregion // Fields.

        #region Events

        /// <summary>
        ///     This event is raised a property is modified.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion // Events.

        #region Properties

        /// <summary>
        ///     The property descriptors associated with this object.
        /// </summary>
        public PropertyDescriptorCollection PropertyDescriptors
        {
            get;
        }

        /// <summary>
        ///     Gets the dynamic type.
        /// </summary>
        public string DynamicType
        {
            get;
        }

        #endregion // Properties.

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DynamicTypedObject" /> class.
        /// </summary>
        protected DynamicTypedObject()
        {
            TypeDescriptor.AddProvider(new DynamicTypedObjectDescriptionProvider(), this);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="DynamicTypedObject" /> class.
        /// </summary>
        /// <param name="pPropertyType">Type of the property.</param>
        /// <param name="pDynamicType">The dynamic type.</param>
        public DynamicTypedObject(Type pPropertyType, string pDynamicType) : this()
        {
            this.DynamicType = pDynamicType;
            this.PropertyDescriptors = new PropertyDescriptorCollection(null);
            this.PropertyDescriptors.Add(new DynamicTypedObjectPropertyDescriptor("PropertyValue", pPropertyType));
            this.mMembers["PropertyValue"] = pPropertyType.Default();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="DynamicTypedObject" /> class.
        /// </summary>
        /// <param name="pPropertyValue">The property value.</param>
        /// <param name="pDynamicType">The dynamic type.</param>
        public DynamicTypedObject(object pPropertyValue, string pDynamicType) : this(pPropertyValue.GetType(), pDynamicType)
        {
            this.mMembers["PropertyValue"] = pPropertyValue;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="DynamicTypedObject" /> class.
        /// </summary>
        /// <param name="pPropertyDescriptors">The property descriptors.</param>
        /// <param name="pDynamicType">The dynamic type.</param>
        public DynamicTypedObject(PropertyDescriptorCollection pPropertyDescriptors, string pDynamicType) : this()
        {
            this.DynamicType = pDynamicType;
            this.PropertyDescriptors = new PropertyDescriptorCollection(null);
            foreach (PropertyDescriptor lInitialDescriptor in pPropertyDescriptors)
            {
                var lDynamicDescriptor = new DynamicTypedObjectPropertyDescriptor(lInitialDescriptor);
                this.PropertyDescriptors.Add(lDynamicDescriptor);
            }

            if (this.PropertyDescriptors.Count != 0)
            {
                foreach (PropertyDescriptor lProperty in this.PropertyDescriptors)
                {
                    this.mMembers[lProperty.Name] = lProperty.PropertyType.Default();
                }
            }
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="DynamicTypedObject" /> class.
        /// </summary>
        /// <param name="pPropertyDescriptors">The property descriptors.</param>
        /// <param name="pComponent">The component.</param>
        public DynamicTypedObject(PropertyDescriptorCollection pPropertyDescriptors, object pComponent) : this(pPropertyDescriptors, pComponent.GetType().FullName)
        {
            foreach (PropertyDescriptor lProperty in pPropertyDescriptors)
            {
                this.mMembers[lProperty.Name] = lProperty.GetValue(pComponent);
            }
        }

        #endregion // Constructors.

        #region Methods

        /// <summary>
        ///     This method is used to retrieve a member value.
        /// </summary>
        /// <param name="pBinder">The member binder.</param>
        /// <param name="pResult">The member result.</param>
        /// <returns>True if the value has been retrieved, false otherwise.</returns>
        public override bool TryGetMember(GetMemberBinder pBinder, out object pResult)
        {
            return this.TryGetMember(pBinder.Name, out pResult);
        }

        /// <summary>
        ///     This method is used to retrieve a member value.
        /// </summary>
        /// <param name="pMemberName">The member binder.</param>
        /// <param name="pResult">The member result.</param>
        /// <returns>True if the value has been retrieved, false otherwise.</returns>
        public virtual bool TryGetMember(string pMemberName, out object pResult)
        {
            return this.mMembers.TryGetValue(pMemberName, out pResult);
        }

        /// <summary>
        ///     This method is used to modify a member value.
        /// </summary>
        /// <param name="pBinder">The member binder.</param>
        /// <param name="pValue">The value to set.</param>
        /// <returns>True if the value has been retrieved, false otherwise.</returns>
        public override bool TrySetMember(SetMemberBinder pBinder, object pValue)
        {
            return this.TrySetMember(pBinder.Name, pValue);
        }

        /// <summary>
        ///     This method is used to modify a member value.
        /// </summary>
        /// <param name="pMemberName">The member name.</param>
        /// <param name="pValue">The value to set.</param>
        /// <returns>True if the value has been retrieved, false otherwise.</returns>
        public virtual bool TrySetMember(string pMemberName, object pValue)
        {
            if (this.mMembers.ContainsKey(pMemberName))
            {
                if (this.mMembers[pMemberName] != pValue)
                {
                    this.mMembers[pMemberName] = pValue;
                    this.NotifyPropertyChanged(pMemberName);
                }

                return true;
            }

            return false;
        }

        /// <summary>
        ///     Retrieves the definded member names.
        /// </summary>
        /// <returns>An enumeration of all members</returns>
        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return this.mMembers.Keys;
        }

        /// <summary>
        ///     This method notifies a property modification.
        /// </summary>
        /// <param name="pPropertyName">The property name.</param>
        protected void NotifyPropertyChanged(string pPropertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(pPropertyName));
            }
        }

        #endregion // Methods.
    }
}
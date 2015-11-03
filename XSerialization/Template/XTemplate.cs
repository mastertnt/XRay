using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.ComponentModel;

namespace XSerialization.Template
{
    /// <summary>
    /// This class represents an object template containing the template informations into an XElement.
    /// </summary>
    /// <!-- DPE -->
    public class XTemplate<TObject> : IXTemplate
    {
        #region Fields

        /// <summary>
        /// Stores the node containing the template information.
        /// </summary>
        private XElement mTemplateNode;
        
        #endregion // Fields.

        #region Properties

        /// <summary>
        /// Gets the base templated type.
        /// </summary>
        [Browsable(false)]
        public Type BaseTemplatedType
        {
            get
            {
                return typeof(TObject);
            }
        }

        /// <summary>
        /// Gets the real type of the templated object.
        /// </summary>
        /// <remarks>
        /// This property is initialized when the InitializeFrom method is called.
        /// </remarks>
        [Browsable(false)]
        public Type TemplatedType
        {
            get;
            protected set;
        }

        #endregion // Properties.

        #region Methods

        /// <summary>
        /// Initialize the template from the informations contained in the given object.
        /// </summary>
        /// <param name="pObject">The object initializing the template.</param>
        /// <returns>True if the type of the given object is the same or inherits the templated type.</returns>
        bool IXTemplate.InitializeFrom(object pObject)
        {
            try
            {
                if (pObject != null && this.BaseTemplatedType.IsAssignableFrom(pObject.GetType()))
                {
                    return this.InitializeFrom((TObject)pObject);
                }

                return false;
            }
            catch (Exception /*lEx*/)
            {
                return false;
            }
        }

        /// <summary>
        /// Initialize the template from the informations contained in the given object.
        /// </summary>
        /// <param name="pObject">The object initializing the template.</param>
        /// <param name="pSerializer">The serializer</param>
        /// <returns>True if the type of the given object is the same or inherits the templated type.</returns>
        bool IXTemplate.InitializeFrom(object pObject, XSerializer pSerializer)
        {
            try
            {
                if 
                    ( pObject != null && 
                      this.BaseTemplatedType.IsAssignableFrom( pObject.GetType() ) )
                {
                    return this.InitializeFrom( (TObject)pObject, pSerializer );
                }

                return false;
            }
            catch 
                ( Exception /*lEx*/ )
            {
                return false;
            }
        }

        /// <summary>
        /// Create an instance of the template type.
        /// </summary>
        /// <returns>The instance is the creation succed, null otherwise.</returns>
        object IXTemplate.Create()
        {
            return this.Create();
        }

        /// <summary>
        /// Create an instance of the template type.
        /// </summary>
        /// <param name="pSerializer">The serializer</param>
        /// <returns>The instance is the creation succed, null otherwise.</returns>
        object IXTemplate.Create(XSerializer pSerializer)
        {
            return this.Create( pSerializer );
        }

        /// <summary>
        /// Initialize the template from the informations contained in the given object.
        /// </summary>
        /// <param name="pObject">The object initializing the template.</param>
        /// <returns>True if the type of the given object is the same or inherits the templated type.</returns>
        public virtual bool InitializeFrom(TObject pObject)
        {
            if (pObject != null)
            {
                this.TemplatedType = pObject.GetType();
            }
            XSerializer lSerializer = new XSerializer();
            this.mTemplateNode = lSerializer.Serialize( pObject );
            return (this.mTemplateNode!= null);
        }

        /// <summary>
        /// Initialize the template from the informations contained in the given object.
        /// </summary>
        /// <param name="pObject">The object initializing the template.</param>
        /// <param name="pSerializer">The serializer</param>
        /// <returns>True if the type of the given object is the same or inherits the templated type.</returns>
        public virtual bool InitializeFrom(TObject pObject, XSerializer pSerializer)
        {
            bool lIsSuppliedObjectValid = pObject != null;
            if 
                ( lIsSuppliedObjectValid )
            {
                this.TemplatedType = pObject.GetType();
            }

            bool lHasSerializerProvided = pSerializer != null;
            if 
                ( lHasSerializerProvided )
            {
                this.mTemplateNode = pSerializer.Serialize( pObject );
            }
            else
            {
                XSerializer lSerializer = new XSerializer();
                this.mTemplateNode = lSerializer.Serialize( pObject );
            }

            bool lIsTemplateNodeValid = this.mTemplateNode != null;

            return lIsTemplateNodeValid;
        }

        /// <summary>
        /// Create an instance of the template type.
        /// </summary>
        /// <returns>The instance is the creation succed, default value otherwise.</returns>
        public virtual TObject Create()
        {
            try
            {
                if (this.mTemplateNode == null)
                {
                    return default(TObject);
                }
                XSerializer lSerializer = new XSerializer();
                return (TObject)lSerializer.Deserialize(this.mTemplateNode);
            }
            catch (Exception /*lEx*/)
            {
                return default(TObject);
            }
        }

        /// <summary>
        /// Create an instance of the template type.
        /// </summary>
        /// <returns>The instance is the creation succed, default value otherwise.</returns>
        public virtual TObject Create(XSerializer pSerializer)
        {
            try
            {
                bool lIsTemplateNodeInvalid = this.mTemplateNode == null;
                if 
                    ( lIsTemplateNodeInvalid )
                {
                    return default( TObject );
                }

                bool lHasSerializerProvided = pSerializer != null;
                if
                    ( lHasSerializerProvided )
                {
                    return (TObject)pSerializer.Deserialize( this.mTemplateNode );
                }
                else
                {
                    XSerializer lSerializer = new XSerializer();
                    return (TObject)lSerializer.Deserialize( this.mTemplateNode );
                }
            }
            catch 
                ( Exception /*lEx*/ )
            {
                return default( TObject );
            }
        }

        #endregion Methods
    }
}

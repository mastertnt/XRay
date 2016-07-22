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
    public class XTemplate<TObject> : IXTemplate where TObject : class, INotifyPropertyChanged
    {
        #region Fields

        /// <summary>
        /// Stores the flag indicating if the behaviour is disposed.
        /// </summary>
        private bool mDisposed;

        /// <summary>
        /// Stores the node containing the template information.
        /// </summary>
        private XElement mTemplateNode;

        /// <summary>
        /// Stores the object defining the template and used as editor.
        /// </summary>
        private TObject mEditor;
        
        #endregion // Fields.

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="XTemplate{TObject}"/> class.
        /// </summary>
        public XTemplate()
        {
            this.mDisposed = false;
            this.mTemplateNode = null;
            this.Editor = null;
            this.TemplatedType = null;
        }

        #endregion // Constructors.

        #region Finalizers

        /// <summary>
        /// Finalizes an instance of the <see cref="XTemplate{TObject}"/> class.
        /// </summary>
        ~XTemplate()
        {
            this.Dispose(false);
        }

        #endregion // Finalizers.

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

        /// <summary>
        /// Gets or sets the editor.
        /// </summary>
        protected virtual TObject Editor
        {
            get
            {
                return this.mEditor;
            }

            set
            {
                if (this.mEditor != null)
                {
                    this.mEditor.PropertyChanged -= this.OnEditorPropertyChanged;
                }

                this.mEditor = value;

                if (this.mEditor != null)
                {
                    this.mEditor.PropertyChanged += this.OnEditorPropertyChanged;
                }
            }
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
                if (pObject != null && this.BaseTemplatedType.IsAssignableFrom(pObject.GetType()))
                {
                    return this.InitializeFrom((TObject)pObject, pSerializer);
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
        public bool InitializeFrom(TObject pObject)
        {
            return this.InitializeFrom(pObject, null);
        }

        /// <summary>
        /// Initialize the template from the informations contained in the given object.
        /// </summary>
        /// <param name="pObject">The object initializing the template.</param>
        /// <param name="pSerializer">The serializer</param>
        /// <returns>True if the type of the given object is the same or inherits the templated type.</returns>
        public virtual bool InitializeFrom(TObject pObject, XSerializer pSerializer)
        {
            if (pObject != null)
            {
                this.Editor = pObject;
                this.TemplatedType = pObject.GetType();

                if (pSerializer != null)
                {
                    this.mTemplateNode = pSerializer.Serialize(pObject);
                }
                else
                {
                    XSerializer lSerializer = new XSerializer();
                    this.mTemplateNode = lSerializer.Serialize(pObject);
                }

                return (this.mTemplateNode != null);
            }

            return false;
        }

        /// <summary>
        /// Create an instance of the template type.
        /// </summary>
        /// <returns>The instance is the creation succed, default value otherwise.</returns>
        public TObject Create()
        {
            return this.Create(null);
        }

        /// <summary>
        /// Create an instance of the template type.
        /// </summary>
        /// <returns>The instance is the creation succed, default value otherwise.</returns>
        public virtual TObject Create(XSerializer pSerializer)
        {
            try
            {
                if (this.mTemplateNode == null)
                {
                    return default(TObject);
                }

                if (pSerializer != null)
                {
                    return (TObject)pSerializer.Deserialize(this.mTemplateNode);
                }
                else
                {
                    XSerializer lSerializer = new XSerializer();
                    return (TObject)lSerializer.Deserialize(this.mTemplateNode);
                }
            }
            catch (Exception /*lEx*/)
            {
                return default(TObject);
            }
        }

        /// <summary>
        /// Clone this template.
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            // Cloning the node defining the template.
            XElement lTemplateNodeCopy = null;
            if (this.mTemplateNode != null)
            {
                lTemplateNodeCopy = new XElement(this.mTemplateNode);
            }

            // Cloning the template.
            XTemplate<TObject> lTemplateCopy = new XTemplate<TObject>();
            lTemplateCopy.mTemplateNode = lTemplateNodeCopy;
            return lTemplateCopy;
        }

        /// <summary>
        /// Returns the template editor.
        /// </summary>
        /// <returns>The template editor.</returns>
        object IXTemplate.GetEditor()
        {
            return this.GetEditor();
        }

        /// <summary>
        /// Returns the template editor.
        /// </summary>
        /// <returns>The template editor.</returns>
        public virtual TObject GetEditor()
        {
            return this.mEditor;
        }

        /// <summary>
        /// Delegate called when an editor property is modified.
        /// </summary>
        /// <param name="pSender">The modified editor.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private void OnEditorPropertyChanged(object pSender, PropertyChangedEventArgs pEventArgs)
        {
            this.InitializeFrom(this.Editor);
        }

        /// <summary>
        /// Dispose this behavior.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);

#pragma warning disable 1587
            /// This object will be cleaned up by the Dispose method.
            /// Therefore, GC.SupressFinalize should be called to take this object off the finalization queue 
            /// and prevent finalization code for this object from executing a second time.
#pragma warning restore 1587

            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose this behavior.
        /// </summary>
        /// <param name="pDisposing">Flag indicating if the owned objects have to be cleaned as well.</param>
        protected virtual void Dispose(Boolean pDisposing)
        {
            if (this.mDisposed == false)
            {
                // Free other state (managed objects) section.
                if (pDisposing)
                {
                    if (this.mEditor != null)
                    {
                        this.mEditor.PropertyChanged -= this.OnEditorPropertyChanged;
                        this.mEditor = null;
                    }
                }

                // Free your own state (unmanaged objects) section.

                this.mDisposed = true;
            }
        }

        #endregion // Methods.
    }
}

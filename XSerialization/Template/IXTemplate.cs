using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XSerialization.Template
{
    /// <summary>
    /// This interface represents an object template containing the template informations into an XElement.
    /// </summary>
    /// <!-- DPE -->
    public interface IXTemplate
    {
        #region Properties

        /// <summary>
        /// Gets the templated type.
        /// </summary>
        Type BaseTemplatedType
        {
            get;
        }

        /// <summary>
        /// Gets the real type of the templated object.
        /// </summary>
        /// <remarks>
        /// This property is initialized when the InitializeFrom method is called.
        /// </remarks>
        Type TemplatedType
        {
            get;
        }

        #endregion // Properties.

        #region Methods

        /// <summary>
        /// Initialize the template from the informations contained in the given object.
        /// </summary>
        /// <param name="pObject">The object initializing the template.</param>
        /// <returns>True if the type of the given object is the same or inherits the templated type.</returns>
        bool InitializeFrom(object pObject);

        /// <summary>
        /// Initialize the template from the informations contained in the given object.
        /// </summary>
        /// <param name="pObject">The object initializing the template.</param>
        /// <param name="pSerializer">The serializer</param>
        /// <returns>True if the type of the given object is the same or inherits the templated type.</returns>
        bool InitializeFrom(object pObject, XSerializer pSerializer);

        /// <summary>
        /// Create an instance of the template type.
        /// </summary>
        /// <returns>The instance if the creation succeed, null otherwise.</returns>
        object Create();

        /// <summary>
        /// Create an instance of the template type.
        /// </summary>
        /// <param name="pSerializer">The serializer</param>
        /// <returns>The instance if the creation succeed, null otherwise.</returns>
        object Create(XSerializer pSerializer);

        #endregion Methods
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Markup;

namespace XGraph.Extensions.Markup
{
    /// <summary>
    /// Markup extension for the <see cref="GridLength"/> class.
    /// </summary>
    [MarkupExtensionReturnType(typeof(GridLength))]
    public class GridLengthExtension : MarkupExtension
    {
        #region Properties

        /// <summary>
        /// Gets or sets the lenght value.
        /// </summary>
        [ConstructorArgument("pValue")]
        public double Value
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the leght unit type.
        /// </summary>
        [ConstructorArgument("pType")]
        public GridUnitType Type
        {
            get;
            set;
        }

        #endregion // Properties.

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GridLengthExtension"/> class.
        /// </summary>
        public GridLengthExtension() 
            : this(0.0, GridUnitType.Pixel)
        { 
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GridLengthExtension"/> class.
        /// </summary>
        /// <param name="pValue">The length value in pixel.</param>
        public GridLengthExtension(double pValue)
            : this(pValue, GridUnitType.Pixel)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GridLengthExtension"/> class.
        /// </summary>
        /// <param name="pValue">The length value in teh specified unit.</param>
        /// <param name="pType">THe length unit.</param>
        public GridLengthExtension(double pValue, GridUnitType pType)
            : base()
        {
            this.Value = pValue;
            this.Type = pType;
        }

        #endregion // Constructors.

        #region Methods

        /// <summary>
        /// Provides the value created from the extension properties.
        /// </summary>
        /// <param name="pServiceProvider">The service provider.</param>
        /// <returns>The created grid lenght.</returns>
        public override object ProvideValue(IServiceProvider pServiceProvider)
        {
            return new GridLength(this.Value, this.Type);
        }

        #endregion // Methods.
    }
}

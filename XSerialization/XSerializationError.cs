using System;

namespace XSerialization
{
    /// <summary>
    /// Enumerated type for all error.
    /// </summary>
    public enum XErrorType
    {
        /// <summary>
        /// A parsing errror occured.
        /// </summary>
        Parsing,
        /// <summary>
        /// A number is out of range.
        /// </summary>
        NumberOverflow,     
        /// <summary>
        /// Multiple contracts found
        /// </summary>
        MultipleContract,   
        /// <summary>
        ///  Invalid XML
        /// </summary>
        InvalidXml,
        /// <summary>
        ///  Invalid XML
        /// </summary>
        User, 
        /// <summary>
        /// Reflection error
        /// </summary>
        UnkwnonType         
    }

    /// <summary>
    /// This class represents an error.
    /// </summary>
    public class XSerializationError
    {
        #region Properties

        /// <summary>
        /// Gets the type of the error.
        /// </summary>
        public XErrorType Type
        {
            get; 
            private set;
        }

        /// <summary>
        /// Gets the line of the error.
        /// </summary>
        public int Line
        {
            get; 
            private set; 
        }

        /// <summary>
        /// Gets the column of the error.
        /// </summary>
        public int Column
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the uri of the file.
        /// </summary>
        public Uri Uri
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the uri of the file.
        /// </summary>
        public string AdditionalInformation
        {
            get;
            private set;
        }

        #endregion // Properties

        #region Constructors
        
        /// <summary>
        /// Initializes a new instance of the <see cref="XSerializationError"/> class.
        /// </summary>
        /// <param name="pType">Type of error.</param>
        /// <param name="pLine">The line.</param>
        /// <param name="pColumn">The column.</param>
        /// <param name="pUri">The uri of the xml.</param>
        /// <param name="pAdditionalInformation">Some additional information.</param>
        public XSerializationError(XErrorType pType, int pLine, int pColumn, Uri pUri, string pAdditionalInformation)
        {
            this.Type = pType;
            this.Line = pLine;
            this.Column = pColumn;
            this.Uri = pUri;
            this.AdditionalInformation = pAdditionalInformation;
        }

        #endregion // Constructors.
    }
}

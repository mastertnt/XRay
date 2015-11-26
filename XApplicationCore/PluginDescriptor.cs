using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace XApplicationCore
{
    /// <summary>
    /// This class defines a plugin descriptor.
    /// A plugin descriptor is only used to describe a plugin type.
    /// </summary>
    public class PluginDescriptor
    {
        #region Properties

        /// <summary>
        /// Gets the plugin type (The class which implements the plugin).
        /// </summary>
        /// <example>ClassBrowserPlugin</example>
        public Type PluginType
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the plugin version.
        /// The version is extracted from the file "AssemblyInfo.cs" in the attribute AssemblyVersion
        /// </summary>
        /// <example>[assembly: AssemblyVersion("1.0.0.0")]</example>
        public Version Version
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the flag to know if the plugin has been correctly loaded.
        /// </summary>
        public Boolean HasBeenLoaded
        {
            get;
            internal set;
        }

        /// <summary>
        /// Exception message occured during attached or construction.
        /// </summary>
        public string LoadError
        {
            get; set;  
        }

        /// <summary>
        /// Gets the missing dependencies.
        /// </summary>
        public List<string> MissingDependencies
        {
            get; 
            internal set;
        }

        /// <summary>
        /// Gets the flag to know if the plugin has missing dependencies.
        /// </summary>
        public Boolean HasMissingDependencies
        {
            get
            {
                if (this.MissingDependencies == null)
                {
                    return false;
                }
                return this.MissingDependencies.Any();
            }
        }

        /// <summary>
        /// Returns the list of required services.
        /// </summary>
        /// <remarks>A required service is set with RequiresServiceAttribute</remarks>
        /// <see cref="RequireServiceAttribute"/>
        /// <returns>The list of required services with the version.</returns>
        public ICollection<Type> RequiredServices { get; internal set; }

        /// <summary>
        /// Returns the list of optional services.
        /// </summary>
        /// <remarks>A required service is set with MayRequireServiceAttribute</remarks>
        /// <see cref="OptionalServiceAttribute"/>
        /// <returns>The list of required services with the plugin version.</returns>
        public ICollection<Type> OptionalServices { get; internal set; }

        /// <summary>
        /// Returns the list of optional services.
        /// </summary>
        /// <remarks>A required service is set with MayRequireServiceAttribute</remarks>
        /// <see cref="ProvideServiceAttribute"/>
        /// <returns>The list of required services with the plugin version.</returns>
        public ICollection<Type> ProvidedServices { get; internal set; }

        /// <summary>
        /// Gets the path of the plugin.
        /// </summary>
        public FileInfo FullPathInfo
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets information about the author.
        /// </summary>
        /// <example>N. Baudrey</example>
        public String Author
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets information about the copyright.
        /// </summary>
        /// <example>2001-2011 DGA</example>
        public String Copyright
        {
            get;
            internal set;
        }

        #endregion // Properties.

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginDescriptor"/> class.
        /// </summary>
        /// <param name="pPluginType">The concrete class of the plugin.</param>
        public PluginDescriptor(Type pPluginType)
        {
            this.PluginType = pPluginType;

            //// Get the author.
            //if
            //    (ReflectionHelpers.GetFirstAttributeOfType<PluginAuthorAttribute>(pPluginType) != null)
            //{
            //    this.Author = ReflectionHelpers.GetFirstAttributeOfType<PluginAuthorAttribute>(pPluginType).Value;
            //}

            //// Get the copyright.
            //if
            //    (ReflectionHelpers.GetFirstAttributeOfType<PluginCopyrightAttribute>(pPluginType) != null)
            //{
            //    this.Copyright = ReflectionHelpers.GetFirstAttributeOfType<PluginCopyrightAttribute>(pPluginType).Value;
            //}

            //List<Object> lCustomAttributes = new List<Object>();
            //Type lType = pPluginType;
            //while
            //    (lType != null)
            //{
            //    lCustomAttributes.AddRange(lType.GetCustomAttributes(false));
            //    lType = lType.BaseType;
            //}

            //// Get the provided services.
            //this.ProvidedServices = lCustomAttributes.OfType<ProvidesServiceAttribute>().Select(pProvidesServiceAttribute => pProvidesServiceAttribute.ServiceInterface).ToList();

            //// Get the required services.
            //this.RequiredServices = lCustomAttributes.OfType<RequiresServiceAttribute>().Select(pRequiresServiceAttribute => pRequiresServiceAttribute.ServiceInterface).ToList();

            ////// Get the optional services.
            //this.OptionalServices = lCustomAttributes.OfType<MayRequireServiceAttribute>().Select(pMayRequireServiceAttribute => pMayRequireServiceAttribute.ServiceInterface).ToList();
            //this.HasBeenLoaded = false;
            //this.MissingDependencies = new List<string>();
            //this.SupportedExecutionModes = new List<string>();
        }

        #endregion // Constructors.

        #region Methods

        /// <summary>
        /// Overrides ToString method.
        /// </summary>
        /// <returns>The fullname of the plugin type.</returns>
        public override string ToString()
        {
// ReSharper disable AssignNullToNotNullAttribute
            return this.PluginType.FullName;
// ReSharper restore AssignNullToNotNullAttribute
        }

        #endregion // Methods.
    }
}

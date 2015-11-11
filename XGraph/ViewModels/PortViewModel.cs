using System.Windows;
using PropertyChanged;

namespace XGraph.ViewModels
{
    /// <summary>
    /// This enumeration give the type of the direction.
    /// </summary>
    public enum PortDirection
    {
        Input,
        Output,
    }

    /// <summary>
    /// This class represents a port view model.
    /// A port can be connected to another port.
    /// </summary>
    /// <!-- NBY -->
    [ImplementPropertyChanged]
    public class PortViewModel 
    {
        /// <summary>
        /// Gets or sets the type of the port.
        /// </summary>
        /// <value>
        /// The type of the port.
        /// </value>
        public string PortType
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the direction.
        /// </summary>
        /// <value>
        /// The direction.
        /// </value>
        public PortDirection Direction
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is selected.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is selected; otherwise, <c>false</c>.
        /// </value>
        public virtual bool IsSelected
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is active; otherwise, <c>false</c>.
        /// </value>
        public virtual bool IsActive
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the display string.
        /// </summary>
        /// <value>
        /// The display string.
        /// </value>
        public virtual string DisplayString
        {
            get;
            set;
        }

        /// <summary>
        /// Determines whether this instance [can be connected to] the specified p port view model.
        /// </summary>
        /// <param name="pPortViewModel">The p port view model.</param>
        /// <returns></returns>
        public bool CanBeConnectedTo(PortViewModel pPortViewModel)
        {
            return true;
        }

        /// <summary>
        /// Gets the data template.
        /// </summary>
        /// <value>
        /// The data template.
        /// </value>
        public DataTemplate DataTemplate
        {
            get
            {
                DataTemplates lTemplates = new DataTemplates();
                return lTemplates["PortViewDataTemplate"] as DataTemplate;
            }
        }
    }
}

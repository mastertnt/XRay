using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using PropertyChanged;

namespace XGraph.ViewModels
{

    /// <summary>
    /// This class represents the view model of a graph node.
    /// A node is composed by :
    ///     - Ports
    ///     - Connections between ports.
    /// </summary>
    /// <!-- NBY -->
    [ImplementPropertyChanged]
    public class NodeViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NodeViewModel"/> class.
        /// </summary>
        public NodeViewModel()
        {
            this.Ports = new ObservableCollection<PortViewModel>();
        }

        /// <summary>
        /// Gets or sets the x.
        /// </summary>
        /// <value>
        /// The x.
        /// </value>
        public double X
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the y position of the node.
        /// </summary>
        /// <value>
        /// The y.
        /// </value>
        public double Y
        {
            get;
            set;
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
        /// Gets or sets the ports.
        /// </summary>
        /// <value>
        /// The ports.
        /// </value>
        public ObservableCollection<PortViewModel> Ports { get; set; }

        /// <summary>
        /// Gets or sets the connections.
        /// </summary>
        /// <value>
        /// The connections.
        /// </value>
        public ObservableCollection<ConnectionViewModel> Connections { get; set; }

        /// <summary>
        /// Gets or sets the display string.
        /// </summary>
        /// <value>
        /// The display string.
        /// </value>
        public virtual string DisplayString { get; set; }

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
                DataTemplates lTempaltes = new DataTemplates();
                return lTempaltes["NodeViewDataTemplate"] as DataTemplate;
            }
        }
    }
}

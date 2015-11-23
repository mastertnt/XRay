using System.Collections.ObjectModel;
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
    public class NodeViewModel : IGraphItemViewModel, IPositionable
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeViewModel"/> class.
        /// </summary>
        public NodeViewModel()
        {
            this.Ports = new ObservableCollection<PortViewModel>();
        }

        #endregion // Constructors.

        #region Properties

        /// <summary>
        /// Gets or sets the x.
        /// </summary>
        public double X
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the y position of the node.
        /// </summary>
        public double Y
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is selected.
        /// </summary>
        public virtual bool IsSelected
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is active.
        /// </summary>
        public virtual bool IsActive
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the ports.
        /// </summary>
        public ObservableCollection<PortViewModel> Ports 
        { 
            get; 
            set; 
        }

        /// <summary>
        /// Gets or sets the display string.
        /// </summary>
        public virtual string DisplayString 
        { 
            get; 
            set; 
        }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public virtual string Description
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the data template.
        /// </summary>
        public DataTemplate DataTemplate
        {
            get
            {
                return Themes.ExpressionDark.Instance["NodeViewDefaultDataTemplate"] as DataTemplate;
            }
        }

        /// <summary>
        /// Gets the style to apply to the container.
        /// </summary>
        public virtual Style ContainerStyle
        {
            get
            {
                return Themes.ExpressionDark.Instance["GraphItemNodeViewDefaultStyleKey"] as Style;
            }
        }

        #endregion // Properties.
    }
}

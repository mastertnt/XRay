using System.Collections.ObjectModel;

namespace XGraph.ViewModels
{
    /// <summary>
    /// This class represents a view model of the graph.
    /// </summary>
    public class GraphViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GraphViewModel"/> class.
        /// </summary>
        public GraphViewModel()
        {
            this.Nodes =  new ObservableCollection<NodeViewModel>();
        }

        /// <summary>
        /// Gets or sets the nodes.
        /// </summary>
        /// <value>
        /// The nodes.
        /// </value>
        public ObservableCollection<NodeViewModel> Nodes
        {
            get; set;
        }
    }
}

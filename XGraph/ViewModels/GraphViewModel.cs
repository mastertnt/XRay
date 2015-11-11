using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using PropertyChanged;
using XGraph.Controls;

namespace XGraph.ViewModels
{
    /// <summary>
    /// This class represents a view model of the graph.
    /// </summary>
    [ImplementPropertyChanged]
    public class GraphViewModel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphViewModel"/> class.
        /// </summary>
        public GraphViewModel()
        {
            this.Nodes =  new ObservableCollection<NodeViewModel>();
            this.Connections = new ObservableCollection<ConnectionViewModel>();
        }

        #endregion // Constructors.

        #region Properties

        /// <summary>
        /// Gets the items to display into the graph.
        /// </summary>
        public IEnumerable<IGraphItemViewModel> Items
        {
            get
            {
                foreach (var lNode in this.Nodes)
                {
                    yield return lNode;
                }
                foreach (var lConnection in this.Connections)
                {
                    yield return lConnection;
                }
            }
        }

        /// <summary>
        /// Gets or sets the nodes.
        /// </summary>
        /// <value>
        /// The nodes.
        /// </value>
        public ObservableCollection<NodeViewModel> Nodes
        {
            get; 
            set;
        }

        /// <summary>
        /// Gets or sets the connections.
        /// </summary>
        /// <value>
        /// The connections.
        /// </value>
        public ObservableCollection<ConnectionViewModel> Connections
        {
            get; 
            set;
        }

        #endregion // Properties.
    }
}

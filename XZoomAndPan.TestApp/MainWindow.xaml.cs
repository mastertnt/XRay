using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using XGraph.ViewModels;
using XZoomAndPan.TestApp.Graph;

namespace XZoomAndPan.TestApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();

            GraphViewModel lViewModel = this.CreateTypeGraph();
            this.GraphView.DataContext = lViewModel;
            this.Overvierview.DataContext = lViewModel;
        }

        /// <summary>
        /// Creates the type graph.
        /// </summary>
        /// <returns></returns>
        public GraphViewModel CreateTypeGraph()
        {
            GraphViewModel lGraph = new GraphViewModel();
            NodeViewModel lNode0 = new TypeNodeViewModel(typeof(SampleClass));
            lGraph.AddNode(lNode0);

            NodeViewModel lNode1 = new TypeNodeViewModel(typeof(SampleClass));
            lGraph.AddNode(lNode1);

            NodeViewModel lNode2 = new NodeViewModel();
            lNode2.DisplayString = "Empty node";
            lGraph.AddNode(lNode2);

            int i = 0;
            foreach (NodeViewModel lNode in lGraph.Nodes)
            {
                lNode.X = 300 * i;
                lNode.Y = 100 * i;
                i++;
            }

            ConnectionViewModel lConnectionViewModel = new ConnectionViewModel();
            lConnectionViewModel.Output = lGraph.Nodes.ElementAt(0).Ports.FirstOrDefault(pPort => pPort.Direction == PortDirection.Output);
            lConnectionViewModel.Input = lGraph.Nodes.ElementAt(1).Ports.FirstOrDefault(pPort => pPort.Direction == PortDirection.Input);
            lGraph.AddConnection(lConnectionViewModel);

            return lGraph;
        }
    }
}

using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using PropertyChanged;
using XGraph.Extensions;
using XGraph.ViewModels;

namespace XGraph.Controls
{
    /// <summary>
    /// This class represents a connection.
    /// </summary>
    /// <!-- Nicolas Baudrey -->
    [ImplementPropertyChanged]
    public class Connection : ContentControl
    {
        #region Dependencies

        /// <summary>
        /// Identifies the IsSelected dependency property.
        /// </summary>
        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register("IsSelected", typeof(bool), typeof(Connection), new FrameworkPropertyMetadata(false, OnIsSelectedChanged));

        /// <summary>
        /// Identifies the OutputConnector dependency property.
        /// </summary>
        public static readonly DependencyProperty OutputConnectorProperty = DependencyProperty.Register("OutputConnector", typeof(OutputConnector), typeof(Connection), new FrameworkPropertyMetadata(null));

        /// <summary>
        /// Identifies the InputConnector dependency property.
        /// </summary>
        public static readonly DependencyProperty InputConnectorProperty = DependencyProperty.Register("InputConnector", typeof(InputConnector), typeof(Connection), new FrameworkPropertyMetadata(null));

        #endregion // Dependencies.

        #region Constructors

        /// <summary>
        /// Static constructor.
        /// </summary>
        static Connection()
        {
            // set the key to reference the style for this control
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(Connection), new FrameworkPropertyMetadata(typeof(Connection)));
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Connection()
        {
        }

        #endregion // Constructors.

        #region Properties

        /// <summary>
        /// Gets or sets the selection state of the connection.
        /// </summary>
        public bool IsSelected
        {
            get
            {
                return (bool)this.GetValue(IsSelectedProperty);
            }
            set
            {
                this.SetValue(IsSelectedProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the output connector of the connection.
        /// </summary>
        public OutputConnector OutputConnector
        {
            get
            {
                return (OutputConnector)this.GetValue(OutputConnectorProperty);
            }
            set
            {
                this.SetValue(OutputConnectorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the input connector of the connection.
        /// </summary>
        public InputConnector InputConnector
        {
            get
            {
                return (InputConnector)this.GetValue(InputConnectorProperty);
            }
            set
            {
                this.SetValue(InputConnectorProperty, value);
            }
        }

        #endregion // Properties.

        #region Methods

        /// <summary>
        /// Method called when the control content changed.
        /// </summary>
        /// <param name="pOldContent">The previous content.</param>
        /// <param name="pNewContent">The new content.</param>
        protected override void OnContentChanged(object pOldContent, object pNewContent)
        {
            // Calling ancestor methods.
            base.OnContentChanged(pOldContent, pNewContent);

            // The content is the view model.
            ConnectionViewModel lViewModel = pNewContent as ConnectionViewModel;
            if (lViewModel == null)
            {
                // Unreferencing the connectors to avoid memory leaks.
                this.OutputConnector = null;
                this.InputConnector = null;
            }
            else
            {
                // Filling the output and input connectors.
                GraphView lParentCanvas = this.FindVisualParent<GraphView>();
                if (lViewModel != null && lParentCanvas != null)
                {
                    NodeView lOutputNode = lParentCanvas.GetContainerForViewModel<NodeViewModel, NodeView>(lViewModel.Output.ParentNode);
                    if (lOutputNode != null)
                    {
                        PortView lOutputPort = lOutputNode.GetContainerForPortViewModel(lViewModel.Output);
                        if (lOutputPort != null)
                        {
                            this.OutputConnector = lOutputPort.Connector as OutputConnector;
                        }
                    }

                    NodeView lInputNode = lParentCanvas.GetContainerForViewModel<NodeViewModel, NodeView>(lViewModel.Input.ParentNode);
                    if (lInputNode != null)
                    {
                        PortView lInputPort = lInputNode.GetContainerForPortViewModel(lViewModel.Input);
                        if (lInputPort != null)
                        {
                            this.InputConnector = lInputPort.Connector as InputConnector;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Delegate called when the selection state changed.
        /// </summary>
        /// <param name="pObject">The modified control.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private static void OnIsSelectedChanged(DependencyObject pObject, DependencyPropertyChangedEventArgs pEventArgs)
        {
            Connection lConnection = pObject as Connection;
            if (lConnection != null)
            {
                lConnection.UpdateVisualState();
            }
        }

        /// <summary>
        /// Updates the visual state of the node.
        /// </summary>
        private void UpdateVisualState()
        {
            if (this.IsSelected)
            {
                VisualStateManager.GoToState(this, "Selected", true);
            }
            else
            {
                VisualStateManager.GoToState(this, "Unselected", true);
            }
        }

        #endregion // Methods.
    }
}

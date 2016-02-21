using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using XGraph.ViewModels;

namespace XGraph.Controls
{
    /// <summary>
    /// Class defining a node in the graph view.
    /// </summary>
    [TemplatePart(Name = PART_PORT_CONTAINER, Type = typeof(PortContainer))]
    public class NodeView : ContentControl
    {
        #region Fields

        /// <summary>
        /// Name of the parts that have to be in the control template.
        /// </summary>
        private const string PART_PORT_CONTAINER = "PART_PortContainer";

        /// <summary>
        /// The inner port container
        /// </summary>
        private PortContainer mInnerPortContainer;

        #endregion // Fields.

        #region Dependencies

        /// <summary>
        /// Identifies the IsSelected dependency property.
        /// </summary>
        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register("IsSelected", typeof(bool), typeof(NodeView), new FrameworkPropertyMetadata(false, OnIsSelectedChanged));

        #endregion // Dependencies.

        #region Constructors

        /// <summary>
        /// Initializes the <see cref="NodeView"/> class.
        /// </summary>
        static NodeView()
        {
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(NodeView), new FrameworkPropertyMetadata(typeof(NodeView)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeView"/> class.
        /// </summary>
        public NodeView()
        {
            
        }

        #endregion // Constructors.

        #region Properties

        /// <summary>
        /// Gets or sets the selection state of the node.
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

        #endregion // Properties.

        #region Methods

        /// <summary>
        /// Method called when the control content changed.
        /// </summary>
        /// <param name="pOldContent">The previous content.</param>
        /// <param name="pNewContent">The new content.</param>
        protected override void OnContentChanged(object pOldContent, object pNewContent)
        {
            base.OnContentChanged(pOldContent, pNewContent);

            BindingOperations.ClearAllBindings(this);

            // The content is the view model.
            NodeViewModel lNewContent = pNewContent as NodeViewModel;
            if (lNewContent != null)
            {
                // Setting the content data template.
                this.ContentTemplate = lNewContent.DataTemplate;
            }
        }

        /// <summary>
        /// Method called when the control template is applied.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            // Getting the parts of the control.
            this.mInnerPortContainer = this.GetTemplateChild(PART_PORT_CONTAINER) as PortContainer;

            if (this.mInnerPortContainer == null)
            {
                throw new Exception("NodeView control template not correctly defined.");
            }

            // Binding the Ports property.
            Binding lNodesBinding = new Binding("Ports");
            lNodesBinding.Source = this.Content;
            lNodesBinding.Mode = BindingMode.OneWay;
            this.mInnerPortContainer.SetBinding(ItemsControl.ItemsSourceProperty, lNodesBinding);
        }
         
        /// <summary>
        /// Delegate called when the selection state changed.
        /// </summary>
        /// <param name="pObject">The modified control.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private static void OnIsSelectedChanged(DependencyObject pObject, DependencyPropertyChangedEventArgs pEventArgs)
        {
            NodeView lNodeView = pObject as NodeView;
            if (lNodeView != null)
            {
                lNodeView.UpdateVisualState();
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

        /// <summary>
        /// Returns the node view containing the given view model.
        /// </summary>
        /// <param name="pItem">The item contained by the view.</param>
        /// <returns>The found view if any, null otherwise.</returns>
        public PortView GetContainerForPortViewModel(PortViewModel pItem)
        {
            return this.mInnerPortContainer.ItemContainerGenerator.ContainerFromItem(pItem) as PortView;
        }

        #endregion // Methods.
    }
}

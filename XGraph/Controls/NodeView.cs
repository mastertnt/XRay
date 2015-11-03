using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using XGraph.ViewModels;

namespace XGraph.Controls
{
    [TemplatePart(Name = PART_PORT_CONTAINER, Type = typeof(PortContainer))]
    public class NodeView : ListBoxItem
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

        // Defining depenency properties so that they can be binding to the view model properties.
        public static readonly DependencyProperty PosXProperty = DependencyProperty.Register("PosX", typeof(double), typeof(NodeView), new FrameworkPropertyMetadata(0.0));

        /// <summary>
        /// The position y property
        /// </summary>
        public static readonly DependencyProperty PosYProperty = DependencyProperty.Register("PosY", typeof(double), typeof(NodeView), new FrameworkPropertyMetadata(0.0));

        #endregion // Dependencies.

        #region Constructors

        /// <summary>
        /// Initializes the <see cref="NodeView"/> class.
        /// </summary>
        static NodeView()
        {
            NodeView.DefaultStyleKeyProperty.OverrideMetadata(typeof(NodeView), new FrameworkPropertyMetadata(typeof(NodeView)));
        }

        #endregion // Constructors.

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
                // Stting the content data template.
                this.ContentTemplate = lNewContent.DataTemplate;

                // Binding the X position.
                Binding lXBinding = new Binding("X");
                lXBinding.Source = lNewContent;
                lXBinding.Mode = BindingMode.TwoWay;
                this.SetBinding(NodeView.PosXProperty, lXBinding);

                // Binding the Y position.
                Binding lYBinding = new Binding("Y");
                lYBinding.Source = lNewContent;
                lYBinding.Mode = BindingMode.TwoWay;
                this.SetBinding(NodeView.PosYProperty, lYBinding);

                // Binding the IsSelected property.
                Binding lIsSelectedBinding = new Binding("IsSelected");
                lIsSelectedBinding.Source = lNewContent;
                lIsSelectedBinding.Mode = BindingMode.TwoWay;
                this.SetBinding(NodeView.IsSelectedProperty, lIsSelectedBinding);
            }
        }

        /// <summary>
        /// Method called when the control template is applied.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            // Getting the part of the control.
            this.mInnerPortContainer = this.GetTemplateChild(PART_PORT_CONTAINER) as PortContainer;

            if (this.mInnerPortContainer == null)
            {
                throw new Exception("NodeView control template not correctly defined.");
            }

            // Binding the Ports property.
            Binding lNodesBinding = new Binding("Ports");
            lNodesBinding.Source = this.Content;
            lNodesBinding.Mode = BindingMode.OneWay;
            this.mInnerPortContainer.SetBinding(PortContainer.ItemsSourceProperty, lNodesBinding);
        }

        #endregion // Methods.

        #region Properties

        /// <summary>
        /// Gets or sets X pos.
        /// </summary>
        public double PosX
        {
            get
            {
                return (double)this.GetValue(PosXProperty);
            }
            set
            {
                this.SetValue(PosXProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets X pos.
        /// </summary>
        public double PosY
        {
            get
            {
                return (double)this.GetValue(PosYProperty);
            }
            set
            {
                this.SetValue(PosYProperty, value);
            }
        }

        #endregion // Properties.
    }
}

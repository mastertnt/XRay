﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using XGraph.ViewModels;

namespace XGraph.Controls
{
    /// <summary>
    /// This item stores all the others control for the graph view.
    /// </summary>
    [TemplatePart(Name = PART_TEMPLATE_CONTROL, Type = typeof(FrameworkElement))]
    public class GraphItem : ListBoxItem
    {
        #region Fields

        /// <summary>
        /// Name of the parts that have to be in the control template.
        /// </summary>
        private const string PART_TEMPLATE_CONTROL = "PART_TemplateControl";

        #endregion // Fields.

        #region Dependencies

        /// <summary>
        /// Identifies the XPos dependency property.
        /// </summary>
        public static readonly DependencyProperty PosXProperty = DependencyProperty.Register("PosX", typeof(double), typeof(GraphItem), new FrameworkPropertyMetadata(0.0));

        /// <summary>
        /// Identifies the YPos dependency property.
        /// </summary>
        public static readonly DependencyProperty PosYProperty = DependencyProperty.Register("PosY", typeof(double), typeof(GraphItem), new FrameworkPropertyMetadata(0.0));

        #endregion // Dependencies.

        #region Constructors

        /// <summary>
        /// Initializes the <see cref="NodeView"/> class.
        /// </summary>
        static GraphItem()
        {
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(GraphItem), new FrameworkPropertyMetadata(typeof(GraphItem)));
        }

        #endregion // Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the specific view representing the item.
        /// </summary>
        public FrameworkElement TemplateControl
        {
            get;
            protected set;
        }

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
            IGraphItemViewModel lNewContent = pNewContent as IGraphItemViewModel;
            if (lNewContent != null)
            {
                // Setting the style.
                this.Style = lNewContent.ContainerStyle;

                // Binding the IsSelected property.
                Binding lIsSelectedBinding = new Binding("IsSelected") {Source = lNewContent, Mode = BindingMode.TwoWay};
                this.SetBinding(GraphItem.IsSelectedProperty, lIsSelectedBinding);

                if (lNewContent is IPositionable)
                {
                    // Binding the X position.
                    Binding lXBinding = new Binding("X") { Source = lNewContent, Mode = BindingMode.TwoWay };
                    this.SetBinding(GraphItem.PosXProperty, lXBinding);

                    // Binding the Y position.
                    Binding lYBinding = new Binding("Y") { Source = lNewContent, Mode = BindingMode.TwoWay };
                    this.SetBinding(GraphItem.PosYProperty, lYBinding);
                }
            }
        }

        /// <summary>
        /// Method called when the control template is applied.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            // Getting the parts of the control.
            this.TemplateControl = this.GetTemplateChild(PART_TEMPLATE_CONTROL) as FrameworkElement;

            if (this.TemplateControl == null)
            {
                throw new Exception("GraphItem control template not correctly defined.");
            }
        }

        #endregion // Methods.
    }
}

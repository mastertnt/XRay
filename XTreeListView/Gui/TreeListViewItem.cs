using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using XTreeListView.Converters;
using XTreeListView.ViewModel;
using XTreeListView.Core.Extensions;

namespace XTreeListView.Gui
{
    /// <summary>
    /// This object represents a tree list view item.
    /// </summary>
    [TemplatePart(Name = PART_DecoratorsContainer, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = PART_Expander, Type = typeof(ToggleButton))]
    [TemplatePart(Name = PART_CheckBox, Type = typeof(CheckBox))]
    [TemplatePart(Name = PART_Icon, Type = typeof(Image))]
    public class TreeListViewItem : ListViewItem
    {
        #region Dependencies

        /// <summary>
        /// Identifies the IsExpanded dependency property.
        /// </summary>
        public static readonly DependencyProperty IsExpandedProperty = DependencyProperty.Register("IsExpanded", typeof(bool), typeof(TreeListViewItem), new FrameworkPropertyMetadata(false));

        /// <summary>
        /// Identifies the Group dependency property.
        /// </summary>
        public static readonly DependencyProperty GroupProperty = DependencyProperty.Register("Group", typeof(object), typeof(TreeListViewItem), new FrameworkPropertyMetadata(string.Empty));

        /// <summary>
        /// Identifies the GroupDataTemplate dependency property.
        /// </summary>
        public static readonly DependencyProperty GroupDataTemplateProperty = DependencyProperty.Register("GroupDataTemplate", typeof(System.Windows.DataTemplate), typeof(TreeListViewItem), new FrameworkPropertyMetadata(null));

        /// <summary>
        /// Identifies the IsGroup dependency property.
        /// </summary>
        public static readonly DependencyProperty IsGroupProperty = DependencyProperty.Register("IsGroup", typeof(bool), typeof(TreeListViewItem), new FrameworkPropertyMetadata(false));

        #endregion // Dependencies.

        #region Fields

        /// <summary>
        /// Name of the parts that have to be in the control template.
        /// </summary>
        private const string PART_DecoratorsContainer = "PART_DecoratorsContainer";
        private const string PART_Expander = "PART_Expander";
        private const string PART_CheckBox = "PART_CheckBox";
        private const string PART_Icon = "PART_Icon";

        /// <summary>
        /// Stores the control containing the items decorators.
        /// </summary>
        private FrameworkElement mDecoratorsContainer;

        /// <summary>
        /// Stores the object responsible for expending the item.
        /// </summary>
        private ToggleButton mExpander;

        /// <summary>
        /// Stores the check box of the item.
        /// </summary>
        private CheckBox mCheckBox;

        /// <summary>
        /// Stores the image responsible for displaying the icon of the item.
        /// </summary>
        private Image mImage;

        /// <summary>
        /// Stores the view model corresponding to this item.
        /// </summary>
        private IHierarchicalItemViewModel mViewModel;

        #endregion // Fields.

        #region Constructor

        /// <summary>
        /// Initializes the <see cref="TreeListViewItem"/> class.
        /// </summary>
        static TreeListViewItem()
        {
            TreeListViewItem.DefaultStyleKeyProperty.OverrideMetadata(typeof(TreeListViewItem), new FrameworkPropertyMetadata(typeof(TreeListViewItem)));
        }

        #endregion // Constructor.

        #region Properties

        /// <summary>
        /// Gets the parent list view.
        /// </summary>
        internal TreeListView ParentTreeListView
        {
            get
            {
                return this.FindVisualParent<TreeListView>();
            }
        }

        /// <summary>
        /// Gets or sets the view model attached to this item.
        /// </summary>
        public IHierarchicalItemViewModel ViewModel 
        {
            get
            {
                return this.mViewModel;
            }

            internal set
            {
                if
                    (this.mViewModel != value)
                {
                    // Clearing the bindings.
                    BindingOperations.ClearAllBindings(this);

                    this.mViewModel = value;

                    // Binding the IsSelected property.
                    Binding lIsSelectedBinding = new Binding("IsSelected");
                    lIsSelectedBinding.Source = this.mViewModel;
                    lIsSelectedBinding.Mode = BindingMode.OneWay;
                    this.SetBinding(TreeListViewItem.IsSelectedProperty, lIsSelectedBinding);

                    // Binding the Visibility property.
                    Binding lVisibilityBinding = new Binding("Visibility");
                    lVisibilityBinding.Source = this.mViewModel;
                    this.SetBinding(TreeListViewItem.VisibilityProperty, lVisibilityBinding);

                    // Binding the Tooltip property.
                    Binding lTooltipBinding = new Binding("ToolTip");
                    lTooltipBinding.Source = this.mViewModel;
                    lTooltipBinding.Converter = new ToolTipConverter();
                    this.SetBinding(TreeListViewItem.ToolTipProperty, lTooltipBinding);

                    // Binding the Background property.
                    Binding lBackgroundBinding = new Binding("Background");
                    lBackgroundBinding.Source = this.mViewModel;
                    this.SetBinding(TreeListViewItem.BackgroundProperty, lBackgroundBinding);

                    // Binding the FirstLevelItemsAsGroup property.
                    MultiBinding lIsGroupBinding = new MultiBinding();
                    Binding lViewModelBinding = new Binding() { BindsDirectlyToSource = true };
                    lViewModelBinding.Source = this.mViewModel;
                    lIsGroupBinding.Bindings.Add(lViewModelBinding);
                    Binding lFirstLevelItemAsGroupBinding = new Binding("FirstLevelItemsAsGroup");
                    lFirstLevelItemAsGroupBinding.Source = this.ParentTreeListView;
                    lIsGroupBinding.Bindings.Add(lFirstLevelItemAsGroupBinding);
                    lIsGroupBinding.Converter = new ItemToIsGroupConverter();
                    this.SetBinding(TreeListViewItem.IsGroupProperty, lIsGroupBinding);

                    // Binding the group item data template.
                    Binding lGroupItemDataTemplateBinding = new Binding("GroupItemDataTemplate");
                    lGroupItemDataTemplateBinding.Source = this.ParentTreeListView;
                    this.SetBinding(TreeListViewItem.GroupDataTemplateProperty, lGroupItemDataTemplateBinding);

                    // View model defines the group model if the item is displayed as a group.
                    this.Group = this.mViewModel;
                }
            }
        }

        /// <summary>
        /// Gets or sets the flag indicating if the item is expanded or not.
        /// </summary>
        public bool IsExpanded
        {
            get
            {
                return (bool)GetValue(IsExpandedProperty);
            }
            set
            {
                SetValue(IsExpandedProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the group object.
        /// </summary>
        public object Group
        {
            get
            {
                return GetValue(GroupProperty);
            }
            set
            {
                SetValue(GroupProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the group data template.
        /// </summary>
        public System.Windows.DataTemplate GroupDataTemplate
        {
            get
            {
                return (System.Windows.DataTemplate)GetValue(GroupDataTemplateProperty);
            }
            set
            {
                SetValue(GroupDataTemplateProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the flag indicating if the item is a group or not.
        /// </summary>
        public bool IsGroup
        {
            get
            {
                return (bool)GetValue(IsGroupProperty);
            }
            set
            {
                SetValue(IsGroupProperty, value);
            }
        }

        #endregion // Properties.

        #region Methods

        /// <summary>
        /// Called when the control template is applied.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            // Getting the parts from the control template.
            this.mDecoratorsContainer = this.GetTemplateChild(PART_DecoratorsContainer) as FrameworkElement;
            this.mExpander = this.GetTemplateChild(PART_Expander) as ToggleButton;
            this.mCheckBox = this.GetTemplateChild(PART_CheckBox) as CheckBox;
            this.mImage = this.GetTemplateChild(PART_Icon) as Image;
            
            if
                (   (this.mExpander == null)
                ||  (this.mDecoratorsContainer == null)
                ||  (this.mCheckBox == null)
                ||  (this.mImage == null)
                )
            {
                throw new Exception("TreeListViewItem control template not correctly defined.");
            }

            // Updating the decorator (icon + expander) clip region.
            this.mDecoratorsContainer.Clip = this.GetDecoratorsClipRegion();

            // Register on the column changed event to update the decorator clip region.
            GridView lView = this.ParentTreeListView.InnerListView.View as GridView;
            if
                (lView != null)
            {
                lView.Columns.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(this.OnGridViewColumnsCollectionChanged);
            }

#pragma warning disable 1587
            /// Binding the width to the view model. 
            /// Used to update the margin of the first column template when a GridView is used.
#pragma warning restore 1587
            this.mDecoratorsContainer.SizeChanged += new SizeChangedEventHandler(this.OnDecoratorsContainerSizeChanged);

            // Binding the visibility of the expander.
            Binding lExpanderVisibilityBinding = new Binding("HasChildren");
            lExpanderVisibilityBinding.Source = this.ViewModel;
            lExpanderVisibilityBinding.Converter = new CanExpandConverter();
            this.mExpander.SetBinding(ToggleButton.VisibilityProperty, lExpanderVisibilityBinding);

            // Binding the indentation of the expander.
            Binding lExpanderMarginBinding = new Binding("Parent");
            lExpanderMarginBinding.Source = this.ViewModel;
            lExpanderMarginBinding.Converter = new LevelToIndentConverter();
            this.mExpander.SetBinding(ToggleButton.MarginProperty, lExpanderMarginBinding);

            // Binding the expanded state.
            this.mExpander.Checked += this.OnExpanderChecked;
            this.mExpander.Unchecked += this.OnExpanderUnchecked;
            Binding lExpanderIsCheckedBinding = new Binding("IsExpanded");
            lExpanderIsCheckedBinding.Source = this.mViewModel;
            lExpanderIsCheckedBinding.Mode = BindingMode.OneWay;
            this.mExpander.SetBinding(ToggleButton.IsCheckedProperty, lExpanderIsCheckedBinding);

            // Binding the icon source.
            Binding lImageSourceBinding = new Binding("IconSource");
            lImageSourceBinding.Source = this.mViewModel;
            this.mImage.SetBinding(Image.SourceProperty, lImageSourceBinding);

            if (this.IsGroup == false)
            {
                // Binding the icon visibility.
                Binding lImageVisibilityBinding = new Binding("IconVisibility");
                lImageVisibilityBinding.Source = this.mViewModel;
                this.mImage.SetBinding(Image.VisibilityProperty, lImageVisibilityBinding);
            }

            // Binding the checked state.
            Binding lCheckBoxIsCheckedBinding = new Binding("IsChecked");
            lCheckBoxIsCheckedBinding.Source = this.mViewModel;
            lCheckBoxIsCheckedBinding.Mode = BindingMode.OneWay;
            this.mCheckBox.SetBinding(CheckBox.IsCheckedProperty, lCheckBoxIsCheckedBinding);
            this.mCheckBox.Checked += this.OnCheckBoxChecked;
            this.mCheckBox.Unchecked += this.OnCheckBoxUnchecked;

            // Binding the checkable state.
            Binding lCheckBoxVisibilityBinding = new Binding("IsCheckable");
            lCheckBoxVisibilityBinding.Source = this.mViewModel;
            lCheckBoxVisibilityBinding.Converter = new BooleanToVisibilityConverter();
            this.mCheckBox.SetBinding(CheckBox.VisibilityProperty, lCheckBoxVisibilityBinding);

            // Binding the checking state.
            Binding lCheckBoxIsEnabledBinding = new Binding("IsCheckingEnabled");
            lCheckBoxIsEnabledBinding.Source = this.mViewModel;
            this.mCheckBox.SetBinding(CheckBox.IsEnabledProperty, lCheckBoxIsEnabledBinding);
        }

        /// <summary>
        /// Delegate called when the columns collection of the grid view is modified.
        /// </summary>
        /// <param name="pSender">The modified grid view.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private void OnGridViewColumnsCollectionChanged(object pSender, NotifyCollectionChangedEventArgs pEventArgs)
        {
            switch
                (pEventArgs.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    throw new NotImplementedException();

                case NotifyCollectionChangedAction.Remove:
                    throw new NotImplementedException();

                case NotifyCollectionChangedAction.Replace:
                    throw new NotImplementedException();

                case NotifyCollectionChangedAction.Move:
                    {
                        // Updating the expander clip region.
                        this.mDecoratorsContainer.Clip = this.GetDecoratorsClipRegion();
                    }

                    break;

                case NotifyCollectionChangedAction.Reset:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Retrieves the region used to clip the decorator container.
        /// </summary>
        private Geometry GetDecoratorsClipRegion()
        {   
            RectangleGeometry lClip = null;
            TreeListView lParent = this.ParentTreeListView;
            if (lParent != null)
            {
                GridView lView = lParent.InnerListView.View as GridView;
                if
                    (   (lView != null)
                    &&  (lView.Columns.Count > 0)
                    )
                {
                    GridViewColumn lFirstColumn = lView.Columns[0];
                    lClip = new RectangleGeometry();

                    Binding lXBinding = new Binding("ActualWidth");
                    lXBinding.Source = lFirstColumn;
                    Binding lYBinding = new Binding("ActualHeight");
                    lYBinding.Source = lParent.InnerListView;
                    MultiBinding lRectBinding = new MultiBinding();
                    lRectBinding.Bindings.Add(lXBinding);
                    lRectBinding.Bindings.Add(lYBinding);
                    lRectBinding.Converter = new WidthHeightToRectConverter() { Margin = new Thickness(0, 0, 6, 0) };

                    BindingOperations.SetBinding(lClip, RectangleGeometry.RectProperty, lRectBinding);
                }
            }

            return lClip;
        }

        /// <summary>
        /// Delegate called when the size of the decorators container changed.
        /// </summary>
        /// <param name="pSender">The modified container.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private void OnDecoratorsContainerSizeChanged(object pSender, SizeChangedEventArgs pEventArgs)
        {
            this.mViewModel.DecoratorWidth = pEventArgs.NewSize.Width;
        }

        /// <summary>
        /// Delegate called when a key is pressed when the item get the focus.
        /// </summary>
        /// <param name="pEventArgs">The event arguments.</param>
        protected override void OnKeyUp(KeyEventArgs pEventArgs)
        {
            TreeListView lParent = this.ParentTreeListView;
            if (lParent != null)
            {
                lParent.InnerListView.OnItemKeyUp(this.mViewModel, pEventArgs);
            }
        }

        /// <summary>
        /// Delegate called when the mouse left button is down.
        /// </summary>
        /// <param name="pEventArgs">The event arguments.</param>
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs pEventArgs)
        {
            TreeListView lParent = this.ParentTreeListView;
            if (lParent != null)
            {
                lParent.InnerListView.OnItemMouseLeftButtonDown(this.mViewModel, pEventArgs);
            }

            pEventArgs.Handled = true;
        }

        /// <summary>
        /// Delegate called when the mouse right button is down.
        /// </summary>
        /// <param name="pEventArgs">The event arguments.</param>
        protected override void OnMouseRightButtonDown(MouseButtonEventArgs pEventArgs)
        {
            TreeListView lParent = this.ParentTreeListView;
            if (lParent != null)
            {
                lParent.InnerListView.OnItemMouseRightButtonDown(this.mViewModel, pEventArgs);
            }

            pEventArgs.Handled = true;
        }

        /// <summary>
        /// Delegate called when the mouse double clicked on this item.
        /// </summary>
        /// <param name="pEventArgs">The event arguments.</param>
        protected override void OnMouseDoubleClick(MouseButtonEventArgs pEventArgs)
        {
            base.OnMouseDoubleClick(pEventArgs);

            TreeListView lParent = this.ParentTreeListView;
            if (lParent != null)
            {
                lParent.InnerListView.OnItemMouseDoubleClicked(this.mViewModel, pEventArgs);
            }

            pEventArgs.Handled = true;
        }

        /// <summary>
        /// Delegate called the check box is checked.
        /// </summary>
        /// <param name="pSender">The check box sender.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private void OnCheckBoxChecked(object pSender, RoutedEventArgs pEventArgs)
        {
            TreeListView lParent = this.ParentTreeListView;
            if (lParent != null)
            {
                lParent.InnerListView.CheckModel.Check(this.mViewModel, false);
            }
        }

        /// <summary>
        /// Delegate called the check box is unchecked.
        /// </summary>
        /// <param name="pSender">The check box sender.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private void OnCheckBoxUnchecked(object pSender, RoutedEventArgs pEventArgs)
        {
            TreeListView lParent = this.ParentTreeListView;
            if (lParent != null)
            {
                lParent.InnerListView.CheckModel.Uncheck(this.mViewModel, false);
            }
        }

        /// <summary>
        /// Delegate called when the expander gets checked.
        /// </summary>
        /// <param name="pSender">The event sender.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private void OnExpanderChecked(object pSender, RoutedEventArgs pEventArgs)
        {
            TreeListView lParent = this.ParentTreeListView;
            if (lParent != null)
            {
                lParent.InnerListView.OnItemExpanderChecked(this.mViewModel);
            }
        }

        /// <summary>
        /// Delegate called when the expander gets unchecked.
        /// </summary>
        /// <param name="pSender">The event sender.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private void OnExpanderUnchecked(object pSender, RoutedEventArgs pEventArgs)
        {
            TreeListView lParent = this.ParentTreeListView;
            if (lParent != null)
            {
                lParent.InnerListView.OnItemExpanderUnchecked(this.mViewModel);
            }
        }

        /// <summary>
        /// Delegate called when the item gets selected.
        /// </summary>
        /// <param name="pEventArgs">The event arguments.</param>
        protected override void OnSelected(RoutedEventArgs pEventArgs)
        {
            base.OnSelected(pEventArgs);

            // When the item get selected, the keyboard and mouse focus is given to it.
            this.Focus();
        }

        #endregion // Methods.
    }
}
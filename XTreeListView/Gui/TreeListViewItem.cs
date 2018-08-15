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
using XTreeListView.Behaviors;

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
        /// Stores the parent grid view ensuring the item can be unregister from Columns.CollectionChanged when removed.
        /// </summary>
        private ExtendedGridView mParentGridView;

        #endregion // Fields.

        #region Constructors

        /// <summary>
        /// Initializes the <see cref="TreeListViewItem"/> class.
        /// </summary>
        static TreeListViewItem()
        {
            TreeListViewItem.DefaultStyleKeyProperty.OverrideMetadata(typeof(TreeListViewItem), new FrameworkPropertyMetadata(typeof(TreeListViewItem)));
            TreeListViewItem.MultiColumnDefaultStyleKey = new ComponentResourceKey(typeof(TreeListViewItem), "MultiColumnDefaultStyleKey");
        }
        
        #endregion // Constructors.

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
        internal IHierarchicalItemViewModel ViewModel
        {
            get
            {
                return this.Content as IHierarchicalItemViewModel;
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

        /// <summary>
        /// Gets the item style key when the tree is in multi column mode.
        /// </summary>
        public static ResourceKey MultiColumnDefaultStyleKey
        {
            get;
            private set;
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

            // Updating the bindings.
            this.UpdateBindings(null, this.ViewModel);
        }

        /// <summary>
        /// Method called when the control content changed.
        /// </summary>
        /// <param name="pOldContent">The previous content.</param>
        /// <param name="pNewContent">The new content.</param>
        protected override void OnContentChanged(object pOldContent, object pNewContent)
        {
            base.OnContentChanged(pOldContent, pNewContent);

            // Updating the bindings.
            this.UpdateBindings(pOldContent as IHierarchicalItemViewModel, pNewContent as IHierarchicalItemViewModel);
        }

        /// <summary>
        /// Registers to the grid columns event.
        /// </summary>
        private void RegisterToGridColumnsEvents()
        {
            if (this.ParentTreeListView == null || this.ParentTreeListView.InnerListView == null)
            {
                return;
            }

            // Registers on the column changed event to update the decorator clip region.
            this.mParentGridView = this.ParentTreeListView.InnerListView.View;
            if (this.mParentGridView != null)
            {
                // Assure you remove a previously registered delegate if passing twice with non null model.
                this.mParentGridView.Columns.CollectionChanged -= this.OnGridViewColumnsCollectionChanged;
                this.mParentGridView.Columns.CollectionChanged += this.OnGridViewColumnsCollectionChanged;
            }
        }

        /// <summary>
        /// Registers to the container events.
        /// </summary>
        private void RegisterToContainerEvents()
        {
            if (this.mDecoratorsContainer != null)
            {
                // Assure you remove a previously registered delegate if passing twice with non null model.
                this.mDecoratorsContainer.SizeChanged -= this.OnDecoratorsContainerSizeChanged;
                this.mDecoratorsContainer.SizeChanged += this.OnDecoratorsContainerSizeChanged;
            }
        }

        /// <summary>
        /// Registers on the expander events.
        /// </summary>
        private void RegisterToExpanderEvents()
        {
            if (this.mExpander != null)
            {
                this.mExpander.Checked -= this.OnExpanderChecked;
                this.mExpander.Unchecked -= this.OnExpanderUnchecked;
                this.mExpander.Checked += this.OnExpanderChecked;
                this.mExpander.Unchecked += this.OnExpanderUnchecked;
            }
        }

        /// <summary>
        /// Registers to the check box events.
        /// </summary>
        private void RegisterToCheckBoxEvents()
        {
            if (this.mExpander != null)
            {
                this.mCheckBox.Checked -= this.OnCheckBoxChecked;
                this.mCheckBox.Unchecked -= this.OnCheckBoxUnchecked;
                this.mCheckBox.Checked += this.OnCheckBoxChecked;
                this.mCheckBox.Unchecked += this.OnCheckBoxUnchecked;
            }
        }

        /// <summary>
        /// Unregisters from the grid columns events.
        /// </summary>
        private void UnregisterFromGridColumnsEvents()
        {
            if (this.mParentGridView != null)
            {
                this.mParentGridView.Columns.CollectionChanged -= this.OnGridViewColumnsCollectionChanged;
                this.mParentGridView = null;
            }
        }

        /// <summary>
        /// Unregisters to the container event.
        /// </summary>
        private void UnregisterFromContainerEvents()
        {
            if (this.mDecoratorsContainer != null)
            {
                this.mDecoratorsContainer.SizeChanged -= this.OnDecoratorsContainerSizeChanged;
            }
        }

        /// <summary>
        /// Unregisters from the expander events.
        /// </summary>
        private void UnregisterFromExpanderEvents()
        {
            if (this.mExpander != null)
            {
                this.mExpander.Checked -= this.OnExpanderChecked;
                this.mExpander.Unchecked -= this.OnExpanderUnchecked;
            }
        }

        /// <summary>
        /// Unregisters from the check box events.
        /// </summary>
        private void UnregisterFromCheckBoxEvents()
        {
            if (this.mCheckBox != null)
            {
                this.mCheckBox.Checked -= this.OnCheckBoxChecked;
                this.mCheckBox.Unchecked -= this.OnCheckBoxUnchecked;
            }
        }

        /// <summary>
        /// Updates the bindings of the view.
        /// </summary>
        /// <param name="pOldViewModel">The old view model.</param>
        /// <param name="pNewViewModel">The new view model.</param>
        private void UpdateBindings(IHierarchicalItemViewModel pOldViewModel, IHierarchicalItemViewModel pNewViewModel)
        {
            if (pOldViewModel != null)
            {
                // Unregistering from the events.
                this.UnregisterFromExpanderEvents();
                this.UnregisterFromCheckBoxEvents();
                this.UnregisterFromContainerEvents();
                this.UnregisterFromGridColumnsEvents();

                // Clear reference to group property.
                this.Group = null;
            }

            if (pNewViewModel != null && pNewViewModel.IsDisposed == false)
            {
                // Registering to uncontextual events.
                this.RegisterToGridColumnsEvents();
                this.RegisterToContainerEvents();

                // Binding the IsSelected property.
                Binding lIsSelectedBinding = new Binding("IsSelected");
                lIsSelectedBinding.Source = pNewViewModel;
                lIsSelectedBinding.Mode = BindingMode.OneWay;
                this.SetBinding(TreeListViewItem.IsSelectedProperty, lIsSelectedBinding);

                // Binding the Tooltip property.
                Binding lTooltipBinding = new Binding("ToolTip");
                lTooltipBinding.Source = pNewViewModel;
                lTooltipBinding.Converter = new ToolTipConverter();
                this.SetBinding(TreeListViewItem.ToolTipProperty, lTooltipBinding);

                // Binding the FirstLevelItemsAsGroup property.
                MultiBinding lIsGroupBinding = new MultiBinding();
                Binding lViewModelBinding = new Binding() { BindsDirectlyToSource = true };
                lViewModelBinding.Source = pNewViewModel;
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
                this.Group = pNewViewModel;

                if (this.mExpander != null)
                {
                    // Binding the visibility of the expander.
                    Binding lExpanderVisibilityBinding = new Binding("HasChildren");
                    lExpanderVisibilityBinding.Source = pNewViewModel;
                    lExpanderVisibilityBinding.Converter = new CanExpandConverter();
                    this.mExpander.SetBinding(ToggleButton.VisibilityProperty, lExpanderVisibilityBinding);

                    // Binding the indentation of the expander.
                    Binding lExpanderMarginBinding = new Binding("Parent");
                    lExpanderMarginBinding.Source = pNewViewModel;
                    lExpanderMarginBinding.Converter = new LevelToIndentConverter();
                    this.mExpander.SetBinding(ToggleButton.MarginProperty, lExpanderMarginBinding);

                    // Binding the expanded state.
                    this.RegisterToExpanderEvents();
                    Binding lExpanderIsCheckedBinding = new Binding("IsExpanded");
                    lExpanderIsCheckedBinding.Source = pNewViewModel;
                    lExpanderIsCheckedBinding.Mode = BindingMode.OneWay;
                    this.mExpander.SetBinding(ToggleButton.IsCheckedProperty, lExpanderIsCheckedBinding);
                }

                if (this.mImage != null)
                {
                    // Binding the icon source.
                    Binding lImageSourceBinding = new Binding("IconSource");
                    lImageSourceBinding.Source = pNewViewModel;
                    this.mImage.SetBinding(Image.SourceProperty, lImageSourceBinding);

                    if (this.IsGroup == false)
                    {
                        // Binding the icon visibility.
                        Binding lImageVisibilityBinding = new Binding("IconVisibility");
                        lImageVisibilityBinding.Source = pNewViewModel;
                        this.mImage.SetBinding(Image.VisibilityProperty, lImageVisibilityBinding);
                    }
                }

                if (this.mCheckBox != null)
                {
                    // Binding the checked state.
                    Binding lCheckBoxIsCheckedBinding = new Binding("IsChecked");
                    lCheckBoxIsCheckedBinding.Source = pNewViewModel;
                    lCheckBoxIsCheckedBinding.Mode = BindingMode.OneWay;
                    this.mCheckBox.SetBinding(CheckBox.IsCheckedProperty, lCheckBoxIsCheckedBinding);
                    this.RegisterToCheckBoxEvents();

                    // Binding the checkable state.
                    Binding lCheckBoxVisibilityBinding = new Binding("IsCheckable");
                    lCheckBoxVisibilityBinding.Source = pNewViewModel;
                    lCheckBoxVisibilityBinding.Converter = new BooleanToVisibilityConverter();
                    this.mCheckBox.SetBinding(CheckBox.VisibilityProperty, lCheckBoxVisibilityBinding);

                    // Binding the checking state.
                    Binding lCheckBoxIsEnabledBinding = new Binding("IsCheckingEnabled");
                    lCheckBoxIsEnabledBinding.Source = pNewViewModel;
                    this.mCheckBox.SetBinding(CheckBox.IsEnabledProperty, lCheckBoxIsEnabledBinding);
                }
            }
        }

        /// <summary>
        /// Delegate called when the columns collection of the grid view is modified.
        /// </summary>
        /// <param name="pSender">The modified grid view.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private void OnGridViewColumnsCollectionChanged(object pSender, NotifyCollectionChangedEventArgs pEventArgs)
        {
            switch (pEventArgs.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    // Nothing to do.
                    break;

                case NotifyCollectionChangedAction.Remove:
                    // Nothing to do.
                    break;

                case NotifyCollectionChangedAction.Replace:
                    // Nothing to do.
                    break;

                case NotifyCollectionChangedAction.Move:
                    {
                        // Updating the expander clip region.
                        this.mDecoratorsContainer.Clip = this.GetDecoratorsClipRegion();
                    }

                    break;

                case NotifyCollectionChangedAction.Reset:
                    // Nothing to do.
                    break;
            }
        }

        /// <summary>
        /// Retrieves the region used to clip the decorator container.
        /// </summary>
        private Geometry GetDecoratorsClipRegion()
        {
            // Blank space between the end of the column and the actual end of the clip region of the column.
            double lBlankSpaceMarginInPixels = 6;

            RectangleGeometry lClip = null;
            TreeListView lParent = this.ParentTreeListView;
            if (lParent != null)
            {
                if  (  (lParent.InnerListView.View != null)
                    && (lParent.InnerListView.View.Columns.Count > 0)
                    )
                {
                    GridViewColumn lFirstColumn = lParent.InnerListView.View.Columns[0];
                    lClip = new RectangleGeometry();

                    Binding lXBinding = new Binding("ActualWidth");
                    lXBinding.Source = lFirstColumn;
                    Binding lYBinding = new Binding("ActualHeight");
                    lYBinding.Source = lParent.InnerListView;
                    MultiBinding lRectBinding = new MultiBinding();
                    lRectBinding.Bindings.Add(lXBinding);
                    lRectBinding.Bindings.Add(lYBinding);
                    lRectBinding.Converter = new WidthHeightToRectConverter() { Margin = new Thickness(0, 0, lBlankSpaceMarginInPixels, 0) };

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
            this.ViewModel.DecoratorWidth = pEventArgs.NewSize.Width;
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
                lParent.InnerListView.OnItemKeyUp(this.ViewModel, pEventArgs);
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
                lParent.InnerListView.OnItemMouseLeftButtonDown(this.ViewModel, pEventArgs);
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
                lParent.InnerListView.OnItemMouseRightButtonDown(this.ViewModel, pEventArgs);
            }

            pEventArgs.Handled = true;
        }

        /// <summary>
        /// Delegate called when the mouse left button is up.
        /// </summary>
        /// <param name="pEventArgs">The event arguments.</param>
        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs pEventArgs)
        {
            TreeListView lParent = this.ParentTreeListView;
            if (lParent != null)
            {
                lParent.InnerListView.OnItemMouseLeftButtonUp(this.ViewModel, pEventArgs);
            }
        }

        /// <summary>
        /// Delegate called when the mouse right button is up.
        /// </summary>
        /// <param name="pEventArgs">The event arguments.</param>
        protected override void OnMouseRightButtonUp(MouseButtonEventArgs pEventArgs)
        {
            TreeListView lParent = this.ParentTreeListView;
            if (lParent != null)
            {
                lParent.InnerListView.OnItemMouseRightButtonUp(this.ViewModel, pEventArgs);
            }
        }
        
        /// <summary>
        /// Delegate called when the mouse clicked on this item.
        /// </summary>
        /// <param name="pEventArgs">The event arguments.</param>
        protected override void OnMouseUp(MouseButtonEventArgs pEventArgs)
        {
            base.OnMouseUp(pEventArgs);

            TreeListView lParent = this.ParentTreeListView;
            if (lParent != null)
            {
                lParent.InnerListView.OnItemMouseClicked(this.ViewModel, pEventArgs);
            }
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
                lParent.InnerListView.OnItemMouseDoubleClicked(this.ViewModel, pEventArgs);
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
                lParent.InnerListView.CheckModel.Check(this.ViewModel, false);
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
                lParent.InnerListView.CheckModel.Uncheck(this.ViewModel, false);
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
                lParent.InnerListView.OnItemExpanderChecked(this.ViewModel);
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
                lParent.InnerListView.OnItemExpanderUnchecked(this.ViewModel);
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
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Specialized;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Data;
using System.ComponentModel;
using System.Collections.ObjectModel;
using XTreeListView.ViewModel;

namespace XTreeListView.Gui
{
    /// <summary>
    /// This class defines a tree list view.
    /// </summary>
    [TemplatePart(Name = PART_ListView, Type = typeof(ExtendedListView))]
    [TemplatePart(Name = PART_DefaultMessage, Type = typeof(Label))]
    public class TreeListView : ListBox
    {
        #region Dependencies

        /// <summary>
        /// Identifies the ViewModel dependency property.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof(IRootHierarchicalItemViewModel), typeof(TreeListView), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnViewModelChanged)));

        /// <summary>
        /// Identifies the DefaultMessage dependency property.
        /// </summary>
        public static readonly DependencyProperty DefaultMessageProperty = DependencyProperty.Register("DefaultMessage", typeof(string), typeof(TreeListView), new FrameworkPropertyMetadata("No message defined."));

        /// <summary>
        /// Identifies the GridView attached dependency property.
        /// </summary>
        public static readonly DependencyProperty GridViewProperty = DependencyProperty.Register("GridView", typeof(ExtendedGridView), typeof(TreeListView), new FrameworkPropertyMetadata(null));

        /// <summary>
        /// Identifies the GroupItemDataTemplate dependency property.
        /// </summary>
        public static readonly DependencyProperty GroupItemDataTemplateProperty = DependencyProperty.Register("GroupItemDataTemplate", typeof(System.Windows.DataTemplate), typeof(TreeListView), new FrameworkPropertyMetadata(null));

        /// <summary>
        /// Identifies the FirstLevelItemsAsGroup dependency property.
        /// </summary>
        public static readonly DependencyProperty FirstLevelItemsAsGroupProperty = DependencyProperty.Register("FirstLevelItemsAsGroup", typeof(bool), typeof(TreeListView), new FrameworkPropertyMetadata(false));

        #endregion // Dependencies.

        #region Fields

        /// <summary>
        /// Name of the parts that have to be in the control template.
        /// </summary>
        private const string PART_ListView = "PART_ListView";
        private const string PART_DefaultMessage = "PART_DefaultMessage";

        /// <summary>
        /// Constant containing the default property name displayed in the tree view.
        /// </summary>
        private const string cDefaultDisplayedPropertyName = "DisplayString";

        /// <summary>
        /// Stores the label used to display the default message.
        /// </summary>
        private Label mDefaultMessage;

        /// <summary>
        /// Stores the flag indicating the item selection mode when the inner tree list view is not loaded yet.
        /// </summary>
        private TreeSelectionOptions mPendingSelectionOption;

        /// <summary>
        /// Stores the resoures of the control.
        /// </summary>
        private Resources mResources;

        /// <summary>
        /// Stores the context menu name.
        /// </summary>
        private string mContextMenuName;

        #endregion // Fields.

        #region Constructors

        /// <summary>
        /// Initializes the <see cref="TreeListView"/> class.
        /// </summary>
        static TreeListView()
        {
            TreeListView.DefaultStyleKeyProperty.OverrideMetadata(typeof(TreeListView), new FrameworkPropertyMetadata(typeof(TreeListView)));
            TreeListView.DisplayMemberPathProperty.OverrideMetadata(typeof(TreeListView), new FrameworkPropertyMetadata(TreeListView.cDefaultDisplayedPropertyName));
            TreeListView.ItemTemplateProperty.OverrideMetadata(typeof(TreeListView), new FrameworkPropertyMetadata(null, new CoerceValueCallback(OnCoerceItemTemplateAndSelector)));
            TreeListView.ItemTemplateSelectorProperty.OverrideMetadata(typeof(TreeListView), new FrameworkPropertyMetadata(null, new CoerceValueCallback(OnCoerceItemTemplateAndSelector)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeListView"/> class.
        /// </summary>
        public TreeListView()
        {
            this.InnerListView = null;
            this.SelectionOption = TreeSelectionOptions.SingleSelection;
            this.mResources = new Resources();
            this.mContextMenuName = string.Empty;

            // Creating the grid vew.
            if (DesignerProperties.GetIsInDesignMode(this) == false)
            {
                // Only create it when not in design mode to avoid XAML edition issues.
                this.GridView = new ExtendedGridView(this.mResources);
                TreeListViewColumn lColumn = new TreeListViewColumn() { DataMemberBindingPath = TreeListView.cDefaultDisplayedPropertyName, Stretch = true };
                this.SetFirstColumn(lColumn);
            }

            // Applying the default group data template.
            this.GroupItemDataTemplate = this.mResources["GroupItemDefaultDataTemplate"] as System.Windows.DataTemplate;
        }

        #endregion // Constructors.

        #region Events

        /// <summary>
        /// Event raised when the selection is modified.
        /// </summary>
        public new event XTreeListView.ViewModel.SelectionChangedEventHandler SelectionChanged;

        /// <summary>
        /// Event raised when an item gets toggled.
        /// </summary>
        public event TreeViewEventHandler ItemsViewModelToggled;

        /// <summary>
        /// Event called when some items are added.
        /// </summary>
        public event TreeViewEventHandler ItemViewModelsAdded;

        /// <summary>
        /// Event called when some items are removed.
        /// </summary>
        public event TreeViewEventHandler ItemViewModelsRemoved;

        /// <summary>
        /// Event called when some items are modified.
        /// </summary>
        public event TreeViewItemEventHander ItemViewModelModified;

        /// <summary>
        /// Event called when an item is clicked.
        /// </summary>  
        public event TreeViewEventHandler ItemViewModelClicked
        {
            add
            {
                throw new NotImplementedException();
            }
            remove
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Event called when an item is double clicked.
        /// </summary>
        public event TreeViewEventHandler ItemViewModelDoubleClicked;

        /// <summary>
        /// Event raised when the view model changed.
        /// </summary>
        public event DependencyPropertyChangedEventHandler ViewModelChanged;

        #endregion // Events.

        #region Properties

        /// <summary>
        /// Gets the tree list view id.
        /// </summary>
        public string Id
        {
            get;
            set;
        }

        /// <summary>
        /// Get the tree list view display name. Used as name in the GUI.
        /// </summary>
        public string DisplayName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the view model of the tree.
        /// </summary>
        public IRootHierarchicalItemViewModel ViewModel
        {
            get
            {
                return (IRootHierarchicalItemViewModel)GetValue(ViewModelProperty);
            }
            set
            {
                SetValue(ViewModelProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the grid view of the tree lit view.
        /// </summary>
        public ExtendedGridView GridView
        {
            get
            {
                return (ExtendedGridView)GetValue(GridViewProperty);
            }

            set
            {
                SetValue(GridViewProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the data template of a group item.
        /// </summary>
        public System.Windows.DataTemplate GroupItemDataTemplate
        {
            get
            {
                return (System.Windows.DataTemplate)GetValue(GroupItemDataTemplateProperty);
            }
            set
            {
                SetValue(GroupItemDataTemplateProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the flag indicating if the first level items must be displayed as group.
        /// </summary>
        public bool FirstLevelItemsAsGroup
        {
            get
            {
                return (bool)GetValue(FirstLevelItemsAsGroupProperty);
            }
            set
            {
                SetValue(FirstLevelItemsAsGroupProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the default message of the tree.
        /// </summary>
        public string DefaultMessage
        {
            get
            {
                return (string)GetValue(DefaultMessageProperty);
            }
            set
            {
                SetValue(DefaultMessageProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the flag indicating the item selection mode.
        /// </summary>
        public TreeSelectionOptions SelectionOption
        {
            get
            {
                if (this.InnerListView != null)
                {
                    return this.InnerListView.SelectionModel.SelectionOption;
                }

                return this.mPendingSelectionOption;
            }

            set
            {
                if (this.InnerListView != null)
                {
                    this.InnerListView.SelectionModel.SelectionOption = value;
                }
                else
                {
                    this.mPendingSelectionOption = value;
                }
            }
        }

        /// <summary>
        /// Gets the selected items view model.
        /// </summary>
        public IEnumerable<IHierarchicalItemViewModel> SelectedViewModels
        {
            get
            {
                if (this.InnerListView != null)
                {
                    return this.InnerListView.SelectionModel.SelectedItemsViewModel;
                }

                return new IHierarchicalItemViewModel[] { };
            }
        }

        /// <summary>
        /// Gets the selected objects.
        /// </summary>
        public IEnumerable<object> SelectedObjects
        {
            get
            {
                if (this.InnerListView != null)
                {
                    return this.InnerListView.SelectionModel.SelectedObjects;
                }

                return new object[] { };
            }
        }

        /// <summary>
        /// Gets the checked items view model.
        /// </summary>
        public IEnumerable<IHierarchicalItemViewModel> CheckedViewModels
        {
            get
            {
                if (this.InnerListView != null)
                {
                    return this.InnerListView.CheckModel.CheckedItemsViewModel;
                }

                return new IHierarchicalItemViewModel[] { };
            }
        }

        /// <summary>
        /// Gets or sets the inner list view.
        /// </summary>
        internal ExtendedListView InnerListView 
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
            this.InnerListView = this.GetTemplateChild(PART_ListView) as ExtendedListView;
            this.mDefaultMessage = this.GetTemplateChild(PART_DefaultMessage) as Label;

            if  (   (this.InnerListView == null)
                ||  (this.mDefaultMessage == null)
                )
            {
                throw new Exception("TreeListView control template not correctly defined.");
            }

            // Loading the view model.
            this.LoadViewModel();

            // Applying the selection option.
            this.InnerListView.SelectionModel.SelectionOption = this.mPendingSelectionOption;

            // Registering on the collection changed event for the selected and checked items.
            this.InnerListView.SelectionModel.SelectionChanged += this.OnInnerListViewSelectionChanged;
            this.InnerListView.CheckModel.ItemsViewModelToggled += this.OnInnerListItemsViewModelToggled;
            this.InnerListView.ItemViewModelsAdded += this.OnInnerListViewItemViewModelsAdded;
            this.InnerListView.ItemViewModelsRemoved += this.OnInnerListViewItemViewModelsRemoved;
            this.InnerListView.ItemViewModelDoubleClicked += this.OnInnerListViewItemViewModelDoubleClicked;
        }

        /// <summary>
        /// This method forces the item to be visible in the viewport.
        /// </summary>
        /// <param name="pItem">The item to make visible into the viewport.</param>
        public void ScrollToItem(IHierarchicalItemViewModel pItem)
        {
            if (this.InnerListView != null)
            {
                this.InnerListView.ScrollIntoView(pItem, true);
            }
        }

        /// <summary>
        /// This method selects an item.
        /// </summary>
        /// <param name="pItem">The item to select.</param>
        public void Select(IHierarchicalItemViewModel pItem)
        {
            if (this.InnerListView != null)
            {
                this.InnerListView.SelectionModel.Select(pItem);
            }
        }

        /// <summary>
        /// This method selects all the items.
        /// </summary>
        public new void SelectAll()
        {
            if (this.InnerListView != null)
            {
                this.InnerListView.SelectionModel.SelectAll();
            }
        }

        /// <summary>
        /// This method unselects an item.
        /// </summary>
        /// <param name="pItem">The item to unselect.</param>
        public void Unselect(IHierarchicalItemViewModel pItem)
        {
            if (this.InnerListView != null)
            {
                this.InnerListView.SelectionModel.Unselect(pItem, false);
            }
        }

        /// <summary>
        /// Unselect all the items in the tree.
        /// </summary>
        public new void UnselectAll()
        {
            if (this.InnerListView != null)
            {
                this.InnerListView.SelectionModel.UnselectAll();
            }
        }

        /// <summary>
        /// Sets the first column of the tree list view.
        /// </summary>
        /// <param name="pColumn">The first column properties.</param>
        public void SetFirstColumn(TreeListViewColumn pColumn)
        {
            if (this.GridView != null)
            {
                this.GridView.SetFirstColumn(pColumn);
            }
        }

        /// <summary>
        /// Adds a new column to the tree list view.
        /// </summary>
        /// <param name="pColumn">The column to add.</param>
        public void AddColumn(TreeListViewColumn pColumn)
        {
            if (this.GridView != null)
            {
                this.GridView.AddColumn(pColumn);
            }
        }

        /// <summary>
        /// Expands the given item.
        /// </summary>
        /// <param name="pItem">The item to expand.</param>
        public void Expand(IHierarchicalItemViewModel pItem)
        {
            if (this.InnerListView != null)
            {
                this.InnerListView.ExpandModel.SetIsExpanded(pItem, true);
            }
        }

        /// <summary>
        /// Collapses the given item.
        /// </summary>
        /// <param name="pItem">The item to expand.</param>
        public void Collapse(IHierarchicalItemViewModel pItem)
        {
            if (this.InnerListView != null)
            {
                this.InnerListView.ExpandModel.SetIsExpanded(pItem, false);
            }
        }

        /// <summary>
        /// Checks the given item.
        /// </summary>
        /// <param name="pItem">The item to check.</param>
        public void Check(IHierarchicalItemViewModel pItem)
        {
            if (this.InnerListView != null)
            {
                this.InnerListView.CheckModel.Check(pItem, false);
            }
        }

        /// <summary>
        /// Unchecks the given item.
        /// </summary>
        /// <param name="pItem">The item to uncheck.</param>
        public void Uncheck(IHierarchicalItemViewModel pItem)
        {
            if (this.InnerListView != null)
            {
                this.InnerListView.CheckModel.Uncheck(pItem, false);
            }
        }

        /// <summary>
        /// This delagate is called when the view model is changed.
        /// </summary>
        /// <param name="pObject">The event sender.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private static void OnViewModelChanged(DependencyObject pObject, DependencyPropertyChangedEventArgs pEventArgs)
        {
            TreeListView lTreeListView = pObject as TreeListView;
            if (lTreeListView != null && lTreeListView.InnerListView != null)
            {
                // Unloading the view model from the inner list.
                lTreeListView.InnerListView.ViewModel = null;

                // Invalidating the old view model.
                IRootHierarchicalItemViewModel lOldViewModel = pEventArgs.OldValue as IRootHierarchicalItemViewModel;
                if (lOldViewModel != null)
                {
                    lOldViewModel.PropertyChanged -= lTreeListView.OnRootViewModelPropertyChanged;
                    lOldViewModel.ItemViewModelModified -= lTreeListView.OnItemViewModelModified;             
                }

                // Loading the new view model.
                lTreeListView.LoadViewModel();
                
                // Notifying the user.
                if (lTreeListView.ViewModelChanged != null)
                {
                    lTreeListView.ViewModelChanged(lTreeListView, pEventArgs);
                }
            }
        }

        /// <summary>
        /// Loads the view model into the inner list view.
        /// </summary>
        private void LoadViewModel()
        {
            // Handling the new view model.
            if (this.ViewModel != null)
            {
                // Registering on the view model PropertyChanged event.
                this.UpdateDefaultMessageVisibility();
                this.ViewModel.PropertyChanged += this.OnRootViewModelPropertyChanged;
                this.ViewModel.ItemViewModelModified += this.OnItemViewModelModified;

                // Initializing the view model.
                this.InnerListView.ViewModel = this.ViewModel;
            }
        }

        /// <summary>
        /// Delegate called when an item view model property is modified.
        /// </summary>
        /// <param name="pSender">The modified item view model.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private void OnItemViewModelModified(object pSender, PropertyChangedEventArgs pEventArgs)
        {
            if (this.ItemViewModelModified != null)
            {
                this.ItemViewModelModified(pSender, pEventArgs);
            }
        }

        /// <summary>
        /// Delegate called when a property is modified on the view model.
        /// </summary>
        /// <param name="pSender">The modified view model.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private void OnRootViewModelPropertyChanged(object pSender, PropertyChangedEventArgs pEventArgs)
        {
            this.UpdateDefaultMessageVisibility();
        }

        /// <summary>
        /// Updates the default message visibility.
        /// </summary>
        private void UpdateDefaultMessageVisibility()
        {
            if (this.ViewModel != null && this.ViewModel.Children.Any())
            {
                // Hidding the default message.
                this.mDefaultMessage.Visibility = Visibility.Hidden;
            }
            else
            {
                // Showing the default message.
                this.mDefaultMessage.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Delegate called when the ItemTemplate or the ItemTemplateSelector property values have to be coerced.
        /// </summary>
        /// <param name="pSender">The modified tree list view.</param>
        /// <param name="pObject">The object to coerce.</param>
        /// <returns>The coerced object.</returns>
        private static object OnCoerceItemTemplateAndSelector(DependencyObject pSender, object pObject)
        {
            TreeListView lControl = pSender as TreeListView;
            if
                (lControl != null)
            {
                if
                    (pObject != null)
                {
#pragma warning disable 1587
                    /// ItemTemplate and ItemTemplateSelector properties cannot be set (exception) if the DisplayMemberPath
                    /// is defined. It is the case has it is the default behaviour of the tree.
                    /// When using the ItemTemplate or ItemTemplateSelector properties, the DisplayMemberPath is then invalidated.
#pragma warning restore 1587
                    lControl.DisplayMemberPath = string.Empty;
                }
            }

            return pObject;
        }

        /// <summary>
        /// Method called when a click is performed in the tree list view.
        /// </summary>
        /// <param name="pEventArgs">The event arguments.</param>
        protected override void OnMouseDown(System.Windows.Input.MouseButtonEventArgs pEventArgs)
        {
            base.OnMouseDown(pEventArgs);

            // This event is raised when the click is performed in the tree but not in a specific item.
            if (this.InnerListView != null)
            {
                this.InnerListView.OnTreeListViewMouseDown(pEventArgs);
            }
        }

        /// <summary>
        /// Delegate called when some items are toggled.
        /// </summary>
        /// <param name="pSender">The modified list view.</param>
        /// <param name="pViewModels">The toggled items.</param>
        private void OnInnerListItemsViewModelToggled(object pSender, IEnumerable<IHierarchicalItemViewModel> pViewModels)
        {
            if (this.ItemsViewModelToggled != null)
            {
                this.ItemsViewModelToggled(this, pViewModels);
            }
        }

        /// <summary>
        /// Delegate called when the selection changed on the inner list view.
        /// </summary>
        /// <param name="pSender">The modified list view.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private void OnInnerListViewSelectionChanged(object pSender, XTreeListView.ViewModel.SelectionChangedEventArgs pEventArgs)
        {
            this.OnSelectionChanged(pEventArgs.RemovedItems, pEventArgs.AddedItems);
        }

        /// <summary>
        /// Method called when the selection changed.
        /// </summary>
        /// <param name="pAddedItems">The added items from the previous selection.</param>
        /// <param name="pRemovedItems">The removed items from the previous selection.</param>
        protected virtual void OnSelectionChanged(IEnumerable pRemovedItems, IEnumerable pAddedItems)
        {
            // Notifying threw the tree.
            if (this.SelectionChanged != null)
            {
                XTreeListView.ViewModel.SelectionChangedEventArgs lSelectionEventArgs = new XTreeListView.ViewModel.SelectionChangedEventArgs(pRemovedItems, pAddedItems);
                this.SelectionChanged(this, lSelectionEventArgs);
            }
        }

        /// <summary>
        /// Delegate called when items are added in the view model. 
        /// </summary>
        /// <param name="pSender">The modified root view model.</param>
        /// <param name="pItems">The added items.</param>
        private void OnInnerListViewItemViewModelsAdded(object pSender, IEnumerable<IHierarchicalItemViewModel> pItems)
        {
            if (this.ItemViewModelsAdded != null)
            {
                this.ItemViewModelsAdded(this, pItems);
            }
        }

        /// <summary>
        /// Delegate called when items are removed from the view model. 
        /// </summary>
        /// <param name="pSender">The modified root view model.</param>
        /// <param name="pItems">The removed items.</param>
        private void OnInnerListViewItemViewModelsRemoved(object pSender, IEnumerable<IHierarchicalItemViewModel> pItems)
        {
            if (this.ItemViewModelsRemoved != null)
            {
                this.ItemViewModelsRemoved(this, pItems);
            }
        }

        /// <summary>
        /// Delegate called when items are double clicked. 
        /// </summary>
        /// <param name="pSender">The modified root view model.</param>
        /// <param name="pItems">The removed items.</param>
        private void OnInnerListViewItemViewModelDoubleClicked(object pSender, IEnumerable<IHierarchicalItemViewModel> pItems)
        {
            if (this.ItemViewModelDoubleClicked != null)
            {
                this.ItemViewModelDoubleClicked(this, pItems);
            }
        }

        #endregion // Methods.
    }
}

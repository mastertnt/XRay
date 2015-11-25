using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.ComponentModel;
using XTreeListView.Behaviors.Column;
using XTreeListView.Gui;

namespace XTreeListView.Behaviors
{
	/// <summary>
	/// Class responsible for handling the size of the column of the ListView GridView.
	/// </summary>
    /// <remarks>Implemented using the project "http://www.codeproject.com/Articles/25058/ListView-Layout-Manager".</remarks>
	public class ColumnResizeBehavior
    {
        #region Fields

        /// <summary>
        /// Stores the delta defining if a range can be considered as zero in pixel.
        /// </summary>
        private const double ZERO_WIDTH_RANGE = 0.1;

        /// <summary>
        /// Stores the managed list view.
        /// </summary>
        private readonly ExtendedListView mListView;

        /// <summary>
        /// Stores the scrol viewer of the list view.
        /// </summary>
        private ScrollViewer mScrollViewer;

        /// <summary>
        /// Stores the flag indicating if the manager is loaded.
        /// </summary>
        private bool mLoaded;

        /// <summary>
        /// Stores the flag indicating if the list view is resizing.
        /// </summary>
        private bool mResizing;

        /// <summary>
        /// Stores the default cursor. Used during resizing.
        /// </summary>
        private Cursor mBackCursor;

        /// <summary>
        /// Stores the vertical scroll bar visibility.
        /// </summary>
        private ScrollBarVisibility mVerticalScrollBarVisibility;

        /// <summary>
        /// Stores the column that need to be resized by a specific process.
        /// </summary>
        private GridViewColumn mAutoSizedColumn;

        #endregion // Fields.

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnResizeBehavior"/> class.
        /// </summary>
        /// <param name="pListView">The managed list view.</param>
        public ColumnResizeBehavior(ExtendedListView pListView)
		{
            if (pListView == null)
			{
                throw new ArgumentNullException("pListView");
			}

            this.mVerticalScrollBarVisibility = ScrollBarVisibility.Auto;
			this.mListView = pListView;
            this.mListView.Loaded += new RoutedEventHandler(this.OnListViewLoaded);
            this.mListView.Unloaded += new RoutedEventHandler(this.OnListViewUnloaded);
		}

        #endregion // Constructors.

        #region Methods

        /// <summary>
        /// Registers on the wanted events on the dependency object.
        /// </summary>
        /// <param name="pObject">The object on witch to register the delegates.</param>
        private void RegisterEvents(DependencyObject pObject)
        {
            for (int lIter = 0; lIter < VisualTreeHelper.GetChildrenCount(pObject); lIter++)
            {
                Visual lChildVisual = VisualTreeHelper.GetChild(pObject, lIter) as Visual;
                if (lChildVisual is Thumb)
                {
                    GridViewColumn lColumn = this.FindParentColumn(lChildVisual);
                    if (lColumn != null)
                    {
                        Thumb lThumb = lChildVisual as Thumb;
                        if (FixedColumn.IsFixedColumn(lColumn) || this.IsFillColumn(lColumn))
                        {
                            // Do not allow resize on the fixed column.
                            lThumb.IsHitTestVisible = false;
                        }
                        else
                        {
                            // Registering.
                            lThumb.PreviewMouseMove += new MouseEventHandler(this.OnThumbPreviewMouseMove);
                            lThumb.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(this.OnThumbPreviewMouseLeftButtonDown);
                            DependencyPropertyDescriptor.FromProperty(GridViewColumn.WidthProperty, typeof(GridViewColumn)).AddValueChanged(lColumn, this.OnGridColumnWidthChanged);
                        }
                    }
                }
                else if (lChildVisual is GridViewColumnHeader)
                {
                    GridViewColumnHeader lColumnHeader = lChildVisual as GridViewColumnHeader;
                    lColumnHeader.SizeChanged += new SizeChangedEventHandler(this.OnGridColumnHeaderSizeChanged);
                }
                else if (this.mScrollViewer == null && lChildVisual is ScrollViewer)
                {
                    this.mScrollViewer = lChildVisual as ScrollViewer;
                    this.mScrollViewer.ScrollChanged += new ScrollChangedEventHandler(this.OnScrollViewerScrollChanged);
                    
                    // Assume we do the regulation of the horizontal scrollbar.
                    this.mScrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;
                    this.mScrollViewer.VerticalScrollBarVisibility = this.mVerticalScrollBarVisibility;
                }

                this.RegisterEvents(lChildVisual);
            }
        }

        /// <summary>
        /// Unregisters from the wanted events of the dependency object.
        /// </summary>
        /// <param name="pObject">The object on witch to unregister the delegates.</param>
        private void UnregisterEvents(DependencyObject pObject)
        {
            for (int lIter = 0; lIter < VisualTreeHelper.GetChildrenCount(pObject); lIter++)
            {
                Visual lChildVisual = VisualTreeHelper.GetChild(pObject, lIter) as Visual;
                if (lChildVisual is Thumb)
                {
                    GridViewColumn lColumn = this.FindParentColumn(lChildVisual);
                    if (lColumn != null)
                    {
                        Thumb lThumb = lChildVisual as Thumb;
                        if (FixedColumn.IsFixedColumn(lColumn) || this.IsFillColumn(lColumn))
                        {
                            lThumb.IsHitTestVisible = true;
                        }
                        else
                        {
                            lThumb.PreviewMouseMove -= new MouseEventHandler(this.OnThumbPreviewMouseMove);
                            lThumb.PreviewMouseLeftButtonDown -= new MouseButtonEventHandler(this.OnThumbPreviewMouseLeftButtonDown);
                            DependencyPropertyDescriptor.FromProperty(GridViewColumn.WidthProperty, typeof(GridViewColumn)).RemoveValueChanged(lColumn, this.OnGridColumnWidthChanged);
                        }
                    }
                }
                else if (lChildVisual is GridViewColumnHeader)
                {
                    GridViewColumnHeader lColumnHeader = lChildVisual as GridViewColumnHeader;
                    lColumnHeader.SizeChanged -= new SizeChangedEventHandler(this.OnGridColumnHeaderSizeChanged);
                }
                else if (this.mScrollViewer == null && lChildVisual is ScrollViewer)
                {
                    this.mScrollViewer = lChildVisual as ScrollViewer;
                    this.mScrollViewer.ScrollChanged -= new ScrollChangedEventHandler(this.OnScrollViewerScrollChanged);
                }

                this.UnregisterEvents(lChildVisual);
            }
        }

        /// <summary>
        /// Initialize the columns by applying there properties.
        /// </summary>
        private void InitColumns()
        {
            GridView lView = this.mListView.View as GridView;
            if (lView == null)
            {
                return;
            }

            foreach (GridViewColumn lColumn in lView.Columns)
            {
                if (RangeColumn.IsRangeColumn(lColumn) == false)
                {
                    continue;
                }

                double? lMinWidth = RangeColumn.GetRangeMinWidth(lColumn);
                double? lMaxWidth = RangeColumn.GetRangeMaxWidth(lColumn);
                if (lMinWidth.HasValue == false && lMaxWidth.HasValue == false)
                {
                    continue;
                }

                GridViewColumnHeader lColumnHeader = this.FindColumnHeader(this.mListView, lColumn);
                if (lColumnHeader == null)
                {
                    continue;
                }

                double lActualWidth = lColumnHeader.ActualWidth;
                if (lMinWidth.HasValue)
                {
                    lColumnHeader.MinWidth = lMinWidth.Value;
                    if (!double.IsInfinity(lActualWidth) && lActualWidth < lColumnHeader.MinWidth)
                    {
                        lColumn.Width = lColumnHeader.MinWidth;
                    }
                }

                if (lMaxWidth.HasValue)
                {
                    lColumnHeader.MaxWidth = lMaxWidth.Value;
                    if (double.IsInfinity(lActualWidth) == false && lActualWidth > lColumnHeader.MaxWidth)
                    {
                        lColumn.Width = lColumnHeader.MaxWidth;
                    }
                }
            }
        }

        /// <summary>
        /// Tries to find the parent column of the given object.
        /// </summary>
        /// <param name="pObject">The object to parse.</param>
        /// <returns>The parent column if any.</returns>
        private GridViewColumn FindParentColumn(DependencyObject pObject)
        {
            if (pObject == null)
            {
                return null;
            }

            while (pObject != null)
            {
                GridViewColumnHeader lColumnHeader = pObject as GridViewColumnHeader;
                if (lColumnHeader != null)
                {
                    return (lColumnHeader).Column;
                }

                pObject = VisualTreeHelper.GetParent(pObject);
            }

            return null;
        }

        /// <summary>
        /// Tries to find the column header of the given column in the given object.
        /// </summary>
        /// <param name="pObject">The object to parse.</param>
        /// <param name="pColumn">The header column.</param>
        /// <returns>The parent column if any.</returns>
        private GridViewColumnHeader FindColumnHeader(DependencyObject pObject, GridViewColumn pColumn)
        {
            for (int lIter = 0; lIter < VisualTreeHelper.GetChildrenCount(pObject); lIter++)
            {
                Visual lChildVisual = VisualTreeHelper.GetChild(pObject, lIter) as Visual;
                if (lChildVisual is GridViewColumnHeader)
                {
                    GridViewColumnHeader lColumnHeader = lChildVisual as GridViewColumnHeader;
                    if (lColumnHeader.Column == pColumn)
                    {
                        return lColumnHeader;
                    }
                }

                // Recursive call.
                GridViewColumnHeader lChildColumnHeader = this.FindColumnHeader(lChildVisual, pColumn);  
                if (lChildColumnHeader != null)
                {
                    return lChildColumnHeader;
                }
            }
            return null;
        }

        /// <summary>
        /// Resizes the grid view columns.
        /// </summary>
        /// <param name="pColumnIndex">The specific index of the resized column, or -1 if the whole control is resized.</param>
        private void DoResizeColumns(int pColumnIndex)
        {
            if (this.mResizing)
            {
                return;
            }

            this.mResizing = true;
            try
            {
                this.ResizeColumns(pColumnIndex);
            }
            finally
            {
                this.mResizing = false;
            }
        }

        /// <summary>
        /// Resizes the grid view columns.
        /// </summary>
        /// <param name="pColumnIndex">The specific index of the resized column, or -1 if the whole control is resized.</param>
        private void ResizeColumns(int pColumnIndex)
        {
            GridView lGridView = this.mListView.View as GridView;
            if (lGridView == null || lGridView.Columns.Count == 0)
            {
                return;
            }

            // Computing the listview width.
            double lActualWidth = double.PositiveInfinity;
            if (mScrollViewer != null)
            {
                lActualWidth = this.mScrollViewer.ViewportWidth;
            }
            if (double.IsInfinity(lActualWidth))
            {
                lActualWidth = this.mListView.ActualWidth;
            }
            if (double.IsInfinity(lActualWidth) || lActualWidth <= 0)
            {
                return;
            }

            // Computing column sizes.
            double lResizeableRegionCount = 0;
            double lOtherColumnsWidth = 0;
            foreach (GridViewColumn lColumn in lGridView.Columns)
            {
                if (ProportionalColumn.IsProportionalColumn(lColumn))
                {
                    double? lProportionalWidth = ProportionalColumn.GetProportionalWidth(lColumn);
                    if (lProportionalWidth != null)
                    {
                        lResizeableRegionCount += lProportionalWidth.Value;
                    }
                }
                else
                {
                    lOtherColumnsWidth += lColumn.ActualWidth;
                }
            }

            if (lResizeableRegionCount <= 0)
            {
                // No proportional columns present : commit the regulation to the scroll viewer.
                if (this.mScrollViewer != null)
                {
                    this.mScrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
                }

                // Searching for the first fill column.
                GridViewColumn lFillColumn = null;
                for (int lIter = 0; lIter < lGridView.Columns.Count; lIter++)
                {
                    GridViewColumn lColumn = lGridView.Columns[lIter];
                    if (IsFillColumn(lColumn))
                    {
                        lFillColumn = lColumn;
                        break;
                    }
                }

                if (lFillColumn != null)
                {
                    // Applying the width to the fill column taking in account the range.
                    double lOtherColumnsWithoutFillWidth = lOtherColumnsWidth - lFillColumn.ActualWidth;
                    double lFillWidth = lActualWidth - lOtherColumnsWithoutFillWidth;
                    if (lFillWidth > 0)
                    {
                        double? lMinWidth = RangeColumn.GetRangeMinWidth(lFillColumn);
                        double? lMaxWidth = RangeColumn.GetRangeMaxWidth(lFillColumn);

                        bool lSetWidth = (lMinWidth.HasValue && lFillWidth < lMinWidth.Value) == false;
                        if (lMaxWidth.HasValue && lFillWidth > lMaxWidth.Value)
                        {
                            lSetWidth = false;
                        }

                        if (lSetWidth)
                        {
                            if (this.mScrollViewer != null)
                            {
                                this.mScrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;
                            }
                            lFillColumn.Width = lFillWidth;
                        }
                    }
                }
                return;
            }

            // Some proportional columns have been defined.
            double lResizeableColumnsWidth = lActualWidth - lOtherColumnsWidth;
            if (lResizeableColumnsWidth <= 0)
            {
                // Missing space.
                return; 
            }

            // Resize proportional columns.
            double lResizeableRegionWidth = lResizeableColumnsWidth / lResizeableRegionCount;
            foreach (GridViewColumn lColumn in lGridView.Columns)
            {
                if (ProportionalColumn.IsProportionalColumn(lColumn))
                {
                    if (pColumnIndex == -1)
                    {
                        // Computing the initial width.
                        double? lProportionalWidth = ProportionalColumn.GetProportionalWidth(lColumn);
                        if (lProportionalWidth != null)
                        {
                            lColumn.Width = lProportionalWidth.Value * lResizeableRegionWidth;
                        }
                    }
                    else
                    {
                        int lCurrentIndex = lGridView.Columns.IndexOf(lColumn);
                        if (pColumnIndex == lCurrentIndex)
                        {
                            // Adapting the ratio so that the column can be resized.
                            ProportionalColumn.ApplyWidth(lColumn, lColumn.Width / lResizeableRegionWidth);
                        }
                        else if (lCurrentIndex > pColumnIndex)
                        {
                            // Computing the initial width for the colums after the one resized.
                            double? proportionalWidth = ProportionalColumn.GetProportionalWidth(lColumn);
                            if (proportionalWidth != null)
                            {
                                lColumn.Width = proportionalWidth.Value * lResizeableRegionWidth;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Applies the bound to the given grid.
        /// </summary>
        /// <param name="pColumn">The column to bound.</param>
        /// <returns>The resize delta after applying the bound.</returns>
        private double SetRangeColumnToBounds(GridViewColumn pColumn)
        {
            if (RangeColumn.IsRangeColumn(pColumn) == false)
            {
                // No need to resize.
                return 0;
            }

            double lStartWidth = pColumn.Width;

            double? lMinWidth = RangeColumn.GetRangeMinWidth(pColumn);
            double? lMaxWidth = RangeColumn.GetRangeMaxWidth(pColumn);

            if ((lMinWidth.HasValue && lMaxWidth.HasValue) && (lMinWidth > lMaxWidth))
            {
                // Invalid case. No resize.
                return 0; 
            }

            // Bounding the width.
            if (lMinWidth.HasValue && pColumn.Width < lMinWidth.Value)
            {
                pColumn.Width = lMinWidth.Value;
            }
            else if (lMaxWidth.HasValue && pColumn.Width > lMaxWidth.Value)
            {
                pColumn.Width = lMaxWidth.Value;
            }

            return pColumn.Width - lStartWidth;
        }

        /// <summary>
        /// Returns the flag indicating if the given is a fill column.
        /// </summary>
        /// <param name="pColumn">The column to test.</param>
        /// <returns></returns>
        private bool IsFillColumn(GridViewColumn pColumn)
        {
            if (pColumn == null)
            {
                return false;
            }

            GridView lView = this.mListView.View as GridView;
            if (lView == null || lView.Columns.Count == 0)
            {
                return false;
            }

            bool? lIsFillColumn = RangeColumn.GetRangeIsFillColumn(pColumn);
            return lIsFillColumn.HasValue && lIsFillColumn.Value;
        }

        /// <summary>
        /// Delegate called when the managed list view is loaded.
        /// </summary>
        /// <param name="pSender">The event sender.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private void OnListViewLoaded(object pSender, RoutedEventArgs pEventArgs)
        {
            this.RegisterEvents(this.mListView);
            this.InitColumns();
            this.DoResizeColumns(-1);
            this.mLoaded = true;
        }

        /// <summary>
        /// Delegate called when the managed list view is unloaded.
        /// </summary>
        /// <param name="pSender">The event sender.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private void OnListViewUnloaded(object pSender, RoutedEventArgs pEventArgs)
        {
            if (this.mLoaded == false)
            {
                return;
            }

            this.UnregisterEvents(this.mListView);
            this.mLoaded = false;
        }

        /// <summary>
        /// Delegate called when a mouse is mouving on a thumb.
        /// </summary>
        /// <param name="pSender">The event sender.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private void OnThumbPreviewMouseMove(object pSender, MouseEventArgs pEventArgs)
        {
            Thumb lThumb = pSender as Thumb;
            if (lThumb == null)
            {
                return;
            }

            GridViewColumn lColumn = this.FindParentColumn(lThumb);
            if (lColumn == null)
            {
                return;
            }

            // Suppress column resizing for fixed and range fill columns.
            if (FixedColumn.IsFixedColumn(lColumn) || this.IsFillColumn(lColumn))
            {
                // Cursor is hidden.
                lThumb.Cursor = null;
                return;
            }

            // Check range column bounds.
            if (lThumb.IsMouseCaptured && RangeColumn.IsRangeColumn(lColumn))
            {
                double? lMinWidth = RangeColumn.GetRangeMinWidth(lColumn);
                double? lMaxWidth = RangeColumn.GetRangeMaxWidth(lColumn);

                if ((lMinWidth.HasValue && lMaxWidth.HasValue) && (lMinWidth > lMaxWidth))
                {
                    // Invalid case.
                    return; 
                }

                if (this.mBackCursor == null)
                {
                    // First time = save the resize cursor.
                    this.mBackCursor = lThumb.Cursor; 
                }

                // Updating the cursor.
                if (lMinWidth.HasValue && lColumn.Width <= lMinWidth.Value)
                {
                    lThumb.Cursor = Cursors.No;
                }
                else if (lMaxWidth.HasValue && lColumn.Width >= lMaxWidth.Value)
                {
                    lThumb.Cursor = Cursors.No;
                }
                else
                {
                    lThumb.Cursor = this.mBackCursor;
                }
            }
        }

        /// <summary>
        /// Delegate called when a mouse left button is down on a thumb.
        /// </summary>
        /// <param name="pSender">The event sender.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private void OnThumbPreviewMouseLeftButtonDown(object pSender, MouseButtonEventArgs pEventArgs)
        {
            Thumb lThumb = pSender as Thumb;
            if (lThumb == null)
            {
                return;
            }

            GridViewColumn lColumn = this.FindParentColumn(lThumb);
            if (lColumn == null)
            {
                return;
            }

            // Suppress column resizing for fixed and range fill columns.
            if (FixedColumn.IsFixedColumn(lColumn) || this.IsFillColumn(lColumn))
            {
                pEventArgs.Handled = true;
            }
        }

        /// <summary>
        /// Delegate called when a column width change.
        /// </summary>
        /// <param name="pSender">The event sender.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private void OnGridColumnWidthChanged(object pSender, EventArgs pEventArgs)
        {
            if (this.mLoaded == false)
            {
                return;
            }

            GridViewColumn lColumn = pSender as GridViewColumn;
            if (lColumn == null)
            {
                return;
            }

            // Suppress column resizing for fixed columns.
            if (FixedColumn.IsFixedColumn(lColumn))
            {
                return;
            }

            // Ensure range column within the bounds.
            if (RangeColumn.IsRangeColumn(lColumn))
            {
                // Special case: auto column width - maybe conflicts with min/max range.
                if (lColumn != null && lColumn.Width.Equals(double.NaN))
                {
                    // Resize is handled by the change header size event.
                    this.mAutoSizedColumn = lColumn;
                    return; 
                }

                // ensure column bounds
                if (Math.Abs(this.SetRangeColumnToBounds(lColumn) - 0) > ZERO_WIDTH_RANGE)
                {
                    return;
                }
            }

            // Force resize.
            GridView lGridView = this.mListView.View as GridView;
            this.DoResizeColumns(lGridView.Columns.IndexOf(lColumn));
        }

        /// <summary>
        /// Delegate called when a column header width change.
        /// </summary>
        /// <param name="pSender">The event sender.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private void OnGridColumnHeaderSizeChanged(object pSender, SizeChangedEventArgs pEventArgs)
        {
            if (this.mAutoSizedColumn == null)
            {
                return;
            }

            // Handling the resizing of the auto sized column.
            GridViewColumnHeader lColumnHeader = pSender as GridViewColumnHeader;
            if (lColumnHeader != null && lColumnHeader.Column == this.mAutoSizedColumn)
            {
                if (lColumnHeader.Width.Equals(double.NaN))
                {
                    // Sync column width. 
                    lColumnHeader.Column.Width = lColumnHeader.ActualWidth;

                    // Force resize.
                    GridView lGridView = this.mListView.View as GridView;
                    DoResizeColumns(lGridView.Columns.IndexOf(lColumnHeader.Column));
                }

                this.mAutoSizedColumn = null;
            }
        }

        /// <summary>
        /// Delegate called whenchanges are detected to the scroll position, extent, or viewport size.
        /// </summary>
        /// <param name="pSender">The event sender.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private void OnScrollViewerScrollChanged(object pSender, ScrollChangedEventArgs pEventArgs)
        {
            if (this.mLoaded && Math.Abs(pEventArgs.ViewportWidthChange - 0) > ZERO_WIDTH_RANGE)
            {
                this.DoResizeColumns(-1);
            }
        }

        #endregion // Methods.
	} 
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using XTreeListView.Extensions;

namespace XTreeListView.Gui
{
    /// <summary>
    /// Class defining a grid view header row presenter.
    /// </summary>
    public class ExtendedGridViewHeaderRowPresenter : GridViewHeaderRowPresenter
    {
        #region Fields

        /// <summary>
        /// Stores the minimum width for padding header.
        /// </summary>
        private const double c_PaddingHeaderMinWidth = 2.0;

        /// <summary>
        /// Stores the padding header.
        /// </summary>
        private GridViewColumnHeader mPaddingHeader;
        
        /// <summary>
        /// Stores the indicator diplayed when a header is dragged.
        /// </summary>
        private Separator mDragIndicator;

        #endregion // Fields.

        #region Properties

        /// <summary>
        /// Gets the flag indicating if a column header is dragging.
        /// </summary>
        private bool IsHeaderDragging
        {
            get
            {
                return this.GetFieldValue<GridViewHeaderRowPresenter, bool>("_isHeaderDragging");
            }
        }

        /// <summary>
        /// Gets the mouse current position when a header is dragged.
        /// </summary>
        private Point DragCurrentPosition
        {
            get
            {
                return this.GetFieldValue<GridViewHeaderRowPresenter, Point>("_currentPos");
            }
        }

        /// <summary>
        /// Gets the mouse start position when a header is going to be dragged.
        /// </summary>
        private Point DragRelativeStartPosition
        {
            get
            {
                return this.GetFieldValue<GridViewHeaderRowPresenter, Point>("_relativeStartPos");
            }
        }

        /// <summary>
        /// Gets the index of the column of the header that is going to be dragged.
        /// </summary>
        private int DragStartColumnIndex
        {
            get
            {
                return this.GetFieldValue<GridViewHeaderRowPresenter, int>("_startColumnIndex");
            }
        }

        /// <summary>
        /// Gets the index of the destination column of the dragged header.
        /// </summary>
        private int DragDestColumnIndex
        {
            get
            {
                return this.GetFieldValue<GridViewHeaderRowPresenter, int>("_desColumnIndex");
            }
        }

        /// <summary>
        /// Gets the header positions list.
        /// </summary>
        private List<Rect> HeadersPositionList
        {
            get
            {
                return this.GetPropertyValue<GridViewHeaderRowPresenter, List<Rect>>("HeadersPositionList");
            }
        }

        /// <summary>
        /// Gets the list of currently reached max value of DesiredWidth of cell in the column
        /// </summary>
        private List<double> DesiredWidthList
        {
            get
            {
                return this.GetPropertyValue<GridViewHeaderRowPresenter, List<double>>("DesiredWidthList");
            }
        }

        /// <summary>
        /// Gets the padding header.
        /// </summary>
        private GridViewColumnHeader PaddingHeader
        {
            get
            {
                if (this.mPaddingHeader != null)
                {
                    return this.mPaddingHeader;
                }

                for (int i = 0; i < this.VisualChildrenCount; ++i)
                {
                    GridViewColumnHeader lHeader = this.GetVisualChild(i) as GridViewColumnHeader;
                    if (lHeader != null)
                    {
                        GridViewColumnHeaderRole lRole = (GridViewColumnHeaderRole)lHeader.GetValue(GridViewColumnHeader.RoleProperty);
                        if (lRole == GridViewColumnHeaderRole.Padding)
                        {
                            this.mPaddingHeader = lHeader;
                            return this.mPaddingHeader;
                        }
                    }
                }

                return null;
            }
        }

        /// <summary>
        /// Gets the floating header.
        /// </summary>
        private GridViewColumnHeader FloatingHeader
        {
            get
            {
                // Do not cache this header as it is modified at each drag.
                for (int i = 0; i < this.VisualChildrenCount; ++i)
                {
                    GridViewColumnHeader lHeader = this.GetVisualChild(i) as GridViewColumnHeader;
                    if (lHeader != null)
                    {
                        GridViewColumnHeaderRole lRole = (GridViewColumnHeaderRole)lHeader.GetValue(GridViewColumnHeader.RoleProperty);
                        if (lRole == GridViewColumnHeaderRole.Floating)
                        {
                            return lHeader;
                        }
                    }
                }

                return null;
            }
        }

        /// <summary>
        /// Gets the drag indicator.
        /// </summary>
        private Separator DragIndicator
        {
            get
            {
                if (this.mDragIndicator != null)
                {
                    return this.mDragIndicator;
                }

                for (int i = 0; i < this.VisualChildrenCount; ++i)
                {
                    Separator lIndicator = this.GetVisualChild(i) as Separator;
                    if (lIndicator != null)
                    {
                        this.mDragIndicator = lIndicator;
                        return this.mDragIndicator;
                    }
                }

                return null;
            }
        }

        #endregion // Properties.

        #region Methods

        /// <summary>
        /// Override of <seealso cref="FrameworkElement.MeasureOverride" />.
        /// </summary>
        /// <param name="pConstraint">Constraint size is an "upper limit" that the return value should not exceed.</param>
        /// <returns>The GridViewHeaderRowPresenter's desired size.</returns>
        protected override Size MeasureOverride(Size pConstraint)
        {
            double lMaxHeight = 0.0;
            double lAccumulatedWidth = 0.0;    
            double lConstraintHeight = pConstraint.Height;
            bool lDesiredWidthListEnsured = false;

            if (this.Columns != null)
            {
                // Measure working headers.
                for (int i = 0; i < this.Columns.Count; ++i)
                {
                    // Getting the column header visual element.
                    UIElement lColumnHeader = this.GetVisualChild(this.GetVisualIndex(i)) as UIElement;
                    if (lColumnHeader == null)
                    { 
                        continue; 
                    }

                    double lChildConstraintWidth = Math.Max(0.0, pConstraint.Width - lAccumulatedWidth);
                    ExtendedGridViewColumn lColumn = this.Columns[i] as ExtendedGridViewColumn;

                    if (lColumn.State == ExtendedGridViewColumnMeasureState.Init)
                    {
                        if (lDesiredWidthListEnsured == false)
                        {
                            this.EnsureDesiredWidthList();
                            this.LayoutUpdated += this.OnLayoutUpdated;
                            lDesiredWidthListEnsured = true;
                        }

                        lColumnHeader.Measure(new Size(lChildConstraintWidth, lConstraintHeight));
                        DesiredWidthList[lColumn.ActualIndex] = lColumn.EnsureDesiredWidth(lColumnHeader.DesiredSize.Width);
                        lAccumulatedWidth += lColumn.DesiredWidth;
                    }
                    else if (lColumn.State == ExtendedGridViewColumnMeasureState.Headered
                            || lColumn.State == ExtendedGridViewColumnMeasureState.Data)
                    {
                        //ScrollViewer lParentScrolViewer = this.FindVisualParent<ScrollViewer>();
                        //if (lParentScrolViewer.ViewportWidth > 0.0)
                        //{
                        //    lColumn.Width = lParentScrolViewer.ViewportWidth / DesiredWidthList.Count;
                        //}
                        lChildConstraintWidth = Math.Min(lChildConstraintWidth, lColumn.DesiredWidth);
                        lColumnHeader.Measure(new Size(lChildConstraintWidth, lConstraintHeight));
                        lAccumulatedWidth += lColumn.DesiredWidth;
                    }
                    else 
                    {
                        // ColumnMeasureState.SpecificWidth.
                        lChildConstraintWidth = Math.Min(lChildConstraintWidth, lColumn.Width);
                        lColumnHeader.Measure(new Size(lChildConstraintWidth, lConstraintHeight));
                        lAccumulatedWidth += lColumn.Width;
                    }

                    lMaxHeight = Math.Max(lMaxHeight, lColumnHeader.DesiredSize.Height);
                }
            }

            // Measure padding header.
            this.PaddingHeader.Measure(new Size(0.0, lConstraintHeight));
            lMaxHeight = Math.Max(lMaxHeight, this.PaddingHeader.DesiredSize.Height);

            // Reserve space for padding header next to the last column.
            lAccumulatedWidth += c_PaddingHeaderMinWidth;

            // Measure indicator & floating header in re-ordering.
            if (this.IsHeaderDragging)
            {
                // Measure indicator.
                this.DragIndicator.Measure(pConstraint);

                // Measure floating header.
                this.FloatingHeader.Measure(pConstraint);
            }

            return new Size(lAccumulatedWidth, lMaxHeight);
        }
        
        /// <summary>
        /// Delegate called when the layout is updated.
        /// </summary>
        /// <param name="pSender">The event sender.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private void OnLayoutUpdated(object pSender, EventArgs pEventArgs)
        {
            // Whether the shared minimum width has been changed since last layout.
            bool lDesiredWidthChanged = false; 

            if (this.Columns != null)
            {
                foreach (ExtendedGridViewColumn lColumn in this.Columns)
                {
                    if ((lColumn.State != ExtendedGridViewColumnMeasureState.SpecificWidth))
                    {
                        if (lColumn.State == ExtendedGridViewColumnMeasureState.Init)
                        {
                            lColumn.State = ExtendedGridViewColumnMeasureState.Headered;
                        }

                        if (this.DesiredWidthList == null || lColumn.ActualIndex >= this.DesiredWidthList.Count)
                        {
                            // How can this happen?
                            // Between the last measure was called and this update is called, there can be a
                            // change done to the ColumnCollection and result in DesiredWidthList out of sync
                            // with the columnn collection. What can we do is end this call asap and the next
                            // measure will fix it.
                            lDesiredWidthChanged = true;
                            break;
                        }

                        // TODO Modifying the DesiredWidth if the column is stretched.
                        //ScrollViewer lParentScrolViewer = this.FindVisualParent<ScrollViewer>();
                        //column.DesiredWidth = lParentScrolViewer.ViewportWidth / DesiredWidthList.Count;
                        //desiredWidthChanged = true;

                        if (Math.Abs(lColumn.DesiredWidth - this.DesiredWidthList[lColumn.ActualIndex]) > 0.001)
                        {
                            // Update the record because collection operation latter on might
                            // need to verified this list again, e.g. insert an 'auto'
                            // column, so that we won't trigger unnecessary update due to
                            // inconsistency of this column.
                            this.DesiredWidthList[lColumn.ActualIndex] = lColumn.DesiredWidth;

                            lDesiredWidthChanged = true;
                        }
                    }
                }
            }

            if (lDesiredWidthChanged)
            {
                this.InvalidateMeasure();
            }

            this.LayoutUpdated -= this.OnLayoutUpdated;
        }

        /// <summary>
        /// Computes the position of its children inside each child's Margin and calls Arrange on each child.
        /// </summary>
        /// <param name="pArrangeSize">Size the GridViewHeaderRowPresenter will assume.</param>
        protected override Size ArrangeOverride(Size pArrangeSize)
        {
            GridViewColumnCollection columns = Columns;

            double accumulatedWidth = 0.0;
            double remainingWidth = pArrangeSize.Width;
            Rect rect;

            HeadersPositionList.Clear();

            if (columns != null)
            {
                // Arrange working headers
                for (int i = 0; i < columns.Count; ++i)
                {
                    UIElement child = this.GetVisualChild(this.GetVisualIndex(i)) as UIElement;
                    if (child == null) { continue; }

                    ExtendedGridViewColumn column = columns[i] as ExtendedGridViewColumn;

                    // has a given value or 'auto'
                    double childArrangeWidth = Math.Min(remainingWidth, ((column.State == ExtendedGridViewColumnMeasureState.SpecificWidth) ? column.Width : column.DesiredWidth));

                    // calculate the header rect
                    rect = new Rect(accumulatedWidth, 0.0, childArrangeWidth, pArrangeSize.Height);

                    // arrange header
                    child.Arrange(rect);

                    //Store rect in HeadersPositionList as i-th column position
                    HeadersPositionList.Add(rect);

                    remainingWidth -= childArrangeWidth;
                    accumulatedWidth += childArrangeWidth;
                }

                // check width to hide previous header's right half gripper, from the first working header to padding header
                // only happens after column delete, insert, move
                //if (_isColumnChangedOrCreated)
                //{
                //    for (int i = 0; i < columns.Count; ++i)
                //    {
                //        GridViewColumnHeader header = children[GetVisualIndex(i)] as GridViewColumnHeader;

                //        header.CheckWidthForPreviousHeaderGripper();
                //    }

                //    _paddingHeader.CheckWidthForPreviousHeaderGripper();

                //    _isColumnChangedOrCreated = false;
                //}
            }

            // Arrange padding header
            Debug.Assert(this.PaddingHeader != null, "padding header is null");
            rect = new Rect(accumulatedWidth, 0.0, Math.Max(remainingWidth, 0.0), pArrangeSize.Height);
            this.PaddingHeader.Arrange(rect);
            this.HeadersPositionList.Add(rect);

            // if re-order started, arrange floating header & indicator
            if (this.IsHeaderDragging)
            {
                this.FloatingHeader.Arrange(new Rect(new Point(this.DragCurrentPosition.X - this.DragRelativeStartPosition.X, 0), this.HeadersPositionList[this.DragStartColumnIndex].Size));

                Point lPos = this.GetHeaderPositionByColumnIndex(this.DragDestColumnIndex);
                this.DragIndicator.Arrange(new Rect(lPos, new Size(this.DragIndicator.DesiredSize.Width, pArrangeSize.Height)));
            }

            return pArrangeSize;
        }

        /// <summary>
        /// Map column collection index to header collection index in visual tree.
        /// </summary>
        /// <param name="pColumnIndex">The column collection index.</param>
        /// <returns>The header collection index.</returns>
        private int GetVisualIndex(int pColumnIndex)
        {
            return this.CallMethod<GridViewHeaderRowPresenter, int>("GetVisualIndex", pColumnIndex);
        }

        /// <summary>
        /// Returns the header position by logic column index.
        /// </summary>
        /// <param name="pColumnIndex">The column index.</param>
        /// <returns>The corresponding header position.</returns>
        private Point GetHeaderPositionByColumnIndex(int pColumnIndex)
        {
            return this.CallMethod<GridViewHeaderRowPresenter, Point>("FindPositionByIndex", pColumnIndex);
        }

        /// <summary>
        /// Returns the header from the column.
        /// </summary>
        /// <param name="pColumn">The header column.</param>
        /// <returns>The header corresponding to the column.</returns>
        private GridViewColumnHeader GetHeaderByColumn(GridViewColumn pColumn)
        {
            for (int i = 0; i < this.VisualChildrenCount; ++i)
            {
                GridViewColumnHeader lChild = this.GetVisualChild(i) as GridViewColumnHeader;
                if (lChild != null && lChild.Column == pColumn)
                {
                    return lChild;
                }
            }

            return null;
        }

        /// <summary>
        /// Ensures ShareStateList have at least columns.Count items.
        /// </summary>
        private void EnsureDesiredWidthList()
        {
            this.CallMethod<GridViewHeaderRowPresenter>("EnsureDesiredWidthList");
        }

        #endregion // Methods.
    }
}

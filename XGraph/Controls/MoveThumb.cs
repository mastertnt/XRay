using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using XGraph.ViewModels;

namespace XGraph.Controls
{
    /// <summary>
    /// This control is used to move a node.
    /// </summary>
    public class MoveThumb : Thumb
    {
        #region Constructors

        /// <summary>
        /// Initializes the <see cref="MoveThumb"/> class.
        /// </summary>
        static MoveThumb()
        {
            MoveThumb.DefaultStyleKeyProperty.OverrideMetadata(typeof(MoveThumb), new FrameworkPropertyMetadata(typeof(MoveThumb)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MoveThumb"/> class.
        /// </summary>
        public MoveThumb()
        {
            this.DragDelta += this.OnDragDelta;
        }

        #endregion // Constructors.

        #region Methods

        /// <summary>
        /// This delegate is called when the item is dragged.
        /// </summary>
        /// <param name="pEventSender">The event sender.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private void OnDragDelta(Object pEventSender, DragDeltaEventArgs pEventArgs)
        {
            NodeViewModel lItem = this.DataContext as NodeViewModel;
            if (lItem == null)
            {
                return;
            }

            Double lLeft = lItem.X;
            Double lTop = lItem.Y;

            if
                (Double.IsNaN(lLeft))
            {
                lItem.X = 0;
            }
            else
            {
                lItem.X = lLeft + pEventArgs.HorizontalChange;
            }

            if
                (Double.IsNaN(lTop))
            {
                lItem.Y = 0;
            }
            else
            {
                lItem.Y = lTop + pEventArgs.VerticalChange;
            }
        }

        #endregion // Methods.
    }
}

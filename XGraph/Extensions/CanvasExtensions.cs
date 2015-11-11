using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using XGraph.Controls;

namespace XGraph.Extensions
{
    /// <summary>
    /// Class extending the <see cref="Canvas"/> class.
    /// </summary>
    public static class CanvasExtensions
    {
        /// <summary>
        /// Hits the control of the specified type in the given canvas at the given position.
        /// </summary>
        /// <typeparam name="TControlType">The control type.</typeparam>
        /// <param name="pThis">The canvas.</param>
        /// <param name="pSourcePoint">The hit source point.</param>
        /// <returns>The found control if any.</returns>
        public static TControlType HitControl<TControlType>(this Canvas pThis, Point pSourcePoint) where TControlType : DependencyObject
        {
            DependencyObject lHitObject = pThis.InputHitTest(pSourcePoint) as DependencyObject;
            if (lHitObject != null)
            {
                return lHitObject.FindVisualParent<TControlType>();    
            }
            return null;
        }
    }
}

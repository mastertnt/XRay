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
    static class CanvasExtensions
    {
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

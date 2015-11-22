using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using XGraph.Extensions;

namespace XGraph.Controls
{
    /// <summary>
    /// Class defining a canvas able to hit its contained adorner control.
    /// </summary>
    /// <!-- Damien Porte -->
    public class AdornerLayeredCanvas : Canvas
    {
        #region Properties

        /// <summary>
        /// Gets the layer 
        /// </summary>
        public AdornerLayer AdornerLayer
        {
            get;
            private set;
        }

        #endregion // Properties.

        #region Methods

        /// <summary>
        /// Delegate called when the control is initialized.
        /// </summary>
        /// <param name="pEventArgs">The event arguments.</param>
        protected override void OnInitialized(EventArgs pEventArgs)
        {
            this.AdornerLayer = AdornerLayer.GetAdornerLayer(this);
        }

        /// <summary>
        /// Hits the control of the specified type in the given canvas at the given position.
        /// </summary>
        /// <typeparam name="TControlType">The control type.</typeparam>
        /// <param name="pThis">The canvas.</param>
        /// <param name="pSourcePoint">The hit source point.</param>
        /// <returns>The found control if any.</returns>
        public TControlType HitControl<TControlType>(Point pSourcePoint) where TControlType : DependencyObject
        {
            TControlType lHitControl = null;

            // Trying to find the object threw the canvas visual children.
            DependencyObject lHitObject = this.InputHitTest(pSourcePoint) as DependencyObject;
            if (lHitObject != null && lHitObject != this)
            {
                if (lHitObject is TControlType)
                {
                    lHitControl = lHitObject as TControlType;
                }
                else
                {
                    lHitControl = lHitObject.FindVisualParent<TControlType>();
                }
            }

            if (lHitControl == null)
            {
                // Trying to find the control trew the adorner layer associated to this canvas.
                AdornerHitTestResult lResult = this.AdornerLayer.AdornerHitTest(pSourcePoint);
                if (lResult != null)
                {
                    lHitObject = lResult.VisualHit as DependencyObject;
                    if (lHitObject is TControlType)
                    {
                        lHitControl = lHitObject as TControlType;
                    }
                    else
                    {
                        lHitControl = lHitObject.FindVisualParent<TControlType>();
                    }
                }
            }

            return lHitControl;
        }

        #endregion // Methods.
    }
}

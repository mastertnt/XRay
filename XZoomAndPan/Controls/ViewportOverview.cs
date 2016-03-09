using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace XZoomAndPan.Controls
{
    /// <summary>
    /// Class defining the viewport overview displayed in the overview control.
    /// </summary>
    public class ViewportOverview : Thumb
    {
        #region Constructors

        /// <summary>
        /// Initializes the <see cref="ViewportOverview"/> class.
        /// </summary>
        static ViewportOverview()
        {
            ViewportOverview.DefaultStyleKeyProperty.OverrideMetadata(typeof(ViewportOverview), new FrameworkPropertyMetadata(typeof(ViewportOverview)));
        }

        #endregion // Constructors.
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace XGraph.ViewModels
{
    /// <summary>
    /// This object represents a graph item view model.
    /// </summary>
    public interface IGraphItemViewModel
    {
        /// <summary>
        /// Gets the style to apply to the container.
        /// </summary>
        Style ContainerStyle
        {
            get;
        }
    }
}

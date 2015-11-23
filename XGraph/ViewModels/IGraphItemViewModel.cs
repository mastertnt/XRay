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

        /// <summary>
        /// Gets or sets a value indicating whether this instance is selected.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is selected; otherwise, <c>false</c>.
        /// </value>
        bool IsSelected
        {
            get;
            set;
        }
    }
}

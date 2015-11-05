using PropertyChanged;

namespace XGraph.ViewModels
{
    [ImplementPropertyChanged]
    public class ConnectionViewModel
    {
        /// <summary>
        /// Gets or sets a value indicating whether this instance is selected.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is selected; otherwise, <c>false</c>.
        /// </value>
        public virtual bool IsSelected
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is active; otherwise, <c>false</c>.
        /// </value>
        public virtual bool IsActive
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the input.
        /// </summary>
        /// <value>
        /// The input.
        /// </value>
        public PortViewModel Input
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the output.
        /// </summary>
        /// <value>
        /// The output.
        /// </value>
        public PortViewModel Output
        {
            get; set;
        }
    }
}

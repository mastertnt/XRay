
namespace XCommand
{
    /// <summary>
    /// Interface defining an object having a tooltip.
    /// </summary>
    public interface ITooltipable
    {
        #region Properties

        /// <summary>
        /// Gets or sets the tooltip description.
        /// </summary>
        string Tooltip
        {
            get;
            set;
        }

        #endregion // Properties.
    }
}

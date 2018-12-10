using System.Windows;

namespace XControls.WatermarkTextBox
{
    /// <summary>
    /// Class defining a watermarked text box.
    /// </summary>
    public class WatermarkTextBox : AutoSelectTextBox.AutoSelectTextBox
    {
        #region Depenencies

        /// <summary>
        /// Identifies the Watermark property.
        /// </summary>
        public static readonly DependencyProperty WatermarkProperty = DependencyProperty.Register("Watermark", typeof(object), typeof(WatermarkTextBox), new UIPropertyMetadata(null));

        /// <summary>
        /// Identifies the WatermarkTemplate property.
        /// </summary>
        public static readonly DependencyProperty WatermarkTemplateProperty = DependencyProperty.Register("WatermarkTemplate", typeof(DataTemplate), typeof(WatermarkTextBox), new UIPropertyMetadata(null));
        
        #endregion // Dependencies.

        #region Properties

        /// <summary>
        /// Gets or sets the watermask.
        /// </summary>
        public object Watermark
        {
            get
            {
                return (object)this.GetValue(WatermarkProperty);
            }
            set
            {
                this.SetValue(WatermarkProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the watermask data template.
        /// </summary>
        public DataTemplate WatermarkTemplate
        {
            get
            {
                return (DataTemplate)this.GetValue(WatermarkTemplateProperty);
            }
            set
            {
                this.SetValue(WatermarkTemplateProperty, value);
            }
        }

        #endregion // Properties.

        #region Constructors

        /// <summary>
        /// Initializes the <see cref="WatermarkTextBox"/> class.
        /// </summary>
        static WatermarkTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WatermarkTextBox), new FrameworkPropertyMetadata(typeof(WatermarkTextBox)));
        }

        #endregion //Constructors
    }
}

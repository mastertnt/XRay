using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using XZoomAndPan.Converters;

namespace XZoomAndPan.Controls
{
    /// <summary>
    /// Defines the default toolbar of the zoom and pan control.
    /// </summary>
    public partial class DefaultToolbar : UserControl, IZoomAndPanToolbar
    {
        #region Fields

        /// <summary>
        /// Stores the used control.
        /// </summary>
        private AZoomAndPanControl mZoomAndPanControl;

        #endregion // Fields.

        #region Contructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultToolbar"/> class.
        /// </summary>
        public DefaultToolbar()
        {
            this.InitializeComponent();
        }

        #endregion // Contructors.

        #region Methods

        /// <summary>
        /// Binds the tool bar to the given zoom and pan control.
        /// </summary>
        /// <param name="pControl">The control to bind.</param>
        public void BindToControl(AZoomAndPanControl pControl)
        {
            if (pControl != null)
            {
                this.mZoomAndPanControl = pControl;

                // Binding the ContentScale property.
                Binding lTextContentScaleBinding = new Binding("ContentScale");
                lTextContentScaleBinding.Source = this.mZoomAndPanControl;
                lTextContentScaleBinding.Converter = new ScaleToPercentConverter();
                lTextContentScaleBinding.StringFormat = "{0} %";
                this.mPercentTextBlock.SetBinding(TextBlock.TextProperty, lTextContentScaleBinding);

                Binding lValueContentScaleBinding = new Binding("ContentScale");
                lValueContentScaleBinding.Source = this.mZoomAndPanControl;
                lValueContentScaleBinding.Converter = new ScaleToPercentConverter();
                this.mZoomSlider.SetBinding(Slider.ValueProperty, lValueContentScaleBinding);

                this.mFillButton.Click += this.OnFillButtonClicked;
                this.mOneHundredPercentButton.Click += this.OnOneHundredPercentButtonClicked;
                this.mZoomOutButton.Click += this.OnZoomOutButtonClicked;
                this.mZoomInButton.Click += this.OnZoomInButtonClicked;
            }
        }

        /// <summary>
        /// Delegate called when the zoom in button is clicked.
        /// </summary>
        /// <param name="pSender">The event sender.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private void OnZoomInButtonClicked(object pSender, RoutedEventArgs pEventArgs)
        {
            this.mZoomAndPanControl.ZoomIn(new Point(this.mZoomAndPanControl.ContentZoomFocusX, this.mZoomAndPanControl.ContentZoomFocusY));
        }

        /// <summary>
        /// Delegate called when the zoom out button is clicked.
        /// </summary>
        /// <param name="pSender">The event sender.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private void OnZoomOutButtonClicked(object pSender, RoutedEventArgs pEventArgs)
        {
            this.mZoomAndPanControl.ZoomOut(new Point(this.mZoomAndPanControl.ContentZoomFocusX, this.mZoomAndPanControl.ContentZoomFocusY));
        }

        /// <summary>
        /// Delegate called when the 100 % button is clicked.
        /// </summary>
        /// <param name="pSender">The event sender.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private void OnOneHundredPercentButtonClicked(object pSender, RoutedEventArgs pEventArgs)
        {
            this.mZoomAndPanControl.AnimatedZoomTo(1.0);
        }

        /// <summary>
        /// Delegate called when the fill button is clicked.
        /// </summary>
        /// <param name="pSender">The event sender.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private void OnFillButtonClicked(object pSender, RoutedEventArgs pEventArgs)
        {
            this.mZoomAndPanControl.AnimatedScaleToFit();
        }

        #endregion // Methods.
    }
}

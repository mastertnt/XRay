using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using PropertyChanged;
using XGraph.ViewModels;

namespace XGraph.Controls
{
    /// <summary>
    /// This class represents a connection.
    /// </summary>
    /// <!-- Nicolas Baudrey -->
    [ImplementPropertyChanged]
    public class Connection : ContentControl
    {
        #region Fields

        /// <summary>
        /// This field stores the input connector.
        /// </summary>
        private AConnector mInput = null;

        /// <summary>
        /// This field stores the output connector.
        /// </summary>
        private AConnector mOutput = null;

        /// <summary>
        /// This field stores the geometry to draw.
        /// </summary>
        private PathGeometry mDrawingGeometry;

        #endregion // Fields.

        #region Properties

        ///// <summary>
        ///// Gets or sets the input connector.
        ///// </summary>
        //public Connector Input
        //{
        //    get
        //    {
        //        return this.mInput;
        //    }
        //    set
        //    {
        //        //if
        //        //    (this.mInput != value)
        //        //{
        //        //    if
        //        //        (this.mInput != null)
        //        //    {
        //        //        this.mInput.PropertyChanged -= new PropertyChangedEventHandler(OnConnectorPropertyChanged);
        //        //        this.mInput.Connections.Remove(this);
        //        //    }

        //        //    this.mInput = value;

        //        //    if 
        //        //        (this.mInput != null)
        //        //    {
        //        //        this.mInput.Connections.Add(this);
        //        //        this.mInput.PropertyChanged += new PropertyChangedEventHandler(OnConnectorPropertyChanged);
        //        //    }

        //        //    this.OnPropertyChanged("Input");
        //        //    this.OnPropertyChanged("Position");
        //        //    this.UpdatePathGeometry();
        //        //}
        //    }
        //}

        ///// <summary>
        ///// Gets or sets the ouput connector.
        ///// </summary>
        //public Connector Output
        //{
        //    get
        //    {
        //        return this.mOutput;
        //    }
        //    set
        //    {
        //        //if 
        //        //    (this.mOutput != value)
        //        //{
        //        //    if 
        //        //        (this.mOutput != null)
        //        //    {
        //        //        this.mOutput.PropertyChanged -= new PropertyChangedEventHandler(OnConnectorPropertyChanged);
        //        //        this.mOutput.Connections.Remove(this);
        //        //    }

        //        //    this.mOutput = value;

        //        //    if 
        //        //        (this.mOutput != null)
        //        //    {
        //        //        this.mOutput.Connections.Add(this);
        //        //        this.mOutput.PropertyChanged += new PropertyChangedEventHandler(OnConnectorPropertyChanged);
        //        //    }

        //        //    this.OnPropertyChanged("Output");
        //        //    this.OnPropertyChanged("Position");
        //        //    this.UpdatePathGeometry();
        //        //}
        //    }
        //}

        /// <summary>
        /// Gets or sets the drawing geometry.
        /// </summary>
        public PathGeometry DrawingGeometry
        {
            get
            {
                return this.mDrawingGeometry;
            }
            set
            {
                this.mDrawingGeometry = value;
            }
        }

        #endregion // Properties.

        #region Constructors

        /// <summary>
        /// Static constructor.
        /// </summary>
        static Connection()
        {
            // set the key to reference the style for this control
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(Connection), new FrameworkPropertyMetadata(typeof(Connection)));
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Connection()
        {

        }

        #endregion // Constructors.

        #region Methods

        /// <summary>
        /// Method called when the control content changed.
        /// </summary>
        /// <param name="pOldContent">The previous content.</param>
        /// <param name="pNewContent">The new content.</param>
        protected override void OnContentChanged(object pOldContent, object pNewContent)
        {
            base.OnContentChanged(pOldContent, pNewContent);

            // The content is the view model.
            ConnectionViewModel lNewContent = pNewContent as ConnectionViewModel;
            if (lNewContent != null)
            {
                // Lets do the job!
            }
        }

        /// <summary>
        /// This method updates the final geometry for the path.
        /// </summary>
        /// <returns>The path geometry.</returns>
        private void UpdatePathGeometry()
        {
            //if
            //    (   (this.Input != null)
            //    &&  (this.Output != null)
            //    )
            //{
            //    if
            //        (this.mDrawingGeometry == null)
            //    {
            //        this.mDrawingGeometry = new PathGeometry();
            //    }
            //    List<Point> lPoints = this.Input.Position.GetShortestPath(this.Output.Position);
            //    this.mDrawingGeometry.Figures.Clear();
            //    PathFigure lFigure = new PathFigure();
            //    lFigure.StartPoint = this.Input.Position;
            //    lPoints.RemoveAt(0);
            //    lFigure.Segments.Add(new PolyLineSegment(lPoints, true));
            //    this.mDrawingGeometry.Figures.Add(lFigure);
            //}
        }

        #endregion // Methods.
    }
}

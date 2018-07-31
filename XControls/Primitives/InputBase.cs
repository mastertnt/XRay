using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace XControls.Primitives
{
    /// <summary>
    /// Class defining an input base class.
    /// </summary>
    public abstract class InputBase : Control
    {
        #region Dependencies

        /// <summary>
        /// Identifies the IsPresenter property.
        /// </summary>
        public static readonly DependencyProperty IsPresenterProperty = DependencyProperty.Register("IsPresenter", typeof(bool), typeof(InputBase), new UIPropertyMetadata(false));
        
        #endregion // Dependencies.

        #region Properties

        /// <summary>
        /// Gets or sets the flag indicating if the input just has to display the value.
        /// </summary>
        public bool IsPresenter
        {
            get
            {
                return (bool) this.GetValue(IsPresenterProperty);
            }
            set
            {
                this.SetValue(IsPresenterProperty, value);
            }
        }

        #endregion // Properties.

        #region AllowTextInput

        /// <summary>
        /// The allow text input property
        /// </summary>
        public static readonly DependencyProperty AllowTextInputProperty = DependencyProperty.Register( "AllowTextInput", typeof( bool ), typeof( InputBase ), new UIPropertyMetadata( true, OnAllowTextInputChanged ) );
        /// <summary>
        /// Gets or sets a value indicating whether [allow text input].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [allow text input]; otherwise, <c>false</c>.
        /// </value>
        public bool AllowTextInput
        {
            get
            {
            return ( bool ) this.GetValue( AllowTextInputProperty );
            }
            set
            {
                this.SetValue( AllowTextInputProperty, value );
            }
        }

        /// <summary>
        /// Called when [allow text input changed].
        /// </summary>
        /// <param name="o">The o.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnAllowTextInputChanged( DependencyObject o, DependencyPropertyChangedEventArgs e )
        {
            InputBase inputBase = o as InputBase;
            if( inputBase != null )
            inputBase.OnAllowTextInputChanged( ( bool )e.OldValue, ( bool )e.NewValue );
        }

        /// <summary>
        /// Called when [allow text input changed].
        /// </summary>
        /// <param name="oldValue">if set to <c>true</c> [old value].</param>
        /// <param name="newValue">if set to <c>true</c> [new value].</param>
        protected virtual void OnAllowTextInputChanged( bool oldValue, bool newValue )
        {
        }

        #endregion //AllowTextInput

        #region CultureInfo

        /// <summary>
        /// The culture information property
        /// </summary>
        public static readonly DependencyProperty CultureInfoProperty = DependencyProperty.Register("CultureInfo", typeof(CultureInfo), typeof(InputBase), new UIPropertyMetadata(System.Threading.Thread.CurrentThread.CurrentUICulture, OnCultureInfoChanged));
        /// <summary>
        /// Gets or sets the culture information.
        /// </summary>
        /// <value>
        /// The culture information.
        /// </value>
        public CultureInfo CultureInfo
        {
            get
            {
            return ( CultureInfo ) this.GetValue( CultureInfoProperty );
            }
            set
            {
                this.SetValue( CultureInfoProperty, value );
            }
        }

        /// <summary>
        /// Called when [culture information changed].
        /// </summary>
        /// <param name="o">The o.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnCultureInfoChanged( DependencyObject o, DependencyPropertyChangedEventArgs e )
        {
            InputBase inputBase = o as InputBase;
            if( inputBase != null )
            inputBase.OnCultureInfoChanged( ( CultureInfo )e.OldValue, ( CultureInfo )e.NewValue );
        }

        /// <summary>
        /// Called when [culture information changed].
        /// </summary>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        protected virtual void OnCultureInfoChanged( CultureInfo oldValue, CultureInfo newValue )
        {

        }

        #endregion //CultureInfo

        #region IsReadOnly

        /// <summary>
        /// The is read only property
        /// </summary>
        public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register( "IsReadOnly", typeof( bool ), typeof( InputBase ), new UIPropertyMetadata( false, OnReadOnlyChanged ) );
        /// <summary>
        /// Gets or sets a value indicating whether this instance is read only.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is read only; otherwise, <c>false</c>.
        /// </value>
        public bool IsReadOnly
        {
            get
            {
            return ( bool ) this.GetValue( IsReadOnlyProperty );
            }
            set
            {
                this.SetValue( IsReadOnlyProperty, value );
            }
        }

        private static void OnReadOnlyChanged( DependencyObject o, DependencyPropertyChangedEventArgs e )
        {
            InputBase inputBase = o as InputBase;
            if( inputBase != null )
            inputBase.OnReadOnlyChanged( ( bool )e.OldValue, ( bool )e.NewValue );
        }

        /// <summary>
        /// Called when [read only changed].
        /// </summary>
        /// <param name="oldValue">if set to <c>true</c> [old value].</param>
        /// <param name="newValue">if set to <c>true</c> [new value].</param>
        protected virtual void OnReadOnlyChanged( bool oldValue, bool newValue )
        {
        }

        #endregion //IsReadOnly

        #region IsUndoEnabled

        /// <summary>
        /// The is undo enabled property
        /// </summary>
        public static readonly DependencyProperty IsUndoEnabledProperty = DependencyProperty.Register( "IsUndoEnabled", typeof( bool ), typeof( InputBase ), new UIPropertyMetadata( true, OnIsUndoEnabledChanged ) );
        /// <summary>
        /// Gets or sets a value indicating whether this instance is undo enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is undo enabled; otherwise, <c>false</c>.
        /// </value>
        public bool IsUndoEnabled
        {
            get
            {
            return ( bool ) this.GetValue( IsUndoEnabledProperty );
            }
            set
            {
                this.SetValue( IsUndoEnabledProperty, value );
            }
        }

        private static void OnIsUndoEnabledChanged( DependencyObject o, DependencyPropertyChangedEventArgs e )
        {
            InputBase inputBase = o as InputBase;
            if( inputBase != null )
            inputBase.OnIsUndoEnabledChanged( ( bool )e.OldValue, ( bool )e.NewValue );
        }

        /// <summary>
        /// Called when [is undo enabled changed].
        /// </summary>
        /// <param name="oldValue">if set to <c>true</c> [old value].</param>
        /// <param name="newValue">if set to <c>true</c> [new value].</param>
        protected virtual void OnIsUndoEnabledChanged( bool oldValue, bool newValue )
        {
        }

        #endregion //IsUndoEnabled

        #region Text

        /// <summary>
        /// The text property
        /// </summary>
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(InputBase), new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnTextChanged, OnCoerceText, false, UpdateSourceTrigger.LostFocus));
        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>
        /// The text.
        /// </value>
        public string Text
        {
            get
            {
            return ( string ) this.GetValue( TextProperty );
            }
            set
            {
                this.SetValue( TextProperty, value );
            }
        }

            /// <summary>
            /// Coerces the text input.
            /// </summary>
            /// <param name="pObject">The modified object.</param>
            /// <param name="pBaseValue">The value to cerce.</param>
            /// <returns>The new value.</returns>
            private static object OnCoerceText(DependencyObject pObject, object pBaseValue)
            {
                InputBase lControl = pObject as InputBase;
                if (lControl != null)
                {
                    return lControl.CoerceText(pBaseValue as string);
                }

                return pBaseValue;
            }

            /// <summary>
            /// Delegate called when the text changed.
            /// </summary>
            /// <param name="pObject">The modified object.</param>
            /// <param name="pEventArgs">The event arguments.</param>
            private static void OnTextChanged(DependencyObject pObject, DependencyPropertyChangedEventArgs pEventArgs)
            {
                InputBase lControl = pObject as InputBase;
                if (lControl != null)
                {
                    lControl.OnTextChanged((string)pEventArgs.OldValue, (string)pEventArgs.NewValue);
                }
            }

            /// <summary>
            /// Coerce the text entered by the user.
            /// </summary>
            /// <param name="pBaseText">The text to coerce.</param>
            /// <returns>The coerced text.</returns>
            protected virtual string CoerceText(string pBaseText)
            {
                return pBaseText;
            }

        /// <summary>
        /// Called when [text changed].
        /// </summary>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        protected virtual void OnTextChanged( string oldValue, string newValue )
        {
            this.RaiseTextChangedEvent(oldValue, newValue);
        }

        #endregion //Text

        #region ValueChanged Event

        /// <summary>
        /// The text changed event
        /// </summary>
        public static readonly RoutedEvent TextChangedEvent = EventManager.RegisterRoutedEvent("TextChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<string>), typeof(InputBase));
        /// <summary>
        /// Occurs when [text changed].
        /// </summary>
        public event RoutedPropertyChangedEventHandler<string> TextChanged
        {
            add
            {
                this.AddHandler(TextChangedEvent, value);
            }
            remove
            {
                this.RemoveHandler(TextChangedEvent, value);
            }
        }


        /// <summary>
        /// Raises the text changed event.
        /// </summary>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        protected virtual void RaiseTextChangedEvent(string oldValue, string newValue)
        {
            RoutedPropertyChangedEventArgs<string> args = new RoutedPropertyChangedEventArgs<string>(oldValue, newValue);
            args.RoutedEvent = TextChangedEvent;
            this.RaiseEvent(args);
        }

        #endregion


        #region TextAlignment

        /// <summary>
        /// The text alignment property
        /// </summary>
        public static readonly DependencyProperty TextAlignmentProperty = DependencyProperty.Register( "TextAlignment", typeof( TextAlignment ), typeof( InputBase ), new UIPropertyMetadata( TextAlignment.Left ) );
        /// <summary>
        /// Gets or sets the text alignment.
        /// </summary>
        /// <value>
        /// The text alignment.
        /// </value>
        public TextAlignment TextAlignment
        {
            get
            {
            return ( TextAlignment ) this.GetValue( TextAlignmentProperty );
            }
            set
            {
                this.SetValue( TextAlignmentProperty, value );
            }
        }


        #endregion //TextAlignment

        #region Watermark

        /// <summary>
        /// The watermark property
        /// </summary>
        public static readonly DependencyProperty WatermarkProperty = DependencyProperty.Register("Watermark", typeof(object), typeof(InputBase), new UIPropertyMetadata(null, null, OnCoerceWatermark));
        /// <summary>
        /// Gets or sets the watermark.
        /// </summary>
        /// <value>
        /// The watermark.
        /// </value>
        public object Watermark
        {
            get
            {
            return ( object ) this.GetValue( WatermarkProperty );
            }
            set
            {
                this.SetValue( WatermarkProperty, value );
            }
        }

        /// <summary>
        /// Delegate called when the watermark is coerced.
        /// </summary>
        /// <param name="pObject">The modified control.</param>
        /// <param name="pBaseValue">The value to coerce.</param>
        /// <returns>The coerced value.</returns>
        private static object OnCoerceWatermark(DependencyObject pObject, object pBaseValue)
        {
            InputBase lInputBase = pObject as InputBase;
            if (lInputBase != null)
            {
                return lInputBase.OnCoerceWatermark((string)pBaseValue);
            }

            return pBaseValue;
        }

        /// <summary>
        /// Coerce the Watermark.
        /// </summary>
        /// <param name="pBaseValue">The Watermark to coerce.</param>
        /// <returns>The coerced Watermark.</returns>
        protected virtual string OnCoerceWatermark(string pBaseValue)
        {
            // Nothing to do by default.
            return pBaseValue;
        }

        #endregion //Watermark

        #region WatermarkTemplate

        public static readonly DependencyProperty WatermarkTemplateProperty = DependencyProperty.Register( "WatermarkTemplate", typeof( DataTemplate ), typeof( InputBase ), new UIPropertyMetadata( null ) );
        /// <summary>
        /// Gets or sets the watermark template.
        /// </summary>
        /// <value>
        /// The watermark template.
        /// </value>
        public DataTemplate WatermarkTemplate
        {
            get
            {
            return ( DataTemplate ) this.GetValue( WatermarkTemplateProperty );
            }
            set
            {
                this.SetValue( WatermarkTemplateProperty, value );
            }
        }

        #endregion //WatermarkTemplate
    }
}

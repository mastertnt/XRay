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

        public static readonly DependencyProperty AllowTextInputProperty = DependencyProperty.Register( "AllowTextInput", typeof( bool ), typeof( InputBase ), new UIPropertyMetadata( true, OnAllowTextInputChanged ) );
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

        private static void OnAllowTextInputChanged( DependencyObject o, DependencyPropertyChangedEventArgs e )
        {
            InputBase inputBase = o as InputBase;
            if( inputBase != null )
            inputBase.OnAllowTextInputChanged( ( bool )e.OldValue, ( bool )e.NewValue );
        }

        protected virtual void OnAllowTextInputChanged( bool oldValue, bool newValue )
        {
        }

        #endregion //AllowTextInput

        #region CultureInfo

        public static readonly DependencyProperty CultureInfoProperty = DependencyProperty.Register("CultureInfo", typeof(CultureInfo), typeof(InputBase), new UIPropertyMetadata(System.Threading.Thread.CurrentThread.CurrentUICulture, OnCultureInfoChanged));
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

        private static void OnCultureInfoChanged( DependencyObject o, DependencyPropertyChangedEventArgs e )
        {
            InputBase inputBase = o as InputBase;
            if( inputBase != null )
            inputBase.OnCultureInfoChanged( ( CultureInfo )e.OldValue, ( CultureInfo )e.NewValue );
        }

        protected virtual void OnCultureInfoChanged( CultureInfo oldValue, CultureInfo newValue )
        {

        }

        #endregion //CultureInfo

        #region IsReadOnly

        public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register( "IsReadOnly", typeof( bool ), typeof( InputBase ), new UIPropertyMetadata( false, OnReadOnlyChanged ) );
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

        protected virtual void OnReadOnlyChanged( bool oldValue, bool newValue )
        {
        }

        #endregion //IsReadOnly

        #region IsUndoEnabled

        public static readonly DependencyProperty IsUndoEnabledProperty = DependencyProperty.Register( "IsUndoEnabled", typeof( bool ), typeof( InputBase ), new UIPropertyMetadata( true, OnIsUndoEnabledChanged ) );
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

        protected virtual void OnIsUndoEnabledChanged( bool oldValue, bool newValue )
        {
        }

        #endregion //IsUndoEnabled

        #region Text

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(InputBase), new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnTextChanged, OnCoerceText, false, UpdateSourceTrigger.LostFocus));
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

        protected virtual void OnTextChanged( string oldValue, string newValue )
        {
            this.RaiseTextChangedEvent(oldValue, newValue);
        }

        #endregion //Text

        #region ValueChanged Event

        public static readonly RoutedEvent TextChangedEvent = EventManager.RegisterRoutedEvent("TextChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<string>), typeof(InputBase));
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


        protected virtual void RaiseTextChangedEvent(string oldValue, string newValue)
        {
            RoutedPropertyChangedEventArgs<string> args = new RoutedPropertyChangedEventArgs<string>(oldValue, newValue);
            args.RoutedEvent = TextChangedEvent;
            this.RaiseEvent(args);
        }

        #endregion


        #region TextAlignment

        public static readonly DependencyProperty TextAlignmentProperty = DependencyProperty.Register( "TextAlignment", typeof( TextAlignment ), typeof( InputBase ), new UIPropertyMetadata( TextAlignment.Left ) );
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

        public static readonly DependencyProperty WatermarkProperty = DependencyProperty.Register("Watermark", typeof(object), typeof(InputBase), new UIPropertyMetadata(null, null, OnCoerceWatermark));
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

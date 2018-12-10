using System;
using System.Windows;
using System.Windows.Controls;

namespace XControls.Themes.Chromes
{
  public class ButtonChromeRenderer : ContentControl
  {
    #region CornerRadius

    public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register( "CornerRadius", typeof( CornerRadius ), typeof( ButtonChromeRenderer ), new UIPropertyMetadata( default( CornerRadius ), new PropertyChangedCallback( OnCornerRadiusChanged ) ) );
    public CornerRadius CornerRadius
    {
      get
      {
        return ( CornerRadius ) this.GetValue(CornerRadiusProperty);
      }
      set
      {
          this.SetValue(CornerRadiusProperty, value );
      }
    }

    private static void OnCornerRadiusChanged( DependencyObject o, DependencyPropertyChangedEventArgs e )
    {
      ButtonChromeRenderer buttonChrome = o as ButtonChromeRenderer;
      if( buttonChrome != null )
        buttonChrome.OnCornerRadiusChanged( ( CornerRadius )e.OldValue, ( CornerRadius )e.NewValue );
    }

    protected virtual void OnCornerRadiusChanged( CornerRadius oldValue, CornerRadius newValue )
    {
      //we always want the InnerBorderRadius to be one less than the CornerRadius
      CornerRadius newInnerCornerRadius = new CornerRadius( Math.Max( 0, newValue.TopLeft - 1 ),
                                                           Math.Max( 0, newValue.TopRight - 1 ),
                                                           Math.Max( 0, newValue.BottomRight - 1 ),
                                                           Math.Max( 0, newValue.BottomLeft - 1 ) );

        this.InnerCornerRadius = newInnerCornerRadius;
    }

    #endregion //CornerRadius

    #region InnerCornerRadius

    public static readonly DependencyProperty InnerCornerRadiusProperty = DependencyProperty.Register( "InnerCornerRadius", typeof( CornerRadius ), typeof( ButtonChromeRenderer ), new UIPropertyMetadata( default( CornerRadius ), new PropertyChangedCallback( OnInnerCornerRadiusChanged ) ) );
    public CornerRadius InnerCornerRadius
    {
      get
      {
        return ( CornerRadius ) this.GetValue( InnerCornerRadiusProperty );
      }
      set
      {
          this.SetValue( InnerCornerRadiusProperty, value );
      }
    }

    private static void OnInnerCornerRadiusChanged( DependencyObject o, DependencyPropertyChangedEventArgs e )
    {
      ButtonChromeRenderer buttonChrome = o as ButtonChromeRenderer;
      if( buttonChrome != null )
        buttonChrome.OnInnerCornerRadiusChanged( ( CornerRadius )e.OldValue, ( CornerRadius )e.NewValue );
    }

    protected virtual void OnInnerCornerRadiusChanged( CornerRadius oldValue, CornerRadius newValue )
    {
      // TODO: Add your property changed side-effects. Descendants can override as well.
    }

    #endregion //InnerCornerRadius

    #region RenderChecked

    public static readonly DependencyProperty RenderCheckedProperty = DependencyProperty.Register( "RenderChecked", typeof( bool ), typeof( ButtonChromeRenderer ), new UIPropertyMetadata( false, OnRenderCheckedChanged ) );
    public bool RenderChecked
    {
      get
      {
        return ( bool ) this.GetValue( RenderCheckedProperty );
      }
      set
      {
          this.SetValue( RenderCheckedProperty, value );
      }
    }

    private static void OnRenderCheckedChanged( DependencyObject o, DependencyPropertyChangedEventArgs e )
    {
      ButtonChromeRenderer buttonChrome = o as ButtonChromeRenderer;
      if( buttonChrome != null )
        buttonChrome.OnRenderCheckedChanged( ( bool )e.OldValue, ( bool )e.NewValue );
    }

    protected virtual void OnRenderCheckedChanged( bool oldValue, bool newValue )
    {
      // TODO: Add your property changed side-effects. Descendants can override as well.
    }

    #endregion //RenderChecked

    #region RenderEnabled

    public static readonly DependencyProperty RenderEnabledProperty = DependencyProperty.Register( "RenderEnabled", typeof( bool ), typeof( ButtonChromeRenderer ), new UIPropertyMetadata( true, OnRenderEnabledChanged ) );
    public bool RenderEnabled
    {
      get
      {
        return ( bool ) this.GetValue( RenderEnabledProperty );
      }
      set
      {
          this.SetValue( RenderEnabledProperty, value );
      }
    }

    private static void OnRenderEnabledChanged( DependencyObject o, DependencyPropertyChangedEventArgs e )
    {
      ButtonChromeRenderer buttonChrome = o as ButtonChromeRenderer;
      if( buttonChrome != null )
        buttonChrome.OnRenderEnabledChanged( ( bool )e.OldValue, ( bool )e.NewValue );
    }

    protected virtual void OnRenderEnabledChanged( bool oldValue, bool newValue )
    {
      // TODO: Add your property changed side-effects. Descendants can override as well.
    }

    #endregion //RenderEnabled

    #region RenderFocused

    public static readonly DependencyProperty RenderFocusedProperty = DependencyProperty.Register( "RenderFocused", typeof( bool ), typeof( ButtonChromeRenderer ), new UIPropertyMetadata( false, OnRenderFocusedChanged ) );
    public bool RenderFocused
    {
      get
      {
        return ( bool ) this.GetValue( RenderFocusedProperty );
      }
      set
      {
          this.SetValue( RenderFocusedProperty, value );
      }
    }

    private static void OnRenderFocusedChanged( DependencyObject o, DependencyPropertyChangedEventArgs e )
    {
      ButtonChromeRenderer buttonChrome = o as ButtonChromeRenderer;
      if( buttonChrome != null )
        buttonChrome.OnRenderFocusedChanged( ( bool )e.OldValue, ( bool )e.NewValue );
    }

    protected virtual void OnRenderFocusedChanged( bool oldValue, bool newValue )
    {
      // TODO: Add your property changed side-effects. Descendants can override as well.
    }

    #endregion //RenderFocused

    #region RenderMouseOver

    public static readonly DependencyProperty RenderMouseOverProperty = DependencyProperty.Register( "RenderMouseOver", typeof( bool ), typeof( ButtonChromeRenderer ), new UIPropertyMetadata( false, OnRenderMouseOverChanged ) );
    public bool RenderMouseOver
    {
      get
      {
        return ( bool ) this.GetValue( RenderMouseOverProperty );
      }
      set
      {
          this.SetValue( RenderMouseOverProperty, value );
      }
    }

    private static void OnRenderMouseOverChanged( DependencyObject o, DependencyPropertyChangedEventArgs e )
    {
      ButtonChromeRenderer buttonChrome = o as ButtonChromeRenderer;
      if( buttonChrome != null )
        buttonChrome.OnRenderMouseOverChanged( ( bool )e.OldValue, ( bool )e.NewValue );
    }

    protected virtual void OnRenderMouseOverChanged( bool oldValue, bool newValue )
    {
      // TODO: Add your property changed side-effects. Descendants can override as well.
    }

    #endregion //RenderMouseOver

    #region RenderNormal

    public static readonly DependencyProperty RenderNormalProperty = DependencyProperty.Register( "RenderNormal", typeof( bool ), typeof( ButtonChromeRenderer ), new UIPropertyMetadata( true, OnRenderNormalChanged ) );
    public bool RenderNormal
    {
      get
      {
        return ( bool ) this.GetValue( RenderNormalProperty );
      }
      set
      {
          this.SetValue( RenderNormalProperty, value );
      }
    }

    private static void OnRenderNormalChanged( DependencyObject o, DependencyPropertyChangedEventArgs e )
    {
      ButtonChromeRenderer buttonChrome = o as ButtonChromeRenderer;
      if( buttonChrome != null )
        buttonChrome.OnRenderNormalChanged( ( bool )e.OldValue, ( bool )e.NewValue );
    }

    protected virtual void OnRenderNormalChanged( bool oldValue, bool newValue )
    {
      // TODO: Add your property changed side-effects. Descendants can override as well.
    }

    #endregion //RenderNormal

    #region RenderPressed

    public static readonly DependencyProperty RenderPressedProperty = DependencyProperty.Register( "RenderPressed", typeof( bool ), typeof( ButtonChromeRenderer ), new UIPropertyMetadata( false, OnRenderPressedChanged ) );
    public bool RenderPressed
    {
      get
      {
        return ( bool ) this.GetValue( RenderPressedProperty );
      }
      set
      {
          this.SetValue( RenderPressedProperty, value );
      }
    }

    private static void OnRenderPressedChanged( DependencyObject o, DependencyPropertyChangedEventArgs e )
    {
      ButtonChromeRenderer buttonChrome = o as ButtonChromeRenderer;
      if( buttonChrome != null )
        buttonChrome.OnRenderPressedChanged( ( bool )e.OldValue, ( bool )e.NewValue );
    }

    protected virtual void OnRenderPressedChanged( bool oldValue, bool newValue )
    {
      // TODO: Add your property changed side-effects. Descendants can override as well.
    }

    #endregion //RenderPressed

    #region Contsructors

    static ButtonChromeRenderer()
    {
      DefaultStyleKeyProperty.OverrideMetadata( typeof( ButtonChromeRenderer ), new FrameworkPropertyMetadata( typeof( ButtonChromeRenderer ) ) );
    }

    #endregion //Contsructors
  }
}

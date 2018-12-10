using System;
using System.ComponentModel;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using XControls.Core.Utilities;

namespace XControls.DropDownButton
{
  [TemplatePart(Name = PART_DROP_DOWN_BUTTON, Type = typeof(ToggleButton))]
  [TemplatePart(Name = PART_CONTENT_PRESENTER, Type = typeof(ContentPresenter))]
  [TemplatePart(Name = PART_POPUP, Type = typeof(Popup))]
  public class DropDownButton : ContentControl, ICommandSource
  {
    private const string PART_DROP_DOWN_BUTTON = "PART_DropDownButton";
    private const string PART_CONTENT_PRESENTER = "PART_ContentPresenter";
    private const string PART_POPUP = "PART_Popup";

    #region Members 

    private ContentPresenter mContentPresenter;
    private Popup mPopup;

    /// <summary>
    /// Stores the timer used to auto hide the popup.
    /// </summary>
    private Timer mVisibilityTimer;

    #endregion

    #region Constructors

    static DropDownButton()
    {
      DefaultStyleKeyProperty.OverrideMetadata( typeof( DropDownButton ), new FrameworkPropertyMetadata( typeof( DropDownButton ) ) );
    }

    public DropDownButton()
    {
      Keyboard.AddKeyDownHandler( this, this.OnKeyDown );
      Mouse.AddPreviewMouseDownOutsideCapturedElementHandler( this, this.OnMouseDownOutsideCapturedElement );

      this.mVisibilityTimer = new Timer();
      this.mVisibilityTimer.Elapsed += this.OnVisibilityTimerElapsed;
      this.mVisibilityTimer.AutoReset = false;
    }

    #endregion //Constructors

    #region Properties



    private ButtonBase mButton;
    protected ButtonBase Button
    {
      get
      {
        return this.mButton;
      }
      set
      {
        if(this.mButton != null ) this.mButton.Click -= this.DropDownButton_Click;

          this.mButton = value;

        if(this.mButton != null ) this.mButton.Click += this.DropDownButton_Click;
      }
    }

    #region DropDownContent

    public static readonly DependencyProperty DropDownVisibilityDelayProperty = DependencyProperty.Register("DropDownVisibilityDelay", typeof(double), typeof(DropDownButton), new UIPropertyMetadata(0.0, OnDropDownVisibilityDelayChanged));
    
    /// <summary>
    /// Gets or sets the delay in seconds.
    /// </summary>
    public double? DropDownVisibilityDelay
    {
        get
        {
            return (double?) this.GetValue(DropDownVisibilityDelayProperty);
        }
        set
        {
            this.SetValue(DropDownVisibilityDelayProperty, value);
        }
    }

    private static void OnDropDownVisibilityDelayChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
    {
        DropDownButton lDropDownButton = o as DropDownButton;
        double lValue = (double)e.NewValue;
        if (lDropDownButton != null)
        {
            if (lValue <= 0.0)
            {
                lDropDownButton.mVisibilityTimer.Stop();
            }
            else
            {
                lDropDownButton.mVisibilityTimer.Interval = lValue * 1000.0;
            }
        }
    }

    public static readonly DependencyProperty DropDownPlacementModeProperty = DependencyProperty.Register("DropDownPlacementMode", typeof(PlacementMode), typeof(DropDownButton), new UIPropertyMetadata(PlacementMode.Bottom));
    public PlacementMode DropDownPlacementMode
    {
        get
        {
            return (PlacementMode) this.GetValue(DropDownPlacementModeProperty);
        }
        set
        {
            this.SetValue(DropDownPlacementModeProperty, value);
        }
    }

    public static readonly DependencyProperty DropDownContentProperty = DependencyProperty.Register( "DropDownContent", typeof( object ), typeof( DropDownButton ), new UIPropertyMetadata( null, OnDropDownContentChanged ) );
    public object DropDownContent
    {
      get
      {
        return ( object ) this.GetValue( DropDownContentProperty );
      }
      set
      {
          this.SetValue( DropDownContentProperty, value );
      }
    }

    private static void OnDropDownContentChanged( DependencyObject o, DependencyPropertyChangedEventArgs e )
    {
      DropDownButton dropDownButton = o as DropDownButton;
      if( dropDownButton != null )
        dropDownButton.OnDropDownContentChanged( ( object )e.OldValue, ( object )e.NewValue );
    }

    protected virtual void OnDropDownContentChanged( object oldValue, object newValue )
    {
      // TODO: Add your property changed side-effects. Descendants can override as well.
    }

    #endregion //DropDownContent

    #region IsOpen

    public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register( "IsOpen", typeof( bool ), typeof( DropDownButton ), new UIPropertyMetadata( false, OnIsOpenChanged ) );
    public bool IsOpen
    {
      get
      {
        return ( bool ) this.GetValue( IsOpenProperty );
      }
      set
      {
          this.SetValue( IsOpenProperty, value );
      }
    }

    private static void OnIsOpenChanged( DependencyObject o, DependencyPropertyChangedEventArgs e )
    {
      DropDownButton dropDownButton = o as DropDownButton;
      if( dropDownButton != null )
        dropDownButton.OnIsOpenChanged( ( bool )e.OldValue, ( bool )e.NewValue );
    }

    protected virtual void OnIsOpenChanged( bool pOldValue, bool pNewValue)
    {
        if (pNewValue)
        {
            this.RaiseRoutedEvent(OpenedEvent);

            IUpdateable lUpdateableContent = this.DropDownContent as IUpdateable;
            if (lUpdateableContent != null)
            {
                lUpdateableContent.Update();
            }

            if (this.DropDownVisibilityDelay > 0.0)
            {
                this.mVisibilityTimer.Start();
            }
        }
        else
        {
            this.mVisibilityTimer.Stop();
            this.RaiseRoutedEvent(ClosedEvent);
        }
    }


    private void OnVisibilityTimerElapsed(object pSender, ElapsedEventArgs pEventArgs)
    {
        this.Dispatcher.Invoke((Action)delegate
        {
            this.CloseDropDown(true);
        });
    }

    #endregion //IsOpen

    #endregion //Properties

    #region Base Class Overrides

    public override void OnApplyTemplate()
    {
      base.OnApplyTemplate();
        this.Button = this.GetTemplateChild( PART_DROP_DOWN_BUTTON ) as ToggleButton;

        this.mContentPresenter = this.GetTemplateChild( PART_CONTENT_PRESENTER ) as ContentPresenter;

      if(this.mPopup != null ) this.mPopup.Opened -= this.Popup_Opened;

        this.mPopup = this.GetTemplateChild( PART_POPUP ) as Popup;

      if(this.mPopup != null ) this.mPopup.Opened += this.Popup_Opened;
    }

    #endregion //Base Class Overrides

    #region Events

    public static readonly RoutedEvent ClickEvent = EventManager.RegisterRoutedEvent( "Click", RoutingStrategy.Bubble, typeof( RoutedEventHandler ), typeof( DropDownButton ) );
    public event RoutedEventHandler Click
    {
      add
      {
          this.AddHandler( ClickEvent, value );
      }
      remove
      {
          this.RemoveHandler( ClickEvent, value );
      }
    }

    public static readonly RoutedEvent OpenedEvent = EventManager.RegisterRoutedEvent( "Opened", RoutingStrategy.Bubble, typeof( RoutedEventHandler ), typeof( DropDownButton ) );
    public event RoutedEventHandler Opened
    {
      add
      {
          this.AddHandler( OpenedEvent, value );
      }
      remove
      {
          this.RemoveHandler( OpenedEvent, value );
      }
    }

    public static readonly RoutedEvent ClosedEvent = EventManager.RegisterRoutedEvent( "Closed", RoutingStrategy.Bubble, typeof( RoutedEventHandler ), typeof( DropDownButton ) );
    public event RoutedEventHandler Closed
    {
      add
      {
          this.AddHandler( ClosedEvent, value );
      }
      remove
      {
          this.RemoveHandler( ClosedEvent, value );
      }
    }

    #endregion //Events

    #region Event Handlers

    private void OnKeyDown( object sender, KeyEventArgs e )
    {
      if( !this.IsOpen )
      {
        if( KeyboardUtilities.IsKeyModifyingPopupState( e ) )
        {
            this.IsOpen = true;
          // ContentPresenter items will get focus in Popup_Opened().
          e.Handled = true;
        }
      }
      else
      {
        if( KeyboardUtilities.IsKeyModifyingPopupState( e ) )
        {
            this.CloseDropDown( true );
          e.Handled = true;
        }
        else if( e.Key == Key.Escape )
        {
            this.CloseDropDown( true );
          e.Handled = true;
        }
      }
    }

    private void OnMouseDownOutsideCapturedElement( object sender, MouseButtonEventArgs e )
    {
        this.CloseDropDown( true );
    }

    private void DropDownButton_Click( object sender, RoutedEventArgs e )
    {
        this.OnClick();
    }

    void CanExecuteChanged( object sender, EventArgs e )
    {
        this.CanExecuteChanged();
    }

    private void Popup_Opened( object sender, EventArgs e )
    {
      // Set the focus on the content of the ContentPresenter.
      if(this.mContentPresenter != null )
      {
          this.mContentPresenter.MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
      }
    }

    #endregion //Event Handlers

    #region Methods

    private void CanExecuteChanged()
    {
      if(this.Command != null )
      {
        RoutedCommand command = this.Command as RoutedCommand;

        // If a RoutedCommand.
        if( command != null )
            this.IsEnabled = command.CanExecute(this.CommandParameter, this.CommandTarget ) ? true : false;
        // If a not RoutedCommand.
        else
            this.IsEnabled = this.Command.CanExecute(this.CommandParameter ) ? true : false;
      }
    }

    /// <summary>
    /// Closes the drop down.
    /// </summary>
    private void CloseDropDown( bool isFocusOnButton )
    {
      if(this.IsOpen ) this.IsOpen = false;
        this.ReleaseMouseCapture();

      if( isFocusOnButton ) this.Button.Focus();
    }

    protected virtual void OnClick()
    {
        this.RaiseRoutedEvent( ClickEvent );
        this.RaiseCommand();
    }

    /// <summary>
    /// Raises routed events.
    /// </summary>
    private void RaiseRoutedEvent( RoutedEvent routedEvent )
    {
      RoutedEventArgs args = new RoutedEventArgs( routedEvent, this );
        this.RaiseEvent( args );
    }

    /// <summary>
    /// Raises the command's Execute event.
    /// </summary>
    private void RaiseCommand()
    {
      if(this.Command != null )
      {
        RoutedCommand routedCommand = this.Command as RoutedCommand;

        if( routedCommand == null )
          ( ( ICommand ) this.Command ).Execute(this.CommandParameter );
        else
          routedCommand.Execute(this.CommandParameter, this.CommandTarget );
      }
    }

    /// <summary>
    /// Unhooks a command from the Command property.
    /// </summary>
    /// <param name="oldCommand">The old command.</param>
    /// <param name="newCommand">The new command.</param>
    private void UnhookCommand( ICommand oldCommand, ICommand newCommand )
    {
      EventHandler handler = this.CanExecuteChanged;
      oldCommand.CanExecuteChanged -= handler;
    }

    /// <summary>
    /// Hooks up a command to the CanExecuteChnaged event handler.
    /// </summary>
    /// <param name="oldCommand">The old command.</param>
    /// <param name="newCommand">The new command.</param>
    private void HookUpCommand( ICommand oldCommand, ICommand newCommand )
    {
      EventHandler handler = new EventHandler(this.CanExecuteChanged );
        this.mCanExecuteChangedHandler = handler;
      if( newCommand != null )
        newCommand.CanExecuteChanged += this.mCanExecuteChangedHandler;
    }

    #endregion //Methods

    #region ICommandSource Members

    // Keeps a copy of the CanExecuteChnaged handler so it doesn't get garbage collected.
    private EventHandler mCanExecuteChangedHandler;

    #region Command

    public static readonly DependencyProperty CommandProperty = DependencyProperty.Register( "Command", typeof( ICommand ), typeof( DropDownButton ), new PropertyMetadata( ( ICommand )null, OnCommandChanged ) );
    [TypeConverter( typeof( CommandConverter ) )]
    public ICommand Command
    {
      get
      {
        return ( ICommand ) this.GetValue( CommandProperty );
      }
      set
      {
          this.SetValue( CommandProperty, value );
      }
    }

    private static void OnCommandChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
    {
      DropDownButton dropDownButton = d as DropDownButton;
      if( dropDownButton != null )
        dropDownButton.OnCommandChanged( ( ICommand )e.OldValue, ( ICommand )e.NewValue );
    }

    protected virtual void OnCommandChanged( ICommand oldValue, ICommand newValue )
    {
      // If old command is not null, then we need to remove the handlers.
      if( oldValue != null ) this.UnhookCommand( oldValue, newValue );

        this.HookUpCommand( oldValue, newValue );

        this.CanExecuteChanged(); //may need to call this when changing the command parameter or target.
    }

    #endregion //Command

    public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register( "CommandParameter", typeof( object ), typeof( DropDownButton ), new PropertyMetadata( null ) );
    public object CommandParameter
    {
      get
      {
        return this.GetValue( CommandParameterProperty );
      }
      set
      {
          this.SetValue( CommandParameterProperty, value );
      }
    }

    public static readonly DependencyProperty CommandTargetProperty = DependencyProperty.Register( "CommandTarget", typeof( IInputElement ), typeof( DropDownButton ), new PropertyMetadata( null ) );
    public IInputElement CommandTarget
    {
      get
      {
        return ( IInputElement ) this.GetValue( CommandTargetProperty );
      }
      set
      {
          this.SetValue( CommandTargetProperty, value );
      }
    }

    #endregion //ICommandSource Members
  }
}

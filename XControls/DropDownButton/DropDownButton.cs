﻿using System;
using System.ComponentModel;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using XControls.Core.Utilities;

namespace XControls
{
  [TemplatePart( Name = PART_DropDownButton, Type = typeof( ToggleButton ) )]
  [TemplatePart( Name = PART_ContentPresenter, Type = typeof( ContentPresenter ) )]
  [TemplatePart( Name = PART_Popup, Type = typeof( Popup ) )]
  public class DropDownButton : ContentControl, ICommandSource
  {
    private const string PART_DropDownButton = "PART_DropDownButton";
    private const string PART_ContentPresenter = "PART_ContentPresenter";
    private const string PART_Popup = "PART_Popup";

    #region Members 

    private ContentPresenter _contentPresenter;
    private Popup _popup;

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
      Keyboard.AddKeyDownHandler( this, OnKeyDown );
      Mouse.AddPreviewMouseDownOutsideCapturedElementHandler( this, OnMouseDownOutsideCapturedElement );

      this.mVisibilityTimer = new Timer();
      this.mVisibilityTimer.Elapsed += this.OnVisibilityTimerElapsed;
      this.mVisibilityTimer.AutoReset = false;
    }

    #endregion //Constructors

    #region Properties



    private System.Windows.Controls.Primitives.ButtonBase _button;
    protected System.Windows.Controls.Primitives.ButtonBase Button
    {
      get
      {
        return _button;
      }
      set
      {
        if( _button != null )
          _button.Click -= DropDownButton_Click;

        _button = value;

        if( _button != null )
          _button.Click += DropDownButton_Click;
      }
    }

    #region DropDownContent

    public static readonly DependencyProperty DropDownVisibilityDelayProperty = DependencyProperty.Register("DropDownVisibilityDelay", typeof(double), typeof(DropDownButton), new UIPropertyMetadata(0.0, OnDropDownVisibilityDelayChanged));
    
    /// <summary>
    /// Gets or sets the delay in seconds.
    /// </summary>
    public double DropDownVisibilityDelay
    {
        get
        {
            return (double)GetValue(DropDownVisibilityDelayProperty);
        }
        set
        {
            SetValue(DropDownVisibilityDelayProperty, value);
        }
    }

    private static void OnDropDownVisibilityDelayChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
    {
        DropDownButton lDropDownButton = o as DropDownButton;
        if (lDropDownButton != null)
        {
            lDropDownButton.mVisibilityTimer.Interval = (double)e.NewValue * 1000.0;
        }
    }

    public static readonly DependencyProperty DropDownPlacementModeProperty = DependencyProperty.Register("DropDownPlacementMode", typeof(PlacementMode), typeof(DropDownButton), new UIPropertyMetadata(PlacementMode.Bottom));
    public PlacementMode DropDownPlacementMode
    {
        get
        {
            return (PlacementMode)GetValue(DropDownPlacementModeProperty);
        }
        set
        {
            SetValue(DropDownPlacementModeProperty, value);
        }
    }

    public static readonly DependencyProperty DropDownContentProperty = DependencyProperty.Register( "DropDownContent", typeof( object ), typeof( DropDownButton ), new UIPropertyMetadata( null, OnDropDownContentChanged ) );
    public object DropDownContent
    {
      get
      {
        return ( object )GetValue( DropDownContentProperty );
      }
      set
      {
        SetValue( DropDownContentProperty, value );
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
        return ( bool )GetValue( IsOpenProperty );
      }
      set
      {
        SetValue( IsOpenProperty, value );
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
            this.RaiseRoutedEvent(DropDownButton.OpenedEvent);

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
            this.RaiseRoutedEvent(DropDownButton.ClosedEvent);
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
      Button = GetTemplateChild( PART_DropDownButton ) as ToggleButton;

      _contentPresenter = GetTemplateChild( PART_ContentPresenter ) as ContentPresenter;

      if( _popup != null )
        _popup.Opened -= Popup_Opened;

      _popup = GetTemplateChild( PART_Popup ) as Popup;

      if( _popup != null )
        _popup.Opened += Popup_Opened;
    }

    #endregion //Base Class Overrides

    #region Events

    public static readonly RoutedEvent ClickEvent = EventManager.RegisterRoutedEvent( "Click", RoutingStrategy.Bubble, typeof( RoutedEventHandler ), typeof( DropDownButton ) );
    public event RoutedEventHandler Click
    {
      add
      {
        AddHandler( ClickEvent, value );
      }
      remove
      {
        RemoveHandler( ClickEvent, value );
      }
    }

    public static readonly RoutedEvent OpenedEvent = EventManager.RegisterRoutedEvent( "Opened", RoutingStrategy.Bubble, typeof( RoutedEventHandler ), typeof( DropDownButton ) );
    public event RoutedEventHandler Opened
    {
      add
      {
        AddHandler( OpenedEvent, value );
      }
      remove
      {
        RemoveHandler( OpenedEvent, value );
      }
    }

    public static readonly RoutedEvent ClosedEvent = EventManager.RegisterRoutedEvent( "Closed", RoutingStrategy.Bubble, typeof( RoutedEventHandler ), typeof( DropDownButton ) );
    public event RoutedEventHandler Closed
    {
      add
      {
        AddHandler( ClosedEvent, value );
      }
      remove
      {
        RemoveHandler( ClosedEvent, value );
      }
    }

    #endregion //Events

    #region Event Handlers

    private void OnKeyDown( object sender, KeyEventArgs e )
    {
      if( !IsOpen )
      {
        if( KeyboardUtilities.IsKeyModifyingPopupState( e ) )
        {
          IsOpen = true;
          // ContentPresenter items will get focus in Popup_Opened().
          e.Handled = true;
        }
      }
      else
      {
        if( KeyboardUtilities.IsKeyModifyingPopupState( e ) )
        {
          CloseDropDown( true );
          e.Handled = true;
        }
        else if( e.Key == Key.Escape )
        {
          CloseDropDown( true );
          e.Handled = true;
        }
      }
    }

    private void OnMouseDownOutsideCapturedElement( object sender, MouseButtonEventArgs e )
    {
      CloseDropDown( true );
    }

    private void DropDownButton_Click( object sender, RoutedEventArgs e )
    {
      OnClick();
    }

    void CanExecuteChanged( object sender, EventArgs e )
    {
      CanExecuteChanged();
    }

    private void Popup_Opened( object sender, EventArgs e )
    {
      // Set the focus on the content of the ContentPresenter.
      if( _contentPresenter != null )
      {
        _contentPresenter.MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
      }
    }

    #endregion //Event Handlers

    #region Methods

    private void CanExecuteChanged()
    {
      if( Command != null )
      {
        RoutedCommand command = Command as RoutedCommand;

        // If a RoutedCommand.
        if( command != null )
          IsEnabled = command.CanExecute( CommandParameter, CommandTarget ) ? true : false;
        // If a not RoutedCommand.
        else
          IsEnabled = Command.CanExecute( CommandParameter ) ? true : false;
      }
    }

    /// <summary>
    /// Closes the drop down.
    /// </summary>
    private void CloseDropDown( bool isFocusOnButton )
    {
      if( IsOpen )
        IsOpen = false;
      ReleaseMouseCapture();

      if( isFocusOnButton )
        Button.Focus();
    }

    protected virtual void OnClick()
    {
      RaiseRoutedEvent( DropDownButton.ClickEvent );
      RaiseCommand();
    }

    /// <summary>
    /// Raises routed events.
    /// </summary>
    private void RaiseRoutedEvent( RoutedEvent routedEvent )
    {
      RoutedEventArgs args = new RoutedEventArgs( routedEvent, this );
      RaiseEvent( args );
    }

    /// <summary>
    /// Raises the command's Execute event.
    /// </summary>
    private void RaiseCommand()
    {
      if( Command != null )
      {
        RoutedCommand routedCommand = Command as RoutedCommand;

        if( routedCommand == null )
          ( ( ICommand )Command ).Execute( CommandParameter );
        else
          routedCommand.Execute( CommandParameter, CommandTarget );
      }
    }

    /// <summary>
    /// Unhooks a command from the Command property.
    /// </summary>
    /// <param name="oldCommand">The old command.</param>
    /// <param name="newCommand">The new command.</param>
    private void UnhookCommand( ICommand oldCommand, ICommand newCommand )
    {
      EventHandler handler = CanExecuteChanged;
      oldCommand.CanExecuteChanged -= handler;
    }

    /// <summary>
    /// Hooks up a command to the CanExecuteChnaged event handler.
    /// </summary>
    /// <param name="oldCommand">The old command.</param>
    /// <param name="newCommand">The new command.</param>
    private void HookUpCommand( ICommand oldCommand, ICommand newCommand )
    {
      EventHandler handler = new EventHandler( CanExecuteChanged );
      canExecuteChangedHandler = handler;
      if( newCommand != null )
        newCommand.CanExecuteChanged += canExecuteChangedHandler;
    }

    #endregion //Methods

    #region ICommandSource Members

    // Keeps a copy of the CanExecuteChnaged handler so it doesn't get garbage collected.
    private EventHandler canExecuteChangedHandler;

    #region Command

    public static readonly DependencyProperty CommandProperty = DependencyProperty.Register( "Command", typeof( ICommand ), typeof( DropDownButton ), new PropertyMetadata( ( ICommand )null, OnCommandChanged ) );
    [TypeConverter( typeof( CommandConverter ) )]
    public ICommand Command
    {
      get
      {
        return ( ICommand )GetValue( CommandProperty );
      }
      set
      {
        SetValue( CommandProperty, value );
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
      if( oldValue != null )
        UnhookCommand( oldValue, newValue );

      HookUpCommand( oldValue, newValue );

      CanExecuteChanged(); //may need to call this when changing the command parameter or target.
    }

    #endregion //Command

    public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register( "CommandParameter", typeof( object ), typeof( DropDownButton ), new PropertyMetadata( null ) );
    public object CommandParameter
    {
      get
      {
        return GetValue( CommandParameterProperty );
      }
      set
      {
        SetValue( CommandParameterProperty, value );
      }
    }

    public static readonly DependencyProperty CommandTargetProperty = DependencyProperty.Register( "CommandTarget", typeof( IInputElement ), typeof( DropDownButton ), new PropertyMetadata( null ) );
    public IInputElement CommandTarget
    {
      get
      {
        return ( IInputElement )GetValue( CommandTargetProperty );
      }
      set
      {
        SetValue( CommandTargetProperty, value );
      }
    }

    #endregion //ICommandSource Members
  }
}

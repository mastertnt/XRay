using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Input;
using XControls.Core.Utilities;

namespace XControls.AutoSelectTextBox
{
    /// <summary>
    /// Class defining a text box extending the base text selection behavior.
    /// </summary>
    public class AutoSelectTextBox : System.Windows.Controls.TextBox
    {
        #region Dependencies

        /// <summary>
        /// Identifies the AutoSelectBehavior property.
        /// </summary>
        public static readonly DependencyProperty AutoSelectBehaviorProperty = DependencyProperty.Register("AutoSelectBehavior", typeof(AutoSelectBehavior), typeof(AutoSelectTextBox), new UIPropertyMetadata(AutoSelectBehavior.Never));

        /// <summary>
        /// Identifies the AutoMoveFocus property.
        /// </summary>
        public static readonly DependencyProperty AutoMoveFocusProperty = DependencyProperty.Register("AutoMoveFocus", typeof(bool), typeof(AutoSelectTextBox), new UIPropertyMetadata(false));

        #endregion // Dependencies.

        #region Routed events

        /// <summary>
        /// Event raised to query if a move focus can be done.
        /// </summary>
        public static readonly RoutedEvent QueryMoveFocusEvent = EventManager.RegisterRoutedEvent("QueryMoveFocus", RoutingStrategy.Bubble, typeof(QueryMoveFocusEventHandler), typeof(AutoSelectTextBox));

        #endregion // Routed events.

        #region Constructors

        /// <summary>
        /// Initializes the <see cref="AutoSelectTextBox"/> class.
        /// </summary>
        static AutoSelectTextBox()
        {
            AutomationProperties.AutomationIdProperty.OverrideMetadata( typeof( AutoSelectTextBox ), new UIPropertyMetadata( "AutoSelectTextBox" ) );
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoSelectTextBox"/> class.
        /// </summary>
        public AutoSelectTextBox()
        {
            // Do not allow the text copy when it is selected.
            DataObject.AddCopyingHandler(this, this.DisableDragCopy);
        }

        #endregion // Constructors.

        #region Properties

        /// <summary>
        /// Gets or sets the flag indicating when the automatic selection can be done.
        /// </summary>
        public AutoSelectBehavior AutoSelectBehavior
        {
            get
            {
                return (AutoSelectBehavior) this.GetValue(AutoSelectBehaviorProperty);
            }
            set
            {
                this.SetValue(AutoSelectBehaviorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the flag indicating if the focus can be moved using the arrow keys.
        /// </summary>
        public bool AutoMoveFocus
        {
            get
            {
                return (bool) this.GetValue(AutoMoveFocusProperty);
            }
            set
            {
                this.SetValue(AutoMoveFocusProperty, value);
            }
        }

        #endregion // Properties.

        #region Methods

        /// <summary>
        /// Delegate called to disable the capability of dragging selected text of the text box.
        /// </summary>
        /// <param name="pSender">The modified text box.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private void DisableDragCopy(object pSender, DataObjectCopyingEventArgs pEventArgs)
        {
            if (pEventArgs.IsDragDrop)
            { 
                pEventArgs.CancelCommand(); 
            }
        }

        /// <summary>
        /// Delegate called when a key is going to be pressed.
        /// </summary>
        /// <param name="pEventArgs">The event arguments.</param>
        protected override void OnPreviewKeyDown(KeyEventArgs pEventArgs)
        {
            if (this.AutoMoveFocus == false)
            {
                base.OnPreviewKeyDown(pEventArgs);
                return;
            }

            if ((pEventArgs.Key == Key.Left) && ((Keyboard.Modifiers == ModifierKeys.None) || (Keyboard.Modifiers == ModifierKeys.Control)))
            {
                pEventArgs.Handled = this.MoveFocusLeft();
            }

            if ((pEventArgs.Key == Key.Right) && ((Keyboard.Modifiers == ModifierKeys.None) || (Keyboard.Modifiers == ModifierKeys.Control)))
            {
                pEventArgs.Handled = this.MoveFocusRight();
            }

            if (((pEventArgs.Key == Key.Up) || (pEventArgs.Key == Key.PageUp)) && ((Keyboard.Modifiers == ModifierKeys.None) || (Keyboard.Modifiers == ModifierKeys.Control)))
            {
                pEventArgs.Handled = this.MoveFocusUp();
            }

            if (((pEventArgs.Key == Key.Down) || (pEventArgs.Key == Key.PageDown)) && ((Keyboard.Modifiers == ModifierKeys.None) || (Keyboard.Modifiers == ModifierKeys.Control)))
            {
                pEventArgs.Handled = this.MoveFocusDown();
            }

            base.OnPreviewKeyDown(pEventArgs);
        }

        /// <summary>
        /// Delegate called when the control is going to have the focus.
        /// </summary>
        /// <param name="pEventArgs">The event arguments.</param>
        protected override void OnPreviewGotKeyboardFocus(KeyboardFocusChangedEventArgs pEventArgs)
        {
            base.OnPreviewGotKeyboardFocus(pEventArgs);

            if (this.AutoSelectBehavior == AutoSelectBehavior.OnFocus)
            {
                // If the focus was not in one of our child ( or popup ), we select all the text.
                if (TreeHelper.IsDescendantOf(pEventArgs.OldFocus as DependencyObject, this) == false)
                {
                    this.SelectAll();
                }
            }
        }

        /// <summary>
        /// Delegate called when the mouse left is going to be pressed.
        /// </summary>
        /// <param name="pEventArgs">The event arguments.</param>
        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs pEventArgs)
        {
            base.OnPreviewMouseLeftButtonDown(pEventArgs);

            if (this.AutoSelectBehavior == AutoSelectBehavior.Never)
            {
                return;
            }

            if (this.IsKeyboardFocusWithin == false)
            {
                this.Focus();

                // Prevent from removing the selection
                pEventArgs.Handled = true;  
            }
        }

        /// <summary>
        /// Delegate called when the text changed.
        /// </summary>
        /// <param name="pEventArgs">The event arguments.</param>
        protected override void OnTextChanged(TextChangedEventArgs pEventArgs)
        {
            base.OnTextChanged(pEventArgs);

            if (this.AutoMoveFocus == false)
            {
                return;
            }
            
            if ((this.Text.Length != 0) && (this.Text.Length == this.MaxLength) && (this.CaretIndex == this.MaxLength))
            {
                // The maximum text length is reached.
                if (this.CanMoveFocus(FocusNavigationDirection.Right, true) == true)
                {
                    FocusNavigationDirection lDirection = (this.FlowDirection == FlowDirection.LeftToRight) ? FocusNavigationDirection.Right : FocusNavigationDirection.Left;
                    this.MoveFocus(new TraversalRequest(lDirection));
                }
            }
        }

        /// <summary>
        /// Tests if the focus can be given to an other control.
        /// </summary>
        /// <param name="pDirection">The focus direction.</param>
        /// <param name="pReachedMax">Flag to know if the maximum text length has been reached.</param>
        /// <returns></returns>
        private bool CanMoveFocus(FocusNavigationDirection pDirection, bool pReachedMax)
        {
            QueryMoveFocusEventArgs lEvent = new QueryMoveFocusEventArgs(pDirection, pReachedMax);
            this.RaiseEvent(lEvent);
            return lEvent.CanMoveFocus;
        }

        /// <summary>
        /// Moves the focus to the left.
        /// </summary>
        /// <returns>True if the focus change succeed, false otherwise.</returns>
        private bool MoveFocusLeft()
        {
            if (this.FlowDirection == FlowDirection.LeftToRight)
            {
                // Occurs only if the cursor is at the beginning of the text.
                if ((this.CaretIndex == 0) && (this.SelectionLength == 0))
                {
                    if (ComponentCommands.MoveFocusBack.CanExecute(null, this))
                    {
                        ComponentCommands.MoveFocusBack.Execute(null, this);
                        return true;
                    }
                    else if (this.CanMoveFocus(FocusNavigationDirection.Left, false))
                    {
                        return this.MoveFocus(new TraversalRequest(FocusNavigationDirection.Left));
                    }
                }
            }
            else
            {
                //occurs only if the cursor is at the end of the text
                if ((this.CaretIndex == this.Text.Length) && (this.SelectionLength == 0))
                {
                    if (ComponentCommands.MoveFocusBack.CanExecute(null, this))
                    {
                        ComponentCommands.MoveFocusBack.Execute(null, this);
                        return true;
                    }
                    else if (this.CanMoveFocus(FocusNavigationDirection.Left, false))
                    {
                        return this.MoveFocus(new TraversalRequest(FocusNavigationDirection.Left));
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Moves the focus to the right.
        /// </summary>
        /// <returns>True if the focus change succeed, false otherwise.</returns>
        private bool MoveFocusRight()
        {
            if (this.FlowDirection == FlowDirection.LeftToRight)
            {
                // Occurs only if the cursor is at the beginning of the text.
                if ((this.CaretIndex == this.Text.Length) && (this.SelectionLength == 0))
                {
                    if (ComponentCommands.MoveFocusForward.CanExecute(null, this))
                    {
                        ComponentCommands.MoveFocusForward.Execute(null, this);
                        return true;
                    }
                    else if (this.CanMoveFocus(FocusNavigationDirection.Right, false))
                    {
                        return this.MoveFocus(new TraversalRequest(FocusNavigationDirection.Right));
                    }
                }
            }
            else
            {
                // Occurs only if the cursor is at the end of the text.
                if ((this.CaretIndex == 0) && (this.SelectionLength == 0))
                {
                    if (ComponentCommands.MoveFocusForward.CanExecute(null, this))
                    {
                        ComponentCommands.MoveFocusForward.Execute(null, this);
                        return true;
                    }
                    else if (this.CanMoveFocus(FocusNavigationDirection.Right, false))
                    {
                        return this.MoveFocus(new TraversalRequest(FocusNavigationDirection.Right));
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Moves the focus up.
        /// </summary>
        /// <returns>True if the focus change succeed, false otherwise.</returns>
        private bool MoveFocusUp()
        {
            int lLineNumber = this.GetLineIndexFromCharacterIndex(this.SelectionStart);

            // Occurs only if the cursor is on the first line.
            if (lLineNumber == 0)
            {
                if (ComponentCommands.MoveFocusUp.CanExecute(null, this))
                {
                    ComponentCommands.MoveFocusUp.Execute(null, this);
                    return true;
                }
                else if (this.CanMoveFocus(FocusNavigationDirection.Up, false))
                {
                    return this.MoveFocus(new TraversalRequest(FocusNavigationDirection.Up));
                }
            }

            return false;
        }

        /// <summary>
        /// Moves the focus down.
        /// </summary>
        /// <returns>True if the focus change succeed, false otherwise.</returns>
        private bool MoveFocusDown()
        {
            int lLineNumber = this.GetLineIndexFromCharacterIndex(this.SelectionStart);

            //occurs only if the cursor is on the first line
            if (lLineNumber == (this.LineCount - 1))
            {
                if (ComponentCommands.MoveFocusDown.CanExecute(null, this))
                {
                    ComponentCommands.MoveFocusDown.Execute(null, this);
                    return true;
                }
                else if (this.CanMoveFocus(FocusNavigationDirection.Down, false))
                {
                    return this.MoveFocus(new TraversalRequest(FocusNavigationDirection.Down));
                }
            }

            return false;
        }

        #endregion // Methods.
    }
}


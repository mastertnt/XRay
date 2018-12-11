using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace XControls.TextBox
{
    /// <summary>
    ///     This class defines a textbox which can be editable.
    /// </summary>
    public partial class EditableInPlaceTextBox : UserControl
    {
        #region Fields

        /// <summary>
        ///     We keep the old text when we go into editmode in case the user aborts with the escape key
        /// </summary>
        private string mOldText;

        #endregion // Fields.

        #region Constructor

        /// <summary>
        ///     This class define a text box that is editable
        /// </summary>
        public EditableInPlaceTextBox()
        {
            this.InitializeComponent();
            this.Focusable = true;
        }

        #endregion // Constructor.

        #region Dependency properties

        /// <summary>
        ///     Property associated to Text property
        /// </summary>
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(EditableInPlaceTextBox), new PropertyMetadata(string.Empty, OnTextChanged));

        /// <summary>
        ///     Property associated to IsEditable property
        /// </summary>
        public static readonly DependencyProperty IsEditableProperty = DependencyProperty.Register("IsEditable", typeof(bool), typeof(EditableInPlaceTextBox), new PropertyMetadata(false));

        /// <summary>
        ///     Property associated to IsInEditMode property
        /// </summary>
        public static readonly DependencyProperty IsInEditModeProperty = DependencyProperty.Register("IsInEditMode", typeof(bool), typeof(EditableInPlaceTextBox), new PropertyMetadata(false, OnEditModeChanged, CoerceEditMode));

        /// <summary>
        ///     Property associated to the TextFormat property
        /// </summary>
        public static readonly DependencyProperty TextFormatProperty = DependencyProperty.Register("TextFormat", typeof(string), typeof(EditableInPlaceTextBox), new PropertyMetadata("{0}"));

        #endregion // Dependency properties.

        #region Properties

        /// <summary>
        ///     The editable text displayed to the user
        /// </summary>
        public string Text
        {
            get
            {
                // Save the text as the old text when it changes
                this.mOldText = (string) this.GetValue(TextProperty);
                return this.mOldText;
            }

            set => this.SetValue(TextProperty, value);
        }

        /// <summary>
        ///     Tells if the item is editable
        /// </summary>
        public bool IsEditable
        {
            get => (bool) this.GetValue(IsEditableProperty);

            set => this.SetValue(IsEditableProperty, value);
        }

        /// <summary>
        ///     Tell if the item is in edit mode
        /// </summary>
        public bool IsInEditMode
        {
            get => (bool) this.GetValue(IsInEditModeProperty);

            set => this.SetValue(IsInEditModeProperty, value);
        }

        /// <summary>
        ///     Used if the editable text should be surrounded by more text.
        ///     The TextFormat property uses the String.Format function to format the text,
        ///     which means that the editable text is referenced by {0} inside a string.
        ///     If the TextFormat property is set to either the empty string (""), the string containing only {0} ("{0}"),
        ///     or is not set at all, the control simply shows the string from the Text property.
        /// </summary>
        public string TextFormat
        {
            get => (string) this.GetValue(TextFormatProperty);

            set
            {
                if (value == string.Empty)
                {
                    value = "{0}";
                }

                this.SetValue(TextFormatProperty, value);
            }
        }

        /// <summary>
        ///     Format the Text according to TextFormat
        /// </summary>
        public string FormattedText => string.Format(this.TextFormat, this.Text);

        #endregion // Properties.

        #region Methods

        /// <summary>
        ///     This delegate is called when the edition mode is changed.
        /// </summary>
        /// <param name="pSender">The event sender.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private static void OnEditModeChanged(DependencyObject pObject, DependencyPropertyChangedEventArgs pEventArgs)
        {
            var lControl = pObject as EditableInPlaceTextBox;
            if (lControl != null)
            {
                if (lControl.IsEditable)
                {
                    if (Convert.ToBoolean(pEventArgs.NewValue))
                    {
                        lControl.mOldText = lControl.Text;
                    }
                }
            }
        }

        /// <summary>
        ///     This delegate is called when the edition mode is changed.
        /// </summary>
        /// <param name="pSender">The event sender.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private static void OnTextChanged(DependencyObject pObject, DependencyPropertyChangedEventArgs pEventArgs)
        {
            var lControl = pObject as EditableInPlaceTextBox;
            if (lControl != null)
            {
                lControl.ForceFormatedTextUpdate();
            }
        }

        /// <summary>
        ///     This delegate is called to coerce the edition mode.
        /// </summary>
        /// <param name="pObject">The dependency object.</param>
        /// <param name="pMode">The edition mode to coerce.</param>
        /// <returns></returns>
        private static object CoerceEditMode(DependencyObject pObject, object pMode)
        {
            var lNewMode = (bool) pMode;
            var lControl = pObject as EditableInPlaceTextBox;
            if (lControl != null)
            {
                if (lControl.IsEditable)
                {
                    return lNewMode;
                }

                return false;
            }

            return lNewMode;
        }

        /// <summary>
        ///     Invoked when we enter edit mode.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">RoutedEventArgs</param>
        private void OnTextBoxLoaded(object pSender, RoutedEventArgs pEventArgs)
        {
            var lTxt = pSender as System.Windows.Controls.TextBox;

            // Give the TextBox input focus
            lTxt.Focus();

            lTxt.SelectAll();
        }

        /// <summary>
        ///     Invoked when we exit edit mode.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">RoutedEventArgs</param>
        private void OnTextBoxLostFocus(object pSender, RoutedEventArgs pEventArgs)
        {
            this.IsInEditMode = false;
        }

        /// <summary>
        ///     Invoked when the user edits the annotation.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">KeyEventArgs</param>
        private void OnTextBoxKeyDown(object pSender, KeyEventArgs pEventArgs)
        {
            if (pEventArgs.Key == Key.Enter)
            {
                this.IsInEditMode = false;
                pEventArgs.Handled = true;
            }
            else if (pEventArgs.Key == Key.Escape)
            {
                this.IsInEditMode = false;
                // it undo the change done to the text
                this.Text = this.mOldText;
                pEventArgs.Handled = true;
            }
        }

        /// <summary>
        ///     Invoked when the user clicks twice.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private void OnMouseDoubleClick(object pSender, MouseButtonEventArgs pEventArgs)
        {
            if (this.IsEditable && this.IsInEditMode == false)
            {
                this.IsInEditMode = true;
            }
        }

        /// <summary>
        ///     Force the update of the formated string into the view when the
        ///     control is not in edit mode.
        /// </summary>
        /// <remarks>
        ///     This method is mainly used to refresh the text displayed in the control
        ///     when it is not in the edit mode.
        ///     In this state, the FormatedText property is used to display the text.
        ///     The NotifyPropertyChanged could be used to force the refresh, but it doesn't
        ///     work because the data context is not "this" object.
        ///     The only way to force the refresh is then to force the edit mode and then
        ///     go back to the non edit mode. This way, the FormatedText property is called
        ///     when the corresponding DataTemplate is loaded, and the GUI is refreshed.
        /// </remarks>
        private void ForceFormatedTextUpdate()
        {
            if (this.IsLoaded && !this.IsInEditMode)
            {
                var lBackupIsEditable = this.IsEditable;
                var lBackupIsInEditMode = this.IsInEditMode;

                this.IsEditable = true;
                this.IsInEditMode = true;
                this.IsInEditMode = false;
                this.IsEditable = false;

                this.IsEditable = lBackupIsEditable;
                this.IsInEditMode = lBackupIsInEditMode;
            }
        }

        #endregion // Methods.
    }
}
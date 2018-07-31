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
using XCommand;

namespace XCommand.TestApp
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Fields

        /// <summary>
        /// Stores the command manager.
        /// </summary>
        private UserCommandManager mCommandManager;

        #endregion // Fields.

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();

            this.mCommandManager = new UserCommandManager();
            this.mCommandManager.SessionCreated += this.OnCommandManagerSessionCreated;

            ContextView lContextView1 = new ContextView();
            ContextView lContextView2 = new ContextView();
            SessionView lSessionView = new SessionView();
            lSessionView.mContextes.Children.Add(lContextView1);
            lSessionView.mContextes.Children.Add(lContextView2);
            this.mSessions.Children.Add(lSessionView);

            for (int i = 0; i < 12; i++)
            {
                lContextView1.mCommands.Children.Add(new CommandView());
            }
        }

        #endregion // Constructors.

        #region Methods

        /// <summary>
        /// Delegate called when a new session is created.
        /// </summary>
        /// <param name="pSource">The source manager.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private void OnCommandManagerSessionCreated(IUserCommandManager pSource, SessionCreatedEventArgs pEventArgs)
        {
            //throw new NotImplementedException();
        }

        #endregion // Methods.
    }
}

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

namespace XZoomAndPan.TestApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();

            List<String> lModel = new List<string>();
            lModel.Add("Ceci est un exemple.... :)");
            lModel.Add("Pouet");
            lModel.Add("Pouet");
            lModel.Add("Pouet");
            lModel.Add("Pouet");
            lModel.Add("Pouet");
            lModel.Add("Pouet");

            this.mMainListBox.ItemsSource = lModel;
            this.mOvervierview.ItemsSource = lModel;
        }
    }
}

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

namespace XControls.TestApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.DoubleUpDown.TextChanged += DoubleUpDown_TextChanged;
            this.SetZeroDouble.Click += SetZeroDouble_Click;
        }

        void SetZeroDouble_Click(object sender, RoutedEventArgs e)
        {
            this.DoubleUpDown.Text = "-0";
        }

        void DoubleUpDown_TextChanged(object sender, RoutedPropertyChangedEventArgs<string> e)
        {
            Console.WriteLine(e.NewValue);
        }
    }
}

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

namespace Tcp_Controller
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Client client;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonConnect_Click(object sender, RoutedEventArgs e)
        {
            client = new Client();

            ((Button)sender).IsEnabled = false;
        }

        private void ButtonLeft_Click(object sender, RoutedEventArgs e)
        {
            client.MoveLeft();
        }

        private void ButtonRight_Click(object sender, RoutedEventArgs e)
        {
            client.MoveRight();
        }

        private void ButtonDown_Click(object sender, RoutedEventArgs e)
        {
            client.MoveDown();
        }

        private void ButtonUp_Click(object sender, RoutedEventArgs e)
        {
            client.MoveUp();
        }
    }
}

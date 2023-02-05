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
using System.Windows.Threading;  // timer

namespace Tcp_Receiver
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Server server;

        public MainWindow()
        {
            InitializeComponent();

            server = new Server();
            server.Start();

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(10);  // 10 ms
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            lblInfo.Content = server.IsConnected;

            Canvas.SetLeft(rectPlayer, server.X);
            Canvas.SetTop(rectPlayer, server.Y);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }
    }
}

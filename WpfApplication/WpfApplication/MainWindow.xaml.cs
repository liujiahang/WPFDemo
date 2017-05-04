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
using MyNetUtil;

namespace WpfApplication
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private SocketProvider mServer;
        public MainWindow()
        {
            InitializeComponent();
            mServer = new SocketProvider();
        }

        private void Conn_Click(object sender, RoutedEventArgs e)
        {
            mServer.start(8080);
        }

        private void Send_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}

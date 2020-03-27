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
using FlightSimulatorGui.Model;
using System.Threading;

namespace FlightSimulatorGui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MyTcpServer server = new MyTcpServer();
            //Thread a = new Thread(server.createAndRunServer);
           /// a.Start();
            MyTcpClient client = new MyTcpClient();
            //Thread b = new Thread(client.createAndRunClient);
            //b.Start();

            server.createAndRunServer();


        }
    }
}

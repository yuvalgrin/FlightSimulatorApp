using FlightSimulatorGui.ViewModel;
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

namespace FlightSimulatorGui.Views
{
    public partial class ControlRoom : UserControl
    {
        public ControlRoom()
        {
            InitializeComponent();
            this.DataContext = (Application.Current as App).ControlRoomViewModel;

            // Initial value
            StringBuilder sb = new StringBuilder("get /position/latitude-deg\r\nget /position/longitude-deg");
            tbInput.Text = sb.ToString();
        }

        private void btnData_Click(object sender, RoutedEventArgs e)
        {
            (Application.Current as App).ControlRoomViewModel.QueryUpdate(tbInput.Text);
        }
    }
}

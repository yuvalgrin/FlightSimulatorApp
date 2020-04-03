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
    public partial class ConnSettings : UserControl
    {
        public ConnSettings()
        {
            InitializeComponent();
            this.DataContext = (Application.Current as App).ConnSettingsViewModel;
        }

        private void btnData_Click(object sender, RoutedEventArgs e)
        {
            (Application.Current as App).ConnSettingsViewModel.queryUpdate(tbIp.Text, tbPort.Text);
        }
    }
}

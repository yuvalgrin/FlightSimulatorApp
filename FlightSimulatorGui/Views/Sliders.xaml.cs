using FlightSimulatorGui.server;
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
    /// <summary>
    /// Interaction logic for Sliders.xaml
    /// </summary>
    public partial class Sliders : UserControl
    {
        public Sliders()
        {
            InitializeComponent();
        }

        private void sldAilronValueChange(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //DatabaseManager.addCommandToQueue(sldAilron.Value);
        }

        private void sldReronValueChange(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //DatabaseManager.addCommandToQueue(sldAilron.Value);
        }
    }
}

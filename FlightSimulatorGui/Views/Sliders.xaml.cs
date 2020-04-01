using FlightSimulatorGui.Model;
using FlightSimulatorGui.server;
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
    /// <summary>
    /// Interaction logic for Sliders.xaml
    /// </summary>
    public partial class Sliders : UserControl
    {
        SlidersViewModel slidersViewModel;
        public Sliders()
        {
            InitializeComponent();
            this.slidersViewModel = new SlidersViewModel();
        }

        private void sldThrottleValueChange(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            slidersViewModel.sldThrottleUpdate(e.NewValue);
        }

        private void sldAileronValueChange(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            slidersViewModel.sldAileronUpdate(e.NewValue);
        }
    }
}

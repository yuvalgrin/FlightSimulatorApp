using FlightSimulatorGui.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace FlightSimulatorGui.Views
{
    public partial class Sliders : UserControl
    {
        public Sliders()
        {
            InitializeComponent();
            this.DataContext = (Application.Current as App).SlidersViewModel;
        }

        private void sldThrottleValueChange(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            (Application.Current as App).SlidersViewModel.sldThrottleUpdate(e.NewValue);
        }

        private void sldRudderValueChange(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            (Application.Current as App).SlidersViewModel.sldRudderUpdate(e.NewValue);
        }
    }
}

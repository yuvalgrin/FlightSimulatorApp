using FlightSimulatorApp.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace FlightSimulatorApp.Views
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
            (Application.Current as App).SlidersViewModel.SldThrottleUpdate(e.NewValue);
        }

        private void sldAileronValueChange(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            (Application.Current as App).SlidersViewModel.SldAileronUpdate(e.NewValue);
        }
    }
}

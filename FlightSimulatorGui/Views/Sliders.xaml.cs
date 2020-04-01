using FlightSimulatorGui.ViewModel;
using System.Windows;
using System.Windows.Controls;

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
            this.DataContext = slidersViewModel;

        }

        private void sldThrottleValueChange(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            slidersViewModel.sldThrottleUpdate(e.NewValue);
        }

        private void sldRudderValueChange(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            slidersViewModel.sldRudderUpdate(e.NewValue);
        }
    }
}

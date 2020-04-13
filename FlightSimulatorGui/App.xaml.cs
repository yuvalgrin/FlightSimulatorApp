using FlightSimulatorGui.Model;
using FlightSimulatorGui.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace FlightSimulatorGui
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        public ConnSettingsViewModel ConnSettingsViewModel { get; internal set; }
        public ControlRoomViewModel ControlRoomViewModel { get; internal set; }
        public DataViewViewModel DataViewViewModel { get; internal set; }
        public JoystickViewModel JoystickViewModel { get; internal set; }
        public MapViewModel MapViewModel { get; internal set; }
        public SlidersViewModel SlidersViewModel { get; internal set; }

        public void ApplicationDispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            if (e != null)
            {
                FlightSimulatorModel.Get().ThrowNewError(e.Exception.Message);
                e.Handled = true;
            }
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            FlightSimulatorModel.Get().ExitProgram();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            ConnSettingsViewModel = new ConnSettingsViewModel();
            ControlRoomViewModel = new ControlRoomViewModel();
            DataViewViewModel = new DataViewViewModel();
            JoystickViewModel = new JoystickViewModel();
            MapViewModel = new MapViewModel();
            SlidersViewModel = new SlidersViewModel();
        }
    }
}

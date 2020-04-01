using FlightSimulatorGui.Model;
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

        public void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            FlightSimulatorModel.get().ErrorMsg = e.Exception.Message;
            e.Handled = true;
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            FlightSimulatorModel.get().exitProgram();
        }
    }
}

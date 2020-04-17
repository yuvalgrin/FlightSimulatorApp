using FlightSimulatorApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSimulatorApp.ViewModel
{
    public class JoystickViewModel : BaseNotify
    {
        public JoystickViewModel()
        {
            Model.PropertyChanged +=
                delegate (Object sender, PropertyChangedEventArgs e) {
                    NotifyPropertyChanged("VM" + e.PropertyName);
                };
        }

        public void JoyElevatorUpdate(double value)
        {
            Model.AddCommandToQueue(new SetCommand("/controls/flight/elevator", value));
        }

        public void JoyRudderUpdate(double value)
        {
            Model.AddCommandToQueue(new SetCommand("/controls/flight/rudder", value));
        }

    }
}

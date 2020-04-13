using FlightSimulatorGui.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSimulatorGui.ViewModel
{
    public class JoystickViewModel : BaseNotify
    {
        public JoystickViewModel()
        {
            model.PropertyChanged +=
                delegate (Object sender, PropertyChangedEventArgs e) {
                    NotifyPropertyChanged("VM_" + e.PropertyName);
                };
        }

        public void JoyElevatorUpdate(double value)
        {
            model.addCommandToQueue(new SetCommand("/controls/flight/elevator", value));
        }

        public void JoyRudderUpdate(double value)
        {
            model.addCommandToQueue(new SetCommand("/controls/flight/rudder", value));
        }

    }
}

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

        public void joyElevatorUpdate(double value)
        {
            model.AddCommandToQueue(new SetCommand("/controls/flight/elevator", value));
        }

        public void joyAileronUpdate(double value)
        {
            model.AddCommandToQueue(new SetCommand("/controls/flight/aileron", value));
        }

    }
}

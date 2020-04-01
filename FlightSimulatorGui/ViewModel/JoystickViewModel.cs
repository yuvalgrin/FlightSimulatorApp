using FlightSimulatorGui.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSimulatorGui.ViewModel
{
    class JoystickViewModel : BaseNotify
    {

        public JoystickViewModel()
        {
        }

        public void joyElevatorUpdate(double value)
        {
            model.addCommandToQueue(new SetCommand("/controls/flight/elevator", value));
        }

        public void joyRudderUpdate(double value)
        {
            model.addCommandToQueue(new SetCommand("/controls/flight/rudder", value));
        }

    }
}

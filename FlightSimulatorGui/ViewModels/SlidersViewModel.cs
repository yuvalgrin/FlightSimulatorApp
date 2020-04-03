using FlightSimulatorGui.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSimulatorGui.ViewModel
{
    public class SlidersViewModel : BaseNotify
    {
        public SlidersViewModel()
        {
            model.PropertyChanged +=
                delegate (Object sender, PropertyChangedEventArgs e) {
                    NotifyPropertyChanged("VM_" + e.PropertyName);
                };
        }

        public void sldThrottleUpdate(double value)
        {
            model.addCommandToQueue(new SetCommand("/controls/engines/current-engine/throttle", value));
        }

        public void sldRudderUpdate(double value)
        {
            model.addCommandToQueue(new SetCommand("/controls/flight/aileron", value));
        }

    }
}

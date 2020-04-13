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

        public void SldThrottleUpdate(double value)
        {
            model.AddCommandToQueue(new SetCommand("/controls/engines/current-engine/throttle", value));
        }

        public void SldAileronUpdate(double value)
        {
            model.AddCommandToQueue(new SetCommand("/controls/flight/aileron", value));
        }

    }
}

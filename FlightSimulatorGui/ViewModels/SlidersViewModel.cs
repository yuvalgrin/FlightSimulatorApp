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
            Model.PropertyChanged +=
                delegate (Object sender, PropertyChangedEventArgs e) {
                    NotifyPropertyChanged("VM" + e.PropertyName);
                };
        }

        public void SldThrottleUpdate(double value)
        {
            Model.AddCommandToQueue(new SetCommand("/controls/engines/current-engine/throttle", value));
        }

        public void SldAileronUpdate(double value)
        {
            Model.AddCommandToQueue(new SetCommand("/controls/flight/aileron", value));
        }

    }
}

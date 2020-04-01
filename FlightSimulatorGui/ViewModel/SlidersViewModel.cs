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
        private Double _VM_Throttle;
        public Double VM_Throttle
        {
            get { return _VM_Throttle; }
            set {
                _VM_Throttle = value;
                NotifyPropertyChanged("VM_Throttle"); }
        }

        private Double _VM_Rudder;
        public Double VM_Rudder
        {
            get { return _VM_Rudder; }
            set
            {
                _VM_Rudder = value;
                NotifyPropertyChanged("VM_Rudder"); }
        }

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
            VM_Throttle = value;
        }

        public void sldRudderUpdate(double value)
        {
            model.addCommandToQueue(new SetCommand("/controls/flight/aileron", value));
            VM_Rudder = value;
        }

    }
}

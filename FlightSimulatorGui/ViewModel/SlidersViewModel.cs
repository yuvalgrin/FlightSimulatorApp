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
        Double VM_Throttle
        {
            get { return _VM_Throttle; }
            set {
                _VM_Throttle = value;
                NotifyPropertyChanged("VM_Throttle"); }
        }

        private Double _VM_Aileron;
        Double VM_Aileron
        {
            get { return _VM_Aileron; }
            set
            {
                _VM_Aileron = value;
                NotifyPropertyChanged("VM_Aileron"); }
        }

        public SlidersViewModel()
        {
        }

        public void sldThrottleUpdate(double value)
        {
            model.addCommandToQueue(new SetCommand("/controls/engines/current-engine/throttle", value));
            VM_Throttle = value;
        }

        public void sldAileronUpdate(double value)
        {
            model.addCommandToQueue(new SetCommand("/controls/flight/aileron", value));
            VM_Aileron = value;
        }

    }
}

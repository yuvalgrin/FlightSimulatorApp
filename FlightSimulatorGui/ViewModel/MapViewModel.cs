using FlightSimulatorGui.Model;
using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSimulatorGui.ViewModel
{
    class MapViewModel : BaseNotify
    {
        private FlightSimulatorModel model;

        public Location VM_Location
        {
            get { return model.Location; }
        }

        public MapViewModel()
        {
            this.model = FlightSimulatorModel.get();
        }

    }
}

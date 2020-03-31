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
    class MapViewModel : INotifyPropertyChanged
    {
        private FlightSimulatorModel model;
        public event PropertyChangedEventHandler PropertyChanged;

        public Location VM_Location
        {
            get { return model.Location; }
        }

        public MapViewModel()
        {
            this.model = FlightSimulatorModel.get();
        }

        public void NotifyPropertyChanged(string propName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

    }
}

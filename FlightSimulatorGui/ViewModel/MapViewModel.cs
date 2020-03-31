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

        public double VM_Latitude
        {
            get { return VM_Latitude; }
            set {
                VM_Latitude = value;
                VM_Location = new Location(VM_Location.Latitude, VM_Latitude);
                NotifyPropertyChanged("VM_Latitude");
            }
        }

        public double VM_Longitude
        {
            get { return VM_Longitude; }
            set {
                VM_Longitude = value;
                VM_Location = new Location(VM_Longitude, VM_Location.Longitude);
                NotifyPropertyChanged("VM_Longitude");
            }
        }

        public Location VM_Location
        {
            get { return VM_Location; }
            set
            {
                VM_Location = value;
                NotifyPropertyChanged("VM_Location");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public MapViewModel()
        {
            this.model = FlightSimulatorModel.get();
            this.model.PropertyChanged +=
                delegate (Object sender, PropertyChangedEventArgs e) {
                    NotifyPropertyChanged("VM_" + e.PropertyName);
                };
        }

        public void NotifyPropertyChanged(string propName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

    }
}

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

        public double VM_latitude
        {
            get { return VM_latitude; }
            set {
                VM_latitude = value;
                VM_Location = new Location(VM_Location.Latitude, VM_latitude);
                NotifyPropertyChanged("VM_latitude");
            }
        }

        public double VM_longitude
        {
            get { return VM_longitude; }
            set {
                VM_longitude = value;
                VM_Location = new Location(VM_longitude, VM_Location.Longitude);
                NotifyPropertyChanged("VM_longitude");
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
                    switch(e.PropertyName)
                    {
                        case "latitude":
                            Location = new Location(this.valueMap[key]);
                            break;
                        case "longtitude":
                            break;
                    }

                    NotifyPropertyChanged("VM_" + e.PropertyName);
                };
        }

        public void NotifyPropertyChanged(string propName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

    }
}

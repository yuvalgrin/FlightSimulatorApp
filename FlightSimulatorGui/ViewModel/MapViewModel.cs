using FlightSimulatorGui.Model;
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

        private double VM_Latitude
        {
            get { return VM_Latitude; }
            set {
                VM_Latitude = value;
                NotifyPropertyChanged("VM_Latitude");
            }
        }

        private double VM_Longitude
        {
            get { return VM_Longitude; }
            set {
                VM_Longitude = value;
                NotifyPropertyChanged("VM_Longitude");
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

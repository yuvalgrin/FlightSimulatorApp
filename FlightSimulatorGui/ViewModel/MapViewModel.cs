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
        private FSConnector model;

        private double VM_Lat
        {
            get { return VM_Lat; }
            set {
                VM_Lat = value;
                NotifyPropertyChanged("VM_Lat");
            }
        }

        private double VM_Lon
        {
            get { return VM_Lon; }
            set {
                VM_Lon = value;
                NotifyPropertyChanged("VM_Lat");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public MapViewModel()
        {
            this.model = FSConnector;
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

using FlightSimulatorGui.Model;
using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using FlightSimulatorGui.server;

namespace FlightSimulatorGui.ViewModel
{
    class MapViewModel : BaseNotify
    {
        public Location VM_Location
        {
            get { return model.Location; }
        }
        public Double VM_ErrorEnabled
        {
            get { return model.ErrorEnabled; }
        }      
        public String VM_ErrorMsg
        {
            get { return model.ErrorMsg; }
        }


        public MapViewModel()
        {
            model.PropertyChanged +=
                delegate (Object sender, PropertyChangedEventArgs e) {
                    // Add a delegate function to update the airplane location object from lat/lon values
                    switch (e.PropertyName)
                    {
                        case "latitude":
                            model.Location = new Location(model.getFlightValue("latitude"), model.Location.Longitude);
                            NotifyPropertyChanged("VM_Location");
                            return;
                        case "longtitude":
                            model.Location = new Location(model.getFlightValue("longtitude"), model.Location.Longitude);
                            NotifyPropertyChanged("VM_Location");
                            return;
                    }

                    // For values other than location
                    NotifyPropertyChanged("VM_" + e.PropertyName);
                };
        }

    }
}

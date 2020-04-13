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
    public class MapViewModel : BaseNotify
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
                    Double value;
                    switch (e.PropertyName)
                    {
                        case "latitude":
                            value = model.GetFlightValue("latitude");
                            if (value > 90) value = 90;
                            if (value < -90) value = -90;
                            model.Location = new Location(value, model.Location.Longitude);
                            NotifyPropertyChanged("VM_Location");
                            return;
                        case "longtitude":
                           value = model.GetFlightValue("longtitude");
                            if (value > 180) value = 180;
                            if (value < -180) value = -180;
                            model.Location = new Location(value, model.Location.Longitude);
                            NotifyPropertyChanged("VM_Location");
                            return;
                    }

                    // For values other than location
                    NotifyPropertyChanged("VM_" + e.PropertyName);
                };
        }

    }
}

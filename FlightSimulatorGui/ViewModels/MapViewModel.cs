using FlightSimulatorApp.Model;
using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using FlightSimulatorApp.server;

namespace FlightSimulatorApp.ViewModel
{
    public class MapViewModel : BaseNotify
    {
        public Location VMLocation
        {
            get { return Model.Location; }
        }
        public Double VMErrorEnabled
        {
            get { return Model.ErrorEnabled; }
        }      
        public String VMErrorMsg
        {
            get { return Model.ErrorMsg; }
        }


        public MapViewModel()
        {
            Model.PropertyChanged +=
                delegate (Object sender, PropertyChangedEventArgs e) {
                    // Add a delegate function to update the airplane location object from lat/lon values
                    Double value;
                    switch (e.PropertyName)
                    {
                        case "Latitude":
                            value = Model.GetFlightValue("Latitude");
                            if (value > 90) value = 90;
                            if (value < -90) value = -90;
                            Model.Location = new Location(value, Model.Location.Longitude);
                            NotifyPropertyChanged("VMLocation");
                            return;
                        case "Longtitude":
                           value = Model.GetFlightValue("Longtitude");
                            if (value > 180) value = 180;
                            if (value < -180) value = -180;
                            Model.Location = new Location(value, Model.Location.Longitude);
                            NotifyPropertyChanged("VMLocation");
                            return;
                    }

                    // For values other than location
                    NotifyPropertyChanged("VM" + e.PropertyName);
                };
        }

    }
}

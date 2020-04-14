using FlightSimulatorGui.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSimulatorGui.ViewModel
{
    public class DataViewViewModel : BaseNotify
    {
        public double VMHeading
        {
            get { return Math.Round(Model.GetFlightValue("Heading"), 3); }
        }
        public double VMVerticalSpeed
        {
            get { return Math.Round(Model.GetFlightValue("VerticalSpeed"), 3); }
        }
        public double VMGroundSpeed
        {
            get { return Math.Round(Model.GetFlightValue("GroundSpeed"), 3); }
        }
        public double VMAirSpeed
        {
            get { return Math.Round(Model.GetFlightValue("AirSpeed"), 3); }
        }
        public double VMAltitude
        {
            get { return Math.Round(Model.GetFlightValue("Altitude"), 3); }
        }
        public double VMRoll
        {
            get { return Math.Round(Model.GetFlightValue("Roll"), 3); }
        }
        public double VMPitch
        {
            get { return Math.Round(Model.GetFlightValue("Pitch"), 3); }
        }
        public double VMAltimeter
        {
            get { return Math.Round(Model.GetFlightValue("Altimeter"), 3); }
        }

        public DataViewViewModel()
        {
            Model.PropertyChanged +=
                delegate (Object sender, PropertyChangedEventArgs e) {
                    NotifyPropertyChanged("VM" + e.PropertyName);
                };
        }
    }
}

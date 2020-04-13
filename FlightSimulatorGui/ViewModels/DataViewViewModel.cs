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
        public double VM_heading
        {
            get { return Math.Round(model.GetFlightValue("heading"), 3); }
        }
        public double VM_vertical_speed
        {
            get { return Math.Round(model.GetFlightValue("vertical_speed"), 3); }
        }
        public double VM_ground_speed
        {
            get { return Math.Round(model.GetFlightValue("ground_speed"), 3); }
        }
        public double VM_air_speed
        {
            get { return Math.Round(model.GetFlightValue("air_speed"), 3); }
        }
        public double VM_altitude
        {
            get { return Math.Round(model.GetFlightValue("altitude"), 3); }
        }
        public double VM_roll
        {
            get { return Math.Round(model.GetFlightValue("roll"), 3); }
        }
        public double VM_pitch
        {
            get { return Math.Round(model.GetFlightValue("pitch"), 3); }
        }
        public double VM_altimeter
        {
            get { return Math.Round(model.GetFlightValue("altimeter"), 3); }
        }

        public DataViewViewModel()
        {
            model.PropertyChanged +=
                delegate (Object sender, PropertyChangedEventArgs e) {
                    NotifyPropertyChanged("VM_" + e.PropertyName);
                };
        }
    }
}

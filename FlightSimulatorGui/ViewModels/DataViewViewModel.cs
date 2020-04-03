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
            get { return Math.Round(model.getFlightValue("heading"), 3); }
        }
        public double VM_vertical_speed
        {
            get { return Math.Round(model.getFlightValue("vertical_speed"), 3); }
        }
        public double VM_ground_speed
        {
            get { return Math.Round(model.getFlightValue("ground_speed"), 3); }
        }
        public double VM_air_speed
        {
            get { return Math.Round(model.getFlightValue("air_speed"), 3); }
        }
        public double VM_altitude
        {
            get { return Math.Round(model.getFlightValue("altitude"), 3); }
        }
        public double VM_roll
        {
            get { return Math.Round(model.getFlightValue("roll"), 3); }
        }
        public double VM_pitch
        {
            get { return Math.Round(model.getFlightValue("pitch"), 3); }
        }
        public double VM_altimeter
        {
            get { return Math.Round(model.getFlightValue("altimeter"), 3); }
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

using FlightSimulatorGui.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSimulatorGui.ViewModel
{
    class DataViewViewModel : BaseNotify
    {
        public double VM_heading
        {
            get { return model.getFlightValue("heading"); }
        }
        public double VM_vertical_speed
        {
            get { return model.getFlightValue("vertical_speed"); }
        }
        public double VM_ground_speed
        {
            get { return model.getFlightValue("ground_speed"); }
        }
        public double VM_air_speed
        {
            get { return model.getFlightValue("air_speed"); }
        }
        public double VM_altitude
        {
            get { return model.getFlightValue("altitude"); }
        }
        public double VM_roll
        {
            get { return model.getFlightValue("roll"); }
        }
        public double VM_pitch
        {
            get { return model.getFlightValue("pitch"); }
        }
        public double VM_altimeter
        {
            get { return model.getFlightValue("altimeter"); }
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

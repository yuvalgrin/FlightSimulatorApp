using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSimulatorApp.ViewModel
{
    public class ControlRoomViewModel : BaseNotify
    {
        public ControlRoomViewModel()
        {
            Model.PropertyChanged +=
                delegate (Object sender, PropertyChangedEventArgs e) {
                    NotifyPropertyChanged("VM" + e.PropertyName);
                };
        }

        public String VMQueryRes
        {
            get { return Model.QueryRes; }
            set
            {
                NotifyPropertyChanged("VMQueryRes");
            }
        }

        public void QueryUpdate(String query)
        {
            VMQueryRes = "Loading...";
            Model.ExecuteCtrlRoomQuery(query);
        }
    }
}

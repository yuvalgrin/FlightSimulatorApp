using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSimulatorGui.ViewModel
{
    public class ControlRoomViewModel : BaseNotify
    {
        public ControlRoomViewModel()
        {
            Model.PropertyChanged +=
                delegate (Object sender, PropertyChangedEventArgs e) {
                    NotifyPropertyChanged("VM_" + e.PropertyName);
                };
        }

        public String VM_QueryRes
        {
            get { return Model.QueryRes; }
            set
            {
                NotifyPropertyChanged("VM_QueryRes");
            }
        }

        public void QueryUpdate(String query)
        {
            VM_QueryRes = "Loading...";
            Model.ExecuteCtrlRoomQuery(query);
        }
    }
}

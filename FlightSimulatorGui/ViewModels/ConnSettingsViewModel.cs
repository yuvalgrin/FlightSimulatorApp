using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSimulatorGui.ViewModel
{
    public class ConnSettingsViewModel : BaseNotify
    {
        public ConnSettingsViewModel()
        {
            model.PropertyChanged +=
                delegate (Object sender, PropertyChangedEventArgs e) {
                    NotifyPropertyChanged("VM_" + e.PropertyName);
                };
        }

        public String VM_ConnRes
        {
            get { return model.ConnRes; }
            set
            {
                NotifyPropertyChanged("VM_ConnRes");
            }
        }

        public void queryUpdate(String ip, String port)
        {
            VM_ConnRes = "Loading...";
            model.ExecuteSwitchServer(ip, port);
        }
    }
}

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
            Model.PropertyChanged +=
                delegate (Object sender, PropertyChangedEventArgs e) {
                    NotifyPropertyChanged("VM" + e.PropertyName);
                };
        }

        public String VMConnRes
        {
            get { return Model.ConnRes; }
            set
            {
                NotifyPropertyChanged("VMConnRes");
            }
        }

        public void QueryUpdate(String ip, String port)
        {
            VMConnRes = "Loading...";
            Model.ExecuteSwitchServer(ip, port);
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSimulatorGui.ViewModel
{
    public class ConnSettingsViewModel : BaseNotify
    {
        String VM_ConnRes
        {
            set
            {
                NotifyPropertyChanged("VM_ConnRes");
            }
        }

        public void queryUpdate(String ip, String port)
        {
            VM_ConnRes = "Loading...";
            //model.executeCtrlRoomQuery(ip, port);
        }
    }
}
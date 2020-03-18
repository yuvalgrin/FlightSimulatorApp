using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSimulatorGui.server
{
    // Runs a thread that accesses the FS and:
    // 1. Executes the command from DB manager queue
    // 2. Read data from server and update DB manager map
    class FSConnector
    {
        public static void initlizeBackgroundThread() { }

        public void runCommandToFS() { }

        public void updateDataFromFS() { }

    }
}

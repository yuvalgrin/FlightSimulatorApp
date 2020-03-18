using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSimulatorGui.server
{
    // Hold the data from FS
    // Hold and update queue of queries to be sent
    // Get updates from FS into the data map
    class DatabaseManager
    {
        private Queue<Command> queue;

        public DatabaseManager(){
            this.queue = new Queue<Command>();
        }

        public Queue<Command> getCommandsQueue() { return null; }

        // If set command had value more than max (same for less than min) put the closest valid value
        public void addCommandToQueue(Command c) { return; }

        // before sending to queue
        public string createSetCommand(string request) { return null; }


        // If value is error (equals ERR) throw exception?
        public string getDataByKey(string key) { return null; }
        public void putDataByKey(string key, string value) { return; }

    }
}

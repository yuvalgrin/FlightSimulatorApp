using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSimulatorGui.Model
{
    // Hold the data from FS
    // Hold and update queue of queries to be sent
    // Get updates from FS into the data map
    class DatabaseManager
    {
        private static DatabaseManager instance = null;
        private Queue<Command> queue;

        private DatabaseManager(){
            this.queue = new Queue<Command>();
        }

        public static DatabaseManager get()
        {
            if (instance == null)
                instance = new DatabaseManager();
            return instance;
        }

        public Queue<Command> getCommandsQueue() { return null; }

        // If set command had value more than max (same for less than min) put the closest valid value
        public void addCommandToQueue(Command c) { return; }

        // If value is error (equals ERR) throw exception?
        public string getDataByKey(string key) { return null; }
        public void putDataByKey(string key, string value) { return; }

    }
}

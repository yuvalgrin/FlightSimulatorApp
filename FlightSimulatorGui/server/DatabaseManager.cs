using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;

namespace FlightSimulatorGui.Model
{
    // Hold the data from FS
    // Hold and update queue of queries to be sent
    // Get updates from FS into the data map
    class DatabaseManager
    {
        private static DatabaseManager instance = null;
        private  Queue<Command> queue;
        private  Dictionary<string, string> valueMap;

        private DatabaseManager()
        {
            //holds commands coming from gui
            this.queue = new Queue<Command>();
            // map that holds the values of the FS
            this.valueMap = new Dictionary<string, string>();
            this.parseXml();
        }

        public static DatabaseManager get()
        {
            if (instance == null)
                instance = new DatabaseManager();
            return instance;
        }

        //incharge of the update of the values and the view model
        public void updateValueMap(List<string> values) 
        {
            for (int i = 0; i < 36; i++)
            {
                if (values.ElementAt(i) != this.valueMap.ElementAt(i).Value)
                {
                    this.valueMap[this.valueMap.ElementAt(i).Key] = values.ElementAt(i);
                    //this.notify();
                }
            }
        }

        public Queue<Command> getCommandsQueue() { return null; }

        public Dictionary<string, string> getValueMap()
        {
            return this.valueMap;
        }

        // If set command had value more than max (same for less than min) put the closest valid value
        public void addCommandToQueue(Command c) { return; }

        // before sending to queue
        public Command createSetCommand(string request) { return null; }

        //parse xml from last semeter
        public void parseXml()
        {
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load("values.xml");
            }
            catch
            {
                Console.WriteLine("nope");
            }

            
            XmlNodeReader nodeReader = new XmlNodeReader(doc);
            XmlReaderSettings settings = new XmlReaderSettings();
            XmlReader reader = XmlReader.Create(nodeReader, settings);
            int i = 0;
            while (reader.Read())
            {

                if (reader.Name == "name")
                {
                    reader.Read();
                    if (reader.Value != string.Empty)
                    {
                        valueMap.Add(reader.Value, "0.0");
                    }

                }
            }

        }



        // If value is error (equals ERR) throw exception?
        public string getDataByKey(string key) 
        {
            return valueMap[key]; 
        }
        public void putDataByKey(string key, string value) 
        {
            valueMap[key] = value;
        }
    }
    
}

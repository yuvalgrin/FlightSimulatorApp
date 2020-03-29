using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Threading;
using System.ComponentModel;

namespace FlightSimulatorGui.Model
{
    // Hold the data from FS
    // Hold and update queue of queries to be sent
    // Get updates from FS into the data map
    class FlightSimulatorModel
    {
        private static FlightSimulatorModel instance = null;
        private  Queue<Command> queue;
        private  Dictionary<string, string> valueMap;
        public event PropertyChangedEventHandler PropertyChanged;

        private FlightSimulatorModel()
        {
            //holds commands coming from gui
            this.queue = new Queue<Command>();
            // map that holds the values of the FS
            this.valueMap = new Dictionary<string, string>();
            this.parseXml();
        }

        public static FlightSimulatorModel get()
        {
            if (instance == null)
                instance = new FlightSimulatorModel();
            return instance;
        }




        //incharge of the update of the values and the view model
        public void updateValueMap(string key, string newValue) 
        {
            if (newValue != this.valueMap[key])
            {
                this.valueMap[key] = newValue;
            }
            NotifyPropertyChanged(key);
        }

        public Queue<Command> getCommandsQueue() { return this.queue; }

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

                if (reader.Name == "node")
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
        public void sendCommandsToQueue()
        {
            while (true)
            {
                foreach (string key in this.valueMap.Keys)
                {
                    Command c = new GetCommand(key);
                    this.queue.Enqueue(c);
                }
                Thread.Sleep(500);
            }
        }




        public void runBackground()
        {
            Thread getCommand = new Thread(sendCommandsToQueue);
            getCommand.Start();
            
            MyTcpClient client = new MyTcpClient();
            Thread clientThread = new Thread(client.createAndRunClient);
            clientThread.Start();
        }

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }


    }

    
    
}

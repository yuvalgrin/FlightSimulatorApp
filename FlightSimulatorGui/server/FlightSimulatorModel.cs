using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Threading;
using System.ComponentModel;
using Microsoft.Maps.MapControl.WPF;
using System.Net.Sockets;
using FlightSimulatorGui.Model;
using System.Configuration;

namespace FlightSimulatorGui.Model
{
    // Hold the data from FS
    // Hold and update queue of queries to be sent
    // Get updates from FS into the data map
    public class FlightSimulatorModel
    {
        private static FlightSimulatorModel instance = null;
        private  Queue<Command> queue;
        private  Dictionary<string, string> valueMap;
        private Dictionary<string, string> settingsMap;
        public event PropertyChangedEventHandler PropertyChanged;

        // Init default location.
        public static double defaultLat = 31.643854;
        public static double defaultLon = 34.920341;
        public Location Location = new Location(defaultLat, defaultLon);

        public String QueryRes
        {
            get { return QueryRes; }
            set
            {
                QueryRes = value;
                NotifyPropertyChanged("QueryRes");
            }
        }
        public String ConnRes
        {
            get { return ConnRes; }
            set
            {
                ConnRes = value;
                NotifyPropertyChanged("ConnRes");
            }
        }

        private FlightSimulatorModel()
        {
            //holds commands coming from gui
            this.queue = new Queue<Command>();
            // map that holds the values of the FS
            this.valueMap = new Dictionary<string, string>();
            this.settingsMap = new Dictionary<string, string>();
            this.parseXml();
            PropertyChanged += notifyUpdate;
        }
        
        // Check if a property was updated > if so notify the ViewModel
        public void notifyUpdate(Object sender, PropertyChangedEventArgs e)
        {
            // Add a delegate function to update the airplane location object from lat/lon values
            switch (e.PropertyName)
            {
                case "latitude":
                    Location = new Location(getFlightValue("latitude"), Location.Longitude);
                    NotifyPropertyChanged("VM_Location");
                    break;
                case "longtitude":
                    Location = new Location(getFlightValue("latitude"), Location.Longitude);
                    NotifyPropertyChanged("VM_Location");
                    break;
            }

            // For values other than location
            NotifyPropertyChanged("VM_" + e.PropertyName);
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
            NotifyPropertyChanged(this.settingsMap[key]);
        }

        // Get the flight stats from the data map using only the referernce name (and not the long coded name)
        public double getFlightValue(String valueRef)
        {
            try
            {
                String val = this.valueMap[settingsMap[valueRef]];
                return Double.Parse(val);
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        public Queue<Command> getCommandsQueue() { return this.queue; }

        public Dictionary<string, string> getValueMap()
        {
            return this.valueMap;
        }

        //Execute a query from the control room and update the value via ViewModel
        public void executeCtrlRoomQuery(String query)
        {
            Command cmd = Command.parseRawCommand(query);
            if (cmd == null)
                QueryRes = "ERR";

            if (cmd is SetCommand)
                addCommandToQueue(cmd);

            QueryRes = cmd.getValue();
        }

        //Execute a switch server from the Connection Settings View Model
        public void executeSwitchServer(String ip, String port)
        {
            ConnRes = switchServer(ip, port);
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
            string name = "";
            while (reader.Read())
            {

                if (reader.Name == "name")
                {
                    reader.Read();
                    if (reader.Value != string.Empty)
                    {
                        name = reader.Value;
                    }

                }

                if (reader.Name == "node")
                {
                    reader.Read();
                    if (reader.Value != string.Empty)
                    {
                        valueMap.Add(reader.Value, "0.0");
                        settingsMap[reader.Value] = name;
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
            while (MyTcpClient.getRunning())
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
            NetworkStream stream = client.initializeConnection(null, null);
            Thread clientThread = new Thread(() => client.createAndRunClient(stream));
            clientThread.Start();
        }

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        public void exitProgram()
        {
            MyTcpClient.killClient();
        }

        public string switchServer(string ip, string port)
        {
            string reply = String.Empty;
            MyTcpClient client = new MyTcpClient();
            NetworkStream stream = client.initializeConnection(ip, port);
            if (stream == null)
                reply = "Could not connect to the given ip and port";
            else
            {
                MyTcpClient.killClient();
                Thread clientThread = new Thread(() => client.createAndRunClient(stream));
                clientThread.Start();
                reply = "Connected succefully to the new server";
            }
            return reply;
        }
    }
}

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
using System.IO;
using FlightSimulatorGui.server;
using System.Windows;
using FlightSimulatorGui.Models;

namespace FlightSimulatorGui.Model
{
    // Hold the data from FS
    // Hold and update queue of queries to be sent
    // Get updates from FS into the data map
    public class FlightSimulatorModel
    {
        private static FlightSimulatorModel instance = null;
        private  Queue<Command> queue;
        private  Dictionary<string, string> flightData;
        private Thread sendCommandsToQ;
        public event PropertyChangedEventHandler PropertyChanged;

        // Init default location.
        public static double defaultLat = 31.643854;
        public static double defaultLon = 34.920341;
        public Location Location = new Location(defaultLat, defaultLon);

        public String _QueryRes;
        public String QueryRes
        {
            get { return _QueryRes; }
            set
            {
                _QueryRes = value;
                NotifyPropertyChanged("QueryRes");
            }
        }
        public String _ConnRes;
        public String ConnRes
        {
            get { return _ConnRes; }
            set
            {
                _ConnRes = value;
                NotifyPropertyChanged("ConnRes");
            }
        } 
        private String _ErrorMsg;
        public String ErrorMsg
        {
            get { return _ErrorMsg; }
            set
            {
                _ErrorMsg = value;
                ErrorEnabled = 1;
                DelayedExecutionService.DelayedExecute(() => ErrorEnabled = 0);
                NotifyPropertyChanged("ErrorMsg");
            }
        }
        private Double _ErrorEnabled;
        public Double ErrorEnabled
        {
            get { return _ErrorEnabled; }
            set
            {
                _ErrorEnabled = value;
                NotifyPropertyChanged("ErrorEnabled");
            }
        }

        private FlightSimulatorModel()
        {
            this.sendCommandsToQ = new Thread(sendCommandsToQueue);
            //holds commands coming from gui
            this.queue = new Queue<Command>();
            // map that holds the values of the FS
            this.flightData = new Dictionary<string, string>()
            {
                {"/instrumentation/airspeed-indicator/indicated-speed-kt", "0.0"},
                {"/instrumentation/altimeter/indicated-altitude-ft", "0.0"},
                {"/instrumentation/attitude-indicator/internal-pitch-deg", "0.0"},
                {"/instrumentation/attitude-indicator/internal-roll-deg", "0.0"},
                {"/instrumentation/gps/indicated-altitude-ft", "0.0"},
                {"/instrumentation/gps/indicated-ground-speed-kt", "0.0"},
                {"/instrumentation/gps/indicated-vertical-speed", "0.0"},
                {"/instrumentation/heading-indicator/indicated-heading-deg", "0.0"},
                {"/controls/flight/aileron", "0.0"},
                {"/controls/flight/elevator", "0.0"},
                {"/controls/flight/rudder", "0.0"},
                {"/controls/engines/current-engine/throttle", "0.0"},
                {"/position/latitude-deg", "0.0"},
                {"/position/longitude-deg", "0.0"}
            };
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
            Double res;
            if (newValue != this.flightData[key])
            {
                bool isValidVal = Double.TryParse(newValue, out res);
                if (isValidVal)
                {
                    this.flightData[key] = newValue;
                    NotifyPropertyChanged(FlightSimulatorResources.fullNameToShort[key]);
                } else
                {
                    throwNewError("Recieved invalid value from Flight Simulator");
                }
            }
        }

        // Get the flight stats from the data map using only the referernce name (and not the long coded name)
        public double getFlightValue(String valueRef)
        {
            Double res;
            String val = this.flightData[FlightSimulatorResources.shortNameToFull[valueRef]];
            bool isValidVal = Double.TryParse(val, out res);

            if (isValidVal)
                return res;

            throwNewError("Recieved invalid value from Flight Simulator");
            return 0;
        }

        public Queue<Command> getCommandsQueue() { return this.queue; }

        public Dictionary<string, string> getValueMap()
        {
            return this.flightData;
        }

        //Execute a query from the control room and update the value via ViewModel
        public void executeCtrlRoomQuery(String query)
        {
            StringBuilder result = new StringBuilder();
            StringReader sr = new StringReader(query);
            String row;
            while ((row = sr.ReadLine()) != null)
            {
                Command cmd = Command.parseRawCommand(row);
                if (cmd == null)
                {
                    QueryRes = "ERR";
                    throwNewError("Invalid command was entered in Control Room");
                    return;
                }

                if (cmd is SetCommand)
                    addCommandToQueue(cmd);

                result.Append(cmd.getValue()).Append("  ");
            }
            QueryRes = result.ToString();
        }

        //Execute a switch server from the Connection Settings View Model
        public void executeSwitchServer(String ip, String port)
        {
            ConnRes = switchServer(ip, port);
        }

        // If set command had value more than max (same for less than min) put the closest valid value
        public void addCommandToQueue(Command c) 
        {
            this.queue.Enqueue(c);        
        }

        public string getDataByKey(string key) 
        {
            return flightData[key]; 
        }
        public void putDataByKey(string key, string value) 
        {
            flightData[key] = value;
        }
        public void sendCommandsToQueue()
        {
            while (MyTcpClient.getRunning())
            {
                foreach (string key in FlightSimulatorResources.fullNameToShort.Keys)
                {
                    Command c = new GetCommand(key);
                    this.queue.Enqueue(c);
                }
                Thread.Sleep(1000);
            }
        }


        public void initRunBackground()
        {
            ErrorEnabled = 0;
            String error = runBackground();
            if (error != null)
            {
                throwNewError(error + "\r\nWill try to reconnect in 5 seconds");
                DelayedExecutionService.DelayedExecute(() => initRunBackground(), 5000);
            }
        }

        public String runBackground()
        {
            MyTcpClient client = new MyTcpClient();
            NetworkStream stream = client.initializeConnection(null, null);
            if (stream == null)
            {
                return "Could not connect to the default server";
            }

            if (!sendCommandsToQ.IsAlive)
                sendCommandsToQ.Start();

            Thread clientThread = new Thread(() => client.createAndRunClient(stream));
            clientThread.Start();
            return null;
        }

        void OnDispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            throwNewError(e.Exception.Message);
            e.Handled = true;
        }

        public void throwNewError(String msg)
        {
            ErrorMsg = msg;
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
            if (String.IsNullOrEmpty(ip) || String.IsNullOrEmpty(port))
            {
                reply = "Invalid IP or Port inserted";
                throwNewError(reply);
                return reply;
            }
            MyTcpClient client = new MyTcpClient();
            NetworkStream stream = client.initializeConnection(ip, port);
            if (stream == null)
            {
                reply = "Could not connect to the given IP and Port";
                throwNewError(reply);
                return reply;
            }
            else
            {
                if (!sendCommandsToQ.IsAlive)
                    sendCommandsToQ.Start();

                if (MyTcpClient.threadAlreadyRunning)
                    MyTcpClient.killClient();
                MyTcpClient.m.WaitOne();
                Thread clientThread = new Thread(() => client.createAndRunClient(stream));
                clientThread.Start();
                MyTcpClient.threadAlreadyRunning = true;
                reply = "Connected succefully to the new server";
            }
            return reply;
        }
    }
}

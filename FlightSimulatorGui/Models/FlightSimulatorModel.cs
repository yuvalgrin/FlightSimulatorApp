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
    // Hold the data from FS.
    // Hold and update queue of queries to be sent
    // Get updates from FS into the data map
    public class FlightSimulatorModel
    {
        private static FlightSimulatorModel _instance = null;
        private Thread _commandsQueueThread;

        private Queue<Command> _priorityQueue;
        public Queue<Command> PriorityQueue
        {
            get { return _priorityQueue; }
        }
        private Queue<Command> _queue;
        public Queue<Command> Queue
        {
            get { return _queue; }
        }
        private Dictionary<string, string> _flightData;
        public Dictionary<string, string> FlightData
        {
            get { return _flightData; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public Location Location { get; set; }

        private String _queryRes;
        public String QueryRes
        {
            get { return _queryRes; }
            set
            {
                _queryRes = value;
                NotifyPropertyChanged("QueryRes");
            }
        }
        private String _connRes;
        public String ConnRes
        {
            get { return _connRes; }
            set
            {
                _connRes = value;
                NotifyPropertyChanged("ConnRes");
            }
        }
        private String _errorMsg;
        public String ErrorMsg
        {
            get { return _errorMsg; }
            set
            {
                _errorMsg = value;
                ErrorEnabled = 1;
                DelayedExecutionService.DelayedExecute(() => ErrorEnabled = 0);
                NotifyPropertyChanged("ErrorMsg");
            }
        }
        private Double _errorEnabled;
        public Double ErrorEnabled
        {
            get { return _errorEnabled; }
            set
            {
                _errorEnabled = value;
                NotifyPropertyChanged("ErrorEnabled");
            }
        }

        private FlightSimulatorModel()
        {
            // Init default location 
            Location = new Location(31.643854, 34.920341);
            //Init the thread that sends commands to queue
            this._commandsQueueThread = new Thread(SendCommandsToQueue);
            //holds commands coming from gui
            this._priorityQueue = new Queue<Command>();
            this._queue = new Queue<Command>();
            // map that holds the values of the FS
            this._flightData = new Dictionary<string, string>()
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

        public static FlightSimulatorModel Get()
        {
            if (_instance == null)
                _instance = new FlightSimulatorModel();
            return _instance;
        }

        //incharge of the update of the values and the view model
        public void UpdateValueMap(string key, string newValue)
        {
            if (newValue != this._flightData[key])
            {
                this._flightData[key] = newValue;
            }
            NotifyPropertyChanged(FlightSimulatorResources.FullNameToShort[key]);
        }

        // Get the flight stats from the data map using only the referernce name (and not the long coded name)
        public double GetFlightValue(String valueRef)
        {
            Double res;
            String val = this._flightData[FlightSimulatorResources.ShortNameToFull[valueRef]];
            bool isValidVal = Double.TryParse(val, out res);

            if (isValidVal)
                return res;

            return 0;
        }


        //Execute a query from the control room and update the value via ViewModel
        public void ExecuteCtrlRoomQuery(String query)
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
                    ThrowNewError("Invalid command was entered in Control Room");
                    return;
                }

                if (cmd is SetCommand)
                    AddCommandToPriorityQueue(cmd);

                result.Append(cmd.getValue()).Append("  ");
            }
            QueryRes = result.ToString();
        }

        //Execute a switch server from the Connection Settings View Model
        public void ExecuteSwitchServer(String ip, String port)
        {
            ConnRes = SwitchServer(ip, port);
        }

        // If set command had value more than max (same for less than min) put the closest valid value
        public void AddCommandToQueue(Command c)
        {
            this._queue.Enqueue(c);
        }

        // If set command had value more than max (same for less than min) put the closest valid value
        public void AddCommandToPriorityQueue(Command c)
        {
            this._priorityQueue.Enqueue(c);
        }

        public string GetDataByKey(string key)
        {
            return _flightData[key];
        }
        public void PutDataByKey(string key, string value)
        {
            _flightData[key] = value;
        }
        public void SendCommandsToQueue()
        {
            while (MyTcpClient.RunClient)
            {
                foreach (string key in FlightSimulatorResources.FullNameToShort.Keys)
                {
                    Command c = new GetCommand(key);
                    this._queue.Enqueue(c);
                }
                Thread.Sleep(1000);
            }
        }


        public void InitRunBackground()
        {
            ErrorEnabled = 0;
            String error = RunBackground();
            if (error != null)
            {
                ThrowNewError(error + "\r\nWill try to reconnect in 5 seconds");
                DelayedExecutionService.DelayedExecute(() => InitRunBackground(), 5000);
            }
        }

        public String RunBackground()
        {
            MyTcpClient client = new MyTcpClient();
            NetworkStream stream = client.InitializeConnection(null, null);
            if (stream == null)
            {
                return "Could not connect to the default server";
            }
            if (!_commandsQueueThread.IsAlive)
                _commandsQueueThread.Start();

            Thread clientThread = new Thread(() => client.CreateAndRunClient(stream));
            clientThread.Start();
            return null;
        }

        void OnDispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            ThrowNewError(e.Exception.Message);
            e.Handled = true;
        }

        public void ThrowNewError(String msg)
        {
            ErrorMsg = msg;
        }

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        public void ExitProgram()
        {
            MyTcpClient.KillClient();
        }

        public string SwitchServer(string ip, string port)
        {
            string reply = String.Empty;
            if (String.IsNullOrEmpty(ip) || String.IsNullOrEmpty(port))
            {
                reply = "Invalid IP or Port inserted";
                ThrowNewError(reply);
                return reply;
            }
            MyTcpClient client = new MyTcpClient();
            NetworkStream stream = client.InitializeConnection(ip, port);
            if (stream == null)
            {
                reply = "Could not connect to the given IP and Port";
                ThrowNewError(reply);
                return reply;
            }
            else
            {
                if (!_commandsQueueThread.IsAlive)
                    _commandsQueueThread.Start();

                if (MyTcpClient.ThreadAlreadyRunning)
                    MyTcpClient.KillClient();
                MyTcpClient.M.WaitOne();
                Thread clientThread = new Thread(() => client.CreateAndRunClient(stream));
                clientThread.Start();
                MyTcpClient.ThreadAlreadyRunning = true;
                reply = "Connected succefully to the new server";
            }
            return reply;
        }
    }
}
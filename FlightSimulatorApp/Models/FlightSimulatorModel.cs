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
using FlightSimulatorApp.Model;
using System.Configuration;
using System.IO;
using FlightSimulatorApp.server;
using System.Windows;
using FlightSimulatorApp.Models;

namespace FlightSimulatorApp.Model
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
            this._flightData = new Dictionary<string, string>();
            foreach (string key in FlightSimulatorResources.FullNameToShort.Keys)
            {
                this._flightData[key] = "0.0";
            }
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
            StringBuilder result = new StringBuilder("");
            using (StringReader sr = new StringReader(query))
            {
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
            }
            QueryRes = result.ToString();
        }

        //Execute a switch server from the Connection Settings View Model
        public void ExecuteSwitchServer(String ip, String port)
        {
            ConnRes = SwitchServer(ip, port);
        }

        // Add the command to queue only when connected to a server
        public void AddCommandToQueue(Command c)
        {
            try
            {
                if (MyTcpClient.ThreadAlreadyRunning && _commandsQueueThread.IsAlive)
                    this._queue.Enqueue(c);
            } finally
            {
                // Error during enqueue operation
            }
        }

        // Add the command to queue only when connected to a server
        // This queue is prioritized for control room commands
        public void AddCommandToPriorityQueue(Command c)
        {
            try
            {
                if (MyTcpClient.ThreadAlreadyRunning && _commandsQueueThread.IsAlive)
                    this._priorityQueue.Enqueue(c);
            } finally
            {
                // Error during enqueue operation
            }
        }

        public string GetDataByKey(string key)
        {
            return _flightData[key];
        }
        public void PutDataByKey(string key, string value)
        {
            _flightData[key] = value;
        }

        // Sends commands to queue with a time interval
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

        //Init the first connection to the server wrapper
        public void InitRunBackground()
        {
            ErrorEnabled = 0;
            String error = RunBackground();
            if (error != null)
            {
                ThrowNewError(error + "\r\nPlease insert your required ip and port.");
                //DelayedExecutionService.DelayedExecute(() => InitRunBackground(), 3000);
            }
        }

        //Init the first connection to the server logic
        public String RunBackground()
        {
            NetworkStream stream = MyTcpClient.InitializeConnection(null, null);
            if (stream == null)
            {
                return "Could not connect to the default server";
            }
            if (!_commandsQueueThread.IsAlive)
                _commandsQueueThread.Start();

            Thread clientThread = new Thread(() => MyTcpClient.CreateAndRunClient(stream));
            clientThread.Start();
            MyTcpClient.ThreadAlreadyRunning = true;
            return null;
        }

        // Connect to a different server
        public string SwitchServer(string ip, string port)
        {
            string reply = String.Empty;
            if (String.IsNullOrEmpty(ip) || String.IsNullOrEmpty(port))
            {
                reply = "Invalid IP or Port inserted";
                ThrowNewError(reply);
                return reply;
            }
            NetworkStream stream = MyTcpClient.InitializeConnection(ip, port);
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
                {
                    MyTcpClient.KillClient();
                    MyTcpClient.Mutex.WaitOne();
                }
                Thread clientThread = new Thread(() => MyTcpClient.CreateAndRunClient(stream));
                clientThread.Start();
                MyTcpClient.ThreadAlreadyRunning = true;
                reply = "Connected succefully to the new server";
            }
            return reply;
        }

        // Catch all the errors
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

        public static void ExitProgram()
        {
            MyTcpClient.KillClient();
        }
    }
}
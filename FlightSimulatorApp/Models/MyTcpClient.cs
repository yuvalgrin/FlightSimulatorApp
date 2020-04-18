using System;
using System.IO;
using System.Net.Sockets;
using FlightSimulatorApp.Model;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Threading;
using System.Text;
using System.Globalization;

public static class MyTcpClient
{
    private static bool _runClient = true;
    public static bool RunClient{ get { return _runClient; } }
    private static bool _threadAlreadyRunning = false;
    public static bool ThreadAlreadyRunning { get { return _threadAlreadyRunning; } set { _threadAlreadyRunning = value; } }
    private static AutoResetEvent _m = new AutoResetEvent(false);
    public static AutoResetEvent Mutex { get { return _m; } set { _m = value; } }
    
    private static string _ip = String.Empty;
    private static int _port = 0;

    public static NetworkStream InitializeConnection(string ip, string port)
    {
        Int32 connectionPort; string server;
        if (ip == null || port == null)
        {
            connectionPort = int.Parse(ConfigurationManager.AppSettings["ServerPort"], CultureInfo.InvariantCulture);
            server = ConfigurationManager.AppSettings["ServerIP"];
        } else
        {
            bool isValidPort = int.TryParse(port, out connectionPort);
            server = ip;
            if (!isValidPort)
                return null;
        }
        try
        {
            //Already connected to this ip port
            if (connectionPort == MyTcpClient._port && server == MyTcpClient._ip)
                return null;

            TcpClient tcpClient = new TcpClient(server, connectionPort);
            NetworkStream stream = tcpClient.GetStream();

            if (stream != null)
            {
                // Save the details because we openend a connection successfully
                MyTcpClient._port = connectionPort;
                MyTcpClient._ip = server;
            }
            return stream;
        }
        catch (Exception e)  when (e is ArgumentNullException || e is ArgumentOutOfRangeException || e is SocketException)
        {
            return null;
        }
    }


    //create a tcp server with the default _port and _ip
    public static void CreateAndRunClient(NetworkStream stream)
    {
        try
        {
            if (stream == null)
                return;

            Byte[] data = null;
            // Get a client stream for reading and writing.
            //  Stream stream = client.GetStream();
            if (!_runClient)
            {
                FlightSimulatorModel.Get().ThrowNewError("Oops! you got runclient problem");
                return;
            }
            while (_runClient)
            {
                // Translate the passed message into ASCII and store it as a Byte array.
                Command c = ChooseCommand();
                if (c == null)
                    continue;
                data = System.Text.Encoding.ASCII.GetBytes(c.execute());
                
                // Send the message to the connected TcpServer. 
                stream.Write(data, 0, data.Length);
                data = new Byte[256];

                // String to store the response ASCII representation.
                String responseData = String.Empty;

                // Read the first batch of the TcpServer response bytes.
                Thread.Sleep(30);
                stream.ReadTimeout = 15000;
                Int32 bytes = stream.Read(data, 0, data.Length);
                responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                responseData = responseData.Substring(0, responseData.Length - 1);
                FlightSimulatorModel.Get().UpdateValueMap(c.path(), responseData);
            }
            // Close everything
            stream.Close();
            //MyTcpClient.client.Close();
        }
        catch (IOException e)
        {
            if (stream != null)
                stream.Close();
            //MyTcpClient.client.Close();
            ThreadAlreadyRunning = false;
            FlightSimulatorModel.Get().ThrowNewError("Connection to the server was lost\r\nPlease insert IP and Port in the connection tab\r\n"+e.Message);
            _ip = String.Empty;
            _port = 0;
        }
        finally
        {
            _runClient = true;
            Mutex.Set();
        }
        
    }

    private static Command ChooseCommand()
    {
        if (FlightSimulatorModel.Get().PriorityQueue.Count != 0)
            return FlightSimulatorModel.Get().PriorityQueue.Dequeue();
        else if (FlightSimulatorModel.Get().Queue.Count != 0)
            return FlightSimulatorModel.Get().Queue.Dequeue();
        else
            return null;
    }

    //the function that will be run in a thread
    public static void KillClient()
    {
        MyTcpClient._runClient = false;
    }
    public static void SClient()
    {
        MyTcpClient._runClient = true;
    }

}

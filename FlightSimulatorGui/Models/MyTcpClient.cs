using System;
using System.IO;
using System.Net.Sockets;
using FlightSimulatorGui.Model;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Threading;

public class MyTcpClient
{
    public MyTcpClient()
    {
    }

    private static bool runClient = true;
    private static string ip = String.Empty;
    private static int port = 0;
    public static AutoResetEvent m = new AutoResetEvent(false);
    public static bool threadAlreadyRunning = true;



    public NetworkStream initializeConnection(string ip, string port)
    {
        Int32 connectionPort; string server;
        if (ip == null || port == null)
        {
            connectionPort = int.Parse(ConfigurationSettings.AppSettings["ServerPort"]);
            server = ConfigurationSettings.AppSettings["ServerIP"];
        } else
        {
            bool isValidPort = int.TryParse(port, out connectionPort);
            server = ip;
            if (!isValidPort)
                return null;
        }
        try
        {
            if (connectionPort == MyTcpClient.port && server == MyTcpClient.ip)
            {
                return null;
            }
            MyTcpClient.port = connectionPort;
            MyTcpClient.ip = server;
            TcpClient tcpClient = new TcpClient(server, connectionPort);
            NetworkStream stream = tcpClient.GetStream();
            return stream;
        }
        catch (Exception e)
        {
            return null;
        }
    }


    //create a tcp server with the default port and ip
    public void createAndRunClient(NetworkStream stream)
    {
        
        
        try
        {
            Byte[] data = null;
            // Get a client stream for reading and writing.
            //  Stream stream = client.GetStream();
            if (!runClient)
                throw new Exception("runclient problem");
            while (runClient)
            {
                // Translate the passed message into ASCII and store it as a Byte array.
                Command c = FlightSimulatorModel.get().getCommandsQueue().Dequeue();
                data = System.Text.Encoding.ASCII.GetBytes(c.execute());
                // Send the message to the connected TcpServer. 
                stream.Write(data, 0, data.Length);
                data = new Byte[256];

                // String to store the response ASCII representation.
                String responseData = String.Empty;

                // Read the first batch of the TcpServer response bytes.
                Thread.Sleep(30);
                stream.ReadTimeout = 1000;
                Int32 bytes = stream.Read(data, 0, data.Length);
                responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                responseData = responseData.Substring(0, responseData.Length - 1);
                FlightSimulatorModel.get().updateValueMap(c.path(), responseData);
            }
            // Close everything
            stream.Close();
            //MyTcpClient.client.Close();
        }
        catch (IOException e)
        {
            stream.Close();
            //MyTcpClient.client.Close();
            threadAlreadyRunning = false;
            FlightSimulatorModel.get().throwNewError("Connection to the server was lost\r\n Please insert IP and Port in the connection tab");
            ip = String.Empty;
            port = 0;
        }
        catch (Exception e)
        {
            FlightSimulatorModel.get().throwNewError(e.Message);
        }
        finally
        {
            runClient = true;
            m.Set();
        }
        
    }

    //the function that will be run in a thread
    public static void killClient()
    {
        MyTcpClient.runClient = false;
    }
    public static void sClient()
    {
        MyTcpClient.runClient = true;
    }

    public static bool getRunning()
    {
        return MyTcpClient.runClient;
    }

}

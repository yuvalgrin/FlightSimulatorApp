using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using FlightSimulatorGui.Model;
using System.Collections.Generic;

public class MyTcpServer
{
    private FlightSimulatorModel dbManager = FlightSimulatorModel.get();


    public MyTcpServer()
    {
    }

    private static bool runServer = true;


    //create a tcp server with the default port and ip
    public void createAndRunServer()
    {


        TcpListener server = null;
        try
        {
            // Set the TcpListener on port 5400.
            Int32 port = 5402;
            IPAddress localAddr = IPAddress.Parse("127.0.0.1");

            // TcpListener server = new TcpListener(port);
            server = new TcpListener(localAddr, port);

            // Start listening for client requests.
            server.Start();

            // Buffer for reading data
            Byte[] bytes = new Byte[256];
            String data = null;

            // Start listening loop.

            // Perform a blocking call to accept requests.
            // You could also user server.AcceptSocket() here.
            TcpClient client = server.AcceptTcpClient();


            data = null;

            // Get a stream object for reading and writing
            NetworkStream stream = client.GetStream();

            int i;
            List<string> storedValues = new List<string>();
            string[] values;
            // Loop to receive all the data sent by the client.
            while (runServer)
            {
                i = stream.Read(bytes, 0, bytes.Length);
                // Translate data bytes to a ASCII string.
                data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);

                //byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);
                values = data.Split(',');
                this.parseMsg(values, storedValues);
                if (storedValues.Count == 36)
                {
                    //FlightSimulatorModel.get().updateValueMap(storedValues);
                    storedValues.Clear();
                }
                //////push command to db
                //Command c = dbManager.createSetCommand(msg.ToString());
                //dbManager.addCommandToQueue(c);
            }

        }
        catch (SocketException e)
        {
            Console.WriteLine("SocketException: {0}", e);
        }
        finally
        {
            // Stop listening for new clients.
            server.Stop();
        }

        Console.WriteLine("\nHit enter to continue...");
        Console.Read();
    }


    //the function that will be run in a thread
    public static void killServer()
    {
        MyTcpServer.runServer = false;
    }

    public void setPortAndIp(int port, int ip) { return; }


    private void parseMsg(string[] msg, List<string> list)
    {
        
        int len = msg.Length;
        for (int i = 0; i < len; i++)
        {
            if (msg[i] != "\n")
            {
                list.Add(msg[i]);
            }
            else
            {
                break;
            }
        }
    }
}

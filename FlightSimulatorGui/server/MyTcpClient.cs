using System;
using System.Net.Sockets;
using FlightSimulatorGui.Model;
using System.Collections.Generic;
using System.Linq;

public class MyTcpClient
{
    public MyTcpClient()
    {
    }

    private static bool runClient = true;
    

    //create a tcp server with the default port and ip
    public void createAndRunClient()
    {
        try
        {
            // Create a TcpClient.
            // Note, for this client to work you need to have a TcpServer 
            // connected to the same address as specified by the server, port
            // combination.
            Int32 port = 5402;
            string server = "127.0.0.1";

            TcpClient client = new TcpClient(server, port);


            Byte[] data = null;
            // Get a client stream for reading and writing.
            //  Stream stream = client.GetStream();

            NetworkStream stream = client.GetStream();
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
                Int32 bytes = stream.Read(data, 0, data.Length);
                responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                responseData = responseData.Substring(0, responseData.Length - 1);
                FlightSimulatorModel.get().updateValueMap(c.path(), responseData);

                    
                Console.WriteLine("Received: {0}", responseData);
                    
                    
                    
                
                
            }
            // Close everything.
            stream.Close();
            client.Close();
        }
        catch (ArgumentNullException e)
        {
            Console.WriteLine("ArgumentNullException: {0}", e);
        }
        catch (SocketException e)
        {
            Console.WriteLine("SocketException: {0}", e);
        }
    }

    //the function that will be run in a thread
    public static void killClient()
    {
        MyTcpClient.runClient = false;
    }

}

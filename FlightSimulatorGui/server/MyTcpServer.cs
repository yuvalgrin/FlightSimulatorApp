﻿using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using FlightSimulatorGui.Model;

public class MyTcpServer
{
    private DatabaseManager dbManager = DatabaseManager.get();


    public MyTcpServer()
    {
    }


    //create a tcp server with the default port and ip
    public void createAndRunServer() {


            TcpListener server = null;
            try
            {
                // Set the TcpListener on port 5400.
                Int32 port = 5400;
                IPAddress localAddr = IPAddress.Parse("127.0.0.1");

                // TcpListener server = new TcpListener(port);
                server = new TcpListener(localAddr, port);

                // Start listening for client requests.
                server.Start();

                // Buffer for reading data
                Byte[] bytes = new Byte[256];
                String data = null;

                // Enter the listening loop.
                while (true)
                {
                    Console.Write("Waiting for a connection... ");

                    // Perform a blocking call to accept requests.
                    // You could also user server.AcceptSocket() here.
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("Connected!");

                    data = null;

                    // Get a stream object for reading and writing
                    NetworkStream stream = client.GetStream();

                    int i;

                    // Loop to receive all the data sent by the client.
                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        // Translate data bytes to a ASCII string.
                        data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);

                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

                        //////push command to db
                        Command c = Command.parseRawCommand(msg.ToString());
                        dbManager.addCommandToQueue(c);
                    


                        // Send back a response.
                    stream.Write(msg, 0, msg.Length);
                        Console.WriteLine("Sent: {0}", data);
                    }
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
    public void killServer() { return; }

    public void setPortAndIp(int port, int ip) { return; }
}
﻿using System.Net.Sockets;
using System.Net;
using System.Text;

namespace TcpServer
{
    public class TcpServer
    {
        public TcpListener server;
        private int port;
        private IPAddress ip;
        private byte[] bytes;

        public TcpServer(int port, string ip, int bytes)
        {
            this.port = port;
            this.ip = IPAddress.Parse(ip);
            this.server = new TcpListener(this.ip, this.port);
            this.bytes = new byte[bytes];
        }

        public void StartServer()
        {
            try
            {
                server.Start();

                // Enter the listening loop.
                while (true)
                {
                    Console.Write("Waiting for a connection... ");

                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine($"\n\n{client.Client.RemoteEndPoint} Connected!\n\n");

                    NetworkStream stream = client.GetStream();

                    byte[] msg = System.Text.Encoding.ASCII.GetBytes($"What is your name ? :)\n");
                    stream.Write(msg, 0, msg.Length);
                    Console.WriteLine($"Sent : {Encoding.Default.GetString(msg)}");

                    int i;
                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        String data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                        Console.WriteLine($"Received : {data}");

                        if(data.Contains("exit"))
                        {
                            break;
                        }

                        msg = System.Text.Encoding.ASCII.GetBytes($"Hello {data}\n");

                        stream.Write(msg, 0, msg.Length);
                        Console.WriteLine($"Sent : {Encoding.Default.GetString(msg)}");
                    }

                    client.Close();
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                server.Stop();
            }

            Console.WriteLine("\nHit enter to continue...");
            Console.Read();
        }
        

    }
}
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace TCPEchoServer
{
    internal class ServiceEcho
    {
        private readonly List<EchoService> echoServices = new List<EchoService>();
        private readonly TcpListener serverSocket;

        public ServiceEcho()
        {
            serverSocket = new TcpListener(IPAddress.Parse("127.0.0.1"), 80); //65080
        }

        public void run()
        {
            serverSocket.Start();

            var checkThread = new Thread(CheckConnections);
            checkThread.Start();

            while (true)
            {
                Console.WriteLine("Waiting for a new client...");
                var connectionSocket = serverSocket.AcceptTcpClient();

                Console.WriteLine("Server activated");

                try
                {
                    var echoService = new EchoService(connectionSocket);
                    echoServices.Add(echoService);
                    var thread = new Thread(echoService.DoIt);
                    thread.Start();
// OR A FACTORY
//                    Task.Factory.StartNew(echoService.DoIt);
                }
                catch (SocketException socketException)
                {
                    Debug.Write(socketException.StackTrace);
                }
                catch (IOException ioException)
                {
                    Debug.Write(ioException.StackTrace);
                }
            }

            serverSocket.Stop();
        }

        public void CheckConnections()
        {
            while (true)
            {
                var echoServicesRemove = new List<EchoService>();
                foreach (var echoService in echoServices)
                {
                    if (echoService.ConnectionSocket.Connected == false)
                    {
                        Console.WriteLine("Client " + echoService.clientNumber + " closed.");
                        echoServicesRemove.Add(echoService);
                    }
                }
                foreach (var echoService in echoServicesRemove)
                {
                    echoServices.Remove(echoService);
                }
                Thread.Sleep(100);
            }
        }
    }
}
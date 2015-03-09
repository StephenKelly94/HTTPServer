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
        private readonly List<EchoService> _echoServices = new List<EchoService>();
        private readonly TcpListener _serverSocket;

        public ServiceEcho()
        {
            _serverSocket = new TcpListener(IPAddress.Parse("127.0.0.1"), 80); //65080
        }

        public void Run()
        {
            _serverSocket.Start();

            var checkThread = new Thread(CheckConnections);
            checkThread.Start();

            while (true)
            {
                Console.WriteLine("Waiting for a new client...");
                var connectionSocket = _serverSocket.AcceptTcpClient();

                Console.WriteLine("Server activated");

                try
                {
                    var echoService = new EchoService(connectionSocket);
                    _echoServices.Add(echoService);
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

            _serverSocket.Stop();
        }

        public void CheckConnections()
        {
            while (true)
            {
                var echoServicesRemove = new List<EchoService>();
                foreach (var echoService in _echoServices)
                {
                    if (echoService.ConnectionSocket.Connected == false)
                    {
                        Console.WriteLine("Client " + echoService.ClientNumber + " closed.");
                        echoServicesRemove.Add(echoService);
                    }
                }
                foreach (var echoService in echoServicesRemove)
                {
                    _echoServices.Remove(echoService);
                }
                Thread.Sleep(100);
            }
        }
    }
}
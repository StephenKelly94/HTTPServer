using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace TCPEchoServer
{
    public class ServiceStart
    {
        private readonly List<EchoService> _echoServices = new List<EchoService>();
        private readonly TcpListener _serverSocket;
        private bool _isRunning = true;

        public ServiceStart()
        {
            _serverSocket = new TcpListener(IPAddress.Parse("127.0.0.1"), 80); //65080
        }

        public void Run()
        {
            _serverSocket.Start();

            //Wait for closing...
            Task.Run(new Action(WaitForEnter));

            var checkThread = new Thread(CheckConnections);
            checkThread.Start();

            while (_isRunning)
            {
                try
                {
                    Console.WriteLine("Waiting for a new client...");
                    var connectionSocket = _serverSocket.AcceptTcpClient();
                    Console.WriteLine("Server activated");
                    EchoService echoService = new EchoService(connectionSocket);
                    _echoServices.Add(echoService);
//                    Thread thread = new Thread(echoService.DoIt);
//                    thread.Start();
// OR A FACTORY
//                    Task.Factory.StartNew(echoService.DoIt);
                    Task.Run(new Action(echoService.DoIt));
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

        public void WaitForEnter()
        {
            Console.ReadLine();
            _isRunning = false;
            _serverSocket.Server.Close();
        }

        public void Kill()
        {
            _isRunning = false;
            _serverSocket.Server.Close();
        }

        public void CheckConnections()
        {
            while (_isRunning)
            {
                List<EchoService> echoServicesRemove = new List<EchoService>();
                foreach (var echoService in _echoServices)
                {
                    if (echoService.ConnectionSocket.Connected == false)
                    {
                        Console.WriteLine("Client " + echoService.ClientNumber + " closed.");
                        echoServicesRemove.Add(echoService);
                    }
                }
                foreach (EchoService echoService in echoServicesRemove)
                {
                    _echoServices.Remove(echoService);
                }
                Thread.Sleep(100);
            }
        }
    }
}
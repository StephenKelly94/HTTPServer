using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TCPEchoServer
{
    class ServiceEcho
    {
        private TcpListener serverSocket;
        private List<EchoService> echoServices = new List<EchoService>();
        public ServiceEcho()
        {
            //serverSocket = new TcpListener(65080);
            //serverSocket = new TcpListener(IPAddress.Parse("127.0.0.1"), 65080);
            serverSocket = new TcpListener(IPAddress.Parse("127.0.0.1"), 80);

            
        }

        public void run()
        {
            serverSocket.Start();

            Thread checkThread = new Thread(CheckConnections);
            checkThread.Start();

            while (true)
            {
//                IPAddress[] ipAddresses = Dns.GetHostEntry("localhost").AddressList;
//                string addresses = "";
//                foreach (IPAddress ipAddress in ipAddresses)
//                {
//                    addresses = String.Join(", ", addresses, ipAddress.ToString());
//                }
//                Console.WriteLine("address list:\n" + addresses + "------\n");

                Console.WriteLine("Waiting for a new client...");
                TcpClient connectionSocket = serverSocket.AcceptTcpClient();
                
                //Socket connectionSocket = serverSocket.AcceptSocket();
                Console.WriteLine("Server activated");

                try
                {
                    EchoService echoService = new EchoService(connectionSocket);
                    echoServices.Add(echoService);
                    Thread thread = new Thread(echoService.DoIt);
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

                // Managing closing clients in a thread
                //                connectionSocket.Close();
                //                Console.WriteLine("Client Closed.");
                //                Console.WriteLine
            }

            serverSocket.Stop();
        }

        public void CheckConnections()
        {
            while (true)
            {
                List<EchoService> echoServicesRemove = new List<EchoService>();
                foreach (EchoService echoService in echoServices)
                {
                    if (echoService.ConnectionSocket.Connected == false)
                    {
                        Console.WriteLine("Client " + echoService.clientNumber + " closed.");
                        echoServicesRemove.Add(echoService);
                    }
                }
                foreach (EchoService echoService in echoServicesRemove)
                {
                    echoServices.Remove(echoService);
                }
                Thread.Sleep(100);
            }
        }
    }
}

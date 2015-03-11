using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace TCPEchoServer
{
    public class ServerThread
    {
        private static int _newestClientNumber;
        public int ClientNumber;

        public ServerThread(TcpClient connection)
        {
            ConnectionSocket = connection;
            ClientNumber = ++_newestClientNumber;
        }

        public TcpClient ConnectionSocket { get; private set; }

        public void DoIt()
        {
            Stream ns = ConnectionSocket.GetStream();

            StreamReader sr = new StreamReader(ns);
            StreamWriter sw = new StreamWriter(ns);
            sw.AutoFlush = true; // enable automatic flushing

            ReadingRequest readingRequest = new ReadingRequest(sr);
            Console.WriteLine("Client " + ClientNumber + ": " + readingRequest.toString());
            Console.WriteLine("-----------------------------------------------");
            if (readingRequest.RequestPacket != null)
            {
                HandlingRequest handlingRequest = new HandlingRequest();
                HTTPResponse httpResponse = handlingRequest.HandleRequest(readingRequest.RequestPacket);

                SendingResponse sendingResponse = new SendingResponse();
                sendingResponse.SendResponse(sw, httpResponse);
            }

            ns.Close();
            ConnectionSocket.Close();
        }

    }
}
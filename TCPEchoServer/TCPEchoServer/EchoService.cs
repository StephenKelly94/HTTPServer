using System;
using System.IO;
using System.Net.Sockets;

namespace TCPEchoServer
{
    internal class EchoService
    {
        private static int newestClientNumber;
        private static readonly string RootCatalog = "D:/Mokslai/CODS_SODP_HTTP/RootFolder";
        public int clientNumber;

        public EchoService(TcpClient connection)
        {
            ConnectionSocket = connection;
            clientNumber = ++newestClientNumber;
        }

        public TcpClient ConnectionSocket { get; private set; }

        public void DoIt()
        {
            Stream ns = ConnectionSocket.GetStream();

            var sr = new StreamReader(ns);
            var sw = new StreamWriter(ns);
            sw.AutoFlush = true; // enable automatic flushing


            var message = sr.ReadLine();
            var answer = "";
            while (message != null && message != "")
            {
                Console.WriteLine("Client " + clientNumber + ": " + message);
                var splitted = message.Split(' ');
                if (splitted[0] == "GET")
                {
                    //answer = "You requested: \"" + splitted[1] + "\"";
                    //answer = "\"" + message + "\"";
                    sendFile(splitted[1], sw);
                }
                message = sr.ReadLine();
            }

            ns.Close();
            ConnectionSocket.Close();
        }

        private void sendFile(String fileName, StreamWriter sw)
        {
            var fileStream = new FileStream(RootCatalog + fileName, FileMode.Open);
            var fileStreamReader = new StreamReader(fileStream);

            sw.Write("http/1.0 200 OK \r\n\r\n");
            fileStream.CopyTo(sw.BaseStream);
            fileStreamReader.Close();
            fileStream.Close();
        }
    }
}
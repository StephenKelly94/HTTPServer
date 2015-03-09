using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TCPEchoServer
{
    class EchoService
    {
        private TcpClient connectionSocket;
        private static int newestClientNumber;
        public int clientNumber;
        private static readonly string RootCatalog = "D:/Mokslai/CODS_SODP_HTTP/RootFolder";

        public TcpClient ConnectionSocket
        {
            get { return connectionSocket; }
        }

        public EchoService(TcpClient connection)
        {
            connectionSocket = connection;
            clientNumber = ++newestClientNumber;
        }

        public void DoIt()
        {
            Stream ns = connectionSocket.GetStream();
            // Stream ns = new NetworkStream(connectionSocket);

            StreamReader sr = new StreamReader(ns);
            StreamWriter sw = new StreamWriter(ns);
            sw.AutoFlush = true; // enable automatic flushing


            string message = sr.ReadLine();
            string answer = "";
            while (message != null && message != "")
            {
                Console.WriteLine("Client " + clientNumber + ": " + message);
                String[] splitted = message.Split(' ');
                if (splitted[0] == "GET")
                {
                    //answer = "You requested: \"" + splitted[1] + "\"";
                    //answer = "\"" + message + "\"";
                    sendFile(splitted[1], sw);
                }
                else
                {
                    //answer = "\"" + message + "\"";
                }
                //sw.WriteLine(answer);
                message = sr.ReadLine();
            }

            ns.Close();
            connectionSocket.Close();
        }

        private void sendFile(String fileName, StreamWriter sw)
        {
            FileStream fileStream = new FileStream(RootCatalog + fileName, FileMode.Open);
            StreamReader fileStreamReader = new StreamReader(fileStream);

            sw.Write("http/1.0 200 OK \r\n\r\n");
            fileStream.CopyTo(sw.BaseStream);
            fileStreamReader.Close();
            fileStream.Close();
        }
    }
}

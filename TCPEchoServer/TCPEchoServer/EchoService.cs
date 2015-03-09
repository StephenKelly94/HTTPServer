using System;
using System.IO;
using System.Net.Sockets;

namespace TCPEchoServer
{
    internal class EchoService
    {
        private static int _newestClientNumber;
        private const string RootCatalog = "../../../../RootFolder";
        public int ClientNumber;

        public EchoService(TcpClient connection)
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


            String message = sr.ReadLine();
            while (!string.IsNullOrEmpty(message))
            {
                Console.WriteLine("Client " + ClientNumber + ": " + message);
                String[] splitted = message.Split(' ');
                if (splitted[0] == "GET")
                {
                    sendFile(splitted[1], sw);
                }
                message = sr.ReadLine();
            }

            ns.Close();
            ConnectionSocket.Close();
        }

        private void sendFile(String fileName, StreamWriter sw)
        {
            try
            {
                FileStream fileStream = new FileStream(RootCatalog + fileName, FileMode.Open);
                StreamReader fileStreamReader = new StreamReader(fileStream);
                sw.Write("http/1.0 200 OK \r\n");
                ContentTypes contentTypes = new ContentTypes();
                sw.Write("Content-Type: " + contentTypes.GetContentType(Path.GetExtension(fileName)) + "\r\n");
                sw.Write("Content-Length: " + fileStream.Length + "\r\n\r\n");
                fileStream.CopyTo(sw.BaseStream);
                fileStreamReader.Close();
                fileStream.Close();
            }catch(FileNotFoundException e)
            {
                Console.WriteLine("File {0} not found", e.FileName);
                sw.Write("http/1.0 404 Not Found\r\n\r\n");
                FileStream fileStream = new FileStream(RootCatalog + "/404.html", FileMode.Open);
                StreamReader fileStreamReader = new StreamReader(fileStream);
                fileStream.CopyTo(sw.BaseStream);
                fileStreamReader.Close();
                fileStream.Close();

            }   
        }
    }
}
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HTTPServer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TCPEchoServer;

namespace HTTPServer.Tests
{
    [TestClass()]
    public class EchoServiceTests
    {
        private  ServerStart serviceStart;
        private  TcpClient clientSocket;
        private  Stream ns;
        private  StreamReader sr;
        private  StreamWriter sw;

        [TestInitialize]
        public void InitClass() //TestContext context
        {
            serviceStart = new ServerStart();
            Thread thread = new Thread(serviceStart.Run);
            thread.Start();

            clientSocket = new TcpClient("127.0.0.1", 80);

            ns = clientSocket.GetStream();  //provides a Stream
            sr = new StreamReader(ns);
            sw = new StreamWriter(ns);
            sw.AutoFlush = true; // enable automatic flushing

        }

        [TestMethod]
        public void DoItTestExistingFile()
        {
            sw.Write("GET /file.txt HTTP/1.1\r\n\r\n");
            String response = sr.ReadLine();
            Assert.AreEqual(response, "HTTP/1.0 200 OK");
        }

        [TestMethod]
        public void DoItTestNonExistingFile()
        {
            sw.Write("GET /michael HTTP/1.1\r\n\r\n");
            String response = sr.ReadLine();
            Assert.AreEqual(response, "HTTP/1.0 404 Not Found");
        }

        [TestMethod]
        public void DoItTestContentType()
        {
            sw.Write("GET /file.txt HTTP/1.1\r\n\r\n");

            sr.ReadLine();
            sr.ReadLine();
            String response = sr.ReadLine();

            ContentTypes contentTypes = new ContentTypes();

            Assert.AreEqual(response, "Content-Type: " + contentTypes.GetContentType("txt"));

            sr.ReadToEnd();
        }

        [TestMethod]
        public void DoItTestContentLength()
        {
            sw.Write("GET /file.txt HTTP/1.1\r\n\r\n");

            sr.ReadLine();
            sr.ReadLine();
            sr.ReadLine();
            String response = sr.ReadLine();

            Assert.AreEqual(response, "Content-Length: " + 20);

            sr.ReadToEnd();
        }

        [TestCleanup]
        public void StopServer()
        {
            serviceStart.Kill();
            ns.Close();
            clientSocket.Close();
        }
    }
}

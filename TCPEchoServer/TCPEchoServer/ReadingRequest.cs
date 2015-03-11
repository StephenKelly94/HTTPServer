using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TCPEchoServer
{
    public class ReadingRequest
    {
        private String _httpRequest;

        public ReadingRequest(StreamReader sr)
        {
            _httpRequest = sr.ReadLine();
            while (!String.IsNullOrEmpty(_httpRequest))
            {
                Console.WriteLine(_httpRequest);
                String[] lines = _httpRequest.Split(' ');
                if(lines[0] == "GET")
                    RequestPacket = new HTTPRequest(lines[0], WebUtility.UrlDecode(lines[1]));
                _httpRequest = sr.ReadLine();
            }


        }

        public String toString()
        {
            return _httpRequest;
        }

        public HTTPRequest RequestPacket { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TCPEchoServer
{
    public class HTTPRequest
    {

        public HTTPRequest(string method, string filePath)
        {
            HTTPMethod = method;
            FilePath = filePath;
        }

        public String FilePath { set; get; }
        public String HTTPMethod { set; get; }


    }
}

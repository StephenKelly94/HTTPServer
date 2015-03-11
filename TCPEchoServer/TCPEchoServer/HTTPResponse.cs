using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPEchoServer
{
    public class HTTPResponse
    {
        public HTTPResponse(String statusLine, String date, String contentType, String contentLength, String filePath)
        {
            StatusLine = statusLine;
            Date = date;
            ContentType = contentType;
            ContentLength = contentLength;
            FilePath = filePath;
        }
        public String StatusLine { set; get; }
        public String Date { set; get; }
        public String ContentType { set; get; }
        public String ContentLength { set; get; }
        public String FilePath { set; get; }
    }
}

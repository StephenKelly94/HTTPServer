using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPEchoServer
{
    public class SendingResponse
    {
        public void SendResponse(StreamWriter sw, HTTPResponse response)
        {
            FileStream fileStream = new FileStream(ServerStart.RootCatalog + response.FilePath, FileMode.Open);
            StreamReader fileStreamReader = new StreamReader(fileStream);
            Console.WriteLine(response.StatusLine + response.Date + response.ContentType + response.ContentLength);
            sw.Write(response.StatusLine + response.Date + response.ContentType + response.ContentLength);
            fileStream.CopyTo(sw.BaseStream);
            fileStreamReader.Close();
            fileStream.Close();
        }
    }
}

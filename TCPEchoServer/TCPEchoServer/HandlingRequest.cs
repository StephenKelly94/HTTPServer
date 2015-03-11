using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPEchoServer
{
    public class HandlingRequest
    {
        public HTTPResponse HandleRequest(HTTPRequest request)
        {
            HTTPResponse response;
            if (request.FilePath == "/")
            {
                String indexString = "/index.html";
                FileStream fileStream = new FileStream(ServerStart.RootCatalog + indexString, FileMode.Open);
                ContentTypes contentTypes = new ContentTypes();
                response = new HTTPResponse("HTTP/1.0 200 OK\r\n",
                    "Date: " + string.Format("{0:yyyy-MM-dd_hh-mm-ss-tt}", DateTime.Now) + "\r\n",
                    "",
                    "Content-Length: " + fileStream.Length + "\r\n\r\n",
                    indexString);
                fileStream.Close();
            }
            else if (File.Exists(ServerStart.RootCatalog + request.FilePath))
            {
                FileStream fileStream = new FileStream(ServerStart.RootCatalog + request.FilePath, FileMode.Open);
                ContentTypes contentTypes = new ContentTypes();
                response = new HTTPResponse("HTTP/1.0 200 OK\r\n",
                    "Date: "+ string.Format("{0:yyyy-MM-dd_hh-mm-ss-tt}", DateTime.Now) + "\r\n",
                    "Content-Type: " + contentTypes.GetContentType(Path.GetExtension(request.FilePath)) + "\r\n",
                    "Content-Length: " + fileStream.Length + "\r\n\r\n",
                    request.FilePath);
                fileStream.Close();
            }
            else
            {
                response = new HTTPResponse("HTTP/1.0 404 Not Found\r\n\r\n", "", "", "", "/404.html");
            }
            return response;
        }
    }
}

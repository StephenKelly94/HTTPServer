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
            else if (Directory.Exists(ServerStart.RootCatalog + request.FilePath))
            {
                GenerateHtmlFile(Directory.GetDirectories(ServerStart.RootCatalog + request.FilePath),
                    Directory.GetFiles(ServerStart.RootCatalog + request.FilePath)
                    );

                FileStream fileStream = new FileStream(ServerStart.RootCatalog + "/generatedDirs.html", FileMode.Open);
                response = new HTTPResponse("HTTP/1.0 200 OK\r\n",
                    "Date: " + string.Format("{0:yyyy-MM-dd_hh-mm-ss-tt}", DateTime.Now) + "\r\n",
                    "",
                    "Content-Length: " + fileStream.Length + "\r\n\r\n",
                    "/generatedDirs.html");
                fileStream.Close();
            }
            else
            {
                response = new HTTPResponse("HTTP/1.0 404 Not Found\r\n\r\n", "", "", "", "/404.html");
            }
            return response;
        }

        private void GenerateHtmlFile(String[] folders, String[] files)
        {
            String fileName = ServerStart.RootCatalog + "/generatedDirs.html";
            File.Delete(ServerStart.RootCatalog + "/generatedDirs.html");
            StreamWriter streamWriter = new StreamWriter(new FileStream(fileName, FileMode.OpenOrCreate));
            streamWriter.WriteLine("<!DOCTYPE html><html><body>");

            foreach (string s in folders)
            {
                String ss = (s.Remove(0, ServerStart.RootCatalog.Count())).Replace('\\', '/');
                streamWriter.Write("<a href = \"");
                streamWriter.Write(ss);
                streamWriter.Write("\">");
                streamWriter.Write("<p>" + ss + "</p>");
                streamWriter.Write("</a>");
                Console.WriteLine("--->" + ss);
            }

            foreach (string s in files)
            {
                String ss = (s.Remove(0, ServerStart.RootCatalog.Count())).Replace('\\', '/');
                streamWriter.Write("<a href = \"");
                streamWriter.Write(ss);
                streamWriter.Write("\">");
                streamWriter.Write("<p>" + ss + "</p>");
                streamWriter.Write("</a>");
                Console.WriteLine("--->" + ss);
            }



            streamWriter.WriteLine("</body></html>");
            streamWriter.Close();
        }
    }
}

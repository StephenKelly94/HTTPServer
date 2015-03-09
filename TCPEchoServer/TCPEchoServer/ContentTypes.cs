using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPEchoServer
{
    public class ContentTypes
    {
        private Dictionary<string, string> _contentTypeDictionary;
        public ContentTypes()
        {
            _contentTypeDictionary = new Dictionary<string, string>()
            {
                {"html", 	"text/html"},
                {"htm", 	"text/html"},
                {"doc", 	"application/msword"},
                {"gif", 	"image/gif"},
                {"jpg", 	"image/jpeg"},
                {"pdf", 	"application/pdf"},
                {"css", 	"text/css"},
                {"xml", 	"text/html"},
                {"jar", 	"application/x-java-archive"}            
            };

        }

        public String GetContentType(String filename)
        {
            string value;
            if (_contentTypeDictionary.TryGetValue(filename, out value))
            {
                return value;
            }
            return "application/octet-stream";
        }
    }
}

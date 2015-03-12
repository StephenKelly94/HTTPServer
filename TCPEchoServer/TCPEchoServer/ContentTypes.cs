using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPEchoServer
{
    public class ContentTypes
    {
        private Dictionary<string, string> _contentTypeDictionary;
        private static bool _alreadyLoadedFromConfig = false;
        public ContentTypes()
        {
            _contentTypeDictionary = new Dictionary<string, string>()
            {
//                {"html", 	"text/html"},
//                {"htm", 	"text/html"},
//                {"doc", 	"application/msword"},
//                {"gif", 	"image/gif"},
//                {"jpg", 	"image/jpeg"},
//                {"pdf", 	"application/pdf"},
//                {"css", 	"text/css"},
//                {"xml", 	"text/html"},
//                {"jar", 	"application/x-java-archive"}            
            };

            if (!_alreadyLoadedFromConfig)
            {
                LoadContentTypesFromConfig();
                _alreadyLoadedFromConfig = true;
            }

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

        public void LoadContentTypesFromConfig()
        {
            try
            {
                StreamReader sr = new StreamReader(new FileStream("content_types.cfg", FileMode.Open));
                while (!sr.EndOfStream)
                {
                    String line = sr.ReadLine();
                    String[] splitted = line.Split(' ');
                    if (splitted.Length == 2)
                    {
                        _contentTypeDictionary.Add(splitted[0], splitted[1]);
                    }
                }
                sr.Close();
            }
            catch (FileNotFoundException e)
            {
                Debug.WriteLine(e.StackTrace);
            }
        }

        public void SaveContentTypesToConfig()
        {

        }
    }
}

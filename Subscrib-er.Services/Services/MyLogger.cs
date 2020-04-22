using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Subscrib_er.Services.Services
{
    public class MyLogger
    {
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string Folder { get; set; }

        public MyLogger(string folder, string filename)
        {
            this.FileName = filename;
            this.Folder = folder;
            this.FilePath = Path.Combine(this.Folder, this.FileName);
        }

       
        public bool WriteTokenToFile(string url)
        {
            using (StreamWriter streamWriter = new StreamWriter(this.FilePath, true))
            {
                streamWriter.Write(url);
            }
            return true;
        }
    }
}

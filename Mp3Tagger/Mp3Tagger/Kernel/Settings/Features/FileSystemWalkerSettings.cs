using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Mp3Tagger.Kernel.Interfaces;

namespace Mp3Tagger.Kernel.Settings.Features
{
    public class FileSystemWalkerSettings : ISettings
    {        
        public string Root { get; set; }
        public long MinFileBytes { get; set; }
        public void InitializeByDefault()
        {
            MinFileBytes = 0;
        }
    }
}

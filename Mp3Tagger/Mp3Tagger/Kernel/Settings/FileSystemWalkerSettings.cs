﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mp3Tagger.Kernel.Interfaces;

namespace Mp3Tagger.Kernel.Settings
{
    public class FileSystemWalkerSettings : IFeatureSettings
    {
        public string Root { get; set; }
        public long MinFileBytes { get; set; }       
    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mp3Tagger.Kernel.Models;

namespace Mp3Tagger.Kernel.Interfaces
{
    public interface IFeature
    {
        string Name { get; set; }
        ISettings Settings { get;}
        void Initialize(ISettings settings);                        
    }
}

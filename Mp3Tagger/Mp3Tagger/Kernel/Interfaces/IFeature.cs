using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mp3Tagger.Kernel.Models;

namespace Mp3Tagger.Kernel.Interfaces
{
    public interface IFeature
    {
        string Name { get; set; }
        IFeatureSettings Settings { get;}
        void Initialize(IFeatureSettings settings);                        
    }
}

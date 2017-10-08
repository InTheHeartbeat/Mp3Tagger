using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mp3Tagger.Kernel.Models;

namespace Mp3Tagger.Kernel.Interfaces
{
    public interface IFeature
    {
        string Name { get; set; }
        Task ApplyToList(List<Composition> list, Action<IFeature, int, int> progressUpdatedCallback, Action<IFeature> progressCompletedCallback);
        void ApplyToComposition(Composition composition);        
    }
}
